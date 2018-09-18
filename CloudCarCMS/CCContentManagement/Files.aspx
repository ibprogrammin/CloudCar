<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="Files.aspx.vb" Inherits="CloudCar.CCContentManagement.Files" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">

<asp:Repeater runat="server" ID="rptFiles">
    <HeaderTemplate>
        <h2>Downloads to help you</h2>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
            <li><a href='<%# Eval("Path") & Eval("Filename") %>' title='<%#Eval("Title")%>'><%#Eval("Title")%></a> - <%#Eval("Description")%></li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>

</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server"></asp:Content>