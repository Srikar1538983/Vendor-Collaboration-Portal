using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using Newtonsoft.Json;
using System.Diagnostics;

namespace VendorCollaborationPortal
{
    public partial class VendorTrackingNew : System.Web.UI.Page
    {
        // =========================================================
        // PAGE LOAD
        // =========================================================

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["VendAccount"] == null ||
                Session["Company"] == null)
            {
                Response.Redirect(
                    "Login.aspx",
                    false);

                Context.ApplicationInstance
                    .CompleteRequest();

                return;
            }


            if (!IsPostBack)
            {
                LoadVendorInformation();
            }
        }


        // =========================================================
        // LOAD VENDOR INFORMATION FROM SESSION
        // =========================================================

        private void LoadVendorInformation()
        {
            string vendorAccount =
                Session["VendAccount"].ToString();

            string vendorName =
                Session["PartyName"] != null
                    ? Session["PartyName"].ToString()
                    : "";

            lblVendorAccount.Text =
                vendorAccount;

            lblVendorName.Text =
                vendorName;
        }


        // =========================================================
        // SAVE BUTTON
        // =========================================================

        protected async void btnSave_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                // =========================================
                // CHECK SESSION
                // =========================================

                if (Session["VendAccount"] == null ||
                    Session["Company"] == null)
                {
                    Response.Redirect(
                        "Login.aspx",
                        false);

                    Context.ApplicationInstance
                        .CompleteRequest();

                    return;
                }


                // =========================================
                // VALIDATE RECEIVED DATE
                // =========================================

                DateTime receivedDate;

                if (!DateTime.TryParse(
                        txtReceivedDate.Text,
                        out receivedDate))
                {
                    lblMessage.Text =
                        "Please enter Received Date.";

                    lblMessage.ForeColor =
                        System.Drawing.Color.Red;

                    return;
                }


                // =========================================
                // VALIDATE INVOICED DATE
                // =========================================

                DateTime invoicedDate;

                if (!DateTime.TryParse(
                        txtInvoicedDate.Text,
                        out invoicedDate))
                {
                    lblMessage.Text =
                        "Please enter Invoiced Date.";

                    lblMessage.ForeColor =
                        System.Drawing.Color.Red;

                    return;
                }


                // =========================================
                // VALIDATE CGST
                // =========================================

                decimal cgst;

                if (!decimal.TryParse(
                        txtCGST.Text,
                        out cgst))
                {
                    lblMessage.Text =
                        "Please enter valid CGST.";

                    lblMessage.ForeColor =
                        System.Drawing.Color.Red;

                    return;
                }


                // =========================================
                // VALIDATE SGST
                // =========================================

                decimal sgst;

                if (!decimal.TryParse(
                        txtSGST.Text,
                        out sgst))
                {
                    lblMessage.Text =
                        "Please enter valid SGST.";

                    lblMessage.ForeColor =
                        System.Drawing.Color.Red;

                    return;
                }


                // =========================================
                // VALIDATE IGST
                // =========================================

                decimal igst;

                if (!decimal.TryParse(
                        txtIGST.Text,
                        out igst))
                {
                    lblMessage.Text =
                        "Please enter valid IGST.";

                    lblMessage.ForeColor =
                        System.Drawing.Color.Red;

                    return;
                }


                // =========================================
                // VALIDATE RECEIVER
                // =========================================

                string receiver =
                    txtReceiver.Text.Trim();

                if (string.IsNullOrEmpty(receiver))
                {
                    lblMessage.Text =
                        "Please enter Receiver.";

                    lblMessage.ForeColor =
                        System.Drawing.Color.Red;

                    return;
                }


                // =========================================
                // CALL D365FO
                // =========================================

                lblMessage.Text =
                    "Creating vendor tracking record...";

                lblMessage.ForeColor =
                    System.Drawing.Color.Blue;


                string result =
                    await CreateVendorTrackingAsync(
                        receivedDate,
                        invoicedDate,
                        cgst,
                        sgst,
                        igst,
                        receiver);


                // =========================================
                // SUCCESS
                // =========================================

                lblMessage.Text =
                    "Vendor tracking created successfully.";

                lblMessage.ForeColor =
                    System.Drawing.Color.Green;


                await Task.Delay(1500);


                Response.Redirect(
                    "Dashboard.aspx?section=tracking",
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
                    "Vendor tracking creation failed: "
                    + ex.Message;

                lblMessage.ForeColor =
                    System.Drawing.Color.Red;
            }
        }


        // =========================================================
        // GET ACCESS TOKEN FROM MICROSOFT ENTRA ID
        // =========================================================

        private async Task<string> GetAccessTokenAsync()
        {
            Stopwatch stopwatch =
                Stopwatch.StartNew();


            string tenantId =
                ConfigurationManager.AppSettings[
                    "D365TenantId"];


            string clientId =
                ConfigurationManager.AppSettings[
                    "D365ClientId"];


            string clientSecret =
                ConfigurationManager.AppSettings[
                    "D365ClientSecret"];


            string d365Url =
                ConfigurationManager.AppSettings[
                    "D365EnvironmentUrl"];


            string tokenUrl =
                "https://login.microsoftonline.com/"
                + tenantId
                + "/oauth2/v2.0/token";


            lblMessage.Text =
                "Requesting Azure AD access token...";


            using (HttpClient client =
                new HttpClient())
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
        // CREATE VENDOR TRACKING IN D365FO
        // =========================================================

        private async Task<string>
            CreateVendorTrackingAsync(
                DateTime receivedDate,
                DateTime invoicedDate,
                decimal cgst,
                decimal sgst,
                decimal igst,
                string receiver)
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


                // ==========================================
                // STEP 4 - GET SESSION VALUES
                // ==========================================

                string vendorAccount =
                    Session["VendAccount"].ToString();


                string vendorName =
                    Session["PartyName"] != null
                        ? Session["PartyName"].ToString()
                        : "";


                string company =
                    Session["Company"].ToString();


                // ==========================================
                // STEP 5 - REQUEST PAYLOAD
                // ==========================================

                var requestPayload = new
                {
                    _request = new
                    {
                        DataAreaId = company,

                        VendorCode =
                            vendorAccount,

                        VendorName =
                            vendorName,

                        ReceivedDate =
                            receivedDate.ToString(
                                "yyyy-MM-dd"),

                        InvoicedDate =
                            invoicedDate.ToString(
                                "yyyy-MM-dd"),

                        CGST =
                            cgst,

                        SGST =
                            sgst,

                        IGST =
                            igst,

                        Receiver =
                            receiver
                    }
                };


                string jsonContent =
                    JsonConvert.SerializeObject(
                        requestPayload);


                // ==========================================
                // STEP 6 - DEBUG REQUEST
                // ==========================================

                System.Diagnostics.Debug.WriteLine(
                    "REQUEST:");

                System.Diagnostics.Debug.WriteLine(
                    jsonContent);


                // ==========================================
                // STEP 7 - D365FO ENDPOINT
                // ==========================================

                string endpoint =
                    "api/services/"
                    + "B2B_VendorTrackingServiceGroup/"
                    + "B2B_VendorTrackingService/"
                    + "createVendorTracking";


                System.Diagnostics.Debug.WriteLine(
                    "ENDPOINT:");

                System.Diagnostics.Debug.WriteLine(
                    client.BaseAddress + endpoint);


                lblMessage.Text =
                    "Step 2: Calling D365FO custom service...";


                // ==========================================
                // STEP 8 - CALL D365FO
                // ==========================================

                using (StringContent content =
                    new StringContent(
                        jsonContent,
                        Encoding.UTF8,
                        "application/json"))
                {
                    Stopwatch d365Stopwatch =
                        Stopwatch.StartNew();


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


                    System.Diagnostics.Debug.WriteLine(
                        "D365 RESPONSE TIME: "
                        + d365Stopwatch.ElapsedMilliseconds
                        + " ms");


                    // ==========================================
                    // STEP 9 - READ RESPONSE
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
                    // STEP 10 - SUCCESS
                    // ==========================================

                    if (response.IsSuccessStatusCode)
                    {
                        return responseData;
                    }


                    // ==========================================
                    // STEP 11 - ERROR
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


        // =========================================================
        // CANCEL
        // =========================================================

        protected void btnCancel_Click(
            object sender,
            EventArgs e)
        {
            Response.Redirect(
                "Dashboard.aspx?section=tracking",
                false);

            Context.ApplicationInstance
                .CompleteRequest();
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