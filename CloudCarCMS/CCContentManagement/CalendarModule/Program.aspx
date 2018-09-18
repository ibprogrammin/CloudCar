<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="Program.aspx.vb" Inherits="CloudCar.CCContentManagement.CalendarModule.Program" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    <meta runat="server" id="ProgramKeywords" name="keywords" content="" />
    <meta runat="server" id="ProgramDescription" name="description" content="" />
    
    <link rel="canonical" href="" runat="server" id="ProgramCanonical" />
</asp:Content>

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server"/>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
    
    <div class="TwoEightyFourTenWrapper float-right"><asp:Image runat="server" id="ProgramIcon" visible="false" /></div>
    
    <h1><asp:Literal runat="server" ID="ProgramHeading" /></h1>
    
    <asp:Literal runat="server" ID="ProgramContent" />
    
</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server">
    
    <div class="bread-crumb-holder"><asp:Literal runat="server" ID="BreadCrumbContainer" /></div>

</asp:Content>
<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server"/>