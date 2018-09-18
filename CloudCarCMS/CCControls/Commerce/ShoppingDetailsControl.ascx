<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ShoppingDetailsControl.ascx.vb" Inherits="CloudCar.CCControls.Commerce.ShoppingDetailsControl" %>
<div id="cart">
    <div class="heading">
        <a id="cart-total"><asp:Literal runat="server" id="litItems" /> item(s) - <asp:Literal runat="server" ID="litTotal" /></a><span></span>
    </div>
    <div class="content">
        <div class="empty" id="CartEmptyLabel" runat="server" visible="False">Your shopping cart is empty!</div>
        
        <div class="mini-cart-info" id="CartMiniItemsLabel" runat="server" visible="False">
            <asp:Repeater runat="server" ID="MiniCartItemsRepeater">
                <HeaderTemplate>
                    <table>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="image">
                            <a href="<%# String.Format("/Shop/{0}/{1}.html", Eval("CategoryPermalink"), Eval("Permalink"))%>">
                                <img src="<%# String.Format("/images/db/{0}/47/{1}.jpg", Eval("DefaultImageID"), Eval("Name"))%>" alt="Diamond Ring" title="Diamond Ring" />
                            </a>
                        </td>
                        <td class="name">
                            <a href="<%# String.Format("/Shop/{0}/{1}.html", Eval("CategoryPermalink"), Eval("Permalink"))%>">
                                <%# Eval("Name")%>
                            </a>
                            <div></div>
                        </td>
                        <td class="quantity">x&nbsp;<%# Eval("Quantity")%> <%# Eval("PricingUnit")%></td>
                        <td class="total"><%#Eval("Total", "${0:n2}")%></td>
                        <td class="remove">
                            <!--img src="/CCTemplates/Default/Images/remove-small.png" alt="Remove" title="Remove" onclick="(getURLVar('route') == 'checkout/cart' || getURLVar('route') == 'checkout/checkout') ? location = 'index.php?route=checkout/cart&remove=46::' : $('#cart').load('index.php?route=module/cart&remove=46::' + ' #cart > *');" /-->
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <div class="mini-cart-total" id="CartMiniTotalLabel" runat="server" visible="False">
            <table>
                <tr>
                    <td class="right"><b>Total:</b></td>
                    <td class="right"><asp:Literal runat="server" ID="TotalLiteral" /></td>
                </tr>
            </table>
        </div>
        <div class="checkout" id="CartMiniButtonsLabel" runat="server" visible="False">
            <a href="/Shop/ShoppingCart.html" class="button">View Cart</a>
            <!--a href="/Shop/Checkout.html" class="button">Checkout</a-->
        </div>
    </div>
</div>





		