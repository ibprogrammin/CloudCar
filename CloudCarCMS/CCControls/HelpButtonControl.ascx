<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="HelpButtonControl.ascx.vb" Inherits="CloudCar.CCControls.HelpButtonControl" %>

<style type="text/css">
    .helpPopupControl {
        background-color: #FFFDCC;
        border: solid 0px #c0c0c0;
        color: #603913;
        position: absolute;
        visibility: hidden;
        padding: 10px;
        max-width: 290px;
        margin-top: 10px;
    }
</style>

<asp:Image ID="hbcImgIcon" runat="server" onmouseover="$find('pce').showPopup();" onmouseout="$find('pce').hidePopup();" style="margin-top: 10px;"/>

<div class="helpPopupControl" id="hbcTextWrapper">
    <asp:Literal ID="hbcLiHelpText" runat="server"></asp:Literal>
</div>    

<ajax:PopupControlExtender ID="hbcPopupControlExtender" runat="server" BehaviorID="pce"
   TargetControlID="hbcImgIcon" 
   PopupControlID="hbcTextWrapper" 
   Position="Left" OffsetX="-270" OffsetY="40"> 
</ajax:PopupControlExtender>