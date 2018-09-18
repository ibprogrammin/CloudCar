<%@ Page Title="Reset Your Password" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="ResetPassword.aspx.vb" Inherits="CloudCar.CCAuthentication.ResetPassword" %>

<asp:Content ContentPlaceHolderID="head" runat="server">

<style type="text/css">
    label { width: 140px; display: block; float: left; }
    input { width: 290px;}
    select { width: 302px; height: 35px; }
</style>

</asp:Content>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Reset your password
        <i class="icon-rotate-right"></i>
    </h1><hr />
    

    <p>Fill out the below form to reset your password. You will recieve your new passowrd through the email address you used to sign up.</p><br />
  
    <asp:Label ID="lblStatus" runat="server" Text="" CssClass="status-message" Visible="false" />

    <asp:PlaceHolder runat="server" ID="phUser" Visible="true">
        
        <label>Username</label>
        <asp:TextBox id="txtUserName" runat="server" />
        <br class="clear-both" /><br />
        
        <label>Email</label>
        <asp:TextBox id="txtEmail" runat="server" />
        <br class="clear-both" /><br />
        
        <asp:Button runat="server" ID="btnSubmit" CssClass="SaveButton" Text="Submit" style="margin-left: 150px; width: 312px;" /><br />
        
    </asp:PlaceHolder>

    <asp:PlaceHolder runat="server" ID="phQuestion" Visible="false">
        
        <p>Please answer the required question to retrieve <asp:Literal runat="server" ID="litUser" />'s password.</p><br />
        
        <h2><asp:Literal ID="litQuestion" runat="server" /></h2>
        
        <asp:TextBox ID="txtPwdAnswer" runat="server" />
        <br class="clear-both" /><br />
        
        <ajax:TextBoxWatermarkExtender runat="server" ID="tbwPwdAnswer" TargetControlID="txtPwdAnswer" WatermarkText="Answer" WatermarkCssClass="Watermark" />
        
        <asp:Button runat="server" ID="btnResetPassword" CssClass="SaveButton" Text="Reset" style="margin-left: 150px; width: 312px;" /><br />
        
    </asp:PlaceHolder>
     

    <br class="clear-both" /><br /><br />
    <ul style="margin-left: 20px;">
        <li>Not yet registered with us? Go ahead, <a href="Register.html" title="Create an account with us" class="CalendarMyClassLink">create an account!</a></li>
    </ul>
    <br class="clear-both" /><br />

</asp:Content>

<asp:Content ContentPlaceHolderID="cphAfterContent" runat="server">
    <div class="bread-crumb-holder"><a href="/" title="Home">Home</a> &raquo Reset Password</div>
</asp:Content>