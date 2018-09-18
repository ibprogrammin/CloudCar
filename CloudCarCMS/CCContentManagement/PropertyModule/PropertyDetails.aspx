<%@ Page Title="" Language="vb" enabletheming="False" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="PropertyDetails.aspx.vb" Inherits="CloudCar.CCContentManagement.PropertyModule.PropertyDetails1" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">

    <meta runat="server" id="PageKeywordsMeta" name="keywords" content="" />
    <meta runat="server" id="PageDescriptionMeta" name="description" content="" />
    
    <link runat="server" id="PageCanonicalMeta" rel="canonical" href="" />

</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">

    <asp:UpdatePanel ID="PageUpdatePanel" runat="server">
        <ContentTemplate>

    <div class="box" style="padding: 10px; padding-left: 40px; padding-right: 30px; margin: 0px; margin-left: -40px; width: 750px;">
        <div class="desc-topleft">
            <br/><br/>
    	    <!--h3 class="sub-head">Listing - <asp:Literal runat="server" ID="litListingId" /></h3-->
            <h3 class="heading"><asp:Literal runat="server" ID="litTitle" /></h3>
            <p>
                <asp:Literal runat="server" ID="litAddress" /><br />
                <asp:Literal runat="server" ID="litPrice"/><br/>
                <asp:Literal runat="server" ID="litBedrooms" /> Bedroom(s)
            </p><br />
            <p>
                <b>Property Description</b><br />
                <asp:Literal runat="server" ID="litDetails" />
            </p>
	    </div>

        <CMS:ImageGalleryControl runat="server" ID="igcGallery" />
    </div>

    <br class="clear" />
    
    <asp:Panel runat="server" id="FeaturesPanel" class="features">
        <div class="features-list">
            <h3 style="margin-top: 10px;">Features and Amenities</h3>
            <asp:Repeater runat="server" ID="rptFeatures">
                <HeaderTemplate>
                    <ul class="f-list">
                </HeaderTemplate>
                <ItemTemplate>
                    <li><asp:Literal runat="server" ID="litFeature" Text='<%# Eval("Name") %>' /></li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
            
            <div style="float: right; width: 160px;">
                <asp:Label runat="server" ID="StatusMessage" ForeColor="Red" Visible="False" />
                
                <asp:LinkButton CssClass="submit-btn" runat="server" ID="BookViewingButton" Text="Book Viewing" />
                <a href="/Home/Property-Rental-Application.html" class="submit-btn">Apply Now</a>
                <asp:LinkButton CssClass="submit-btn" runat="server" ID="EmailListingButton" Text="Email Listing" />
                <a href="#" class="submit-btn" onclick="window.print()">Print Listing</a>
            </div>
            
            <asp:Literal runat="server" ID="litTestimonial" Visible="False" />
        </div>
	</asp:Panel>
     
    <div class="gmap" runat="server">
	    <h3>Location</h3>
	    <p style="margin-left: 40px; margin-bottom: 10px;"><asp:HyperLink runat="server" ID="WalkscoreLink" Target="_blank" /></p>
	    <div class="MapBackground"><div id="map_canvas"></div></div>
	</div>
    
    <br /><br />


    <asp:HiddenField ID="DummyTarget" runat="server" Value="" />

    <ajax:ModalPopupExtender runat="server" 
         ID="PropertyPopUp" 
         PopupControlID="EmailListingLabel"
         TargetControlID="DummyTarget" 
         DropShadow="False" 
         BackgroundCssClass="PopupBackground"
         CacheDynamicResults="True">
    </ajax:ModalPopupExtender>

    <asp:Label runat="server" ID="EmailListingLabel" CssClass="EmailListingPopup" EnableViewState="True">
        <asp:UpdatePanel ID="InnerUpdatePanel" runat="server">
            <ContentTemplate>
        
        <h2>Email Listing</h2>
        <p>Enter the email in the box below to send this listing.</p><br />
        <asp:TextBox runat="server" ID="EmailAddressTextBox" Width="250px" EnableViewState="True" />
        
        <br style="clear: both;" />
        
        <asp:LinkButton runat="server" ID="SubmitListingButton" CssClass="submit-btn" Text="Submit" ValidationGroup="EmailListing" />
        <asp:LinkButton runat="server" ID="CancelListingButton" CssClass="submit-btn" Text="Cancel" />

        <asp:RequiredFieldValidator runat="server" 
            ID="EmailListingValidator" 
            ControlToValidate="EmailAddressTextBox" 
            ErrorMessage="Please enter your email" 
            Text="Please enter your email"
            Display="None"
            SetFocusOnError="True" 
            ValidationGroup="EmailListing" />
        <ajax:ValidatorCalloutExtender runat="server" 
            ID="EmailListingExtender"
            TargetControlID="EmailListingValidator" 
            PopupPosition="BottomLeft" 
            CssClass="Callout" />
        <asp:RegularExpressionValidator runat="server"
            ID="EmailRegExValidator" 
            ControlToValidate="EmailAddressTextBox" 
            ErrorMessage="Your email address is not in the proper format." 
            SetFocusOnError="true"
            Display="None" 
            ValidationGroup="EmailListing" 
            ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" />
        <ajax:ValidatorCalloutExtender runat="server" 
            ID="EmailRegexExtender"
            TargetControlID="EmailRegExValidator" 
            PopupPosition="BottomLeft" 
            CssClass="Callout" />
            
            </ContentTemplate>
        </asp:UpdatePanel>
        <br style="clear: both;" />
    </asp:Label>
    
    <asp:Label runat="server" ID="BookViewingLabel" CssClass="EmailListingPopup" EnableViewState="True">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            
            <h2>Book Viewing</h2>
            <p>Fill in the form below and we will schedule a viewing of this property.</p><br /><br />
            
            <label class="lbl3">Name</label><br />
            <asp:TextBox runat="server" ID="NameTextBox" Width="300px" />
            <br class="clear"/><br/>
            
            <label class="lbl3">Phone Number</label><br />
            <asp:TextBox runat="server" ID="PhoneTextBox" Width="300px" />
            <br class="clear"/><br/>
            
            <label class="lbl3">Email Address</label><br />
            <asp:TextBox runat="server" ID="EmailTextBox" Width="300px" />
            <br class="clear"/><br/>
            
            <label class="lbl3">Desired Time</label><br />
            <asp:TextBox runat="server" ID="TimeTextBox" Width="300px" />
            <br class="clear"/><br/>
            
            <label class="lbl3">Desired Move In Date</label><br />
            <asp:TextBox runat="server" ID="MoveInTextBox" Width="300px" />
            <br class="clear"/><br/>
            
            <label class="lbl3">Verification Question</label><br />
            <p>What is <b>5 + 2</b></p>
            <asp:TextBox runat="server" ID="VerificationTextBox" Width="300px" />
            <br class="clear"/><br/>
            
            <br style="clear: both;" />
            
            <asp:LinkButton runat="server" ID="SubmitViewingButton" CssClass="submit-btn" Text="Submit" ValidationGroup="BookViewing" />
            <asp:LinkButton runat="server" ID="CancelViewingButton" CssClass="submit-btn" Text="Cancel" />

            <asp:RequiredFieldValidator runat="server" 
                ID="NameValidator" 
                ControlToValidate="NameTextBox" 
                ErrorMessage="Please enter your name" 
                Text="Please enter your name"
                Display="None"
                SetFocusOnError="True" 
                ValidationGroup="BookViewing" />
            <ajax:ValidatorCalloutExtender runat="server" 
                ID="NameCallout"
                TargetControlID="NameValidator" 
                PopupPosition="BottomLeft" 
                CssClass="Callout" />

            <asp:RequiredFieldValidator runat="server" 
                ID="PhoneValidator" 
                ControlToValidate="PhoneTextBox" 
                ErrorMessage="Please enter your phone number" 
                Text="Please enter your phone number"
                Display="None"
                SetFocusOnError="True" 
                ValidationGroup="BookViewing" />
            <ajax:ValidatorCalloutExtender runat="server" 
                ID="PhoneCallout"
                TargetControlID="PhoneValidator" 
                PopupPosition="BottomLeft" 
                CssClass="Callout" />
                
            <asp:RequiredFieldValidator runat="server" 
                ID="EmailValidator" 
                ControlToValidate="EmailTextBox" 
                ErrorMessage="Please enter your email" 
                Text="Please enter your email"
                Display="None"
                SetFocusOnError="True" 
                ValidationGroup="BookViewing" />
            <ajax:ValidatorCalloutExtender runat="server" 
                ID="EmailCallout"
                TargetControlID="EmailValidator" 
                PopupPosition="BottomLeft" 
                CssClass="Callout" />
            <asp:RegularExpressionValidator runat="server"
                ID="BookingEmailRegexValidator" 
                ControlToValidate="EmailTextBox" 
                ErrorMessage="Your email address is not in the proper format." 
                SetFocusOnError="true"
                Display="None" 
                ValidationGroup="BookViewing" 
                ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" />
            <ajax:ValidatorCalloutExtender runat="server" 
                ID="BookingEmailRegexCallout"
                TargetControlID="BookingEmailRegexValidator" 
                PopupPosition="BottomLeft" 
                CssClass="Callout" />
                
            <asp:RequiredFieldValidator runat="server" 
                ID="VerificationValidator" 
                ControlToValidate="VerificationTextBox" 
                ErrorMessage="Please fill out the verification question" 
                Text="Please fill out the verification question"
                Display="None"
                SetFocusOnError="True" 
                ValidationGroup="BookViewing" />
            <ajax:ValidatorCalloutExtender runat="server" 
                ID="VerificationCallout"
                TargetControlID="VerificationValidator" 
                PopupPosition="BottomLeft" 
                CssClass="Callout" />

            </ContentTemplate>
        </asp:UpdatePanel>

        <br style="clear: both;" />
    </asp:Label>

    <asp:Literal runat="server" ID="litBathrooms" Visible="false" />

    <div id="PromptStatusMessage">
        <h2>Thank You</h2>
        <p>Your request is being processed.</p>
    </div>

       </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server">

    <div class="bar2" runat="server" Visible="False">
    	<h3 class="price"> monthly</h3>
        <a href="/Home/Contact.html"><img src="/images/design/book-viewing.jpg" alt="Book Viewing" /></a>
        <a href="#"><img src="/images/design/email-listing.jpg" alt="Email Listing" /></a>
    </div>

</asp:Content>
<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    
    <asp:Literal runat="server" ID="litScripts" />
    
    <script type="text/javascript" src="http://maps.google.com/maps?file=api&amp;v=2&amp;sensor=false&amp;key=AIzaSyANpV37Z-9fvLUhIsbN0qq5kB0G1aLPG8g"></script>
    <script type="text/javascript" src="/scripts/google.functions.js"></script>
    <script type="text/javascript">
        $(window).bind('load', function() { initialize(); showAddressNoMarker("<%= GoogleMapAddress.tostring %>"); return false; });
    </script>

    
</asp:Content>