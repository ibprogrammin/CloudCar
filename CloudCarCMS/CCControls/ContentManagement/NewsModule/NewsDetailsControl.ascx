<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="NewsDetailsControl.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.NewsModule.NewsDetailsControl" %>

<h3><asp:Literal runat="server" ID="TitleLiteral" /></h3>
<h4><asp:Literal runat="server" ID="SubTitleLiteral" /></h4>
<asp:Image runat="server" ID="ThumbnailImage" Visible="false" />
<asp:Literal runat="server" ID="ContentLiteral" />