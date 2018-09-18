<%@ Page Title="Administration Dashboard" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Default.aspx.vb" Inherits="CloudCar.CCAdmin.Dashboard" %>
<%@ Import Namespace="CloudCar.CCFramework.Commerce" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphMainContent" runat="server">
    
    <h1 class="form-heading-style">
        Dashboard
        <i class="icon-dashboard"></i>
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
            <div id="monthly-visit-chart"></div>
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
       
    <p class="site-details-view">
        <span class="info-label">Visits</span> <span id='visits'></span>
        <span class="info-label">Pageviews</span> <span id='pageviews'></span>
        <span class="info-label">Bounce</span> <span id='bounces'></span>
        <!--a href="http://google.com/analytics" title="Access Google Analytics to view your site statistics.">Go To Google Analytics</a-->

        <!--a href="http://constantcontact.com" title="Access Constant Contact to view your mailing list subscribers.">Constant Contact</a><br /-->
        <span style="float: right; margin-right: 20px;">
            <span class="info-label">Guests</span> <span class="value-label"><asp:Literal runat="server" ID="GuestUsersLiteral" /></span>
            <span class="info-label">Registered</span> <span class="value-label"><asp:Literal runat="server" ID="RegisteredUsersLiteral" /></span>
            <span class="info-label">Total</span> <span class="value-label"><asp:Literal runat="server" ID="TotalUsersLiteral" /></span>
            
            <asp:LinkButton runat="server" ID="RestartApplicationButton" CssClass="reset-button" ToolTip="Restart Application" Text="" OnClientClick="return confirm('Are you sure you wish to restart the application? Any Session information for currently online users will be lost.');" />
        </span>
    </p>
    
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
	            <div id="daily-sales-chart"></div>
	        </div>
	        <div id="tab-monthly-sales">
	            <div id="monthly-sales-chart" style="width: 940px;"></div>
	        </div>
	        <div id="tab-inventory">
                <asp:Repeater runat="server" ID="InventoryRepeater">
                    <HeaderTemplate>
                        <table class="default-table">
                            <thead class="default-table-header">
                                <tr>
                                    <td>Item</td>
                                    <td class="align-center">Sold</td>
                                    <td class="align-center">Stock</td>
                                    <td class="align-right">Level</td>
                                </tr>
                            </thead>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><a href="<%# String.Format("/Product/{0}/{1}.html", Eval("CategoryPermalink"), Eval("Permalink")) %>" title="<%# Eval("Name") %>"><%#Eval("Name")%></a></td>
                            <td class="align-center"><%#Eval("Quantity")%></td>
                            <td class="align-center"><span style="font-weight: bold; color: #888888;"><%#Eval("Inventory")%></span></td>
                            <td class="align-right"><span style="font-weight: bold; text-align: right; color: <%# Eval("InventoryLevelColor")%>;"><%#Eval("InventoryLevelDescription")%></span></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>                 
	        </div>
	        <div id="tab-top-sellers">
	            <asp:Repeater runat="server" ID="TopSellersRepeater">
                    <HeaderTemplate>
                        <table class="default-table">
                            <thead class="default-table-header">
                                <tr>
                                    <td>Item</td>
                                    <td class="align-center">Quantity</td>
                                    <td class="align-right">Level</td>
                                </tr>
                            </thead>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><a href="<%# String.Format("/Product/{0}/{1}.html", Eval("CategoryPermalink"), Eval("Permalink")) %>" title="<%# Eval("Name") %>"><%# Eval("Name")%></a></td>
                            <td class="align-center"><span style="font-weight: bold; color: #888888;"><%#Eval("Quantity")%></span></td>
                            <td class="align-right"><span style="font-weight: bold; text-align: right; color: <%# Eval("InventoryLevelColor")%>;"><%#Eval("InventoryLevelDescription")%></span></td>
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

    <div class="vertical-tab-wrapper">
    <ul class="vertical-tab-container">
        <li class="current">
            <i class="icon-warning-sign"></i>
            Admin
        </li>
        <li>
            <i class="icon-file-text"></i>
            Content
        </li>
        <li>
            <i class="icon-shopping-cart"></i>
            Store
        </li>
        <li>
            <i class="icon-calendar"></i>
            Calendar
        </li>
        <li>
            <i class="icon-map-marker"></i>
            Properties
        </li>
    </ul>
    <ul class="vertical-tab-contents">
        <li>
            <div class="dashboard-box">
                <a href="/CCAdmin/Users.aspx">
                    <i class="icon-user"></i>
                    Users
                </a>
                <p>Manage users, view user details, modify users, delete users.</p>
            </div>
            <div class="dashboard-box">
                <a href="/CCAdmin/Settings.aspx">
                    <i class="icon-gears"></i>
                    Settings
                </a>
                <p>Modify web site settings</p>
            </div>
            <div class="dashboard-box">
                <a href="/CCAdmin/Maintenance.aspx">
                    <i class="icon-wrench"></i>
                    Maintenance
                </a>
                <p>Perform web site maintenance</p>
            </div>
            <div class="dashboard-box">
                <a href="/CCAdmin/Help/HelpDocument.docx">
                    <i class="icon-question-sign"></i>
                    Help
                </a>
                <p>Download the latest help documentation.</p>
            </div>
            <br class="clear-both" />
        </li>
        <li>
            <div class="dashboard-box">
                <a href="/CCAdmin/ContentManagement/Pages.aspx">
                    <i class="icon-file-text"></i>
                    Pages
                </a>
                <p>Add page, view page details, modify pages, delete pages.</p>
            </div>
            <div class="dashboard-box">
                <a href="/CCAdmin/Blogging/Blogs.aspx">
                    <i class="icon-rss"></i>
                    Blogs
                </a>
                <p>Manage blogs entries, view blog details, modify blog entries, delete blog entries.</p>
            </div>
            <div class="dashboard-box">
                <a href="/CCAdmin/ContentManagement/MenuItems.aspx">
                    <i class="icon-screenshot"></i>
                    Navigation
                </a>
                <p>Add, view, modify and delete Navigation Menu Items.</p>
            </div>
            <div class="dashboard-box">
                <a href="/CCAdmin/ContentManagement/UploadFiles.aspx">
                    <i class="icon-cloud-upload"></i>
                    File Uploads
                </a>
                <p>Add, view, modify and delete Files.</p>
            </div>
            <div class="dashboard-box">
                <a href="/CCAdmin/ContentManagement/Images.aspx">
                    <i class="icon-picture"></i>
                    Images
                </a>
                <p>Manage images, view image details, delete images.</p>
            </div>
            <div class="dashboard-box">
                <a href="/CCAdmin/ContentManagement/Rotator.aspx">
                    <i class="icon-desktop"></i>
                    Image Slider
                </a>
                <p>Add, view, modify and delete Image Rotators from Content Pages.</p>
            </div>
            <div class="dashboard-box">
                <a href="~/CCAdmin/ContentManagement/ImageGalleries.aspx">
                    <i class="icon-instagram"></i>
                    Gallery
                </a>
                <p>Add, view, modify and delete Image Galleries.</p>
            </div>
            <div class="dashboard-box">
                <a href="~/CCAdmin/ContentManagement/ImageGalleryItems.aspx">
                    <i class="icon-instagram"></i>
                    Gallery Items
                </a>
                <p>Add, Modify and Delete Image Gallery Items.</p>
            </div>
            <div class="dashboard-box">
                <a href="/CCAdmin/ContentManagement/NewsModule/News.aspx">
                    <i class="icon-bullhorn"></i>
                    News
                </a>
                <p>Manage news articles, view news article details, modify news articles, delete news articles</p>
            </div>
            <div class="dashboard-box">
                <a href="/CCAdmin/ContentManagement/EventModule/Events.aspx">
                    <i class="icon-calendar"></i>
                    Events
                </a>
                <p>Manage events, view event details, modify events, delete events.</p>
            </div>
            <div class="dashboard-box">
                <a href="/CCAdmin/ContentManagement/Testimonials.aspx">
                    <i class="icon-quote-left"></i>
                    Testimonials
                </a>
                <p>Add, view, modify and delete Testimonials.</p>
            </div>
            <div class="dashboard-box">
                <a href="~/CCAdmin/ContentManagement/Videos.aspx">
                    <i class="icon-film"></i>
                    Videos
                </a>
                <p>Add, view, modify and delete Videos.</p>
            </div>
            <div class="dashboard-box">
                <a href="/CCAdmin/ContentManagement/Subscribers.aspx">
                    <i class="icon-pencil"></i>
                    Subscribers
                </a>
                <p>Manage, view, modify and delete Newsletter Subscribers.</p>
            </div>
            <div class="dashboard-box">
                <a href="/CCAdmin/ContentManagement/SalesLeads.aspx">
                    <i class="icon-thumbs-up"></i>
                    Leads <em class="quick-display-status">(New: <asp:Literal runat="server" ID="litNewLeads" />)</em>
                </a>
                <p>Manage, view, modify and delete Website Leads.</p>
            </div>
            <div class="dashboard-box">
                <a href="/CCAdmin/ContentManagement/FAQ.aspx">
                    <i class="icon-comment"></i>
                    FAQ
                </a>
                <p>Add, view, modify and delete Frequently Asked Questions.</p>
            </div>
            <div class="dashboard-box">
                <a href="/CCAdmin/ContentManagement/Links.aspx">
                    <i class="icon-link"></i>
                    Links
                </a>
                <p>Add, view, modify and delete Links.</p>
            </div>
            <div class="dashboard-box">
                <a href="/CCAdmin/ContentManagement/FormModule/CustomIntakeForms.aspx">
                    <i class="icon-list"></i>
                    Forms
                </a>
            </div>
            <div class="dashboard-box">
                <a href="/CCAdmin/ContentManagement/CallToActionModule/CallToAction.aspx">
                    <i class="icon-bell"></i>
                    Call To Action
                </a>
                <p>Add, view, modify and delete Calls to action.</p>
            </div>
            <div class="dashboard-box">
                <a href="/CCAdmin/ContentManagement/CareerModule/Careers.aspx">
                    <i class="icon-briefcase"></i>
                    Careers
                </a>
                <p>Add, view, modify and delete Careers.</p>
            </div>
            <br class="clear-both" />
        </li>
        <li>
            <asp:PlaceHolder ID="phStoreControls" runat="server">
                <div class="dashboard-box">
                    <a href="/CCAdmin/Commerce/Orders.aspx">
                        <i class="icon-money"></i>
                        Orders <br />
                        <em class="quick-display-status">(Waiting: <asp:Literal runat="server" ID="litUnshippedOrders" />)</em>
                    </a>
                    <p>Search for orders, check order status, return, authorize, delete, and ship orders.</p>
                </div>
                <div class="dashboard-box">
                    <a href="/CCAdmin/Commerce/Products.aspx">
                        <i class="icon-tags"></i>
                        Products
                    </a>
                    <p>View/Add/Delete products, activate products, alter product details.</p>
                </div>
                <div class="dashboard-box">
                    <a href="/CCAdmin/Commerce/Customers.aspx">
                        <i class="icon-shopping-cart"></i>
                        Customers
                    </a>
                    <p>View Customers</p>
                </div>
                <div class="dashboard-box">
                    <a href="/CCAdmin/Commerce/Inventory.aspx">
                        <i class="icon-barcode"></i>
                        Inventory
                    </a>
                    <p>Add inventory, modify inventory, delete inventory.</p>
                </div>
                <div class="dashboard-box">
                    <a href="/CCAdmin/Commerce/Reports/DailySales.aspx">
                        <i class="icon-bar-chart"></i>
                        Daily Sales
                    </a>
                    <p>View daily sales using a date range picker.</p>
                </div>
                <div class="dashboard-box">
                    <a href="/CCAdmin/Commerce/Reports/MonthlySales.aspx">
                        <i class="icon-bar-chart"></i>
                        Monthly Sales
                    </a>
                    <p>View monthly sales.</p>
                </div>
                <div class="dashboard-box">
                    <a href="/CCAdmin/Commerce/Reports/ProductSalesReport.aspx">
                        <i class="icon-bar-chart"></i>
                        Product Sales
                    </a>
                    <p>View product sales.</p>
                </div>
                <div class="dashboard-box">
                    <a href="/CCAdmin/Commerce/Countries.aspx">
                        <i class="icon-flag"></i>
                        Countries
                    </a>
                    <p>Add countries, modify countries, delete countries.</p>
                </div>
                <div class="dashboard-box">
                    <a href="/CCAdmin/Commerce/Provinces.aspx">
                        <i class="icon-map-marker"></i>
                        Regions
                    </a>
                    <p>Add, modify and delete provinces and states.</p>    
                </div>
                <div class="dashboard-box">
                    <a href="/CCAdmin/Commerce/Brands.aspx">
                        <i class="icon-eye-open"></i>
                        Brands
                    </a>
                    <p>Add/Modify/Delete Brands.</p>    
                </div>
                <div class="dashboard-box">
                    <a href="/CCAdmin/Commerce/Categories.aspx">
                        <i class="icon-sitemap"></i>
                        Categories
                    </a>
                    <p>Add categories, modify categories, delete categories.</p>    
                </div>        
                <div class="dashboard-box">
                    <a href="/CCAdmin/Commerce/Colours.aspx">
                        <i class="icon-tint"></i>
                        Colours
                    </a>
                    <p>Add colours, modify colours, delete colours.</p>    
                </div>
                <div class="dashboard-box">
                    <a href="/CCAdmin/Commerce/Sizes.aspx">
                        <i class="icon-resize-vertical"></i>
                        Sizes
                    </a>
                    <p>Add sizes, modify sizes, delete sizes.</p>
                </div>
                <div class="dashboard-box">
                    <a href="/CCAdmin/Commerce/PromoCodes.aspx">
                        <i class="icon-certificate"></i>
                        Promo Codes
                    </a>
                    <p>Add and modify Promo Codes.</p>
                </div>
                <div class="dashboard-box">
                    <a href="/CCAdmin/Commerce/Shipping/ShippingRateDetails.aspx">
                        <i class="icon-truck"></i>
                        Shipping Rates
                    </a>
                    <p>Add/Modify Fixed Shipping Rates</p>
                </div>
                <div class="dashboard-box">
                    <a href="/CCAdmin/Commerce/Shipping/ShippingZoneDetails.aspx">
                        <i class="icon-truck"></i>
                        Shipping Zones
                    </a>
                    <p>Add/Modify Fixed Shipping Zones</p>
                </div>
                <div class="dashboard-box">
                    <a href="/CCAdmin/Commerce/ProductReviews.aspx">
                        <i class="icon-star"></i>
                        Reviews
                    </a>
                    <p>View and delete product reviews.</p>
                </div>   
            </asp:PlaceHolder>
            <br class="clear-both" />
        </li>
        <li>
            <div class="dashboard-box">
                <a href="/CCAdmin/ContentManagement/CalendarModule/Programs.aspx">
                    <i class="icon-book"></i>
                    Programs
                </a>
                <p>Manage programs, view program details, modify programs, delete programs.</p>
            </div>
            <div class="dashboard-box">
                <a href="/CCAdmin/ContentManagement/CalendarModule/Instructors.aspx">
                    <i class="icon-bullhorn"></i>
                    Instructors
                </a>
                <p>Manage instructors, view instructor details, modify instructors, delete instructors.</p>
            </div>
            <div class="dashboard-box">
                <a href="/CCAdmin/ContentManagement/CalendarModule/Schedules.aspx">
                    <i class="icon-calendar"></i>
                    Schedules
                </a>
                <p>Manage schedules, view schedule details, modify schedules, delete schedules.</p>
            </div>
            <div class="dashboard-box">
                <a href="/CCAdmin/ContentManagement/CalendarModule/MembershipStatus.aspx">
                    <i class="icon-paste"></i>
                    Memberships
                </a>
                <p>View and set memberships active and inactive.</p>
            </div>
            <br class="clear-both" />
        </li>
        <li>
            <div class="dashboard-box">
                <a href="/CCAdmin/ContentManagement/PropertyModule/Properties.aspx">
                    <i class="icon-home"></i>
                    Properties
                </a>
                <p>Add, view, modify and delete properties.</p>
            </div>
            <div class="dashboard-box">
                <a href="/CCAdmin/ContentManagement/PropertyModule/Features.aspx">
                    <i class="icon-check"></i>
                    Features
                </a>
                <p>Add, view, modify and delete features.</p>
            </div>
            <br class="clear-both" />
        </li>
    </ul>
    <br class="clear-both" />
    </div>
    <br class="clear-both" /><br />

</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cphScripts">
    <script type="text/javascript">
        var ooChart_api_key = "0b3c91b0336053572a98fa39cabc2aea2c834511";
        var profile_id = "24382466";
        var daily_sales_data = <%= OrderController.GetDailySalesFormatedForChart(30) %>;
        var monthly_sales_data = <%= OrderController.GetMonthlySalesFormatedForChart(16) %>;
    </script>
</asp:Content>