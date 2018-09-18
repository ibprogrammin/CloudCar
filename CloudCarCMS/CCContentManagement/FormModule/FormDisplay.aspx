<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="FormDisplay.aspx.vb" Inherits="CloudCar.CCContentManagement.FormModule.FormDisplay" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server" />

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">
    
    <asp:HiddenField runat="server" ID="FormIdHiddenField" />
    
    <h1><asp:Literal runat="server" ID="FormNameLiteral" /></h1>
    <hr />

    <asp:Literal runat="server" ID="FormDetailsLiteral"/>
    
    <fieldset>

        <asp:PlaceHolder runat="server" ID="FormFieldsPlaceHolder" />
        <asp:Button runat="server" ID="FormSubmitButton" Text="Submit" CssClass="form-button" CausesValidation="True" ValidationGroup="CurrentForm" />

    </fieldset>

</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">    
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <CMS:CloudCarScriptManagementControl runat="server" id="PageScriptManagementControl" />
</asp:Content>