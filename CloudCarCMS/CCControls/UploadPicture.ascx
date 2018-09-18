<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UploadPicture.ascx.vb" Inherits="CloudCar.CCControls.UploadPicture" %>

<asp:HiddenField runat="server" ID="hfPictureID" />
<asp:HiddenField runat="server" ID="hfMaxSize" />
<asp:Label runat="server" ID="dummy" />
	<ajax:ModalPopupExtender runat="server" ID="mpeImageUpload" TargetControlID="dummy" PopupControlID="panImageUpload" BackgroundCssClass="modalBackground" CancelControlID="ibClose" />
	<asp:Panel runat="server" ID="panImageUpload" Style="display: none; padding: 5px;" BackColor="Black" Width="300px" BorderColor="White" BorderStyle="Solid" BorderWidth="1px">
    <asp:ImageButton runat="server" ID="ibClose" Style="float: right;" ImageUrl="~/images/Kyoto Studio/delete_16.png" />
    <asp:FileUpload runat="server" ID="fuImage" /><br />
    <asp:Button runat="server" ID="btnUpload" Text="Upload Image" />
</asp:Panel>
