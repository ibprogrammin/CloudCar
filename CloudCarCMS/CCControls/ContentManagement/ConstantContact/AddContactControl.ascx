<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AddContactControl.ascx.vb" Inherits="SMECommerceTemplate.AddContactControl" %>

    <!--h3 style="margin-bottom:16px;">"A Little Piece of Loewen" newsletter</h3-->
    <asp:TextBox ID="txtNewEmail" runat="server" style="width: 270px;" TabIndex="50" Visible="false" />
    
    <asp:Button ID="btnSignUp" runat="server" Text="Join Our Mailinglist" OnClick="btnSearch_Click" CssClass="Orange" style="float: right; font-size: 14px; margin-left: -10px; width: 180px;" TabIndex="51" />
    <br style="clear: both;" />
    
    <asp:CustomValidator ID="customValidator" runat="server" Display="None" OnServerValidate="customValidator_ServerValidate" />
    <ajax:TextBoxWatermarkExtender runat="server" ID="tbweEmail" TargetControlID="txtNewEmail" WatermarkText="Email Address" Enabled="false" />
    
    <!--div style="width: auto; padding: 20px; background-color: #F5F5F5;"></div-->