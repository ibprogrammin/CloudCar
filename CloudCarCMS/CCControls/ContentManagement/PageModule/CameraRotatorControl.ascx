<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CameraRotatorControl.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.PageModule.CameraRotatorControl" %>
<%@ Import Namespace="CloudCar.CCFramework.Core" %>


<asp:Repeater runat="server" ID="RotatorRepeater" Visible="False">
    <HeaderTemplate>
        <div class="camera_white_skin camera_wrap" id="camera_wrap_1">
    </HeaderTemplate>
    <ItemTemplate>
        <%# String.Format("<div data-src=""{0}"" data-thumb=""{0}""></div>", PictureController.GetPictureLink(CInt(Eval("ImageID"))))%>
    </ItemTemplate>
    <FooterTemplate>
        </div>
    </FooterTemplate>
</asp:Repeater>

<script type="text/javascript">
    $('#camera_wrap_1').camera({
        height: '36.458%',
        loader: 'none',
        thumbnails: true,
        portrait: true,
        hover: true,
        opacityOnGrid: true,
    });
</script>