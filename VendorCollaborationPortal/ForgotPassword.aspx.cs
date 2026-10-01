using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace VendorCollaborationPortal
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["D365FOConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSendOTP_Click(object sender, EventArgs e)
        {
            string vendorAccount = txtVendorAccount.Text.Trim();
            string enteredEmail  = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(vendorAccount))
            {
                ShowMessage("Please enter vendor account.", false);
                return;
            }
            if (string.IsNullOrEmpty(enteredEmail))
            {
                ShowMessage("Please enter primary email.", false);
                return;
            }
            
            try
            {
                string registeredEmail = "";
                string vendorName      = "";

                string query = @" SELECT TOP 1
                        VT.ACCOUNTNUM AS VendorId,
                        DPT.NAME AS VendorName,
                        LEA.LOCATOR AS Email,
                        VT.DATAAREAID
                    FROM VENDTABLE AS VT
                    INNER JOIN DIRPARTYTABLE AS DPT
                        ON VT.PARTY = DPT.RECID
                    INNER JOIN DIRPARTYLOCATION AS DPL
                        ON VT.PARTY = DPL.PARTY
                    INNER JOIN LOGISTICSELECTRONICADDRESS AS LEA
                        ON DPL.LOCATION = LEA.LOCATION
                    WHERE LEA.TYPE = 2
                      AND LEA.ISPRIMARY = 1
                      AND VT.ACCOUNTNUM = @VendorAccount";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@VendorAccount", vendorAccount);
                        connection.Open();

                        using (SqlDataReader reader =  command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                registeredEmail = reader["Email"].ToString().Trim();
                                vendorName      = reader["VendorName"].ToString().Trim();
                            }
                            else
                            {
                                ShowMessage("Vendor account does not exist or does not have a primary email address.", false);
                                return;
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(registeredEmail))
                {
                    ShowMessage("No primary email address is registered for this vendor.", false);
                    return;
                }

                if (!string.Equals(enteredEmail, registeredEmail, StringComparison.OrdinalIgnoreCase))
                {
                    ShowMessage("Vendor account and primary email do not match.", false);
                    return;
                }

                Random random = new Random();
                string otp    = random.Next(100000, 1000000).ToString();

                Session["ForgotPasswordOTP"]         = otp;
                Session["ForgotPasswordVendor"]      = vendorAccount;
                Session["ForgotPasswordEmail"]       = registeredEmail;
                Session["ForgotPasswordOTPExpiry"]   = DateTime.Now.AddMinutes(10);
                Session["ForgotPasswordOTPVerified"] = false;
                SendOTPEmail(registeredEmail, vendorName, otp);

                pnlOTP.Visible           = true;
                txtVendorAccount.Enabled = false;
                txtEmail.Enabled         = false;
                btnSendOTP.Enabled       = false;

                ShowMessage("OTP has been sent successfully to your primary email address.", true);
            }
            catch (Exception ex)
            {
                ShowMessage("Error sending OTP: " + ex.Message, false);
            }
        }

        protected void btnVerifyOTP_Click(object sender, EventArgs e)
        {
            string enteredOTP = txtOTP.Text.Trim();

            if (string.IsNullOrEmpty(enteredOTP))
            {
                ShowMessage("Please enter OTP.", false);
                return;
            }

            if (Session["ForgotPasswordOTP"] == null)
            {
                ShowMessage("OTP has expired. Please request a new OTP.", false);
                return;
            }

            if (Session["ForgotPasswordOTPExpiry"] == null)
            {
                ShowMessage("OTP has expired. Please request a new OTP.", false);
                return;
            }

            DateTime expiry = (DateTime) Session["ForgotPasswordOTPExpiry"];

            if (DateTime.Now > expiry)
            {
                ShowMessage("OTP has expired. Please request a new OTP.", false);
                return;
            }

            string storedOTP = Session["ForgotPasswordOTP"].ToString();
            if (enteredOTP != storedOTP)
            {
                ShowMessage("Invalid OTP.", false);
                return;
            }

            Session["ForgotPasswordOTPVerified"] = true;
            
            pnlOTP.Visible      = false;
            pnlPassword.Visible = true;

            ShowMessage("OTP verified successfully. Please enter your new password.", true);
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            string newPassword     = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (Session["ForgotPasswordOTPVerified"] == null || !(bool)Session["ForgotPasswordOTPVerified"])
            {
                ShowMessage("Please verify OTP first.", false);
                return;
            }

            if (string.IsNullOrEmpty(newPassword))
            {
                ShowMessage("Please enter new password.", false);
                return;
            }

            if (!IsValidPassword(newPassword))
            {
                ShowMessage("Password must contain at least 8 characters, " + "1 uppercase character, 1 lowercase character, " + "and 1 symbol.", false);
                return;
            }

            if (string.IsNullOrEmpty(confirmPassword))
            {
                ShowMessage("Please confirm new password.", false);
                return;
            }

            if (newPassword != confirmPassword)
            {
                ShowMessage("New password and confirm password do not match.", false);
                return;
            }

            string vendorAccount = Session["ForgotPasswordVendor"]?.ToString();

            if (string.IsNullOrEmpty(vendorAccount))
            {
                ShowMessage("Session expired. Please start the password reset process again.", false);
                return;
            }

            try
            {
                string updateQuery = @" UPDATE VENDTABLE
                                        SET SSALVENDORPORTALPASSWORD = @NewPassword
                                        WHERE ACCOUNTNUM = @VendorAccount";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@NewPassword", newPassword);
                        command.Parameters.AddWithValue("@VendorAccount", vendorAccount);
                        connection.Open();

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Session.Remove("ForgotPasswordOTP");
                            Session.Remove("ForgotPasswordOTPExpiry");
                            Session.Remove("ForgotPasswordOTPVerified");
                            Session.Remove("ForgotPasswordVendor");
                            Session.Remove("ForgotPasswordEmail");

                            ShowMessage("Password reset successfully. " + "You will be redirected to the login page in 10 seconds.", true);

                            string script = @" setTimeout(function () { window.location.href = 'Login.aspx'; }, 5000); ";

                            ClientScript.RegisterStartupScript(this.GetType(), "ForgotPasswordRedirect", script, true);
                        }
                        else
                        {
                            ShowMessage("Password could not be updated.", false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Database error: " + ex.Message, false);
            }
        }
        
        private void SendOTPEmail(string email, string vendorName, string otp)
        {
            string smtpServer   = "smtp.gmail.com";
            int smtpPort        = 587;
            string smtpUsername = "srikaryerroju001@gmail.com";
            string smtpPassword = "xdof zefn apjj kprp";
            string fromEmail    = smtpUsername;

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(fromEmail);

                mail.To.Add(new MailAddress(email));

                mail.Subject = "Vendor Collaboration Portal - Password Reset OTP";

                mail.Body =
                    "Dear " +
                    vendorName +
                    "," +
                    Environment.NewLine +
                    Environment.NewLine +

                    "We received a request to reset your Vendor Collaboration Portal password." +
                    Environment.NewLine +
                    Environment.NewLine +

                    "Your OTP is:" +
                    Environment.NewLine +
                    Environment.NewLine +

                    otp +
                    Environment.NewLine +
                    Environment.NewLine +

                    "This OTP is valid for 10 minutes." +
                    Environment.NewLine +
                    Environment.NewLine +

                    "If you did not request a password reset, please ignore this email." +
                    Environment.NewLine +
                    Environment.NewLine +

                    "Regards," +
                    Environment.NewLine +

                    "Vendor Collaboration Portal";

                mail.IsBodyHtml = false;

                using (SmtpClient smtp = new SmtpClient(smtpServer, smtpPort))
                {
                    smtp.EnableSsl   = true;
                    smtp.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtp.Send(mail);
                }
            }
        }

        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            if (password.Length < 8)
            {
                return false;
            }

            bool hasUppercase = false;
            bool hasLowercase = false;
            bool hasSymbol    = false;

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

            return hasUppercase && hasLowercase && hasSymbol;
        }

        private void ShowMessage(string message, bool success)
        {
            lblMessage.Text = message;

            if (success)
            {
                lblMessage.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}