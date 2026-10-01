<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="ForgotPassword.aspx.cs"
    Inherits="VendorCollaborationPortal.ForgotPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>Forgot Password - Vendor Portal</title>

    <style>

        body {
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
            background: #f3f6f9;
        }

        .forgot-container {
            width: 420px;
            margin: 70px auto;
            background: white;
            padding: 35px;
            border-radius: 10px;
            box-shadow: 0 4px 15px rgba(0,0,0,0.15);
        }

        .title {
            text-align: center;
            margin-bottom: 30px;
        }

        .title h2 {
            margin-bottom: 5px;
        }

        .title p {
            color: #777;
            font-size: 14px;
        }

        .form-group {
            margin-bottom: 20px;
        }

        .form-group label {
            display: block;
            margin-bottom: 7px;
            font-weight: bold;
        }

        .form-control {
            width: 100%;
            padding: 12px;
            border: 1px solid #ccc;
            border-radius: 5px;
            box-sizing: border-box;
        }

        .action-button {
            width: 100%;
            padding: 12px;
            background: #0078d4;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 16px;
        }

        .action-button:hover {
            background: #005a9e;
        }

        .action-button:disabled {
            background: #999;
            cursor: not-allowed;
        }

        .message {
            display: block;
            text-align: center;
            margin-top: 15px;
            line-height: 22px;
        }

        .back-link {
            text-align: center;
            margin-top: 20px;
        }

        .back-link a {
            color: #0078d4;
            text-decoration: none;
            font-weight: bold;
        }

        .back-link a:hover {
            text-decoration: underline;
        }

        .otp-section,
        .password-section {
            margin-top: 25px;
            padding-top: 20px;
            border-top: 1px solid #ddd;
        }

        .email-note {
            font-size: 13px;
            color: #666;
            margin-top: -8px;
            margin-bottom: 15px;
            line-height: 20px;
        }

        .success-message {
            color: green;
        }

        .error-message {
            color: red;
        }

        .password-requirements {
            margin-top: 8px;
            padding: 10px;
            background: #f5f8fb;
            border-radius: 5px;
            color: #666;
            font-size: 12px;
            line-height: 20px;
        }

    </style>

</head>

<body>

<form id="form1" runat="server">

    <div class="forgot-container">

        <!-- TITLE -->

        <div class="title">

            <h2>Forgot Password</h2>

            <p>
                Reset your Vendor Collaboration Portal password
            </p>

        </div>


        <!-- VENDOR ACCOUNT -->

        <div class="form-group">

            <asp:Label
                ID="lblVendorAccount"
                runat="server"
                Text="Vendor Account">
            </asp:Label>

            <asp:TextBox
                ID="txtVendorAccount"
                runat="server"
                CssClass="form-control"
                placeholder="Enter vendor account">
            </asp:TextBox>

        </div>


        <!-- PRIMARY EMAIL -->

        <div class="form-group">

            <asp:Label
                ID="lblEmail"
                runat="server"
                Text="Primary Email">
            </asp:Label>

            <asp:TextBox
                ID="txtEmail"
                runat="server"
                CssClass="form-control"
                TextMode="Email"
                placeholder="Enter primary email">
            </asp:TextBox>

        </div>


        <!-- SEND OTP -->

        <asp:Button
            ID="btnSendOTP"
            runat="server"
            Text="Send OTP"
            CssClass="action-button"
            OnClick="btnSendOTP_Click" />


        <!-- OTP SECTION -->

        <asp:Panel
            ID="pnlOTP"
            runat="server"
            CssClass="otp-section"
            Visible="false">

            <div class="form-group">

                <asp:Label
                    ID="lblOTP"
                    runat="server"
                    Text="Enter OTP">
                </asp:Label>

                <asp:TextBox
                    ID="txtOTP"
                    runat="server"
                    CssClass="form-control"
                    MaxLength="6"
                    placeholder="Enter 6 digit OTP">
                </asp:TextBox>

            </div>

            <div class="email-note">

                An OTP has been sent to your registered
                primary email address.

                <br />

                The OTP is valid for 10 minutes.

            </div>

            <asp:Button
                ID="btnVerifyOTP"
                runat="server"
                Text="Verify OTP"
                CssClass="action-button"
                OnClick="btnVerifyOTP_Click" />

        </asp:Panel>


        <!-- PASSWORD SECTION -->

        <asp:Panel
            ID="pnlPassword"
            runat="server"
            CssClass="password-section"
            Visible="false">

            <div class="form-group">

                <asp:Label
                    ID="lblNewPassword"
                    runat="server"
                    Text="New Password">
                </asp:Label>

                <asp:TextBox
                    ID="txtNewPassword"
                    runat="server"
                    CssClass="form-control"
                    TextMode="Password"
                    placeholder="Enter new password">
                </asp:TextBox>

                <div class="password-requirements">

                    Password must contain:

                    <br />
                    • At least 8 characters
                    <br />
                    • At least 1 uppercase character
                    <br />
                    • At least 1 lowercase character
                    <br />
                    • At least 1 symbol

                </div>

            </div>


            <div class="form-group">

                <asp:Label
                    ID="lblConfirmPassword"
                    runat="server"
                    Text="Confirm New Password">
                </asp:Label>

                <asp:TextBox
                    ID="txtConfirmPassword"
                    runat="server"
                    CssClass="form-control"
                    TextMode="Password"
                    placeholder="Re-enter new password">
                </asp:TextBox>

            </div>


            <asp:Button
                ID="btnResetPassword"
                runat="server"
                Text="Reset Password"
                CssClass="action-button"
                OnClick="btnResetPassword_Click" />

        </asp:Panel>


        <!-- MESSAGE -->

        <asp:Label
            ID="lblMessage"
            runat="server"
            CssClass="message">
        </asp:Label>


        <!-- BACK TO LOGIN -->

        <div class="back-link">

            <asp:HyperLink
                ID="lnkLogin"
                runat="server"
                NavigateUrl="Login.aspx">

                Back to Login

            </asp:HyperLink>

        </div>

    </div>

</form>

</body>

</html>