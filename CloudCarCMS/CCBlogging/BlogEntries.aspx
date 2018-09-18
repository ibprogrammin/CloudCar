<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="BlogEntries.aspx.vb" Inherits="CloudCar.CCBlogging.BlogEntries" %>
<%@ Register TagPrefix="SM" Namespace="CloudCar.CCControls" Assembly="CloudCarFramework" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">

    <meta name="description" content="" />
    <meta name="keywords" content="" />

    <link runat="server" id="PageCanonicalMeta" rel="canonical" href="" />

</asp:Content>

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">
    <h1>Articles</h1>
    
    <div style="float: right; right: 0px; width: 250px; margin-top: 0px; margin-left: 40px;">
        <h3>Recent Posts</h3>
        <p><asp:Literal runat="server" ID="litBlogDescription" /></p><br />
        <Blog:RecentBlogsControl runat="server" id="rbcRecentBlogs" Visible="true" />
        <br />

        <Blog:TwitterFeedControl runat="server" id="tfcRecentTwitter" Visible="true" />
    </div>

    <a href="/feeds/rss.xml" title="Subscribe to our RSS feed." id="RSSFeedButton"></a>

    <SM:DataPagerRepeater runat="server" ID="rptBlogEntries" PersistentDataSource="true">
        <ItemTemplate>
            <div class="blog-summary">
                <h2>
                    <a href="/Blog/<%# Eval("Permalink") %>.html" title="<%# Eval("Title") %>">
                        <%# Eval("Title") %>
                    </a>
                </h2>
                <p><i>By <%#Eval("Author")%> on <%#Eval("DatePosted")%></i></p><br />
            
                <img src="<%# Eval("ImageLink") %>" alt="<%# Eval("Title") %>" style="visibility: <%# iif(CStr(Eval("ImageLink")) = string.empty, "hidden", "visible") %>; display: <%# iif(CStr(Eval("ImageLink")) = string.empty, "none", "block") %>;" />
                <p><%#Eval("ContentSummary")%></p><br />
                <p>
                    <a href="/Blog/<%# Eval("Permalink") %>.html" title="Jump to <%# Eval("Title") %>">More please...</a>
                </p>
            </div>
        </ItemTemplate>
        <SeparatorTemplate>
            <br /><br />
        </SeparatorTemplate>
    </SM:DataPagerRepeater>
    <br />        
    <asp:DataPager ID="dpBlogEntries" runat="server" PagedControlID="rptBlogEntries" PageSize="3" style="font-size: 16px;">
        <Fields>
            <asp:NextPreviousPagerField ButtonType="Link" ShowPreviousPageButton="true" ShowNextPageButton="false" ShowFirstPageButton="true" ShowLastPageButton="false" PreviousPageText="Prev" FirstPageText="&nbsp;&laquo;&nbsp;" />
            <asp:NumericPagerField ButtonCount="10" ButtonType="Link" RenderNonBreakingSpacesBetweenControls="true" />
            <asp:NextPreviousPagerField ButtonType="Link" ShowNextPageButton="true" ShowLastPageButton="true" ShowFirstPageButton="false" ShowPreviousPageButton="false" NextPageText="Next" LastPageText="&nbsp;&raquo;&nbsp;" />
        </Fields>
    </asp:DataPager>
    
    <br /><br />
    
    <Corp:SubscribeControl runat="server" />

</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
    
</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server">
    <div class="bread-crumb-holder"><a href="#">Home</a> &raquo Blog</div>
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <CMS:CloudCarScriptManagementControl runat="server" id="PageScriptManagementControl" />
</asp:Content>