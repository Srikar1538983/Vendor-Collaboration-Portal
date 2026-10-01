<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs"
    Inherits="VendorCollaborationPortal.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>Vendor Portal - Login</title>

    <style>

        body {
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
            background: #f3f6f9;
        }

        .login-container {
            width: 400px;
            margin: 100px auto;
            background: white;
            padding: 35px;
            border-radius: 10px;
            box-shadow: 0 4px 15px rgba(0,0,0,0.15);
        }

        .login-title {
            text-align: center;
            margin-bottom: 30px;
        }

        .login-title h2 {
            margin-bottom: 5px;
        }

        .login-title p {
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
            margin-bottom: 15px;
        }

        .login-button {
            width: 100%;
            padding: 12px;
            background: #0078d4;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 16px;
        }

        .login-button:hover {
            background: #005a9e;
        }

        .error-message {
            display: block;
            margin-top: 15px;
            color: red;
            text-align: center;
        }

        .register-link {
            text-align: center;
            margin-top: 20px;
            font-size: 14px;
        }

        .register-link a {
            color: #0078d4;
            text-decoration: none;
            font-weight: bold;
        }

        .password-links {
            display: flex;
            justify-content: space-between;
            margin-top: 15px;
            margin-bottom: 10px;
        }

        .password-links a {
            color: #0078d4;
            text-decoration: none;
            font-size: 14px;
            font-weight: bold;
        }

        .password-links a:hover {
            text-decoration: underline;
        }

    </style>

</head>

<body>

<form id="form1" runat="server">

    <div class="login-container">

        <div class="login-title">

            <h2>Vendor Collaboration Portal</h2>

            <p>Sign in to continue</p>

        </div>

        <!-- Username -->

        <div class="form-group">

            <asp:Label
                ID="lblUsername"
                runat="server"
                Text="Username">
            </asp:Label>

            <asp:TextBox
                ID="txtUsername"
                runat="server"
                CssClass="form-control"
                placeholder="Enter Username">
            </asp:TextBox>

        </div>

        <!-- Password -->

        <div class="form-group">

            <asp:Label
                ID="lblPassword"
                runat="server"
                Text="Password">
            </asp:Label>

            <asp:TextBox
                ID="txtPassword"
                runat="server"
                CssClass="form-control"
                TextMode="Password"
                placeholder="Enter Password">
            </asp:TextBox>

        </div>

        <!-- Login Button -->

        <asp:Button
            ID="btnLogin"
            runat="server"
            Text="Login"
            CssClass="login-button"
            OnClick="btnLogin_Click" />

        <!-- Password Links -->

        <div class="password-links">

            <asp:HyperLink
                ID="lnkForgotPassword"
                runat="server"
                NavigateUrl="ForgotPassword.aspx">
                Forgot Password?
            </asp:HyperLink>

            <asp:HyperLink
                ID="lnkChangePassword"
                runat="server"
                NavigateUrl="ChangePassword.aspx">
                Change Password
            </asp:HyperLink>

        </div>

        <!-- Message -->

        <asp:Label
            ID="lblMessage"
            runat="server"
            CssClass="error-message">
        </asp:Label>

        <!-- Register -->

        <div class="register-link">

            Don't have an account?

            <asp:HyperLink
                ID="HyperLink1"
                runat="server"
                NavigateUrl="Register.aspx">

                Register Here

            </asp:HyperLink>

        </div>

    </div>

</form>

</body>

</html>