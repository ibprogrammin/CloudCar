<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ModalPopup.ascx.vb" Inherits="CloudCar.CCControls.ModalPopup.ModalPopup" %>
<asp:Label runat="server" ID="dummy" />
<ajax:ModalPopupExtender runat="server" ID="mpeMessage" OkControlID="btnOK" PopupControlID="panMessage"
    TargetControlID="dummy" BackgroundCssClass="modalBackground" />
<asp:Panel runat="server" ID="panMessage" Width="300" BackColor="black" BorderColor="White" BorderStyle="Solid" BorderWidth="1" style="padding:5px; display:none;">
    <asp:Label runat="server" ID="lblMessage" />
    <br />
    <asp:Button runat="server" ID="btnOK" class="FormButton" style="float:right;" Text="OK" />
</asp:Panel>
