<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="BrandControl.ascx.vb" Inherits="CloudCar.CCControls.Commerce.BrandControl" %>

<asp:Repeater runat="server" ID="BrandRepeater">
    <HeaderTemplate>
        <div id="home-carousel-list">
            <div id="carousel0" class="carousel-normal clearfix">
    </HeaderTemplate>
    <ItemTemplate>
        <div class="carousel-item">
			<div class="carousel-image">
				<a href="<%# String.Format("/Shop/Brand/{0}.html", Eval("Permalink")) %>" title="<%# Eval("Name")%>">
				    <img src="<%# String.Format("/images/db/{0}/80/{1}.png", Eval("LogoImageID"), Eval("permalink"))%>" alt="<%# Eval("Name")%>" title="<%# Eval("Name")%>" />
				</a>
			</div>
		</div>
    </ItemTemplate>
    <FooterTemplate>
        </div>
        <div class="carousel-navigation">
		    <a href="#" id="home-carousel-prev" class="carousel-prev">Prev</a>
		    <a href="#" id="home-carousel-next" class="carousel-next">Next</a>
	    </div>
        <script type="text/javascript">
            jQuery("#carousel0").carouFredSel({
                circular: true,
                responsive: true,
                auto: false,
                items: {
                    width: 150,
                    visible: {
                        min: 1,
                        max: 5
                    }
                },
                scroll: {
                    duration: 900,
                    easing: "quadratic",
                    wipe: true
                },
                prev: {
                    button: "#home-carousel-prev",
                    key: "left"
                },
                next: {
                    button: "#home-carousel-next",
                    key: "right"
                }
            });
        </script>
    </div>
    </FooterTemplate>
</asp:Repeater>

	

	
	
    

