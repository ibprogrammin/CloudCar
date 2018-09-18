<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="Blog.aspx.vb" Inherits="CloudCar.CCBlogging.BlogPage" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">

    <meta runat="server" id="PageKeywordsMeta" name="keywords" content="" />
    <meta runat="server" id="PageDescriptionMeta" name="description" content="" />
    
    <link runat="server" id="PageCanonicalMeta" rel="canonical" href="" />

</asp:Content>

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">
    <h1><asp:Literal runat="server" ID="litTitle" /></h1>
    
    <h4><asp:Literal runat="server" ID="litDatePosted" /></h4><br />
    
    <Blog:FBLikeButtonControl id="fblbControl" runat="Server" /><br />
    
    <div style="float: right; width: 250px; margin-top: 0px; margin-left: 40px;">
        <h3>Recent</h3>

	    <Blog:RecentBlogsControl runat="server" id="rbcRecentBlogs" />

	    <br />

	    <p>
            <a id="hlAuthor" runat="server" href="" accesskey="C" title="">Jump to author</a><br />
	        <a id="hlComments" runat="server" href="" accesskey="C" title="">Jump to comments</a>
	    </p>
        
        <br />

        <Blog:FollowUsControl runat="server" id="fcTwitterFacebook" Visible="false" />
        <Blog:FBLikeBoxControl runat="server" id="fblbLikeBox" Width="292" Height="565" Visible="false" />
	    <Blog:TwitterFeedControl runat="server" id="tfcTwitterFeed" Visible="false" />
    </div>
    
    <asp:Literal runat="server" ID="litSubHeading" />

    <img id="imgHeadingImage" runat="server" visible="false" src="" alt="" />
    
    <asp:Literal runat="server" ID="litBlogContent" />

    <br class="clear-both" />
    
    <div style="margin-top: 0px; margin-bottom: 0px; max-height: 60px; height: 60px;">
        <!-- AddThis Button BEGIN -->
        <div class="addthis_toolbox addthis_default_style">          
            <a class="addthis_button_print"></a>
            <a class="addthis_button_email"></a>
            <a class="addthis_button_facebook"></a>
            <a class="addthis_button_myspace"></a>
            <a class="addthis_button_google"></a>
            <a class="addthis_button_twitter"></a>
            <span class="addthis_separator">|</span>
            <a href="http://addthis.com/bookmark.php?v=250&amp;username=xa-4c12637f201cd1c8" class="addthis_button_compact">Share</a>
        </div>
        <script type="text/javascript" src="http://s7.addthis.com/js/250/addthis_widget.js#username=xa-4c12637f201cd1c8"></script>
        <!-- AddThis Button END -->
    </div>

    <div style="padding: 10px; border: 1px solid #CCC; width: 600px; padding-bottom: 10px;">
        <a name="Author"></a>
        <img id="imgAuthorPortrait" runat="server" src="" alt="" style="float: left; margin-right: 20px; margin-bottom: 0px;" />
        <p><asp:Literal runat="server" ID="litAuthorDescription" /></p>
        <br class="clear-both"/>
    </div>

    
    <br />
    <a name="Comments"></a>
    <h3>Comments</h3>
    <br />

    <Blog:FBCommentsControl id="fbccComments" Width="620" runat="Server" />

    <asp:Repeater runat="server" ID="rptComments">
        <ItemTemplate>
            <div style="border: 1px solid #EEE; background-color: #FEFEFE; padding: 10px; margin: 10px;">
                <h4><a href="<%# Eval("Url") %>" target="_blank" rel="nofollow"><%# Eval("Name")%></a><br /></h4>
                <h5><asp:Literal ID="Literal1" runat="server" Text='<%# Eval("DatePosted","{0:r}")%>' /></h5>
                <p><%#Eval("Comment")%></p>
                <br style="clear: both;" />
            </div>    
        </ItemTemplate>
    </asp:Repeater>
    
    <Blog:AddCommentControl runat="server" id="accAddComments" Visible="False" />
    
    <Corp:SubscribeControl runat="server" />

</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server">
    <div class="bread-crumb-holder">
        <a href="/">Home</a> 
        &raquo; <a href="/Blog/Index.html">Blog</a> 
        &raquo; <asp:Literal runat="server" ID="BreadCrumbLiteral" />
    </div>
</asp:Content>
<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <CMS:CloudCarScriptManagementControl runat="server" id="PageScriptManagementControl" />
</asp:Content>