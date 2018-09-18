<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SubscribeControl.ascx.vb" Inherits="CloudCar.CCControls.SubscribeControl" %>

<div id="st-newsletter">
    <div class="newsletter" style="display:none;">
	    <div class="box-content">
		    <div id="newsletter_message" style="display:none; background: #FFFFCC; border: 1px solid #FFCC33; padding: 5px; top: -15px; left: 79px; width: 172px;  margin-bottom: 0px; position: absolute;"></div>
			<div style="text-align: left;" class="email-form">
			    <img class="poshytip newsletter-icon" title="Newsletter" alt="Newsletter" src="/CCTemplates/Default/Images/newsletter.png">
				<asp:TextBox runat="server" ID="SubscriberEmailTextBox" style="width:90%; padding: 3px 3px 3px 8px; box-shadow: none;" CssClass="email" type="text" name="newsletter_email_custom" value="Enter your email..." onclick="this.value = '';" onblur="if (this.value == ''){this.value = 'Enter your email...';}" onfocus="if (this.value == 'Enter your email...'){this.value = '';}" /><br />
				<input type="hidden" name="name" value="" />		
				<br />
				<input name="subscribe" value="1" type="hidden" />
			</div>
			<div style="width: 100%;" class="inputs">
				<div>
					<input type="radio" style="vertical-align: middle;" id="subscribe" name="subscribe" value="1" checked="checked"/>
					<label style="font-size:10px; vertical-align: middle;" for="subscribe">Subscribe</label>
				</div>
				<div>
					<input type="radio" style="vertical-align: middle;" id="unsubscribe" name="subscribe" value="0" />
					<label style="font-size:10px; vertical-align: middle;" for="unsubscribe">Unsubscribe</label>
				</div>
			</div>
			<div class="newsletter-button">
			    <asp:LinkButton runat="server" ID="SubscribeButton" CssClass="button" Text="">
			        <img src="/CCTemplates/Default/Images/newsletter-subscribe.png" alt="Subscribe"/>
			    </asp:LinkButton>
			</div>
	    </div>
    </div>
</div>

<Corp:ModalPopup runat="server" ID="MessagePopUp" />