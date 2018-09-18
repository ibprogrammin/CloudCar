<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SearchControl.ascx.vb" Inherits="CloudCar.CCControls.SearchControl" %>

<asp:Panel runat="server" DefaultButton="SearchButton" ClientIDMode="Static" ID="search">
    <asp:TextBox id="SearchTextBox" runat="server" TabIndex="2" name="search" placeholder="Search..." />
    <asp:LinkButton id="SearchButton" CssClass="button-search" Text="" runat="server" TabIndex="3" CausesValidation="False" />
</asp:Panel>

