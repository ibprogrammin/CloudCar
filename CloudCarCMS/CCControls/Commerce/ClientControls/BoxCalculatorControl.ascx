<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="BoxCalculatorControl.ascx.vb" Inherits="CloudCar.CCControls.Commerce.ClientControls.BoxCalculatorControl" %>

<asp:ScriptManagerProxy runat="server" ID="smpProxy" />

<asp:HiddenField ID="lblDummy" runat="server" Value="" />

<div id="BoxCalculatorWrapper">
	<h2 class="BoxCalcHeading">Use Our New Box Calculator</h2>
    
    <div id="BoxCalcHow">
    	<h3>How does it work?</h3>
        <p>Enter the amount of rooms in your home, based on the average Canadian household, the recommended products will be displayed.</p>
    </div>
    
    <div id="BoxCalcFormWrapper">
        <asp:Repeater runat="server" ID="rptBoxCalc">
            <ItemTemplate>
                <label class="BoxCalcLabel"><%#Eval("RoomName")%></label>
                <asp:TextBox runat="server" id="txtRoomCount" CssClass="BoxCalcTextBox" RoomID='<%# Eval("ID") %>' style="width: 37px; margin-top: 20px; float: left;" />
                <ajax:TextBoxWatermarkExtender runat="server" ID="tbweRoomCount" TargetControlID="txtRoomCount" WatermarkText="0" WatermarkCssClass="Watermark" Enabled="false" />
            </ItemTemplate>
        </asp:Repeater>
    </div>
    
    <asp:Button runat="server" id="btnBCSubmit" title="Calculate my move" Text="" CssClass="btnBCSubmit" OnClick="btnBCSubmit_Click" />
    
    <asp:updatepanel id="upBoxCalc" runat="server">
        <ContentTemplate>    
            
        </ContentTemplate>
    </asp:updatepanel>
</div>

<ajax:ModalPopupExtender runat="server" ID="mpeConfirm" PopupControlID="lblDisplaySelectedProducts" 
         TargetControlID="lblDummy" DropShadow="true" BackgroundCssClass="PopupBackground" CancelControlID="btnContinueShopping">
</ajax:ModalPopupExtender>

<asp:Label runat="server" ID="lblDisplaySelectedProducts" Visible="false" CssClass="ModalStyle" style="width: 495px; padding: 20px; height: 500px; overflow: scroll; overflow-x: hidden; -ms-overflow-x:hidden;">

    <asp:UpdatePanel ID="udpInnerUpdatePanel" runat="server">
        <ContentTemplate>

    <h2>Your Recommendation</h2>
    <p>The following items will be added to your cart based on your requirements. Thank you for using the AMJ Box Calculator.</p>
    
    <asp:Repeater runat="server" ID="rptProducts">
        <HeaderTemplate>
            <table class="ProductDisplayTable" cellspacing="0" style="width: 485px;">
    	        <thead>
        	        <tr>
            	        <td width="180">Item(s)</td>
                        <td width="360">Name</td>
                        <td align="right">Quantity</td>
                    </tr>
                </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><img runat="server" src='<%# String.Format("/images/db/{0}/90/{1}.jpg", Eval("ImageID"), Eval("Name")) %>' alt='<%# Eval("Name") %>' class="ProductDisplayImage" /><br /></td>
                <td><a href="<%#String.Format("/product/{0}/{1}.html", Eval("Category"), Eval("Permalink"))%>" title='<%#Eval("Name")%>'><%#Eval("Name")%></a></td>
                <td align="right"><asp:Label runat="server" ID="lblQuantity" Text='<%# Eval("Quantity") %>' /><br /></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    
    <br style="clear: both;" />
    
    <asp:LinkButton runat="server" ID="btnContinueShopping" style="margin-top: 20px; float: left;">&laquo; Return to Shopping</asp:LinkButton>
    <asp:Button ID="btnAddToCart" runat="server" OnClick="btnAddToCart_Click" CssClass="btnAddToCart" style="float: right;" />

    <br style="clear: both;" />

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Label>

<% ' asp:UpdateProgress AssociatedUpdatePanelID="upBoxCalc" runat="server" ID="upProgress" DynamicLayout="false" >
    '<ProgressTemplate>
    '    <div class="loading-box"><h3 style="text-align: center; position: relative; top: 32px;">Loading please wait...</h3></div>
    '</ProgressTemplate>
    '</asp:UpdateProgress> %>