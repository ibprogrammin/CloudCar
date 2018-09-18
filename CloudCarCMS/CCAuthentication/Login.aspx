<%@ Page Title="Login" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Login.aspx.vb" Inherits="CloudCar.CCAuthentication.Login" MaintainScrollPositionOnPostback="true" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        label { width: 250px; display: block;float: left; }
        .checkbox-label { margin-top: -10px; display: block; float: left; }
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1  class="form-heading-style">
        Login
        <i class="icon-key"></i>
    </h1><hr />

    <p>Thank you for coming back! Please enter your login  to continue.</p><br />

    <asp:ValidationSummary id="vsValidation" runat="server" displaymode="BulletList" ValidationGroup="Login" CssClass="error-display" 
        HeaderText="<b>Oh no! You broke it! hehehe... Just Kidding!</b><br />" />

    <asp:Label runat="server" ID="lblError" CssClass="error-display" Visible="false" />
    
    <label>User Name</label>
    <asp:TextBox id="Username" runat="server" value="" ValidationGroup="Login" TabIndex="4" />
    <br style="clear: both;" /><br />
    
    <label>Password</label>
    <asp:TextBox id="Password" runat="server" ClientIDMode="Static" value="" ValidationGroup="Login" TextMode="Password" TabIndex="5" />
    <br style="clear: both;" /><br />

    <asp:CheckBox ID="RememberMe" runat="server" Checked="false" TabIndex="6" style="margin-left: 260px; display: block; float: left;" />
    <label class="checkbox-label">&nbsp;Keep me logged in!</label>

    <br class="clear-both" /><br />

    <asp:Button id="btnLogin" runat="server" CausesValidation="true" ValidationGroup="Login" CssClass="SaveButton" Text="Login" TabIndex="7" style="margin-left: 260px; width: 244px;" />

    <asp:requiredfieldvalidator id="rfvUsername" runat="server" controltovalidate="Username" errormessage="You forgot to enter your username silly!" SetFocusOnError="true" display="None" ValidationGroup="Login" />
    <asp:requiredfieldvalidator id="rfvPassword" runat="server" controltovalidate="Password" errormessage="You forgot to enter your password silly!" SetFocusOnError="true" display="None" ValidationGroup="Login" />

    
    <br class="clear-both" /><br />

    <ul style="margin-left: 20px;">
        <li><a href="Reset-Password.html" class="CalendarMyClassLink" tabindex="8">Forgot your password?</a></li>
        <li>Not yet registered with us? Go ahead, <a href="Register.html" class="CalendarMyClassLink" tabindex="9">create an account!</a></li>
    </ul>

    <br class="clear-both" /><br />

</asp:Content>

<asp:Content ContentPlaceHolderID="cphAfterContent" runat="server">
    <div class="bread-crumb-holder"><a href="/" title="Home">Home</a> <i class="icon-caret-right"></i> Login</div>
</asp:Content>