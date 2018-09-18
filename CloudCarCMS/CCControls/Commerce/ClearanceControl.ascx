<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ClearanceControl.ascx.vb" Inherits="CloudCar.CCControls.Commerce.ClearanceControl" %>

<asp:Label runat="server" ID="StatusMessage" CssClass="shopping-status-message" Visible="false" />

<asp:Repeater ID="SmallProductRepeater" runat="server" Visible="false">
    <HeaderTemplate>
        <div class="box">
            <div class="box-heading"><h3>Specials</h3></div>
            <div class="box-content">
                <div class="box-product">
    </HeaderTemplate>
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
    <FooterTemplate>
                </div>
            </div>
        </div>
    </FooterTemplate>
</asp:Repeater>

<asp:Repeater ID="LargeProductRepeater" runat="server" Visible="false">
    <HeaderTemplate>
        <section class="featured">
            <div class="box-heading"><h3>Featured</h3></div>
            <div id="home-featured-list">
	            <div id="featured" class="featured-normal clearfix">
    </HeaderTemplate>
    <ItemTemplate>
        <div class="featured-item">
			<div class="featured-image">
                <a href="<%# String.Format("/Shop/{0}/{1}.html", Eval("Category.Permalink"), Eval("Permalink"))%>">
                    <img src="<%# String.Format("/images/db/{0}/210/{1}.jpg", Eval("DefaultImageId"), Eval("Name"))%>" alt="<%# Eval("Name")%>" />
                </a>		
			</div>
			<div class="name">
				<a href="<%# String.Format("/Shop/{0}/{1}.html", Eval("Category.Permalink"), Eval("Permalink"))%>">
				    <%# Eval("Name")%>
				</a>
			</div>
            <div class="featured-description">
				<div class="price"><%# Eval("Price")%></div>
				<div class="cart">
					<asp:Button value="Add to Cart" runat="server" onclick="AddToCartButtonClick" class="button-featured" ProductId='<%# Eval("Id") %>' />
				</div>
			</div>
		</div>
    </ItemTemplate>
    <FooterTemplate>
                </div>
	            <div class="featured-navigation">
		            <a href="#" id="home-featured-prev" class="featured-prev">Prev</a>
		            <a href="#" id="home-featured-next" class="featured-next">Next</a>
	            </div>
	            <div id="home-featured-pagination"></div>
	            <script type="text/javascript">
	                jQuery("#featured").carouFredSel({
	                    circular: true,
	                    responsive: true,
	                    auto: { pauseDuration: 3500 },
	                    items: {
	                        width: 300,
	                        visible: {
	                            min: 1,
	                            max: 4
	                        }
	                    },
	                    scroll: {
	                        pauseOnHover: true,
	                        duration: 1800,
	                        easing: "quadratic",
	                        wipe: true
	                    },
	                    prev: {
	                        button: "#home-featured-prev",
	                        key: "left"
	                    },
	                    next: {
	                        button: "#home-featured-next",
	                        key: "right"
	                    },
	                    pagination: "#home-featured-pagination"
	                });
	            </script>
            </div>
            <br style="clear: both;" />
        </section>
    </FooterTemplate>
</asp:Repeater>