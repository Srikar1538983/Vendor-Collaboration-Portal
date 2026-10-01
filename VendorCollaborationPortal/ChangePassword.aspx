<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="VendorCollaborationPortal.ChangePassword" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Change Password</title>
    <style>
        body {
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
            background: #f3f6f9;
        }
        .password-container {
            width: 400px;
            margin: 100px auto;
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
        .password-requirements {
            margin-top: 8px;
            padding: 10px;
            background: #f5f8fb;
            border-radius: 5px;
            color: #666;
            font-size: 12px;
            line-height: 20px;
        }
        .change-button {
            width: 100%;
            padding: 12px;
            background: #0078d4;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 16px;
        }
        .change-button:hover {
            background: #005a9e;
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
    </style>
</head>
<body>
<form id="form1" runat="server">
    <div class="password-container">
        <div class="title">
            <h2>Change Password</h2>
            <p>
                Update your vendor account password
            </p>
        </div>
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
                placeholder="Enter Vendor Account">
            </asp:TextBox>

        </div>

        <div class="form-group">

            <asp:Label
                ID="lblCurrentPassword"
                runat="server"
                Text="Current Password">
            </asp:Label>

            <asp:TextBox
                ID="txtCurrentPassword"
                runat="server"
                CssClass="form-control"
                TextMode="Password"
                placeholder="Enter current password">
            </asp:TextBox>

        </div>

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
                <strong>Password must contain:</strong>
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
                Text="Re-enter New Password">
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
            ID="btnChangePassword"
            runat="server"
            Text="Change Password"
            CssClass="change-button"
            OnClick="btnChangePassword_Click" />

        <asp:Label
            ID="lblMessage"
            runat="server"
            CssClass="message">
        </asp:Label>

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