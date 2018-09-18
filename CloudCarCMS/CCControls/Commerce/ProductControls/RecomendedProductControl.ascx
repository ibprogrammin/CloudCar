<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="RecomendedProductControl.ascx.vb" Inherits="CloudCar.CCControls.Commerce.ProductControls.RecomendedProductControl" %>
<asp:Repeater runat="server" ID="RecomendedProductRepeater">
    <HeaderTemplate>
        <div class="recomended-products-control">
        <h5>Recomended Products</h5>
    </HeaderTemplate>
    <ItemTemplate>
        <div>
            <img src='<%# String.Format("/images/db/{0}/100/{1}.jpg", Eval("DefaultImageId"), Eval("DefaultImageId"))%>' alt='<%# Eval("Name")%>' />
            <br class="clear-both" />
            <label><a href='<%# String.Format("/Shop/{0}/{1}.html", Eval("Category.Permalink"), Eval("Permalink"))%>' title='<%# Eval("Name")%>'><%# Eval("Name")%></a></label><br />
            <span>$<%# Eval("Price")%></span>
        </div>
    </ItemTemplate>
    <FooterTemplate>
            <br class="clear-both" />
        </div>
    </FooterTemplate>
</asp:Repeater>