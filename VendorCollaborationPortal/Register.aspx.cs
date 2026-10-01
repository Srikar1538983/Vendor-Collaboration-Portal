using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using Newtonsoft.Json;
using System.Diagnostics;

namespace VendorCollaborationPortal
{
    public partial class Register : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["D365FOConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected async void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();

            if (string.IsNullOrEmpty(username))
            {
                lblMessage.Text =
                    "Please enter username.";

                lblMessage.ForeColor =
                    System.Drawing.Color.Red;

                return;
            }

            // =========================================
            // CHECK USERNAME ALREADY EXISTS
            // =========================================

            try
            {
                if (IsUsernameAlreadyTaken(username))
                {
                    lblMessage.Text =
                        "Username already exists. Please choose another username.";

                    lblMessage.ForeColor =
                        System.Drawing.Color.Red;

                    return;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text =
                    "Unable to validate username: " + ex.Message;

                lblMessage.ForeColor =
                    System.Drawing.Color.Red;

                return;
            }

            // =========================================
            // PASSWORD MATCH
            // =========================================

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                lblMessage.Text =
                    "Password and Confirm Password do not match.";

                lblMessage.ForeColor =
                    System.Drawing.Color.Red;

                return;
            }

            // =========================================
            // PASSWORD COMPLEXITY
            // =========================================

            if (!IsValidPassword(txtPassword.Text))
            {
                lblMessage.Text =
                    "Password must contain at least 8 characters, " +
                    "1 uppercase character, 1 lowercase character, " +
                    "and 1 symbol.";

                lblMessage.ForeColor =
                    System.Drawing.Color.Red;

                return;
            }

            // =========================================
            // VENDOR TYPE
            // =========================================

            if (string.IsNullOrEmpty(ddlVendorType.SelectedValue))
            {
                lblMessage.Text =
                    "Please select Vendor Type.";

                lblMessage.ForeColor =
                    System.Drawing.Color.Red;

                return;
            }

            try
            {
                lblMessage.Text =
                    "Creating vendor...";

                lblMessage.ForeColor =
                    System.Drawing.Color.Blue;

                string result =
                    await CreateVendorAsync();

                lblMessage.Text =
                    "Vendor registered successfully.";

                lblMessage.ForeColor =
                    System.Drawing.Color.Green;

                await Task.Delay(10000);

                Response.Redirect(
                    "Login.aspx",
                    false);

                Context.ApplicationInstance
                    .CompleteRequest();
            }
            catch (TaskCanceledException)
            {
                lblMessage.Text =
                    "D365FO request timed out.";

                lblMessage.ForeColor =
                    System.Drawing.Color.Red;
            }
            catch (Exception ex)
            {
                lblMessage.Text =
                    "Vendor registration failed: " +
                    ex.Message;

                lblMessage.ForeColor =
                    System.Drawing.Color.Red;
            }
        }


        // =========================================================
        // GET ACCESS TOKEN FROM MICROSOFT ENTRA ID
        // =========================================================

        private async Task<string> GetAccessTokenAsync()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            string tenantId =
                ConfigurationManager.AppSettings["D365TenantId"];

            string clientId =
                ConfigurationManager.AppSettings["D365ClientId"];

            string clientSecret =
                ConfigurationManager.AppSettings["D365ClientSecret"];

            string d365Url =
                ConfigurationManager.AppSettings["D365EnvironmentUrl"];

            string tokenUrl =
                "https://login.microsoftonline.com/"
                + tenantId
                + "/oauth2/v2.0/token";

            lblMessage.Text =
                "Requesting Azure AD access token...";

            using (HttpClient client = new HttpClient())
            {
                client.Timeout =
                    TimeSpan.FromSeconds(30);

                var tokenRequest =
                    new FormUrlEncodedContent(
                        new[]
                        {
                    new KeyValuePair<string, string>(
                        "client_id",
                        clientId),

                    new KeyValuePair<string, string>(
                        "client_secret",
                        clientSecret),

                    new KeyValuePair<string, string>(
                        "scope",
                        d365Url.TrimEnd('/') + "/.default"),

                    new KeyValuePair<string, string>(
                        "grant_type",
                        "client_credentials")
                        });

                HttpResponseMessage response =
                    await client.PostAsync(
                        tokenUrl,
                        tokenRequest);

                string responseData =
                    await response.Content
                        .ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(
                        "Azure AD authentication failed. "
                        + "HTTP "
                        + (int)response.StatusCode
                        + " - "
                        + response.ReasonPhrase
                        + ". Response: "
                        + responseData);
                }

                TokenResponse tokenResponse =
                    JsonConvert.DeserializeObject<TokenResponse>(
                        responseData);

                if (tokenResponse == null ||
                    string.IsNullOrEmpty(
                        tokenResponse.access_token))
                {
                    throw new Exception(
                        "Azure AD did not return an access token.");
                }

                stopwatch.Stop();

                System.Diagnostics.Debug.WriteLine(
                    "TOKEN TIME: "
                    + stopwatch.ElapsedMilliseconds
                    + " ms");

                return tokenResponse.access_token;
            }
        }

        // =========================================================
        // CREATE VENDOR IN D365FO
        // =========================================================

        private async Task<string> CreateVendorAsync()
        {
            Stopwatch stopwatch =
                Stopwatch.StartNew();

            // ==========================================
            // STEP 1 - GET TOKEN
            // ==========================================

            lblMessage.Text =
                "Step 1: Getting Azure AD token...";

            string token =
                await GetAccessTokenAsync();

            System.Diagnostics.Debug.WriteLine(
                "Token received after: "
                + stopwatch.ElapsedMilliseconds
                + " ms");


            // ==========================================
            // STEP 2 - D365 URL
            // ==========================================

            string d365Url =
                ConfigurationManager
                    .AppSettings[
                        "D365EnvironmentUrl"];


            System.Diagnostics.Debug.WriteLine(
                "D365 URL: "
                + d365Url);


            // ==========================================
            // STEP 3 - HTTP CLIENT
            // ==========================================

            using (HttpClient client =
                new HttpClient())
            {
                client.Timeout =
                    TimeSpan.FromSeconds(120);

                client.BaseAddress =
                    new Uri(
                        d365Url.TrimEnd('/') + "/");


                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        token);


                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue(
                        "application/json"));


                var requestPayload = new
                {
                    _request = new
                    {
                        DataAreaId  = "INMF",
                        VendorName  = txtCompanyName.Text.Trim(),
                        VendorType  = ddlVendorType.SelectedValue,
                        VendorGroup = "Blank",
                        Email       = txtEmail.Text.Trim(),
                        Phone       = txtPhone.Text.Trim(),
                        Username    = txtUsername.Text.Trim(),
                        Password    = txtPassword.Text
                    }
                };


                string jsonContent =
                    JsonConvert.SerializeObject(
                        requestPayload);


                System.Diagnostics.Debug.WriteLine(
                    "REQUEST:");

                System.Diagnostics.Debug.WriteLine(
                    jsonContent);

                string endpoint =
                    "api/services/"
                    + "B2B_VendorServiceGroup/"
                    + "B2B_VendorService/"
                    + "createVendor";


                System.Diagnostics.Debug.WriteLine(
                    "ENDPOINT:");

                System.Diagnostics.Debug.WriteLine(
                    client.BaseAddress + endpoint);


                lblMessage.Text =
                    "Step 2: Calling D365FO custom service...";


                // ==========================================
                // STEP 6 - CALL D365FO
                // ==========================================

                using (StringContent content =
                    new StringContent(
                        jsonContent,
                        Encoding.UTF8,
                        "application/json"))
                {
                    Stopwatch d365Stopwatch = Stopwatch.StartNew();

                    lblMessage.Text =
                        "Calling D365FO custom service...";

                    HttpResponseMessage response =
                        await client.PostAsync(
                            endpoint,
                            content);

                    d365Stopwatch.Stop();

                    System.Diagnostics.Debug.WriteLine(
                        "D365FO RESPONSE TIME = "
                        + d365Stopwatch.Elapsed.TotalSeconds
                        + " seconds");


                    d365Stopwatch.Stop();


                    System.Diagnostics.Debug.WriteLine(
                        "D365 RESPONSE TIME: "
                        + d365Stopwatch.ElapsedMilliseconds
                        + " ms");


                    // ==========================================
                    // STEP 7 - READ RESPONSE
                    // ==========================================

                    string responseData =
                        await response.Content
                            .ReadAsStringAsync();


                    System.Diagnostics.Debug.WriteLine(
                        "HTTP STATUS: "
                        + (int)response.StatusCode);


                    System.Diagnostics.Debug.WriteLine(
                        "D365 RESPONSE:");

                    System.Diagnostics.Debug.WriteLine(
                        responseData);


                    // ==========================================
                    // STEP 8 - SUCCESS
                    // ==========================================

                    if (response.IsSuccessStatusCode)
                    {
                        return responseData;
                    }


                    // ==========================================
                    // STEP 9 - ERROR
                    // ==========================================

                    throw new Exception(
                        "D365FO returned HTTP "
                        + (int)response.StatusCode
                        + " - "
                        + response.ReasonPhrase
                        + ". Response: "
                        + responseData);
                }
            }
        }

        private bool IsUsernameAlreadyTaken(string username)
        {
            string query = @" SELECT COUNT(1)
                                FROM VendTable
                                WHERE SSALVENDORPORTALUSERNAME = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    connection.Open();

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }

        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            if (password.Length < 8)
                return false;

            bool hasUppercase = false;
            bool hasLowercase = false;
            bool hasSymbol = false;

            foreach (char character in password)
            {
                if (char.IsUpper(character))
                {
                    hasUppercase = true;
                }
                else if (char.IsLower(character))
                {
                    hasLowercase = true;
                }
                else if (!char.IsLetterOrDigit(character))
                {
                    hasSymbol = true;
                }
            }

            return hasUppercase &&
                   hasLowercase &&
                   hasSymbol;
        }

        // =========================================================
        // TOKEN RESPONSE
        // =========================================================

        public class TokenResponse
        {
            public string token_type { get; set; }

            public int expires_in { get; set; }

            public string access_token { get; set; }
        }
    }
}