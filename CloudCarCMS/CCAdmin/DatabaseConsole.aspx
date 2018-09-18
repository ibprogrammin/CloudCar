<%@ Page Title="" ValidateRequest="False" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="DatabaseConsole.aspx.vb" Inherits="CloudCar.CCAdmin.DatabaseConsole" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

<h1 class="form-heading-style">
    Console
    <i class="icon-terminal"></i>
</h1>
<hr />

<asp:Label runat="server" ID="StatusLabel" CssClass="status-message" Visible="False" />

<h3>Submit Query (Select)</h3><br/>
<asp:TextBox runat="server" ID="QueryTextBox" TextMode="MultiLine" Rows="6" CssClass="form-text-box-full" />
<br class="clear-both" /><br/>

<asp:Button runat="server" ID="SubmitQueryButton" Text="Submit" CssClass="GreenButton" />
<br class="clear-both" /><br/><hr />

<h3>Submit Commands (Insert/Update/Delete)</h3><br/>

<asp:TextBox runat="server" ID="CommandTextBox" TextMode="MultiLine" Rows="8" CssClass="form-text-box-full" />
<br class="clear-both" /><br/>

<asp:Button runat="server" ID="SubmitCommandButton" Text="Submit" CssClass="GreenButton" />

<br class="clear-both" />

</asp:Content>