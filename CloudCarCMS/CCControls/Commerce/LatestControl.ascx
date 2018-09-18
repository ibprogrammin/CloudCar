<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="LatestControl.ascx.vb" Inherits="CloudCar.CCControls.Commerce.LatestControl" %>

<asp:Label runat="server" ID="StatusMessage" CssClass="shopping-status-message" Visible="false" />

<asp:Repeater ID="SmallProductRepeater" runat="server" Visible="false">
    <ItemTemplate>
        <div>
            <div class="image">
                <a href="<%# String.Format("/Shop/{0}/{1}.html", Eval("Category.Permalink"), Eval("Permalink"))%>">
                    <img src="<%# String.Format("/images/db/{0}/60/{1}.jpg", Eval("DefaultImageId"), Eval("Name"))%>" alt="<%# Eval("Name")%>" />
                </a>
            </div>
            <div class="name">
                <a href="<%# String.Format("/Shop/{0}/{1}.html", Eval("Category.Permalink"), Eval("Permalink"))%>">
                    <%# Eval("Name")%>
                </a>
            </div>
            <div class="price">
                <%# If(Not Eval("ListPrice") Is Nothing, String.Format("<span class=""price-old"">{0}</span>", Eval("ListPrice")), "")%>
                <span class="price-new"><%# Eval("Price")%></span>
            </div>
            <div class="cart">
                <asp:Button value="Add to Cart" runat="server" onclick="AddToCartButtonClick" class="button" ProductId='<%# Eval("Id") %>' />
            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>

<asp:Repeater ID="LargeProductRepeater" runat="server" Visible="false">
    <HeaderTemplate>
        <section class="latest">
            <div class="box-heading"><h3>Latest</h3></div>
            <div id="home-latest-list">
	            <div id="latest" class="latest-normal clearfix">
    </HeaderTemplate>
    <ItemTemplate>
        <div class="latest-item">
			<div class="latest-image">
                <a href="<%# String.Format("/Shop/{0}/{1}.html", Eval("Category.Permalink"), Eval("Permalink"))%>">
                    <img src="<%# String.Format("/images/db/{0}/210/{1}.jpg", Eval("DefaultImageId"), Eval("Name"))%>" alt="<%# Eval("Name")%>" />
                </a>		
			</div>
			<div class="name">
				<a href="<%# String.Format("/Shop/{0}/{1}.html", Eval("Category.Permalink"), Eval("Permalink"))%>">
				    <%# Eval("Name")%>
				</a>
			</div>
            <div class="latest-description">
				<div class="price"><%# Eval("Price")%></div>
				<div class="cart">
				    <asp:Button ID="Button1" value="Add to Cart" runat="server" onclick="AddToCartButtonClick" class="button-latest" ProductId='<%# Eval("Id") %>' />
				</div>
			</div>
		</div>
    </ItemTemplate>
    <FooterTemplate>
                </div>
	            <div class="latest-navigation">
		            <a href="#" id="home-latest-prev" class="latest-prev">Prev</a>
		            <a href="#" id="home-latest-next" class="latest-next">Next</a>
	            </div>
	            <div id="home-latest-pagination"></div>
	            <script type="text/javascript">
	                jQuery("#latest").carouFredSel({
	                    circular: true,
	                    responsive: true,
	                    auto: false,
	                    items: {
	                        width: 310,
	                        visible: {
	                            min: 1,
	                            max: 4
	                        }
	                    },
	                    scroll: {
	                        pauseOnHover: true,
	                        duration: 1300,
	                        easing: "quadratic",
	                        wipe: true
	                    },
	                    prev: {
	                        button: "#home-latest-prev",
	                        key: "left"
	                    },
	                    next: {
	                        button: "#home-latest-next",
	                        key: "right"
	                    },
	                    pagination: "#home-latest-pagination"
	                });
	                </script>
            </div>
            <br style="clear: both;" />
        </section>
    </FooterTemplate>
</asp:Repeater>