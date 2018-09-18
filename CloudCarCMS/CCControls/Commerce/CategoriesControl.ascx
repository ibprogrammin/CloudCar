<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CategoriesControl.ascx.vb" Inherits="CloudCar.CCControls.Commerce.CategoriesControl" %>
<%@ Import Namespace="CloudCar.CCFramework.Commerce" %>

<asp:Repeater ID="CategoryRepeater" runat="server">
    <HeaderTemplate>
        <div class="box">
            <div class="box-heading"><h3>Categories</h3></div>
            <div class="box-content">
                <div class="box-category">
                    <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li><a href="<%# String.Format("/Shop/{0}.html", Eval("Permalink")) %>"><%# Eval("Name") %></a></li>
    </ItemTemplate>
    <FooterTemplate>
                    </ul>
                </div>
            </div>
        </div>
        <!--div class="hidden-links"></div-->
        
        <script type="text/javascript">
            $(function () {
                var activeCat = ".box-category ul li a.active";
                if ($(activeCat).length > 0) {
                    $(activeCat).parent("li").find("ul").show(500);
                    $(activeCat).parent("li").find("a.toggle").toggleClass("subhead");
                }
                $(".ja-sidenav a").click(function () {
                    $(".ja-sidenav a").removeClass("subhead");
                    var ul = $(this).parent("div").parent("li").find("ul");
                    if (!ul.is(":visible")) {
                        $(".box-category ul li > ul").slideUp(500);
                        $(this).toggleClass("subhead");
                    } else {
                        $(this).removeClass("subhead");
                    }
                    ul.stop(true, true).slideToggle(500);
                });
            });
        </script>
    </FooterTemplate>
</asp:Repeater>

<asp:Repeater ID="CategoryImageRepeater" runat="server">
    <ItemTemplate>
        <a href='<%# String.Format("/Shop/{0}.html", Eval("Permalink")) %>' runat="server" Visible='<%# CategoryController.GetRandomCategoryImage(CInt(Eval("ID"))) <> 0 %>'>
            <img src="<%# String.Format("/images/db/{0}/140/{1}.jpg", CategoryController.GetRandomCategoryImage(CInt(Eval("ID"))), Eval("permalink")) %>" alt="" />
        </a>
    </ItemTemplate>
</asp:Repeater>