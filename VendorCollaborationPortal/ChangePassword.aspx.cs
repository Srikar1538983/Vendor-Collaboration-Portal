using System;
using System.Data.SqlClient;
using System.Configuration;

namespace VendorCollaborationPortal
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["D365FOConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["VendAccount"] != null)
                {
                    txtVendorAccount.Text = Session["VendAccount"].ToString();
                    txtVendorAccount.ReadOnly = true;
                }
            }
        }


        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            string vendorAccount   = txtVendorAccount.Text.Trim();
            string currentPassword = txtCurrentPassword.Text;
            string newPassword     = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrEmpty(vendorAccount))
            {
                ShowMessage("Please enter vendor account.", false);
                return;
            }

            if (string.IsNullOrEmpty(currentPassword))
            {
                ShowMessage("Please enter current password.", false);
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
                ShowMessage("Please re-enter new password.", false);
                return;
            }

            if (newPassword != confirmPassword)
            {
                ShowMessage("New password and re-entered password do not match.", false);
                return;
            }

            if (currentPassword == newPassword)
            {
                ShowMessage("New password must be different from current password.", false);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = @" SELECT TOP 1
                                            SSALVENDORPORTALPASSWORD
                                        FROM VendTable
                                        WHERE ACCOUNTNUM = @VendorAccount";
                    
                    string storedPassword = null;

                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@VendorAccount", vendorAccount);
                        object result = command.ExecuteScalar();

                        if (result == null || result == DBNull.Value)
                        {
                            ShowMessage("Vendor account does not exist.", false);
                            return;
                        }
                        storedPassword = result.ToString();
                    }

                    if (currentPassword != storedPassword)
                    {
                        ShowMessage("Current password is incorrect.", false);
                        return;
                    }

                    string updateQuery = @" UPDATE VendTable
                                            SET SSALVENDORPORTALPASSWORD = @NewPassword
                                            WHERE ACCOUNTNUM = @VendorAccount";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@NewPassword", newPassword);
                        command.Parameters.AddWithValue("@VendorAccount", vendorAccount);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            ShowMessage("Password changed successfully. " + "You will be redirected to the login page in 10 seconds.", true);

                            txtCurrentPassword.Text = "";
                            txtNewPassword.Text     = "";
                            txtConfirmPassword.Text = "";

                            string script = @" setTimeout(function () { window.location.href = 'Login.aspx';}, 5000);";

                            ClientScript.RegisterStartupScript(this.GetType(), "PasswordChangeRedirect", script, true);
                        }
                        else
                        {
                            ShowMessage("Password could not be changed.", false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Database error: " + ex.Message, false);
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