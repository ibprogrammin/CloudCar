<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ImageGalleryControl.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.GalleryControls.ImageGalleryControl" %>
<%@ Import Namespace="CloudCar.CCFramework.Core" %>

<asp:Repeater runat="server" ID="rptGalleryItems">
    <ItemTemplate>
        <a href='<%# String.Format("/images/db/{0}/770/{1}", Eval("ImageId"), PictureController.GetPictureFilename(CInt(Eval("ImageId")))) %>' rel='<%# String.Format("lightbox[{0}]", Category.Replace(" ", "")) %>' title='<%# Eval("Title") %>'>
            <span></span>
            <img src='<%# String.Format("/images/db/{0}/160/{1}", Eval("ImageId"), PictureController.GetPictureFilename(CInt(Eval("ImageId")))) %>' alt="" />
        </a>
    </ItemTemplate>
</asp:Repeater>