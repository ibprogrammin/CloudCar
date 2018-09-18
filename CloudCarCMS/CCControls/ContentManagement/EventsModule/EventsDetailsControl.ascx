<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EventsDetailsControl.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.EventsModule.EventsDetailsControl" %>

<asp:Image runat="server" ID="ThumbnailImage" Visible="false" Style="float: right;" />

<h3><asp:Literal runat="server" ID="TitleLiteral" /></h3>
<h5><i>
    <asp:Literal runat="server" ID="StartDateLiteral" /> 
    <asp:Literal runat="server" ID="TimeLiteral" />
</i></h5><br />

<asp:Literal runat="server" ID="LocationLiteral" /><br /><br />
<asp:Literal runat="server" ID="ContentLiteral" />