<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="BrandCategoriesControl.ascx.vb" Inherits="CloudCar.CCControls.Commerce.ProductControls.BrandCategoriesControl" %>

<asp:Repeater runat="server" ID="rptCategories">
    <ItemTemplate>
        <li><a href="/Product/<%# Eval("Permalink") %>.html"><%#Eval("Name")%></a></li>
    </ItemTemplate>
</asp:Repeater>