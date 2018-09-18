<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="LatestBlogControl.ascx.vb" Inherits="CloudCar.CCControls.Blogging.LatestBlogControl" %>
<div id="LatestBlogWrapper"> 
	<a href="/feeds/rss.xml" title=""><img src="/images/css/lg.rss.icon.jpg" alt="" width="27" height="27" style="float: right; margin-right: 20px; margin-top:2px;" /></a>
	<h2><a id="hlBlogLinkHeading" runat="server" href="" title=""><asp:Literal runat="server" ID="litHeading" /></a></h2>
    <p><asp:Literal runat="server" ID="litDetails" />...<br /><br />
    <a id="hlBlogLinkBottom" runat="server" href="" title="">read on &raquo;</a></p>
</div>