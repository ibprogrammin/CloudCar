<%@ Page Title="Administration Dashboard" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="OldDashboard.aspx.vb" Inherits="CloudCar.CCAdmin.AdminDefault" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
    <!--link rel="stylesheet" type="text/css" href="/styles/jqplot/jquery.jqplot.min.css" /-->
    <!--link type="text/css" href="/styles/admin/jquery-ui-1.8.13.custom.css" rel="stylesheet" /-->
</asp:Content>

<asp:Content ContentPlaceHolderID="cphBeforeContent" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="cphAfterContent" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">
    
    <h1 class="form-heading-style">
        Dashboard
        <img src="/CCTemplates/Admin/Images/icons/cc.dashboard.icon.dark.png" alt="Dashboard" />
    </h1>
    
    <hr />
    
    <div class="tab-container">
	    <ul class="tabs">
		    <li class="tab"><a href="#tab-visitor-overview">Visitor Overview</a></li>
		    <li class="tab"><a href="#tab-browser-usage">Browser Usage</a></li>
            <li class="tab"><a href="#tab-search-engines">Search Engines</a></li>
            <li class="tab"><a href="#tab-regions">Regions</a></li>
            <li class="tab"><a href="#tab-pageviews">Page Views</a></li>
	    </ul>
        <div id="tab-visitor-overview">
            <div id='visitor-overview'></div>
        </div>
        <div id="tab-browser-usage">
            <div id='browser-usage' class="float-left"></div>
            <div id='screen-resolution' class="float-left"></div>
            <br class="clear-both" />
        </div>
        <div id="tab-search-engines">
            <div id='search-engines' class="float-left"></div>
            <div id='keywords-overview' class="float-left"></div>
            <br class="clear-both" />
        </div>
        <div id="tab-regions">
            <div id='region-country' class="float-left"></div>
            <div id='region-city' class="float-left"></div>
            <br class="clear-both" />
        </div>
        <div id="tab-pageviews">
            <div id='page-views'></div>
        </div>
    </div>
    
    <a id="A1" href="" style="float: right; margin-right: 30px; margin-top: 0px;" runat="server" visible="false"><img src="/images/round.help.icon.png" alt="" width="20" height="20" /></a>
    
    <p>
        Visits: <span id='visits'></span> |
        Bounce Rate: <span id='bounces'></span> |
        Page Views: <span id='pageviews'></span> | 
        <a href="http://google.com/analytics" title="Access Google Analytics to view your site statistics.">Go To Google Analytics</a>

        <!--a href="http://constantcontact.com" title="Access Constant Contact to view your mailing list subscribers.">Constant Contact</a><br /-->
        <span style="float: right; margin-right: 20px;">
            <asp:LinkButton runat="server" ID="btnRestartApplication" Text="Restart Website" OnClientClick="return confirm('Are you sure you wish to restart the application? Any Session information for currently online users will be lost.');" /> | 
            Users Online: <b><asp:Literal runat="server" ID="litTotalUsers" /></b> | Registered Users: <b><asp:Literal runat="server" ID="litLoggedInUsers" /></b> | 
            Total Users: <b><asp:Literal runat="server" ID="litRegisteredUsers" /></b>
        </span>
    </p>
    
    <div class="dashboard-box">
        <!--div class="user-icon"></div-->
        <!--img src="/CCTemplates/Admin/Images/icons/user.icon.png" alt="Add, Modify and Delete User Profiles" width="75" height="75" class="DashboardIcon" /-->
        <a href="/CCAdmin/Users.aspx" class="account-link">Users</a>
        <p>Manage users, view user details, modify users, delete users.</p>
    </div>
    <div class="dashboard-box">
        <!--div class="settings-icon"></div-->
        <!--img src="/CCTemplates/Admin/Images/icons/settings.icon.png" alt="Web Site Settings" width="75" height="75" class="DashboardIcon" /-->
        <a href="/CCAdmin/Settings.aspx" class="settings-link">Settings</a>
        <p>Modify web site settings</p>
    </div>
    <div class="dashboard-box">
        <!--div class="maintenance-icon"></div-->
        <!--img src="/CCTemplates/Admin/Images/icons/settings.icon.png" alt="Web Site Maintenance" width="75" height="75" class="DashboardIcon" /-->
        <a href="/CCAdmin/Maintenance.aspx" class="maintenance-link">Maintenance</a>
        <p>Perform web site maintenance</p>
    </div>
    <div class="dashboard-box">
        <!--div class="help-icon"></div-->
        <!--img src="/CCTemplates/Admin/Images/icons/help.icon.png" alt="Download the latest help documentation" width="75" height="75" class="DashboardIcon" /-->
        <a href="/CCAdmin/Help/HelpDocument.docx" class="help-link">Help</a>
        <p>Download the latest help documentation.</p>
    </div>

    <br class="clear-both" /><br />
    <hr />

    <h2 class="dashboard-heading">Content</h2>
    <div class="dashboard-box">
        <!--img src="/CCTemplates/Admin/Images/icons/page.icon.png" alt="Create/Modify Your Pages" width="75" height="75" class="DashboardIcon" /-->
        <a href="/CCAdmin/ContentManagement/Pages.aspx" class="pages-link">Pages</a>
        <p>Add page, view page details, modify pages, delete pages.</p>
    </div>
    <div class="dashboard-box">
        <!--img src="/CCTemplates/Admin/Images/icons/blog.icon.png" alt="Manage blogs entries, view blog details, modify blog entries, delete blog entries" width="75" height="75" class="DashboardIcon" /-->
        <a href="/CCAdmin/Blogging/Blogs.aspx" class="blogs-link">Blogs</a>
        <p>Manage blogs entries, view blog details, modify blog entries, delete blog entries.</p>
    </div>
    <div class="dashboard-box">
        <!--img src="/CCTemplates/Admin/Images/icons/links.icon.png" alt="Add, Modify and Delete Navigation Menu Items" width="75" height="75" class="DashboardIcon" /-->
        <a href="/CCAdmin/ContentManagement/MenuItems.aspx" class="navigation-link">Navigation</a>
        <p>Add, view, modify and delete Navigation Menu Items.</p>
    </div>
    <div class="dashboard-box">
        <!--img src="/CCTemplates/Admin/Images/icons/upload.icon.png" alt="Upload, Modify and Delete Files" width="75" height="75" class="DashboardIcon" /-->
        <a href="/CCAdmin/ContentManagement/UploadFiles.aspx" class="file-upload-link">File Uploads</a>
        <p>Add, view, modify and delete Files.</p>
    </div>
    <div class="dashboard-box">
        <!--img src="/CCTemplates/Admin/Images/icons/images.icon.png" alt="Add, Modify and Delete Images" width="75" height="75" class="DashboardIcon" /-->
        <a href="/CCAdmin/ContentManagement/Images.aspx" class="image-link">Images</a>
        <p>Manage images, view image details, delete images.</p>
    </div>
    <div class="dashboard-box">
        <!--img src="/CCTemplates/Admin/Images/icons/image.rotator.icon.png" alt="Add, Modify and Delete Image Rotators" width="75" height="75" class="DashboardIcon" /-->
        <a href="/CCAdmin/ContentManagement/Rotator.aspx" class="gallery-link">Image Slider</a>
        <p>Add, view, modify and delete Image Rotators from Content Pages.</p>
    </div>
    <div class="dashboard-box">
        <!--img src="/CCTemplates/Admin/Images/icons/image.rotator.icon.png" alt="Add, Modify and Delete Image Galleries" width="75" height="75" class="DashboardIcon" /-->
        <a href="~/CCAdmin/ContentManagement/ImageGalleries.aspx" class="gallery-link">Gallery</a>
        <p>Add, view, modify and delete Image Galleries.</p>
    </div>
    <div class="dashboard-box">
        <!--img src="/CCTemplates/Admin/Images/icons/images.icon.png" alt="Add, Modify and Delete Image Gallery Items" width="75" height="75" class="DashboardIcon" /-->
        <a href="~/CCAdmin/ContentManagement/ImageGalleryItems.aspx" class="image-link">Gallery Items</a>
        <p>Add, Modify and Delete Image Gallery Items.</p>
    </div>
    <div class="dashboard-box">
        <!--img src="/CCTemplates/Admin/Images/icons/cc.news.icon.png" alt="Manage news articles, view news article details, modify news, delete news" class="DashboardIcon news-icon" /-->
        <a href="/CCAdmin/ContentManagement/NewsModule/News.aspx" class="news-link">News</a>
        <p>Manage news articles, view news article details, modify news articles, delete news articles</p>
    </div>
    <div class="dashboard-box">
        <!--img src="/CCTemplates/Admin/Images/icons/news.events.icon.png" alt="Manage events, view event details, modify events, delete events" width="75" height="75" class="DashboardIcon" /-->
        <a href="/CCAdmin/ContentManagement/EventModule/Events.aspx" class="event-link">Events</a>
        <p>Manage events, view event details, modify events, delete events.</p>
    </div>
    <div class="dashboard-box">
        <!--img src="/CCTemplates/Admin/Images/icons/testimonial.icon.png" alt="Add, Modify and Delete Testimonials" width="75" height="75" class="DashboardIcon" /-->
        <a href="/CCAdmin/ContentManagement/Testimonials.aspx" class="testimonial-link">Testimonials</a>
        <p>Add, view, modify and delete Testimonials.</p>
    </div>
    <div class="dashboard-box">
        <!--img src="/CCTemplates/Admin/Images/icons/settings.icon.png" alt="Add, Modify and Delete Videos" width="75" height="75" class="DashboardIcon" /-->
        <a href="~/CCAdmin/ContentManagement/Videos.aspx" class="video-link">Videos</a>
        <p>Add, view, modify and delete Videos.</p>
    </div>
    <div class="dashboard-box">
        <!--img src="/CCTemplates/Admin/Images/icons/newsletter.icon.png" alt="Add, Modify and Delete Newsletter Subscribers" width="75" height="75" class="DashboardIcon" /-->
        <a href="/CCAdmin/ContentManagement/Subscribers.aspx" class="subscribe-link">Subscribers</a>
        <p>Manage, view, modify and delete Newsletter Subscribers.</p>
    </div>
    <div class="dashboard-box">
        <!--img src="/CCTemplates/Admin/Images/icons/leads.icon.png" alt="Manage, view, modify and delete Website Leads" width="75" height="75" class="DashboardIcon" /-->
        <a href="/CCAdmin/ContentManagement/SalesLeads.aspx" class="lead-link">Leads <em class="QuickStatus">(New: <asp:Literal runat="server" ID="litNewLeads" />)</em></a>
        <p>Manage, view, modify and delete Website Leads.</p>
    </div>
    <div class="dashboard-box">
        <!--img src="/CCTemplates/Admin/Images/icons/faq.icon.png" alt="Add, Modify and Delete Frequently Asked Questions" width="75" height="75" class="DashboardIcon" /-->
        <a href="/CCAdmin/ContentManagement/FAQ.aspx" class="faq-link">FAQ</a>
        <p>Add, view, modify and delete Frequently Asked Questions.</p>
    </div>
    <div class="dashboard-box">
        <!--img src="/CCTemplates/Admin/Images/icons/links.icon.png" alt="Add, Modify and Delete Links" width="75" height="75" class="DashboardIcon" /-->
        <a href="/CCAdmin/ContentManagement/Links.aspx" class="link-link">Links</a>
        <p>Add, view, modify and delete Links.</p>
    </div>
    <div class="dashboard-box">
        <a href="/CCAdmin/ContentManagement/CallToActionModule/CallToAction.aspx" class="review-link">Call To Action</a>
        <p>Add, view, modify and delete Calls to action.</p>
    </div>
    <div class="dashboard-box">
        <a href="/CCAdmin/ContentManagement/CareerModule/Careers.aspx" class="account-link">Careers</a>
        <p>Add, view, modify and delete Careers.</p>
    </div>
    <div class="dashboard-box">
        <a href="/CCAdmin/ContentManagement/FormModule/CustomIntakeForms.aspx" class="pages-link">Forms</a>
    </div>
    
    <br class="clear-both" /><br />
    <hr />

    <asp:PlaceHolder ID="phStoreControls" runat="server">
        <h2 class="dashboard-heading">Store</h2>
        <br class="clear-both" />
        
        <asp:PlaceHolder runat="server" ID="phSales">
    
            <div class="tab-container">
	            <ul class="tabs">
		            <li class="tab"><a href="#tab-daily-sales">Daily Sales</a></li>
		            <li class="tab"><a href="#tab-monthly-sales">Monthly Sales</a></li>
		            <li class="tab"><a href="#tab-inventory">Inventory</a></li>
		            <li class="tab"><a href="#tab-top-sellers">Top Sellers</a></li>
		            <li class="tab"><a href="#tab-breakdown">Breakdown</a></li>
	            </ul>
	            <div id="tab-daily-sales">
	                <div id="DailySalesChart" style="width: 910px; height: 170px; margin-left: 10px; margin-top: 30px; margin-bottom: 20px; border-top: 0px solid #DDD;"></div>
	            </div>
	            <div id="tab-monthly-sales">
	                <div id="MonthlySalesChart" style="width: 910px; height: 170px; margin-left: 10px; margin-top: 30px; margin-bottom: 20px; border-top: 0px solid #DDD;"></div>        
	            </div>
	            <div id="tab-inventory">
                    <asp:Repeater runat="server" ID="rptInventory">
                        <HeaderTemplate>
                            <table class="default-table">
                                <thead class="default-table-header">
                                    <tr>
                                        <td>Item</td>
                                        <td>Quantity</td>
                                        <td>Inventory Level</td>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><a href="<%# String.Format("/Product/{0}/{1}.html", Eval("CategoryPermalink"), Eval("Permalink")) %>" title="<%# Eval("Name") %>"><%#Eval("Name")%></a></td>
                                <td><span style="font-weight: bold; color: #888888;"><%#Eval("Inventory")%></span></td>
                                <td><span style="font-weight: bold; text-align: right; color: <%# Eval("InventoryLevelColor")%>;"><%#Eval("InventoryLevelDescription")%></span></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>                 
	            </div>
	            <div id="tab-top-sellers">
	                <asp:Repeater runat="server" ID="rptTopSellers">
                        <HeaderTemplate>
                            <table class="default-table">
                                <thead class="default-table-header">
                                    <tr>
                                        <td>Item</td>
                                        <td>Quantity</td>
                                        <td>Inventory Level</td>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><a href="<%# String.Format("/Product/{0}/{1}.html", Eval("CategoryPermalink"), Eval("Permalink")) %>" title="<%# Eval("Name") %>"><%# Eval("Name")%></a></td>
                                <td><span style="font-weight: bold; color: #888888;"><%#Eval("Quantity")%></span></td>
                                <td><span style="font-weight: bold; text-align: right; color: <%# Eval("InventoryLevelColor")%>;"><%#Eval("InventoryLevelDescription")%></span></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
	            </div>
	            <div id="tab-breakdown">
	                <Store:PaybackTotalControl runat="server" ID="ptcTotals" />
	            </div>
            </div>

        </asp:PlaceHolder> 

        <div class="dashboard-box">
            <!--img src="/CCTemplates/Admin/Images/icons/order.icon.png" alt="Search for orders, check order status, return, authorize, delete, and ship orders." width="75" height="75" class="DashboardIcon" /-->
            <a href="/CCAdmin/Commerce/Orders.aspx" class="order-link">Orders <em class="QuickStatus">(Waiting: <asp:Literal runat="server" ID="litUnshippedOrders" />)</em></a>
            <p>Search for orders, check order status, return, authorize, delete, and ship orders.</p>
        </div>
        <div class="dashboard-box">
            <a href="/CCAdmin/Commerce/Customers.aspx" class="account-link">Customers</a>
            <p>View Customers</p>
        </div>
        <div class="dashboard-box">
            <!--img src="/CCTemplates/Admin/Images/icons/product.icon.png" alt="View/Add/Delete products, activate products, alter product details" width="75" height="75" class="DashboardIcon" /-->
            <a href="/CCAdmin/Commerce/Products.aspx" class="product-link">Products</a>
            <p>View/Add/Delete products, activate products, alter product details.</p>
        </div>
        <div class="dashboard-box">
            <!--img src="/CCTemplates/Admin/Images/icons/inventory.icon.png" alt="Add, Modify and Delete Product Inventory" width="75" height="75" class="DashboardIcon" /-->
            <a href="/CCAdmin/Commerce/Inventory.aspx" class="inventory-link">Inventory</a>
            <p>Add inventory, modify inventory, delete inventory.</p>
        </div>
        <div class="dashboard-box">
            <!--img src="/CCTemplates/Admin/Images/icons/report.icon.png" alt="View daily sales reports" width="75" height="75" class="DashboardIcon" /-->
            <a href="/CCAdmin/Commerce/Reports/DailySalesByOrder.aspx" class="report-link">Daily Sales</a>
            <p>View daily sales using a date range picker.</p>
        </div>
        <div class="dashboard-box">
            <!--img src="/CCTemplates/Admin/Images/icons/report.icon.png" alt="View monthly sales reports" width="75" height="75" class="DashboardIcon" /-->
            <a href="/CCAdmin/Commerce/Reports/MonthlySales.aspx" class="report-link">Monthly Sales</a>
            <p>View daily sales using a date range picker.</p>
        </div>
        <div class="dashboard-box">
            <!--img src="/CCTemplates/Admin/Images/icons/country.icon.png" alt="Add, Modify and Delete Countries" width="75" height="75" class="DashboardIcon" /-->
            <a href="/CCAdmin/Commerce/Countries.aspx" class="country-link">Countries</a>
            <p>Add countries, modify countries, delete countries.</p>
        </div>
        <div class="dashboard-box">
            <!--img src="/CCTemplates/Admin/Images/icons/prov.state.icon.png" alt="Add, modify and delete provinces and states" width="75" height="75" class="DashboardIcon" /-->
            <a href="/CCAdmin/Commerce/Provinces.aspx" class="region-link">Regions</a>
            <p>Add, modify and delete provinces and states.</p>    
        </div>
        <div class="dashboard-box">
            <!--img src="/CCTemplates/Admin/Images/icons/brand.icon.png" alt="Add/Modify/Delete Brands" width="75" height="75" class="DashboardIcon" /-->
            <a href="/CCAdmin/Commerce/Brands.aspx" class="brand-link">Brands</a>
            <p>Add/Modify/Delete Brands.</p>    
        </div>
        <div class="dashboard-box">
            <!--img src="/CCTemplates/Admin/Images/icons/category.icon.png" alt="Add, Modify and Delete Product Cateogries" width="75" height="75" class="DashboardIcon" /-->
            <a href="/CCAdmin/Commerce/Categories.aspx" class="category-link">Categories</a>
            <p>Add categories, modify categories, delete categories.</p>    
        </div>        
        <div class="dashboard-box">
            <!--img src="/CCTemplates/Admin/Images/icons/colour.icon.png" alt="Add, Modify and Delete Product Colours" width="75" height="75" class="DashboardIcon" /-->
            <a href="/CCAdmin/Commerce/Colours.aspx" class="color-link">Colours</a>
            <p>Add colours, modify colours, delete colours.</p>    
        </div>
        <div class="dashboard-box">
            <!--img src="/CCTemplates/Admin/Images/icons/sizes.icon.png" alt="Add, Modify and Delete Product Sizes" width="75" height="75" class="DashboardIcon" /-->
            <a href="/CCAdmin/Commerce/Sizes.aspx" class="size-link">Sizes</a>
            <p>Add sizes, modify sizes, delete sizes.</p>
        </div>
        <div class="dashboard-box">
            <!--img src="/CCTemplates/Admin/Images/icons/promo.code.icon.png" alt="Add, Modify and Delete Promo Codes" width="75" height="75" class="DashboardIcon" /-->
            <a href="/CCAdmin/Commerce/PromoCodes.aspx" class="promo-code-link">Promo Codes</a>
            <p>Add and modify Promo Codes.</p>
        </div>
        <div class="dashboard-box">
            <!--img src="/CCTemplates/Admin/Images/icons/shipping.icon.png" alt="Add, Modify and Delete Product Shipping Details" width="75" height="75" class="DashboardIcon" /-->
            <a href="/CCAdmin/Commerce/Shipping/ShippingRateDetails.aspx" class="shipping-link">Shipping Rates</a>
            <p>Add/Modify Fixed Shipping Rates</p>
        </div>
        <div class="dashboard-box">
            <!--img src="/CCTemplates/Admin/Images/icons/shipping.icon.png" alt="Add, Modify and Delete Product Shipping Details" width="75" height="75" class="DashboardIcon" /-->
            <a href="/CCAdmin/Commerce/Shipping/ShippingZoneDetails.aspx" class="shipping-link">Shipping Zones</a>
            <p>Add/Modify Fixed Shipping Zones</p>
        </div>
        <div class="dashboard-box">
            <!--img src="/CCTemplates/Admin/Images/icons/review.icon.png" alt="Add, Modify and Delete Product Reviews" width="75" height="75" class="DashboardIcon" /-->
            <a href="/CCAdmin/Commerce/ProductReviews.aspx" class="review-link">Reviews</a>
            <p>View and delete product reviews.</p>
        </div>   
        <div class="dashboard-box" runat="server" visible="false">
            <a href="/CCAdmin/Commerce/ClientCustom/BoxCalculator.aspx">Box Calculator</a>
            <p>Add/Modify Box Calculator Products and Rooms.</p>
        </div>    
        
        <br class="clear-both" /><br />
    
    </asp:PlaceHolder>
    
    <hr />

    <h2 class="dashboard-heading">Schedule</h2>
    <div class="dashboard-box">
        <a href="/CCAdmin/ContentManagement/CalendarModule/Programs.aspx" class="settings-link">Programs</a>
        <p>Manage programs, view program details, modify programs, delete programs.</p>
    </div>
    <div class="dashboard-box">
        <a href="/CCAdmin/ContentManagement/CalendarModule/Instructors.aspx" class="account-link">Instructors</a>
        <p>Manage instructors, view instructor details, modify instructors, delete instructors.</p>
    </div>
    <div class="dashboard-box">
        <a href="/CCAdmin/ContentManagement/CalendarModule/Schedules.aspx" class="event-link">Schedules</a>
        <p>Manage schedules, view schedule details, modify schedules, delete schedules.</p>
    </div>
    <div class="dashboard-box">
        <a href="/CCAdmin/ContentManagement/CalendarModule/MembershipStatus.aspx" class="account-link">Memberships</a>
        <p>View and set memberships active and inactive.</p>
    </div>
    
    <br class="clear-both" /><br />
    <hr />

    <h2 class="dashboard-heading">Property</h2>
    <div class="dashboard-box">
        <a href="/CCAdmin/ContentManagement/PropertyModule/Properties.aspx">Properties</a>
        <p>Add, view, modify and delete properties.</p>
    </div>
    <div class="dashboard-box">
        <a href="/CCAdmin/ContentManagement/PropertyModule/Features.aspx">Features</a>
        <p>Add, view, modify and delete features.</p>
    </div>
    
    <br class="clear-both" /><br />
    

</asp:Content>

<asp:Content runat="server" ContentPlaceHolderID="cphScripts">

<!--[if lte IE 8]><script language="javascript" type="text/javascript" src="/CCTemplates/Admin/Scripts/flot/excanvas.min.js"></script><![endif]-->
<script language="javascript" type="text/javascript" src="/CCTemplates/Admin/Scripts/flot/jquery.flot.min.js"></script>
<script language="javascript" type="text/javascript" src="/CCTemplates/Admin/Scripts/flot/jquery.flot.tooltip.js"></script>
<script language="javascript" type="text/javascript" src="/CCTemplates/Admin/Scripts/jquery-ui/jquery-ui-1.8.13.custom.min.js"></script>
<script language="javascript" type="text/javascript" src="/CCTemplates/Shared/Scripts/ooCharts/oocharts.js"></script>
<script type="text/javascript" src="https://www.google.com/jsapi"></script>

<script type="text/javascript">
    var profile_id = "24382466";

    window.onload = function () {
        oo.setAPIKey("fe677510d7739e2e735673747469a6a3fcb76ef2");
        oo.load(function () {
            var visits = new oo.Metric(profile_id, "30d");
            visits.setMetric("ga:visits");
            visits.draw('visits');

            /*var bounce = new oo.Metric("24382466", "30d");
            bounce.setMetric("ga:bounces");
            bounce.draw('bounces');*/

            var bounce = new oo.Metric(profile_id, "30d");
            bounce.setMetric("ga:entranceBounceRate");
            bounce.draw('bounces', function()  {
                var element = document.getElementById('bounces');
                var number = parseFloat(element.innerHTML);
                element.innerHTML = number.toFixed(2) + "%" ;
            });

            var pageviews = new oo.Metric(profile_id, "30d");
            pageviews.setMetric("ga:pageviews");
            pageviews.draw('pageviews');

            var timeline = new oo.Timeline(profile_id, "30d");
            timeline.addMetric("ga:visits", "Visits");
            timeline.addMetric("ga:newVisits", "New Visits");
            timeline.addMetric("ga:bounces", "Bounces");
            timeline.setOptions({
                backgroundColor: { stroke: "#AAAAAA", strokeWidth: 0, fill: '#FFFFFF' },
                isStacked: true, width: 940,
                height: 160,
                pointSize: 5,
                lineWidth: 2,
                chartArea: { left: 40, top: 40, width: 880 },
                legend: { position: 'top' },
                colors: ['#33CCFF', '#66FF66', '#FF0000'],
                hAxis: { baselineColor: "#FFFFFF", gridlines: { color: "#FFFFFF"} },
                vAxis: { baselineColor: "#999999", gridlines: { color: "#EEEEEE", count: 3} }
            });
            timeline.draw('visitor-overview');

            var pie = new oo.Pie(profile_id, "30d");
            pie.setMetric("ga:visits", "Visits");
            pie.setDimension("ga:browser");
            pie.setOptions({ is3D: true, height: 250, width: 470,
                sliceVisibilityThreshold: 1 / 30,
                chartArea: { left: 10, top: 10, width: 490, height: 375 },
                legend: { alignment: 'center' },
                slices: { 2: { offset: 0.2} }
            });
            pie.draw('browser-usage');

            var pie5 = new oo.Pie(profile_id, "30d");
            pie5.setMetric("ga:visits", "Visits");
            pie5.setDimension("ga:screenResolution");
            pie5.setOptions({ is3D: true, height: 250, width: 470,
                sliceVisibilityThreshold: 1 / 30,
                chartArea: { left: 10, top: 10, width: 490, height: 375 },
                legend: { alignment: 'center' },
                slices: { 2: { offset: 0.2} }
            });
            pie5.draw('screen-resolution');

            var pie2 = new oo.Pie(profile_id, "30d");
            pie2.setMetric("ga:organicSearches", "Searches");
            pie2.setDimension("ga:source");
            pie2.setOptions({ is3D: true, height: 250, width: 490,
                chartArea: { left: 10, top: 10, width: 490, height: 375 },
                legend: { alignment: 'center' },
                slices: { 1: { offset: 0.2} }
            });
            pie2.draw('search-engines');

            var table2 = new oo.Table(profile_id, "30d");
            table2.addMetric("ga:organicSearches", "Searches");
            table2.addDimension("ga:keyword", "Keywords");
            table2.setOptions({ width: "450px", sortColumn: 1, sortAscending: false, pageSize: 10, page: "enable" });
            table2.draw('keywords-overview');

            var pie3 = new oo.Pie(profile_id, "30d");
            pie3.setMetric("ga:newVisits", "New Visits");
            pie3.setDimension("ga:country");
            pie3.setOptions({ is3D: true, height: 250, width: 470,
                sliceVisibilityThreshold: 1 / 50,
                chartArea: { left: 10, top: 10, width: 490, height: 375 },
                legend: { alignment: 'center' },
                slices: { 6: { offset: 0.2} }
            });
            pie3.draw('region-country');

            var pie4 = new oo.Pie(profile_id, "30d");
            pie4.setMetric("ga:newVisits", "New Visits");
            pie4.setDimension("ga:city");
            pie4.setOptions({ is3D: true, height: 250, width: 470,
                sliceVisibilityThreshold: 1 / 100,
                chartArea: { left: 10, top: 10, width: 490, height: 375 },
                legend: { alignment: 'center' },
                slices: { 4: { offset: 0.2} }
            });
            pie4.draw('region-city');

            var table3 = new oo.Table(profile_id, "30d");
            table3.addMetric("ga:uniquePageviews", "Views");
            table3.addDimension("ga:pagePath", "Page");
            table3.setOptions({ width: 940, sortColumn: 1, sortAscending: false, pageSize: 10, page: "enable" });
            table3.draw('page-views');
        });
    };
</script>

<asp:Literal runat="server" ID="litMonthlySalesFlotScript" />

<asp:Literal runat="server" ID="litWeeklySalesScript" />
<asp:Literal runat="server" ID="litDailySalesScript" />

<script type="text/javascript">

    $(function() {
        MonthlyPlots();
        DailyPlots();
    });

    function DailyPlots() {
        var d = <%= DailyPlots %>;

        // first correct the timestamps - they are recorded as the daily
        // midnights in UTC+0100, but Flot always displays dates in UTC
        // so we have to add one hour to hit the midnights in the plot
        for (var i = 0; i < d.length; ++i)
            d[i][0] += 60 * 60 * 1000;

        // helper for returning the weekends in a period
        function weekendAreas(axes) {
            var markings = [];
            var d = new Date(axes.xaxis.min);
            // go to the first Saturday
            d.setUTCDate(d.getUTCDate() - ((d.getUTCDay() + 1) % 7));
            d.setUTCSeconds(0);
            d.setUTCMinutes(0);
            d.setUTCHours(0);
            var i = d.getTime();
            do {
                // when we don't set yaxis, the rectangle automatically
                // extends to infinity upwards and downwards
                markings.push({ xaxis: { from: i, to: i + 2 * 24 * 60 * 60 * 1000} });
                i += 7 * 24 * 60 * 60 * 1000;
            } while (i < axes.xaxis.max);

            return markings;
        }

        var options = {
            colors: ["#9DCE76"],
            tooltip: true, 
            tooltipOpts: { xValText: "Date: ", yValText: "Amount: $" },
            xaxis: { mode: "time", tickLength: 10, timeformat: "%b %d" },
            selection: { mode: "x" },
            grid: { markings: weekendAreas, hoverable: true, borderColor: "#FFF", backgroundColor: "#FFF", borderWidth: 2 },
            lines: { show: true, fill: true },
            points: { show: true, radius: 8 },
            legend: { show: true, position: "ne", margin: 20 }
        };

        var plot = $.plot($("#DailySalesChart"), [ { label: "Daily Sales", data: d } ], options);

        // now connect the two

        $("#DailySalesChart").bind("plotselected", function(event, ranges) {
            // do the zooming
            plot = $.plot($("#DailySalesChart"), [d],
                $.extend(true, {}, options, {
                    xaxis: { min: ranges.xaxis.from, max: ranges.xaxis.to }
                }));
        });
    }
    
    
    function MonthlyPlots() {
        var d = <%= MonthlyPlots %>;

        // first correct the timestamps - they are recorded as the daily
        // midnights in UTC+0100, but Flot always displays dates in UTC
        // so we have to add one hour to hit the midnights in the plot
        for (var i = 0; i < d.length; ++i)
            d[i][0] += 60 * 60 * 1000;

        // helper for returning the weekends in a period
        function weekendAreas(axes) {
            var markings = [];
            var d = new Date(axes.xaxis.min);
            // go to the first Saturday
            d.setUTCDate(d.getUTCDate() - ((d.getUTCDay() + 1) % 7));
            d.setUTCSeconds(0);
            d.setUTCMinutes(0);
            d.setUTCHours(0);
            
            var i = d.getTime();
            
            do {
                // when we don't set yaxis, the rectangle automatically
                // extends to infinity upwards and downwards
                markings.push({ xaxis: { from: i, to: i + 2 * 24 * 60 * 60 * 1000} });
                i += 7 * 24 * 60 * 60 * 1000;
            } while (i < axes.xaxis.max);

            return markings;
        }

        var options = {
            colors: ["#9DCE76"],
            tooltip: true, 
            tooltipOpts: { xValText: "Date: ", yValText: "Amount: $" },
            xaxis: { mode: "time", tickLength: 10, timeformat: "%b %y" },
            selection: { mode: "x" },
            grid: { markings: weekendAreas, hoverable: true, borderColor: "#FFF", backgroundColor: "#FFF", borderWidth: 2 },
            lines: { show: true, fill: true },
            points: { show: true, radius: 8 },
            legend: { show: true, position: "ne", margin: 20 }            
        };

        var plot = $.plot($("#MonthlySalesChart"), [ { label: "Monthly Sales", data: d } ], options);

        // now connect the two

        $("#MonthlySalesChart").bind("plotselected", function(event, ranges) {
            // do the zooming
            plot = $.plot($("#MonthlySalesChart"), [d],
                $.extend(true, {}, options, {
                    xaxis: { min: ranges.xaxis.from, max: ranges.xaxis.to }
                }));
        });
    }
</script>

</asp:Content>