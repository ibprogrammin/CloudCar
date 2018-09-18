<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ContactControl.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.ContactControl" %>

<div id="contact-overlay"></div>
<div id="contact-form">
    
    <fieldset>
        
        <h2><span>Get In Touch</span> With Us Now!</h2>     
        <blockquote>
            Fill out our contact form and leave us some details about your project and we 
            will get back to you ASAP or <a href="/Home/Contact.html" title="">Get Direct Contact Information</a>
        </blockquote>

        <label>Name *</label>
        <asp:TextBox id="ContactName" CssClass="form-text-box" ClientIDMode="Static" name="ContactName" runat="server" ValidationGroup="Inquiry" />
        <br />

        <label>Email/Phone *</label>
        <asp:TextBox id="ContactEmail" CssClass="form-text-box" ClientIDMode="Static" name="ContactEmail" runat="server" ValidationGroup="Inquiry" />
        <br /><br />

        <label>Message *</label><br />
        <asp:TextBox id="ContactQuestion" CssClass="form-text-area" ClientIDMode="Static" name="ContactQuestion" runat="server" TextMode="MultiLine" Rows="7" cols="20" ValidationGroup="Inquiry" />
        <br />
        
        <label>Security Question: Two plus Seven = ?</label>
        <asp:TextBox runat="server" ID="QuestionTextBox" CssClass="form-text-box" ValidationGroup="Inquiry" />
        <br />
        
        <asp:Button id="btnSubmit" runat="server" Text="Send Now" CausesValidation="true" ValidationGroup="Inquiry" CssClass="form-submit-button" />
	    or <a href="#" id="CancelContactButton" style="text-decoration: underline;">Cancel</a>

    </fieldset>
    
    <fieldset class="float-left" style="width: 229px; display: none;">
        
        <br />
        <label>Serious Monkey</label>

		<p style="color: #E6E6E6;">
            371 Upper Gage Avenue<br />
        	Hamilton, Ontario<br />
	        L8V 4H8, Canada
        </p><br />
            
        <p style="color: #E6E6E6;">           
	        <b>1 905 390 0635</b><br />
    	    <a href="mailto:info@seriousmonkey.ca" class="Highlight">info@seriousmonkey.ca</a>
        </p>

    </fieldset>

    <br class="clear-both" />
    
    <asp:RequiredFieldValidator runat="server" ID="rfvName" Display="None" ErrorMessage="Please leave us your name" SetFocusOnError="true" ControlToValidate="ContactName" ValidationGroup="Inquiry" />
    <asp:RequiredFieldValidator runat="server" ID="rfvEmail" Display="None" ErrorMessage="Please leave us your email" SetFocusOnError="true" ControlToValidate="ContactEmail" ValidationGroup="Inquiry" />
    <asp:RequiredFieldValidator runat="server" ID="rfvComment" Display="None" ErrorMessage="Please leave us your question and/or comment" SetFocusOnError="true" ControlToValidate="ContactQuestion" ValidationGroup="Inquiry" />
    <asp:RequiredFieldValidator runat="server" ID="QuestionRfv" Display="None" ErrorMessage="Please answer the security question" SetFocusOnError="true" ControlToValidate="QuestionTextBox" ValidationGroup="Inquiry" />

    <ajax:ValidatorCalloutExtender runat="server" ID="vceName" TargetControlID="rfvName" PopupPosition="TopRight" HighlightCssClass="form-validate-error"  />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceEmail" TargetControlID="rfvEmail" PopupPosition="TopRight" HighlightCssClass="form-validate-error" />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceComment" TargetControlID="rfvComment" PopupPosition="TopRight" HighlightCssClass="form-validate-error" />
    <ajax:ValidatorCalloutExtender runat="server" ID="QuestionVce" TargetControlID="QuestionRfv" PopupPosition="TopRight" HighlightCssClass="form-validate-error" />

    <asp:CompareValidator runat="server" ID="cvQuestion" Display="None" Enabled="False" ValueToCompare="9" ValidationGroup="Inquiry" SetFocusOnError="true" ControlToValidate="QuestionTextBox" Type="Integer" ErrorMessage="Please enter the correct answer." Text="Please enter the correct answer." />
    <ajax:ValidatorCalloutExtender runat="server" ID="vceQuestionCompare" TargetControlID="cvQuestion" PopupPosition="TopRight" />

</div>