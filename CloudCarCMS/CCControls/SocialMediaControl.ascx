<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SocialMediaControl.ascx.vb" Inherits="CloudCar.CCControls.SocialMedia" %>
<div id="RightBlogDisplay">
    
    <Blog:RecentBlogsControl runat="server" />
    
    <hr class="SmallLine" />
    <blog:FollowUsControl runat="server" id="fcTwitterFacebook" />
    
    <hr class="SmallLine" />
    <blog:MailingListControl runat="server" id="mlcMailingList" />
    
</div>

<div id="BlogDisplay">
	<a href="/feeds/rss.xml" title="Subscribe to our RSS feed." id="RSSFeedButton"></a>

  	<h1><a id="hlBlogTitle" runat="server" href="/" title=""><asp:Literal ID="litBlogTitle" runat="server" /></a></h1>
    <h5><asp:Literal ID="litDataPosted" runat="server" /> by <asp:Literal ID="litAuthor" runat="server" /></h5><br />
    
    <asp:Image runat="server" ID="imgHeadingImage" Visible="false" CssClass="BlogHeadingImage" BorderWidth="1" />
    <p><asp:Literal ID="litBlogSummary" runat="server" /></p>

    <a id="hlJumpToBlog" runat="server" href="" title="" class="SansLinkSmallBold">Jump to post...</a>
</div>