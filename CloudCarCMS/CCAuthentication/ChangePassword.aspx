<%@ Page Title="Change Your Password" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="ChangePassword.aspx.vb" Inherits="CloudCar.CCAuthentication.ChangePassword" %>

<asp:Content ContentPlaceHolderID="head" runat="server">

<style type="text/css">
    label { width: 240px; display: block; float: left; }
    input { width: 290px;}
    select { width: 302px; height: 35px; }
    .error-display { width: 520px; }
</style>

</asp:Content>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">
    
<h1 class="form-heading-style">
    <asp:Literal runat="server" Text="<%$ Resources:AuthenticationResources, ChangePasswordTitle %>" />
    <i class="icon-lock"></i>
</h1><hr/>

<p><asp:Literal runat="server" Text="<%$ Resources:AuthenticationResources, ChangePasswordHelpMessage %>" /></p><br />

<asp:HiddenField ID="UserNameTextBox" runat="server" />

<asp:ChangePassword runat="server" ID="cpPassword" ChangePasswordTitleText="" DisplayUserName="false">
    <ChangePasswordTemplate>    
        <asp:Label runat="server" ID="FailureText" CssClass="error-display" />
        
        <label><asp:Literal runat="server" Text="<%$ Resources:AuthenticationResources, OldPassword %>" /></label>
        <asp:TextBox id="CurrentPassword" runat="server" TextMode="Password" ValidationGroup="CP" />
        <br style="clear: both;" /><br/>
        
        <label><asp:Literal runat="server" Text="<%$ Resources:AuthenticationResources, NewPassword %>" /></label>
        <asp:TextBox id="NewPassword" runat="server" TextMode="Password" ValidationGroup="CP" CssClass="check-password" />
        <br style="clear: both;" /><br/>
        
        <label><asp:Literal runat="server" Text="<%$ Resources:AuthenticationResources, ConfirmNewPassword %>" /></label>
        <asp:TextBox id="ConfirmNewPassword" runat="server" TextMode="Password" ValidationGroup="CP" />
        <br style="clear: both;" /><br/>
        
        <asp:Button id="ChangePassword" Text="<%$ Resources:AuthenticationResources, Save %>" UseSubmitBehavior="true" CommandName="ChangePassword" runat="server" CausesValidation="true" ValidationGroup="CP" CssClass="SaveButton" style="margin-left: 250px; width: 310px;" />
        <br /><br />
        
    </ChangePasswordTemplate>
    <SuccessTemplate>
    
        <h4><asp:Literal runat="server" Text="<%$ Resources:AuthenticationResources, ChangePasswordSuccessHeading %>" /></h4>
    
        <p><asp:Literal runat="server" Text="<%$ Resources:AuthenticationResources, ChangePasswordSuccessMessage %>" /></p><br /><br />
    
        <asp:Label runat="server" CssClass="status-message" Text="<%$ Resources:AuthenticationResources, ChangePasswordMessage2 %>" />

    </SuccessTemplate>
</asp:ChangePassword>

<br class="clear-both" /><br />

</asp:Content>

<asp:Content ContentPlaceHolderID="cphAfterContent" runat="server">
    <div class="bread-crumb-holder"><a href="/" title="Home">Home</a> <i class="icon-caret-right"></i> Change Password</div>
</asp:Content>
<asp:Content ContentPlaceHolderID="cphScripts" runat="server" />
