<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="MailingListControl.ascx.vb" Inherits="CloudCar.CCControls.Blogging.MailingListControl" %>

<fieldset class="mailing-list">

<h5>Join Our Mailinglist</h5>

<asp:PlaceHolder runat="server" ID="JoinMailingListPlaceHolder">
    <asp:TextBox runat="server" id="EmailAddressTextBox" />
    <ajax:TextBoxWatermarkExtender runat="server" TargetControlID="EmailAddressTextBox" WatermarkText="Email Address" ID="tbweMLEmail" />
    
    <asp:Button id="JoinMailingListButton" runat="server" class="SubmitButton" Text="Join" />
</asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="ThankYouPlaceHolder" Visible="false">
    <h3>Thank you for joining our mailing list!</h3>
</asp:PlaceHolder>

</fieldset>