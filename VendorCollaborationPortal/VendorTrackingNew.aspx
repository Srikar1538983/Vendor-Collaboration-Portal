<%@ Page Language="C#" AutoEventWireup="true" Async="true"
    CodeBehind="VendorTrackingNew.aspx.cs"
    Inherits="VendorCollaborationPortal.VendorTrackingNew" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>New Vendor Tracking</title>

    <meta name="viewport"
          content="width=device-width, initial-scale=1" />

    <style>

        * {
            box-sizing: border-box;
        }

        html,
        body {
            margin: 0;
            padding: 0;
            min-height: 100%;
            font-family: "Segoe UI", Arial, sans-serif;
            background: #f4f7fb;
            color: #172033;
        }

        .page-container {
            min-height: 100vh;
            padding: 40px;
        }

        .form-container {
            max-width: 900px;
            margin: 0 auto;

            background: #ffffff;

            border: 1px solid #e3e8f0;

            border-radius: 14px;

            box-shadow:
                0 8px 30px rgba(15,23,42,0.06);

            overflow: hidden;
        }

        .form-header {
            padding: 25px 30px;

            border-bottom:
                1px solid #e8edf4;

            background:
                linear-gradient(
                    90deg,
                    #f8faff,
                    #ffffff
                );
        }

        .form-title {
            margin: 0;

            color: #14213d;

            font-size: 25px;

            font-weight: 750;
        }

        .form-subtitle {
            margin-top: 6px;

            color: #8793a7;

            font-size: 12px;
        }

        .form-content {
            padding: 30px;
        }

        .vendor-info {
            display: grid;

            grid-template-columns:
                1fr 1fr;

            gap: 20px;

            margin-bottom: 25px;

            padding: 18px;

            background: #f8faff;

            border:
                1px solid #e3e8f0;

            border-radius: 10px;
        }

        .info-group {
            display: flex;
            flex-direction: column;
            gap: 5px;
        }

        .info-label {
            color: #7d899d;

            font-size: 11px;

            font-weight: 600;
        }

        .info-value {
            color: #243450;

            font-size: 14px;

            font-weight: 700;
        }

        .form-grid {
            display: grid;

            grid-template-columns:
                1fr 1fr;

            gap: 20px;
        }

        .form-group {
            display: flex;

            flex-direction: column;

            gap: 6px;
        }

        .form-group.full-width {
            grid-column: 1 / -1;
        }

        .form-label {
            color: #69768b;

            font-size: 11px;

            font-weight: 650;
        }

        .form-control {
            width: 100%;

            height: 40px;

            padding: 8px 11px;

            border:
                1px solid #cfd7e5;

            border-radius: 7px;

            background: #ffffff;

            color: #263650;

            font-size: 13px;

            outline: none;
        }

        .form-control:focus {
            border-color: #3b82f6;

            box-shadow:
                0 0 0 2px
                rgba(59,130,246,0.10);
        }

        .readonly-control {
            background: #f5f7fb;

            color: #526078;

            cursor: not-allowed;
        }

        .button-container {
            display: flex;

            justify-content: flex-end;

            gap: 12px;

            margin-top: 30px;

            padding-top: 20px;

            border-top:
                1px solid #e8edf4;
        }

        .save-button,
        .cancel-button {
            min-width: 100px;

            height: 40px;

            border: none;

            border-radius: 7px;

            font-size: 12px;

            font-weight: 650;

            cursor: pointer;
        }

        .save-button {
            background:
                linear-gradient(
                    90deg,
                    #3b82f6,
                    #6366f1
                );

            color: #ffffff;
        }

        .cancel-button {
            background: #eef1f6;

            color: #4b5870;
        }

        .save-button:hover {
            opacity: 0.92;
        }

        .cancel-button:hover {
            background: #e3e7ee;
        }

        .message {
            display: block;

            margin-top: 15px;

            font-size: 12px;

            font-weight: 600;
        }

        @media (max-width: 700px) {

            .page-container {
                padding: 20px;
            }

            .form-content {
                padding: 20px;
            }

            .form-grid,
            .vendor-info {
                grid-template-columns: 1fr;
            }

            .form-group.full-width {
                grid-column: auto;
            }
        }

    </style>

</head>

<body>

<form id="form1" runat="server">

    <div class="page-container">

        <div class="form-container">

            <!-- HEADER -->

            <div class="form-header">

                <h1 class="form-title">
                    New Vendor Tracking
                </h1>

                <div class="form-subtitle">
                    Enter vendor tracking information
                </div>

            </div>


            <!-- FORM -->

            <div class="form-content">

                <!-- VENDOR INFORMATION -->

                <div class="vendor-info">

                    <div class="info-group">

                        <span class="info-label">
                            Vendor Account
                        </span>

                        <asp:Label
                            ID="lblVendorAccount"
                            runat="server"
                            CssClass="info-value" />

                    </div>


                    <div class="info-group">

                        <span class="info-label">
                            Vendor Name
                        </span>

                        <asp:Label
                            ID="lblVendorName"
                            runat="server"
                            CssClass="info-value" />

                    </div>

                </div>


                <!-- INPUT FIELDS -->

                <div class="form-grid">

                    <!-- RECEIVED DATE -->

                    <div class="form-group">

                        <asp:Label
                            ID="lblReceivedDate"
                            runat="server"
                            Text="Received Date"
                            CssClass="form-label" />

                        <asp:TextBox
                            ID="txtReceivedDate"
                            runat="server"
                            TextMode="Date"
                            CssClass="form-control" />

                    </div>


                    <!-- INVOICED DATE -->

                    <div class="form-group">

                        <asp:Label
                            ID="lblInvoicedDate"
                            runat="server"
                            Text="Invoiced Date"
                            CssClass="form-label" />

                        <asp:TextBox
                            ID="txtInvoicedDate"
                            runat="server"
                            TextMode="Date"
                            CssClass="form-control" />

                    </div>


                    <!-- CGST -->

                    <div class="form-group">

                        <asp:Label
                            ID="lblCGST"
                            runat="server"
                            Text="CGST"
                            CssClass="form-label" />

                        <asp:TextBox
                            ID="txtCGST"
                            runat="server"
                            CssClass="form-control"
                            TextMode="Number" />

                    </div>


                    <!-- SGST -->

                    <div class="form-group">

                        <asp:Label
                            ID="lblSGST"
                            runat="server"
                            Text="SGST"
                            CssClass="form-label" />

                        <asp:TextBox
                            ID="txtSGST"
                            runat="server"
                            CssClass="form-control"
                            TextMode="Number" />

                    </div>


                    <!-- IGST -->

                    <div class="form-group">

                        <asp:Label
                            ID="lblIGST"
                            runat="server"
                            Text="IGST"
                            CssClass="form-label" />

                        <asp:TextBox
                            ID="txtIGST"
                            runat="server"
                            CssClass="form-control"
                            TextMode="Number" />

                    </div>


                    <!-- RECEIVER -->

                    <div class="form-group">

                        <asp:Label
                            ID="lblReceiver"
                            runat="server"
                            Text="Receiver"
                            CssClass="form-label" />

                        <asp:TextBox
                            ID="txtReceiver"
                            runat="server"
                            CssClass="form-control" />

                    </div>

                </div>


                <!-- MESSAGE -->

                <asp:Label
                    ID="lblMessage"
                    runat="server"
                    CssClass="message" />


                <!-- BUTTONS -->

                <div class="button-container">

                    <asp:Button
                        ID="btnCancel"
                        runat="server"
                        Text="Cancel"
                        CssClass="cancel-button"
                        CausesValidation="false"
                        OnClick="btnCancel_Click" />

                    <asp:Button
                        ID="btnSave"
                        runat="server"
                        Text="Save"
                        CssClass="save-button"
                        OnClick="btnSave_Click" />

                </div>

            </div>

        </div>

    </div>

</form>

</body>

</html>