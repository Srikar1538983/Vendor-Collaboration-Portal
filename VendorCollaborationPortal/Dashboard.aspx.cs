using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace VendorCollaborationPortal
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["VendAccount"] == null)
            {
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                txtTransactionDate.Text    = DateTime.Today.ToString("yyyy-MM-dd");
                txtTransactionDate.Enabled = false;

                LoadDashboard();
                LoadPurchaseOrders(ddlPOStatus.SelectedValue);
                LoadVendorTracking();
            }
            else
            {
                LoadPurchaseOrders(ddlPOStatus.SelectedValue);
            }
        }

        private void LoadDashboard()
        {
            string vendAccount = Session["VendAccount"].ToString();
            string vendorName = Session["PartyName"] != null ? Session["PartyName"].ToString() : "";
            string company = Session["Company"] != null ? Session["Company"].ToString() : "";

            lblUser.Text             = vendAccount;
            lblVendorName.Text       = vendorName;
            lblHeaderVendorName.Text = vendorName;

            LoadVendorDetails(vendAccount, company);
            LoadVendorContactInformation(vendAccount, company);
            LoadVendorAddress(vendAccount, company);
            LoadVendorTransactions(vendAccount, company, ddlShow.SelectedValue, txtTransactionDate.Text, chkHideCurrencyRevaluations.Checked, chkSettlement.Checked);
        }
        private void LoadVendorTracking()
        {
            if (Session["VendAccount"] == null ||
                Session["Company"] == null)
            {
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            string vendorAccount = Session["VendAccount"].ToString();
            string company = Session["Company"].ToString();

            string connectionString =
                ConfigurationManager.ConnectionStrings["D365FOConnection"].ConnectionString;

            string query = @"
        SELECT
            SSAL_VendorTrackingID,
            SSAL_VendorCode,
            SSAL_VendorName,
            SSAL_ReceivedDate,
            SSAL_InvoicedDate,
            SSAL_CGST,
            SSAL_SGST,
            SSAL_IGST,
            SSAL_Receiver
        FROM SSAL_VendorTrackingTable
        WHERE SSAL_VendorCode = @VendorAccount
          AND DATAAREAID = @DataAreaId
        ORDER BY SSAL_VendorTrackingID ASC";

            DataTable dt = new DataTable();

            // IMPORTANT:
            // Add the columns to the DataTable before creating/populating DataRows.

            dt.Columns.Add("SSAL_VendorTrackingID");
            dt.Columns.Add("SSAL_VendorCode");
            dt.Columns.Add("SSAL_VendorName");

            dt.Columns.Add("SSAL_ReceivedDate", typeof(DateTime));
            dt.Columns.Add("SSAL_InvoicedDate", typeof(DateTime));

            dt.Columns.Add("SSAL_CGST", typeof(decimal));
            dt.Columns.Add("SSAL_SGST", typeof(decimal));
            dt.Columns.Add("SSAL_IGST", typeof(decimal));

            dt.Columns.Add("SSAL_Receiver");

            using (SqlConnection connection =
                   new SqlConnection(connectionString))
            using (SqlCommand command =
                   new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue(
                    "@VendorAccount",
                    vendorAccount);

                command.Parameters.AddWithValue(
                    "@DataAreaId",
                    company);

                connection.Open();

                using (SqlDataReader reader =
                       command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DataRow row = dt.NewRow();

                        // Vendor Tracking ID
                        row["SSAL_VendorTrackingID"] =
                            reader["SSAL_VendorTrackingID"] == DBNull.Value
                            ? ""
                            : reader["SSAL_VendorTrackingID"].ToString();

                        // Vendor Code
                        row["SSAL_VendorCode"] =
                            reader["SSAL_VendorCode"] == DBNull.Value
                            ? ""
                            : reader["SSAL_VendorCode"].ToString();

                        // Vendor Name
                        row["SSAL_VendorName"] =
                            reader["SSAL_VendorName"] == DBNull.Value
                            ? ""
                            : reader["SSAL_VendorName"].ToString();

                        // Received Date
                        if (reader["SSAL_ReceivedDate"] != DBNull.Value)
                        {
                            row["SSAL_ReceivedDate"] =
                                Convert.ToDateTime(
                                    reader["SSAL_ReceivedDate"]);
                        }

                        // Invoiced Date
                        if (reader["SSAL_InvoicedDate"] != DBNull.Value)
                        {
                            row["SSAL_InvoicedDate"] =
                                Convert.ToDateTime(
                                    reader["SSAL_InvoicedDate"]);
                        }

                        // CGST
                        row["SSAL_CGST"] =
                            reader["SSAL_CGST"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(
                                reader["SSAL_CGST"]);

                        // SGST
                        row["SSAL_SGST"] =
                            reader["SSAL_SGST"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(
                                reader["SSAL_SGST"]);

                        // IGST
                        row["SSAL_IGST"] =
                            reader["SSAL_IGST"] == DBNull.Value
                            ? 0
                            : Convert.ToDecimal(
                                reader["SSAL_IGST"]);

                        // Receiver
                        row["SSAL_Receiver"] =
                            reader["SSAL_Receiver"] == DBNull.Value
                            ? ""
                            : reader["SSAL_Receiver"].ToString();

                        dt.Rows.Add(row);
                    }
                }
            }

            gvVendorTracking.DataSource = dt;
            gvVendorTracking.DataBind();
        }
        private void LoadVendorTransactions(string vendorAccount,
                                            string company,
                                            string showFilter,
                                            string selectedDate,
                                            bool hideCurrencyRevaluations,
                                            bool showSettlement)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["D365FOConnection"].ConnectionString;

            string query = @"SELECT
                                VOUCHER,
                                TRANSDATE,
                                INVOICE,
                                TXT,
                                AMOUNTMST,
                                CURRENCYCODE,
                                DUEDATE,
                                TRANSTYPE,
                                PROMISSORYNOTESTATUS,
                                CLOSED,
                                LASTSETTLEDATE
                            FROM VENDTRANS
                            WHERE ACCOUNTNUM = @VendorAccount
                              AND DATAAREAID = @DataAreaId";

            if (showFilter == "Open")
            {
                query += @" AND (CLOSED IS NULL OR CLOSED = '1900-01-01')";
            }
            else if (showFilter == "Closed")
            {
                query += @" AND CLOSED IS NOT NULL AND CLOSED <> '1900-01-01'";
            }
            else if (showFilter == "OpenIncludingClosedOnOrAfterDate")
            {
                query += @" AND (CLOSED IS NULL OR CLOSED = '1900-01-01' OR LASTSETTLEDATE >= @SelectedDate)";
            }

            if (hideCurrencyRevaluations)
            {
                query += @" AND TRANSTYPE <> 9";
            }

            if (showSettlement)
            {
                query += @" AND TRANSTYPE <> 24";
            }

            query += @" ORDER BY TRANSDATE DESC";

            DataTable dt = new DataTable();

            dt.Columns.Add("VOUCHER");
            dt.Columns.Add("TRANSDATE", typeof(DateTime));
            dt.Columns.Add("INVOICE");
            dt.Columns.Add("TXT");
            dt.Columns.Add("AMOUNTMST", typeof(decimal));
            dt.Columns.Add("CURRENCYCODE");
            dt.Columns.Add("DUEDATE", typeof(DateTime));
            dt.Columns.Add("TransactionType");
            dt.Columns.Add("Status");

            using (SqlConnection connection = new SqlConnection(connectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@VendorAccount", vendorAccount);
                command.Parameters.AddWithValue("@DataAreaId", company);
                if (showFilter == "OpenIncludingClosedOnOrAfterDate")
                {
                    DateTime filterDate;
                    if (!DateTime.TryParse(selectedDate, out filterDate))
                    {
                        filterDate = DateTime.Today;
                    }

                    command.Parameters.Add("@SelectedDate", SqlDbType.DateTime).Value = filterDate;
                }
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DataRow row = dt.NewRow();

                        row["VOUCHER"] = reader["VOUCHER"] == DBNull.Value ? "" : reader["VOUCHER"].ToString();

                        if (reader["TRANSDATE"] != DBNull.Value)
                        {
                            row["TRANSDATE"] = Convert.ToDateTime(reader["TRANSDATE"]);
                        }

                        row["INVOICE"] = reader["INVOICE"] == DBNull.Value ? "" : reader["INVOICE"].ToString();
                        row["TXT"]     = reader["TXT"] == DBNull.Value ? "" : reader["TXT"].ToString();

                        if (reader["AMOUNTMST"] != DBNull.Value)
                        {
                            row["AMOUNTMST"] = Convert.ToDecimal(reader["AMOUNTMST"]);
                        }
                        row["CURRENCYCODE"] = reader["CURRENCYCODE"] == DBNull.Value ? "" : reader["CURRENCYCODE"].ToString();

                        if (reader["DUEDATE"] != DBNull.Value)
                        {
                            row["DUEDATE"] = Convert.ToDateTime(reader["DUEDATE"]);
                        }

                        int transType = reader["TRANSTYPE"] == DBNull.Value ? 0 : Convert.ToInt32(reader["TRANSTYPE"]);
                        row["TransactionType"] = GetTransactionTypeName(transType);

                        int promissoryNoteStatus = reader["PROMISSORYNOTESTATUS"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PROMISSORYNOTESTATUS"]);
                        row["Status"] = GetPromissoryNoteStatusName(promissoryNoteStatus);
                        dt.Rows.Add(row);
                    }
                }
            }

            gvVendorTransactions.DataSource = dt;
            gvVendorTransactions.DataBind();
        }
        
        private string GetPromissoryNoteStatusName(int status)
        {
            switch (status)
            {
                case 0: return "None";
                case 1: return "Drawn";
                case 2: return "Redrawn";
                case 3: return "Protested";
                case 4: return "Honored";
                case 5: return "Remitted";
                case 6: return "Invoiced";
                case 7: return "Invoice Remitted";
                case 8: return "Endorsed";
                case 9: return "Endorsement Settled";
                default: return "Unknown";
            }
        }

        private string GetTransactionTypeName(int transType)
        {
            switch (transType)
            {
                case 0: return "None";
                case 1: return "Transfer";
                case 2: return "Sales";
                case 3: return "Purchase order";
                case 4: return "Inventory";
                case 5: return "Production";
                case 6: return "Project";
                case 7: return "Interest";
                case 8: return "Customer";
                case 9: return "Exchange adjustment";
                case 10: return "Summed up";
                case 11: return "Payroll";
                case 12: return "Fixed assets";
                case 13: return "Collection letter";
                case 14: return "Vendor";
                case 15: return "Payment";
                case 16: return "Tax";
                case 17: return "Bank";
                case 18: return "Conversion";
                case 19: return "Bill of exchange";
                case 20: return "Promissory note";
                case 21: return "Cost";
                case 22: return "Work";
                case 23: return "Fee";
                case 24: return "Settlement";
                case 25: return "Allocation";
                case 26: return "Elimination";
                case 27: return "Cash discount";
                case 28: return "Over/under";
                case 29: return "Penny difference";
                case 30: return "Cross-company settlement";
                case 31: return "Purchase requisition";
                case 32: return "Inflation adjustment";
                case 33: return "Purchase advance application";
                case 34: return "Conversion reporting";
                case 35: return "Fixed assets RU";
                case 36: return "General Journal";
                default: return "Other";
            }
        }

        private void LoadVendorDetails(string vendorAccount, string company)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["D365FOConnection"].ConnectionString;

            string query = @" SELECT TOP 1
                                VT.ACCOUNTNUM,
                                VT.VendGroup,
                                VT.CURRENCY,
                                VT.PAYMTERMID,
                                VT.PAYMMODE,
                                VT.BLOCKED,
                                DPT.NAME,
                                DPT.PRIMARYCONTACTEMAIL
                            FROM VENDTABLE VT
                            INNER JOIN DIRPARTYTABLE DPT
                                ON DPT.RECID = VT.PARTY
                            WHERE VT.ACCOUNTNUM = @VendorAccount
                              AND VT.DATAAREAID = @DataAreaId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@VendorAccount", vendorAccount);
                command.Parameters.AddWithValue("@DataAreaId", company);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lblVendorAccount.Text = reader["ACCOUNTNUM"].ToString();
                        lblVendorDetailName.Text = reader["NAME"].ToString();
                        lblVendorGroup.Text = reader["VendGroup"].ToString();
                        lblVendorCurrency.Text = reader["CURRENCY"].ToString();
                        lblPaymentTerms.Text = reader["PAYMTERMID"].ToString();
                        lblPaymentMethod.Text = reader["PAYMMODE"].ToString();
                    }
                }
            }
        }

        private void LoadVendorContactInformation(string vendorAccount, string company)
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings["D365FOConnection"].ConnectionString;

            DataTable dt = new DataTable();

            dt.Columns.Add("ContactName");
            dt.Columns.Add("Email");
            dt.Columns.Add("Phone");

            DataRow vendorRow = dt.NewRow();

            vendorRow["ContactName"] = "";
            vendorRow["Email"] = "";
            vendorRow["Phone"] = "";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string contactQuery = @"SELECT
                                            DPT.NAME AS ContactName,
                                            DPCIV.LOCATOR,
                                            DPCIV.TYPE,
                                            DPCIV.ISPRIMARY
                                        FROM DirPartyContactInfoView AS DPCIV

                                        INNER JOIN VendTable AS VT
                                            ON DPCIV.PARTY = VT.PARTY

                                        INNER JOIN DirPartyTable AS DPT
                                            ON DPT.RECID = VT.PARTY

                                        WHERE VT.DATAAREAID = @DataAreaId
                                          AND VT.ACCOUNTNUM = @VendorAccount

                                        ORDER BY DPCIV.ISPRIMARY DESC";

                using (SqlCommand command = new SqlCommand(contactQuery, connection))
                {
                    command.Parameters.AddWithValue("@VendorAccount", vendorAccount);
                    command.Parameters.AddWithValue("@DataAreaId", company);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (string.IsNullOrEmpty(vendorRow["ContactName"].ToString()))
                            {
                                if (reader["ContactName"] != DBNull.Value)
                                {
                                    vendorRow["ContactName"] = reader["ContactName"].ToString();
                                }
                            }

                            if (reader["LOCATOR"] == DBNull.Value)
                            {
                                continue;
                            }

                            string locator = reader["LOCATOR"].ToString().Trim();

                            if (string.IsNullOrEmpty(locator))
                            {
                                continue;
                            }

                            int contactType = 0;

                            if (reader["TYPE"] != DBNull.Value)
                            {
                                contactType = Convert.ToInt32(reader["TYPE"]);
                            }

                            if (contactType == 1)
                            {
                                if (string.IsNullOrEmpty(vendorRow["Phone"].ToString()))
                                {
                                    vendorRow["Phone"] = locator;
                                }
                            }

                            else if (contactType == 2)
                            {
                                if (string.IsNullOrEmpty(vendorRow["Email"].ToString()))
                                {
                                    vendorRow["Email"] = locator;
                                }
                            }
                        }
                    }
                }
            }

            dt.Rows.Add(vendorRow);

            gvVendorContacts.DataSource = dt;
            gvVendorContacts.DataBind();
        }

        private void LoadVendorAddress(string vendorAccount, string company)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["D365FOConnection"].ConnectionString;

            DataTable dt = new DataTable();

            dt.Columns.Add("Address");
            dt.Columns.Add("City");
            dt.Columns.Add("State");
            dt.Columns.Add("ZipCode");
            dt.Columns.Add("Country");

            string addressQuery = @" SELECT TOP 1
                                        DPAV.ADDRESS,
                                        DPAV.CITY,
                                        DPAV.STATE,
                                        DPAV.ZIPCODE,
                                        DPAV.COUNTRYREGIONID
                                    FROM DirPartyPostalAddressView AS DPAV
                                    INNER JOIN VendTable AS VT
                                        ON DPAV.PARTY = VT.PARTY
                                    WHERE VT.DATAAREAID = @DataAreaId
                                      AND VT.ACCOUNTNUM = @VendorAccount";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(addressQuery, connection))
            {
                command.Parameters.AddWithValue("@VendorAccount", vendorAccount);
                command.Parameters.AddWithValue("@DataAreaId", company);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DataRow row = dt.NewRow();

                        row["Address"] = reader["ADDRESS"] == DBNull.Value ? "" : reader["ADDRESS"].ToString();
                        row["City"]    = reader["CITY"] == DBNull.Value ? "" : reader["CITY"].ToString();
                        row["State"]   = reader["STATE"] == DBNull.Value ? "" : reader["STATE"].ToString();
                        row["ZipCode"] = reader["ZIPCODE"] == DBNull.Value ? "" : reader["ZIPCODE"].ToString();
                        row["Country"] = reader["COUNTRYREGIONID"] == DBNull.Value ? "" : reader["COUNTRYREGIONID"].ToString();
                        dt.Rows.Add(row);
                    }
                }
            }
            gvVendorAddress.DataSource = dt;
            gvVendorAddress.DataBind();
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            Response.Redirect("Login.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        private void LoadPurchaseOrders(string statusFilter = "All")
        {
            if (Session["VendAccount"] == null || Session["Company"] == null)
            {
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            string vendorAccount = Session["VendAccount"].ToString();
            string company       = Session["Company"].ToString();

            string connectionString = ConfigurationManager.ConnectionStrings["D365FOConnection"].ConnectionString;

            string query = @" SELECT
                                PT.PURCHID,
                                PT.ORDERACCOUNT,
                                PT.PURCHSTATUS,
                                PT.CURRENCYCODE,
                                PT.CREATEDDATETIME AS PURCHORDERDATE
                            FROM PURCHTABLE AS PT
                            WHERE PT.ORDERACCOUNT = @VendorAccount
                              AND PT.DATAAREAID = @DataAreaId";

            if (statusFilter == "OpenOrder")
            {
                query += " AND PT.PURCHSTATUS = 1";
            }
            else if (statusFilter == "Received")
            {
                query += " AND PT.PURCHSTATUS = 2";
            }
            else if (statusFilter == "Invoiced")
            {
                query += " AND PT.PURCHSTATUS = 3";
            }
            else if (statusFilter == "Cancelled")
            {
                query += " AND PT.PURCHSTATUS = 4";
            }

            query += @" ORDER BY PT.PURCHID DESC";

            DataTable dt = new DataTable();

            dt.Columns.Add("PURCHID");
            dt.Columns.Add("ORDERACCOUNT");
            dt.Columns.Add("PURCHSTATUS");
            dt.Columns.Add("CURRENCYCODE");
            dt.Columns.Add("PURCHORDERDATE", typeof(DateTime));

            using (SqlConnection connection = new SqlConnection(connectionString))

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@VendorAccount", vendorAccount);
                command.Parameters.AddWithValue("@DataAreaId", company);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DataRow row = dt.NewRow();

                        row["PURCHID"]      = reader["PURCHID"] == DBNull.Value  ? "" : reader["PURCHID"].ToString();
                        row["ORDERACCOUNT"] = reader["ORDERACCOUNT"] == DBNull.Value ? "" : reader["ORDERACCOUNT"].ToString();
                        int purchStatus     = reader["PURCHSTATUS"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PURCHSTATUS"]);
                        row["PURCHSTATUS"]  = GetPurchaseOrderStatusName(purchStatus);
                        row["CURRENCYCODE"] = reader["CURRENCYCODE"] == DBNull.Value ? "" : reader["CURRENCYCODE"].ToString();

                        if (reader["PURCHORDERDATE"] != DBNull.Value)
                        {
                            row["PURCHORDERDATE"] = Convert.ToDateTime(reader["PURCHORDERDATE"]);
                        }
                        dt.Rows.Add(row);
                    }
                }
            }
            rptPurchaseOrders.DataSource = dt;
            rptPurchaseOrders.DataBind();
            lblNoPurchaseOrders.Visible = dt.Rows.Count == 0;
        }


        private string GetPurchaseOrderStatusName(int status)
        {
            switch (status)
            {
                case 0:
                    return "Open order";

                case 1:
                    return "Open order";

                case 2:
                    return "Received";

                case 3:
                    return "Invoiced";

                case 4:
                    return "Cancelled";

                default:
                    return "Unknown";
            }
        }


        private void LoadPurchaseLines(string purchId, GridView targetGrid)
        {
            if (Session["VendAccount"] == null || Session["Company"] == null)
            {
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            string vendorAccount    = Session["VendAccount"].ToString();
            string company          = Session["Company"].ToString();
            string connectionString = ConfigurationManager.ConnectionStrings["D365FOConnection"].ConnectionString;

            string query = @" SELECT
                            PL.LINENUMBER,
                            PL.ITEMID,
                            PL.NAME,
                            PL.PURCHQTY,
                            PL.PURCHPRICE,
                            PL.LINEAMOUNT,
                            PT.CURRENCYCODE
                        FROM PURCHLINE AS PL

                        INNER JOIN PURCHTABLE AS PT
                            ON PT.PURCHID = PL.PURCHID
                            AND PT.DATAAREAID = PL.DATAAREAID

                        WHERE PL.PURCHID = @PurchId
                          AND PT.ORDERACCOUNT = @VendorAccount
                          AND PL.DATAAREAID = @DataAreaId

                        ORDER BY PL.LINENUMBER";

            DataTable dt = new DataTable();
            dt.Columns.Add("LINENUMBER", typeof(decimal));
            dt.Columns.Add("ITEMID");
            dt.Columns.Add("NAME");
            dt.Columns.Add("PURCHQTY", typeof(decimal));
            dt.Columns.Add("PURCHPRICE", typeof(decimal));
            dt.Columns.Add("LINEAMOUNT", typeof(decimal));
            dt.Columns.Add("CURRENCYCODE");

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PurchId", purchId);
                command.Parameters.AddWithValue("@VendorAccount", vendorAccount);
                command.Parameters.AddWithValue("@DataAreaId", company);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DataRow row = dt.NewRow();
                        if (reader["LINENUMBER"] != DBNull.Value)
                        {
                            row["LINENUMBER"] = Convert.ToDecimal(reader["LINENUMBER"]);
                        }
                        row["ITEMID"] = reader["ITEMID"] == DBNull.Value ? "" : reader["ITEMID"].ToString();
                        row["NAME"]   = reader["NAME"] == DBNull.Value ? "" : reader["NAME"].ToString();

                        if (reader["PURCHQTY"] != DBNull.Value)
                        {
                            row["PURCHQTY"] = Convert.ToDecimal(reader["PURCHQTY"]);
                        }

                        if (reader["PURCHPRICE"] != DBNull.Value)
                        {
                            row["PURCHPRICE"] = Convert.ToDecimal(reader["PURCHPRICE"]);
                        }

                        if (reader["LINEAMOUNT"] != DBNull.Value)
                        {
                            row["LINEAMOUNT"] = Convert.ToDecimal(reader["LINEAMOUNT"]);
                        }

                        row["CURRENCYCODE"] = reader["CURRENCYCODE"] == DBNull.Value ? "" : reader["CURRENCYCODE"].ToString();
                        dt.Rows.Add(row);
                    }
                }
            }

            targetGrid.DataSource = dt;
            targetGrid.DataBind();
        }


        protected void ddlPOStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["ExpandedPurchaseOrder"] = null;

            LoadPurchaseOrders(ddlPOStatus.SelectedValue);
            upPurchaseOrders.Update();

            ScriptManager.RegisterStartupScript(upPurchaseOrders, upPurchaseOrders.GetType(), "KeepPurchaseOrdersOpen", "showPurchaseOrders();", true);
        }


        protected void rptPurchaseOrders_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName != "ViewPO")
            {
                return;
            }

            string purchId    = Convert.ToString(e.CommandArgument);
            string expandedPO = ViewState["ExpandedPurchaseOrder"] as string;

            if (!string.IsNullOrEmpty(expandedPO) && string.Equals(expandedPO, purchId, StringComparison.OrdinalIgnoreCase))
            {
                ViewState["ExpandedPurchaseOrder"] = null;
                upPurchaseOrders.Update();

                ScriptManager.RegisterStartupScript(upPurchaseOrders, upPurchaseOrders.GetType(), "KeepPurchaseOrdersOpenAfterPOClose", "showPurchaseOrders();", true);
                return;
            }

            foreach (RepeaterItem item in rptPurchaseOrders.Items)
            {
                HtmlTableRow detailRow = item.FindControl("trPODetails") as HtmlTableRow;

                if (detailRow != null)
                {
                    detailRow.Visible = false;
                }
            }

            HtmlTableRow selectedDetailRow = e.Item.FindControl("trPODetails") as HtmlTableRow;
            GridView     selectedLinesGrid = e.Item.FindControl("gvItemPurchaseLines") as GridView;
            Label        selectedPOLabel   = e.Item.FindControl("lblItemSelectedPO") as Label;

            if (selectedDetailRow != null && selectedLinesGrid != null)
            {
                if (selectedPOLabel != null)
                {
                    selectedPOLabel.Text = purchId;
                }

                LoadPurchaseLines(purchId, selectedLinesGrid);
                selectedDetailRow.Visible          = true;
                ViewState["ExpandedPurchaseOrder"] = purchId;
            }
            upPurchaseOrders.Update();

            ScriptManager.RegisterStartupScript(upPurchaseOrders, upPurchaseOrders.GetType(), "KeepPurchaseOrdersOpenAfterPOClick", "showPurchaseOrders();", true);
        }


        protected void TransactionFilter_Changed(object sender, EventArgs e)
        {
            if (Session["VendAccount"] == null ||  Session["Company"] == null)
            {
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            string vendAccount = Session["VendAccount"].ToString();
            string company = Session["Company"].ToString();

            txtTransactionDate.Enabled = ddlShow.SelectedValue == "OpenIncludingClosedOnOrAfterDate";

            LoadVendorTransactions(vendAccount, company, ddlShow.SelectedValue, txtTransactionDate.Text, chkHideCurrencyRevaluations.Checked, chkSettlement.Checked);
            upTransactions.Update();

            ScriptManager.RegisterStartupScript(upTransactions, upTransactions.GetType(), "KeepTransactionsOpen", "showTransactions();", true);
        }
        protected void btnNewVendorTracking_Click(object sender, EventArgs e)
        {
            if (Session["VendAccount"] == null || Session["Company"] == null)
            {
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            Response.Redirect("VendorTrackingNew.aspx");
        }
    }
}