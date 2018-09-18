<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="TransferUsers.aspx.vb" Inherits="CloudCar.TransferUsers" %>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">

<div style="font-size: 11px; font-family: arial">

    <p>
        <asp:Button ID="btnReadXL" runat="server" Text="Read Excel File" CssClass="GreenButton" /><br />
        Issue User Access: <asp:CheckBox runat="server" ID="ckbIssueMS" Checked="false" /><br />
        <asp:Label ID="lblFilePath" runat="server" /><br /><br />
    </p>
    
    <asp:Label ID="lblData" runat="server" CssClass="status-message" Visible="false" /><br />
    
    <asp:DataGrid ID="dgExcelData" runat="server" AutoGenerateColumns="true" GridLines="None" BorderWidth="0" CellPadding="4">
        <HeaderStyle Font-Bold="true" VerticalAlign="Top" />
        <AlternatingItemStyle BackColor="#EEEEEE" VerticalAlign="Top" />
        <ItemStyle VerticalAlign="Top" />
    </asp:DataGrid>
    
</div>

</asp:Content>