<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="QuickShopControl.ascx.vb" Inherits="CloudCar.CCControls.Commerce.QuickShopControl" %>
<%@ Import Namespace="CloudCar.CCFramework.Core" %>

<asp:Repeater runat="server" ID="BrandsRepeater">
    <HeaderTemplate>
        <nav>
    </HeaderTemplate>
    <ItemTemplate>
        <a href="/Shop/Brand/<%# Eval("Permalink") %>.html"><%#Eval("Name")%></a>
    </ItemTemplate>
    <FooterTemplate>
            <a href="<%# String.Format("/Shop/{0}.html", Settings.TopSellersPage)%>">Top Sellers</a>
            <a href="<%# String.Format("/Shop/{0}.html", Settings.ClearancePage)%>">Amazing Deals</a>
            <!-- &nbsp;&nbsp;-&nbsp;&nbsp; <a href="#">Special Offers</a> -->
        </nav>
    </FooterTemplate>
</asp:Repeater>
