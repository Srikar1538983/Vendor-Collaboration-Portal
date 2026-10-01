<%@ Page Language="C#"
    Async="true"
    AsyncTimeout="300"
    AutoEventWireup="true"
    CodeBehind="Register.aspx.cs"
    Inherits="VendorCollaborationPortal.Register" %><!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>Vendor Registration</title>

    <style>

        body {
            font-family: Arial;
            background: #f3f6f9;
            margin: 0;
        }

        .register-container {
            width: 600px;
            margin: 40px auto;
            background: white;
            padding: 35px;
            border-radius: 10px;
            box-shadow: 0 4px 15px rgba(0,0,0,.15);
        }

        h2 {
            text-align: center;
        }

        .form-group {
            margin-bottom: 15px;
        }

        label {
            display: block;
            font-weight: bold;
            margin-bottom: 5px;
        }

        .form-control {
            width: 100%;
            padding: 10px;
            box-sizing: border-box;
            border: 1px solid #ccc;
            border-radius: 5px;
        }

        .register-button {
            width: 100%;
            padding: 12px;
            background: #0078d4;
            color: white;
            border: none;
            border-radius: 5px;
            font-size: 16px;
            cursor: pointer;
        }

        .message {
            display: block;
            text-align: center;
            margin-top: 15px;
        }

    </style>

</head>

<body>

<form id="form2" runat="server">

    <div class="register-container">

        <h2>Vendor Registration</h2>

        <div class="form-group">
            <label for="ddlVendorType">Vendor Type</label>

            <asp:DropDownList
                ID="ddlVendorType"
                runat="server"
                CssClass="form-control">

                <asp:ListItem Text="-- Select Type --" Value="" />
                <asp:ListItem Text="Organization" Value="Organization" />
                <asp:ListItem Text="Person" Value="Person" />

            </asp:DropDownList>
        </div>

        <div class="form-group">

            <asp:Label
                ID="lblCompanyName"
                runat="server"
                Text="Vendor Name">
            </asp:Label>

            <asp:TextBox
                ID="txtCompanyName"
                runat="server"
                CssClass="form-control">
            </asp:TextBox>

        </div>

       <%-- <div class="form-group">
            <asp:Label
                ID="lblCustomerGroup"
                runat="server"
                Text="Customer Group">
            </asp:Label>

            <asp:TextBox
                ID="txtContactGroup"
                runat="server"
                CssClass="form-control">
            </asp:TextBox>

        </div>--%>

        <div class="form-group">

            <asp:Label
                ID="lblEmail"
                runat="server"
                Text="Email">
            </asp:Label>

            <asp:TextBox
                ID="txtEmail"
                runat="server"
                CssClass="form-control">
            </asp:TextBox>

        </div>

        <div class="form-group">

            <asp:Label
                ID="lblPhone"
                runat="server"
                Text="Phone">
            </asp:Label>

            <asp:TextBox
                ID="txtPhone"
                runat="server"
                CssClass="form-control">
            </asp:TextBox>

        </div>

        <div class="form-group">

            <asp:Label
                ID="lblUsername"
                runat="server"
                Text="Username">
            </asp:Label>

            <asp:TextBox
                ID="txtUsername"
                runat="server"
                CssClass="form-control">
            </asp:TextBox>

        </div>

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
                TextMode="Password">
            </asp:TextBox>

        </div>

        <div class="form-group">

            <asp:Label
                ID="lblConfirmPassword"
                runat="server"
                Text="Confirm Password">
            </asp:Label>

            <asp:TextBox
                ID="txtConfirmPassword"
                runat="server"
                CssClass="form-control"
                TextMode="Password">
            </asp:TextBox>

        </div>

        <asp:Button
            ID="btnRegister"
            runat="server"
            Text="Register"
            CssClass="register-button"
            OnClick="btnRegister_Click" />

        <asp:Label
            ID="lblMessage"
            runat="server"
            CssClass="message">
        </asp:Label>

    </div>

</form>

</body>
</html>