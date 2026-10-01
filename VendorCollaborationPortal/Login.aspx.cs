using System;
using System.Data.SqlClient;
using System.Configuration;

namespace VendorCollaborationPortal
{
    public partial class Login : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["D365FOConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username))
            {
                lblMessage.Text = "Please enter username.";
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                lblMessage.Text = "Please enter password.";

                return;
            }
            
            string query = @" SELECT TOP 1
                                VT.ACCOUNTNUM,
                                VT.PARTY,
                                VT.DATAAREAID,
                                VT.SSALVENDORPORTALUSERNAME,
                                VT.SSALVENDORPORTALPASSWORD,
                                DPT.NAME AS PartyName,
                                DPT.PRIMARYCONTACTEMAIL
                            FROM VendTable AS VT
                            INNER JOIN DirPartyTable AS DPT
                                ON DPT.RECID = VT.PARTY
                            WHERE VT.SSALVENDORPORTALUSERNAME = @VendorUsername";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@VendorUsername", username);
                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPassword = reader["SSALVENDORPORTALPASSWORD"] == DBNull.Value  ? "" : reader["SSALVENDORPORTALPASSWORD"].ToString();

                                if (password != storedPassword)
                                {
                                    lblMessage.Text = "Invalid password.";
                                    return;
                                }
                                string vendAccount = reader["ACCOUNTNUM"].ToString();
                                string vendorName  = reader["PartyName"].ToString();
                                string company     = reader["DATAAREAID"].ToString();
                                string email       = reader["PRIMARYCONTACTEMAIL"] == DBNull.Value ? "" : reader["PRIMARYCONTACTEMAIL"].ToString();

                                Session["VendAccount"]    = vendAccount;
                                Session["PartyName"]      = vendorName;
                                Session["Company"]        = company;
                                Session["Email"]          = email;
                                Session["VendorUsername"] = username;

                                Response.Redirect("Dashboard.aspx", false);
                                Context.ApplicationInstance.CompleteRequest();
                                return;
                            }
                            else
                            {
                                lblMessage.Text = "Invalid username or password.";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "Database error: " + ex.Message;
                    }
                }
            }
        }
    }
}