<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="Unsubscribe.aspx.vb" Inherits="CloudCar.CCContentManagement.Unsuscribe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">
    <div>
        <span>If you wish to unsubscribe from the mailinglist enter your email below to be removed</span>
        <asp:TextBox ID="txbUnsuscribe" runat="server" Height="28px" Width="262px" Enabled="false"></asp:TextBox>
        <asp:LinkButton ID="btnUnsuscribe" runat="server">Unsuscribe</asp:LinkButton>
        <asp:Label ID="lbmsg" runat="server" Text="Label"></asp:Label>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
</asp:Content>
