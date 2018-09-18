<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="PropertyResults.aspx.vb" Inherits="CloudCar.CCContentManagement.PropertyModule.PropertyResults" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">

    <asp:Literal runat="server" ID="litResults" />

    <asp:Repeater runat="server" ID="rptPropertyResults">
        <ItemTemplate>
            <img src="/image/db/<%# Eval("ImageId") %>/60/<%# Eval("Permalink") %>.jpg" alt="<%# Eval("Title") %>" />
            <a href="/Property/<%# Eval("Permalink") %>.html"><%# Eval("Title") %></a>
            <p><%# Eval("Address") %></p>
            <p>/Property/<%# Eval("Permalink") %>.html</p>
        </ItemTemplate>
    </asp:Repeater>

</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server"></asp:Content>