<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Register.aspx.vb" Inherits="CloudCar.CCAuthentication.Register" Title="Register With Us" %>

<asp:Content ContentPlaceHolderID="head" runat="server">

<style type="text/css">
    label { width: 240px; display: block; float: left; }
    input { width: 390px; }
    select { width: 410px; height: 45px; }
    .form-add-left-margin { margin-left: 250px !important; margin-top: 20px; clear: both; }
    p { width: 650px; color: #888; font-size: 16px; }
    .form-info-details { width: 610px; padding: 20px;background-color: #FFFEF5; margin-bottom: 20px; }
    #spanUsername { text-align: left; position: relative; font-weight: bold; font-size: 18px; font-family: Calibri, Sans-Serif; top: 10px; height: 0px; margin-left: 20px; float: left; }
    #spanUsername i { font-size: 24px; }
</style>

<script type="text/javascript">
    var UserNameTextBox;

    function pageLoad() {
        UserNameTextBox = $get("<%=UserNameTextBox.ClientID %>");
    }

    function CheckUserName() {
        if (UserNameTextBox.value.length > 0) {
            $get("spanUsername").innerHTML = "Checking availability . . .";
            $get("spanUsername").style.color = "red";
            $get("spanUsername").style.fontWeight = "bold";
            $get("spanUsername").style.fontSize = "18px";
            CloudCar.UserService.UserNameExists(txtUsername.value, OnCheckUserName);
        }
        else {
            $get("spanUsername").innerHTML = "";
        }
    }

    function OnCheckUserName(unavailable) {
        if (unavailable == true) {
            $get("spanUsername").innerHTML = "<i class='icon-remove-sign'></i>";
            $get("spanUsername").style.color = "red";
            UserNameTextBox.className = "TextBox TextNotAvailable";
            UserNameTextBox.focus();
            UserNameTextBox.select();
        }
        else if (unavailable != true) {
            $get("spanUsername").innerHTML = "<i class='icon-check'></i>";
            $get("spanUsername").style.color = "green";
            UserNameTextBox.className = "TextBox TextAvailable";
        }
    }
</script>

</asp:Content>

<asp:Content ContentPlaceHolderID="cphAfterContent" runat="server">
    <div class="bread-crumb-holder"><a href="/" title="Home">Home</a> <i class="icon-caret-right"></i> Register</div>
</asp:Content>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

<asp:ScriptManagerProxy runat="server" >
    <Services>
        <asp:ServiceReference Path="~/services/UserService.asmx" />
    </Services>
</asp:ScriptManagerProxy>

<asp:UpdatePanel ID="upLogin" runat="server" ChildrenAsTriggers="true">
    <ContentTemplate>

   
<h1 class="form-heading-style">
    Register
    <i class="icon-pencil"></i>
</h1><hr />

<asp:Panel runat="server" ID="StartMessage">
<p>Thank you for joining us! Go ahead and fill out this form to register a login account. 
We take every measure to ensure your data is kept private and we will never make it available to any third party organizations. 
Your user name will be used as your unique login to our site. Don't forget your password after creating your account</p><br />

<asp:ValidationSummary id="vsValidation" 
    runat="server" 
    displaymode="BulletList" 
    ValidationGroup="Registration" 
    CssClass="error-display" 
    style="width: 615px;"
    HeaderText="<b>Oops! You forgot a few things silly... Please fill out all the following fields!</b><br /><br />" />

</asp:Panel>

<asp:Label runat="server" ID="lblStatus" Visible="false" CssClass="error-display" />

<asp:PlaceHolder runat="server" ID="FormPlaceHolder">

<h2>Credentials</h2><br/>

<label>User Name</label>
<asp:TextBox id="UserNameTextBox" runat="server" ClientIDMode="Static" value="" ValidationGroup="Registration" onblur="CheckUserName()" TabIndex="31" />
<div id="spanUsername"></div>
<br class="clear-both" /><br />

<label>Email Address</label>
<asp:TextBox id="txtEmail" runat="server" value="" ValidationGroup="Registration" TabIndex="32" />
<br class="clear-both" /><br />

<label>Password</label>
<asp:TextBox id="txtPassword" runat="server" value="" CssClass="check-password" TextMode="Password" ValidationGroup="Registration" TabIndex="33" />
<br class="clear-both" /><br />

<label>Confirm Password</label>
<asp:TextBox id="txtConfirmPassword" runat="server" value="" TextMode="Password" ValidationGroup="Registration" TabIndex="34" />
<br class="clear-both" /><br />

<p class="form-info-details">The following information will be used if you forget your passowrd. We do not have the ability to recover your
existing password so use details that you will remember.</p>

<label>Password Question</label>
<asp:DropDownList runat="server" id="PasswordQuestionDropDown" AutoPostBack="true" ValidationGroup="Registration" TabIndex="35">
    <asp:ListItem Value="What year was I born?" Text="What year was I born?" />
    <asp:ListItem Value="What colour are my eyes?" Text="What colour are my eyes?" />
    <asp:ListItem Value="In what city was I born?" Text="In what city was I born?" />
    <asp:ListItem Value="What is my mother's name?" Text="What is my mother's name?" />
    <asp:ListItem Value="What is my father's name?" Text="What is my father's name?" />
    <asp:ListItem Value="What is the name of my favourite pet?" Text="What is the name of my favourite pet?" />
    <asp:ListItem Value="What high school did I attend?" Text="What high school did I attend?" />
    <asp:ListItem Value="What is my favourite movie?" Text="What is my favourite movie?" />
    <asp:ListItem Value="What was the make of my first car?" Text="What was the make of my first car?" />
    <asp:ListItem value="What was your childhood nickname?" text="What was your childhood nickname?" />
    <asp:ListItem value="In what city did you meet your spouse/significant other?" text="In what city did you meet your spouse/significant other?" />
    <asp:ListItem value="What is the name of your favorite childhood friend?" text="What is the name of your favorite childhood friend?" />
    <asp:ListItem value="What street did you live on in third grade?" text="What street did you live on in third grade?" />
    <asp:ListItem value="What is your oldest sibling’s birthday month and year? (e.g., January 1900)" text="What is your oldest sibling’s birthday month and year? (e.g., January 1900)" />
    <asp:ListItem value="What is the middle name of your oldest child?" text="What is the middle name of your oldest child?" />
    <asp:ListItem value="What is your oldest sibling's middle name?" text="What is your oldest sibling's middle name?" />
    <asp:ListItem value="What school did you attend for sixth grade?" text="What school did you attend for sixth grade?" />
    <asp:ListItem value="What was your childhood phone number including area code? (e.g., 000-000-0000)" text="What was your childhood phone number including area code? (e.g., 000-000-0000)" />
    <asp:ListItem value="What is your oldest cousin's first and last name?" text="What is your oldest cousin's first and last name?" />
    <asp:ListItem value="What was the name of your first stuffed animal?" text="What was the name of your first stuffed animal?" />
    <asp:ListItem value="In what city or town did your mother and father meet?" text="In what city or town did your mother and father meet?" />
    <asp:ListItem value="Where were you when you had your first kiss?" text="Where were you when you had your first kiss?" />
    <asp:ListItem value="What is the first name of the boy or girl that you first kissed?" text="What is the first name of the boy or girl that you first kissed?" />
    <asp:ListItem value="What was the last name of your third grade teacher?" text="What was the last name of your third grade teacher?" />
    <asp:ListItem value="In what city does your nearest sibling live?" text="In what city does your nearest sibling live?" />
    <asp:ListItem value="What is your oldest brother’s birthday month and year? (e.g., January 1900)" text="What is your oldest brother’s birthday month and year? (e.g., January 1900)" />
    <asp:ListItem value="What is your maternal grandmother's maiden name?" text="What is your maternal grandmother's maiden name?" />
    <asp:ListItem value="In what city or town was your first job?" text="In what city or town was your first job?" />
    <asp:ListItem value="What is the name of the place your wedding reception was held?" text="What is the name of the place your wedding reception was held?" />
    <asp:ListItem value="What is the name of a college you applied to but didn't attend?" text="What is the name of a college you applied to but didn't attend?" />
    <asp:ListItem Value="Other" Text="Other" />
</asp:DropDownList>
<asp:TextBox id="txtPWQuestion" runat="server" value="" ValidationGroup="Registration" TabIndex="36" Visible="False" CssClass="form-add-left-margin" />
<br class="clear-both" /><br />

<label>Password Answer</label>
<asp:TextBox id="txtPWAnswer" runat="server" value="" ValidationGroup="Registration" TabIndex="37" />
<br class="clear-both" /><br />

<h2>Personal Details</h2><br />

<label>First Name</label>
<asp:TextBox id="txtFirstName" runat="server" value="" ValidationGroup="Registration" TabIndex="38" />
<br class="clear-both" /><br />

<!--label>Middle Name</label-->
<asp:TextBox id="txtMiddleName" runat="server" value="" ValidationGroup="Registration" TabIndex="39" Visible="False" />

<label>Last Name</label>
<asp:TextBox id="txtLastName" runat="server" value="" ValidationGroup="Registration" TabIndex="40" />
<br class="clear-both" /><br />

<label>Phone Number</label>
<asp:TextBox id="txtPhone" runat="server" value="" ValidationGroup="Registration" TabIndex="41" />
<br class="clear-both" /><br />

<label>Birth Date</label>
<asp:TextBox id="txtBirthDate" runat="server" value="" ValidationGroup="Registration" TabIndex="42" />
<br class="clear-both" /><br />

<br />

<h2>Address</h2><br />

<label>Street Address</label>
<asp:TextBox id="txtAddress" runat="server" value="" ValidationGroup="Registration" TabIndex="43" />
<br class="clear-both" /><br />

<label>City</label>
<asp:TextBox id="txtCity" runat="server" value="" ValidationGroup="Registration" TabIndex="44" />
<br class="clear-both" /><br />

<label>Country</label>
<asp:DropDownList ID="ddlCountry" runat="server" DataTextField="Name" DataValueField="ID" AutoPostBack="true" ValidationGroup="Registration" TabIndex="45" />
<br class="clear-both" /><br />

<label>Province/State</label>
<asp:DropDownList ID="ddlProvince" runat="server" DataTextField="Name" DataValueField="ID" ValidationGroup="Registration" TabIndex="46">
    <asp:ListItem Text="Province" Value="" />
</asp:DropDownList>
<br class="clear-both" /><br />

<label>Postal Code</label>
<asp:TextBox id="txtPC" runat="server" value=""  ValidationGroup="Registration" TabIndex="47" />
<br class="clear-both" /><br />

<asp:Button id="btnRegister" runat="server" Text="Register" CausesValidation="true" ValidationGroup="Registration" TabIndex="48" CssClass="SaveButton" style="margin-left: 250px; width: 410px;" />

<br class="clear-both" /><br />

<ajax:CalendarExtender ID="ceBirthDate" runat="server" TargetControlID="txtBirthDate" PopupPosition="BottomLeft" Enabled="False" />

<asp:RequiredFieldValidator id="rfvUsername" runat="server" controltovalidate="UserNameTextBox" errormessage="User Name" SetFocusOnError="true" display="None" ValidationGroup="Registration"/>    
<asp:RequiredFieldValidator id="rfvPassword" runat="server" controltovalidate="txtPassword" errormessage="Password" SetFocusOnError="true" display="None" ValidationGroup="Registration"/>
<asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword" ErrorMessage="Confirmation Password" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
<asp:RequiredFieldValidator ID="rfvPasswordQuestion" runat="server" ControlToValidate="txtPWQuestion" ErrorMessage="Password Question" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
<asp:RequiredFieldValidator ID="rfvPasswordAnswer" runat="server" ControlToValidate="txtPWAnswer" ErrorMessage="Password Answer" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
<asp:RequiredFieldValidator ID="rfvEmailAddress" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email Address" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
<asp:RequiredFieldValidator id="rfvAddress" runat="server" controltovalidate="txtAddress" errormessage="Street Address" SetFocusOnError="true" display="None" ValidationGroup="Registration"/>
<asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity" ErrorMessage="City" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
<asp:RequiredFieldValidator ID="rfvCountry" runat="server" ControlToValidate="ddlCountry" ErrorMessage="Country" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
<asp:RequiredFieldValidator ID="rfvProvince" runat="server" ControlToValidate="ddlProvince" ErrorMessage="Province/State" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
<asp:RequiredFieldValidator ID="rfvPC" runat="server" ControlToValidate="txtPC" ErrorMessage="Postal Code/Zip Code" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
<asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName" ErrorMessage="First Name" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
<asp:RequiredFieldValidator Enabled="False" ID="rfvMiddleName" runat="server" ControlToValidate="txtMiddleName" ErrorMessage="You forgot to enter your middle name!" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
<asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName" ErrorMessage="Last Name" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
<asp:RequiredFieldValidator ID="rfvPhoneNumber" runat="server" ControlToValidate="txtPhone" ErrorMessage="Phone Number" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />

<asp:RegularExpressionValidator ID="revEmailAdressFormat" runat="server" ControlToValidate="txtEmail" ErrorMessage="Your Email address is not in the correct format!" SetFocusOnError="true"
    Display="None" ValidationGroup="Registration" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" />

<asp:CompareValidator ID="cvConfirmPassword" ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword" runat="server" ErrorMessage="Oops! The password and confirmation password fields don't match." SetFocusOnError="true" Display="None" ValidationGroup="Registration" />

</asp:PlaceHolder>

<br class="clear-both" />

    </ContentTemplate>
</asp:UpdatePanel>

<Corp:ModalPopup runat="server" ID="mpMessage" />

</asp:Content>
