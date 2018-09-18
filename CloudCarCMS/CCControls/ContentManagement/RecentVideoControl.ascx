<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="RecentVideoControl.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.RecentVideoControl" %>
<asp:PlaceHolder runat="server" ID="VideoPlaceHolder" Visible="False">
    <div class="Video">
        <div>
            <asp:Literal runat="server" ID="FeaturedVideoLiteral" />
        </div>
	    <br />
        <h4><asp:Literal runat="server" ID="VideoTitleLiteral" /></h4>
        <asp:Literal runat="server" ID="VideoDescriptionLiteral" />
    </div>
</asp:PlaceHolder>