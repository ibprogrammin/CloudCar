<%@ Page Title="Submit your grant application | Unfiltered Facts" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="GrantsForm.aspx.vb" Inherits="CloudCar.CCContentManagement.ClientForms.GrantsForm" %>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
    
    <h1 class="PathHeader">Grant Applications</h1>
    
    <div class="LeftWrapper RoundCorners" style="width: 790px; margin-bottom: 60px;">
    
    <fieldset>
    
        <p>You can download the grant applications and instructions below, fill them out and submit them to us either by paper or using the form below, before <b>Thursday, May 31st, 2012</b>:</p>
        <ul>
            <li><a href="/Files/Uploads/HSG Report Planning form 2011-2012 Updated.doc">Health Action Grant Instructions and Application</a></li>
            <li><a href="/Files/Uploads/TTC Jr report_2012.doc">Teen Tobacco Challenge Instructions and Application</a></li>
        </ul><br/><br/>
        
        <label>Name*</label>
        <asp:TextBox runat="server" ID="NameTextBox"/><br/>
        
        <label>Email*</label>
        <asp:TextBox runat="server" ID="EmailTextBox" /><br/>
        
        <label>Details</label>
        <asp:TextBox runat="server" ID="DetailsTextBox" TextMode="MultiLine" /><br/>
        
        <label>Grant Application*</label>
        <asp:FileUpload runat="server" ID="GrantApplicationFileUpload" CssClass="fileupload"/><br />
        
        <asp:Button runat="server" ID="SubmitButton" Text="Submit" CssClass="SubmitButton RoundCorners" ValidationGroup="Grant" />
        
    </fieldset>
    
    </div>
    
</asp:Content>