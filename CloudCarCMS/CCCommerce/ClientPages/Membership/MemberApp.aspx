<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="MemberApp.aspx.vb" Inherits="CloudCar.CCCommerce.Membership.MemberApp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">

    <script type="text/javascript">
        var txtUserName;

        function pageLoad() {
            txtUserName = $get("<%=txtUserName.ClientID %>");
        }

        function CheckUserName() {
            if (txtUserName.value.length > 0) {
                $get("spanUsername").innerHTML = "<br /> Checking Username Availability...";
                $get("spanUsername").style.color = "red";
                $get("spanUsername").style.fontWeight = "bold";
                CloudCar.UserService.UserNameExists(txtUserName.value, OnCheckUserName);
            }
            else {
                $get("spanUsername").innerHTML = "";
            }
        }

        function OnCheckUserName(unavailable) {
            if (unavailable == true) {
                $get("spanUsername").innerHTML = "<br /> Username Unavailable";
                $get("spanUsername").style.color = "red";
                $get("spanUsername").style.fontWeight = "bold";
            }
            else if (unavailable != true) {
                $get("spanUsername").innerHTML = "<br /> Username Available";
                $get("spanUsername").style.color = "green";
                $get("spanUsername").style.fontWeight = "bold";
            }
        }
    </script>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server">

<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" >
    <Services>
        <asp:ServiceReference Path="~/UserService.asmx" />
    </Services>
</asp:ScriptManagerProxy>

<img src="" alt="Register with ______________________" title="Register with ____________________" lang="en" /><br /><br />

<center>
    <asp:Table ID="Table1" runat="server" Width="65%" CellSpacing="4" BackColor="#56524a">
        <asp:TableRow><asp:TableCell ColumnSpan="2" HorizontalAlign="Center"><h2>Register as a _________________ Member</h2></asp:TableCell></asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="User Name" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:TextBox ID="txtUserName" runat="server" CssClass="FormField" onblur="CheckUserName()" />
                <span id="spanUsername"></span>
                <asp:RequiredFieldValidator ID="rfvUN" runat="server" ControlToValidate="txtUserName"
                    ErrorMessage="Please enter a valid user name." SetFocusOnError="true" Display="None"
                    ValidationGroup="Registration" />
                <ajax:ValidatorCalloutExtender ID="vceUN" runat="server" TargetControlID="rfvUN" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="Password" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:TextBox ID="txtPassword" runat="server" CssClass="FormField" TextMode="Password" />
                <asp:RequiredFieldValidator ID="rfvPW" runat="server" ControlToValidate="txtPassword"
                    ErrorMessage="Please Enter a valid password" SetFocusOnError="true" Display="None"
                    ValidationGroup="Registration" />
                <ajax:ValidatorCalloutExtender ID="vcePW" runat="server" TargetControlID="rfvPW" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="Confirm Password" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:TextBox ID="txtRePassword" runat="server" CssClass="FormField" TextMode="Password" />
                <asp:RequiredFieldValidator ID="rfvRP" runat="server" ControlToValidate="txtRePassword"
                    ErrorMessage="Please confirm your password" SetFocusOnError="true" Display="None"
                    ValidationGroup="Registration" />
                <ajax:ValidatorCalloutExtender ID="vceRP1" runat="server" TargetControlID="rfvRP" />
                <asp:CompareValidator ID="cvRP" ControlToCompare="txtPassword" ControlToValidate="txtRePassword"
                    runat="server" ErrorMessage="Confirm password does not match your password."
                    SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
                <ajax:ValidatorCalloutExtender ID="vceRP2" runat="server" TargetControlID="cvRP" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="Password Question" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:TextBox ID="txtPWQuestion" runat="server" CssClass="FormField" TextMode="SingleLine" />
                <asp:RequiredFieldValidator ID="rfvPWQ" runat="server" ControlToValidate="txtPWQuestion"
                    ErrorMessage="Please supply a question for password retrieval." SetFocusOnError="true"
                    Display="None" ValidationGroup="Registration" />
                <ajax:ValidatorCalloutExtender ID="vcePWQ" runat="server" TargetControlID="rfvPWQ" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="Password Answer" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:TextBox ID="txtPWAnswer" runat="server" CssClass="FormField" TextMode="SingleLine" />
                <asp:RequiredFieldValidator ID="rfvPWA" runat="server" ControlToValidate="txtPWAnswer"
                    ErrorMessage="Please provide an answer for password retrieval." SetFocusOnError="true"
                    Display="None" ValidationGroup="Registration" />
                <ajax:ValidatorCalloutExtender ID="vcePWA" runat="server" TargetControlID="rfvPWA" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="Email" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:TextBox ID="txtEmail" runat="server" CssClass="FormField" />
                <asp:RequiredFieldValidator ID="rfvEA" runat="server" ControlToValidate="txtEmail"
                    ErrorMessage="Please enter your Email Address." SetFocusOnError="true" Display="None"
                    ValidationGroup="Registration" />
                <asp:RegularExpressionValidator ID="revEAFormat" runat="server" ControlToValidate="txtEmail"
                    ErrorMessage="Your Email address is not in the correct format." SetFocusOnError="true"
                    Display="None" ValidationGroup="Registration" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" />
                <ajax:ValidatorCalloutExtender ID="vceEA1" runat="server" TargetControlID="rfvEA" />
                <ajax:ValidatorCalloutExtender ID="vceEA2" runat="server" TargetControlID="revEAFormat" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2"><br /><hr style="border-bottom: 0px; border-top: 2px solid #E3E1B8;" /><br /></asp:TableCell></asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="First Name" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top"><asp:TextBox ID="txtFirstName" runat="server" CssClass="FormField" /></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="Middle Name" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top"><asp:TextBox ID="txtMiddleName" runat="server" CssClass="FormField" /></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="Last Name" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top"><asp:TextBox ID="txtLastName" runat="server" CssClass="FormField" /></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="Address" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:TextBox ID="txtAddress" runat="server" CssClass="FormField" />
                <asp:RequiredFieldValidator runat="server" ID="rfvAddress" ControlToValidate="txtAddress" Display="None" ErrorMessage="Please enter in your address" ValidationGroup="SeachAddress" />
                <ajax:ValidatorCalloutExtender runat="server" ID="vceAddress" TargetControlID="rfvAddress" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="City" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:TextBox ID="txtCity" runat="server" CssClass="FormField" />
                <asp:RequiredFieldValidator runat="server" ID="rfvCity" ControlToValidate="txtCity" Display="None" ErrorMessage="Please enter in your city" ValidationGroup="SeachAddress" />
                <ajax:ValidatorCalloutExtender runat="server" ID="vceCity" TargetControlID="rfvCity" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="Country" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="FormField" DataTextField="Name" DataValueField="ID" AutoPostBack="true" />
                <asp:RequiredFieldValidator runat="server" ID="rfvCountry" ControlToValidate="ddlCountry" Display="None" ErrorMessage="Please select your country" ValidationGroup="SeachAddress" />
                <ajax:ValidatorCalloutExtender runat="server" ID="vceCountry" TargetControlID="rfvCountry" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="Province" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:DropDownList ID="ddlProvince" runat="server" CssClass="FormField" DataTextField="Name" DataValueField="ID" />
                <asp:RequiredFieldValidator runat="server" ID="rfvProvince" ControlToValidate="ddlProvince" Display="None" ErrorMessage="Please enter in your province/state" ValidationGroup="SeachAddress" />
                <ajax:ValidatorCalloutExtender runat="server" ID="vceProvince" TargetControlID="rfvProvince" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="Postal Code" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:TextBox ID="txtPC" runat="server" CssClass="FormField" />
                <asp:RequiredFieldValidator runat="server" ID="rfvPostalCode" ControlToValidate="txtPC" Display="None" ErrorMessage="Please enter your Postal/Zip Code" ValidationGroup="SeachAddress" />
                <ajax:ValidatorCalloutExtender runat="server" ID="vcePostalCode" TargetControlID="rfvPostalCode" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="HousePhone" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top"><asp:TextBox ID="txtHP" runat="server" CssClass="FormField" /></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="Work Phone" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top"><asp:TextBox ID="txtWP" runat="server" CssClass="FormField" /></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="Cell Phone" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top"><asp:TextBox ID="txtCP" runat="server" CssClass="FormField" /></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="Contact Method" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:DropDownList ID="ddlContactMethod" runat="server" CssClass="FormField">
                    <asp:ListItem Text="Email" Value="Email" Selected="True" />
                    <asp:ListItem Text="Home Phone" Value="Home Phone" />
                    <asp:ListItem Text="Work Phone" Value="Work Phone" />
                    <asp:ListItem Text="Cell Phone" Value="Cell Phone" />
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2"><br /><hr style="border-bottom: 0px; border-top: 2px solid #E3E1B8;" /><br /></asp:TableCell></asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="Birth Date" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:TextBox ID="txtBirthDate" runat="server" CssClass="FormField" />
                <ajax:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtBirthDate" PopupPosition="BottomLeft" SelectedDate="01/01/1995" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="Height (cm)" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:TextBox ID="txtHeight" runat="server" CssClass="FormField" />
                <asp:CompareValidator Type="Double" Operator="DataTypeCheck" runat="server" ID="cvHeight" ControlToValidate="txtHeight" ErrorMessage="This is not a valid Height" Display="None" ValidationGroup="Registration" /> 
                <ajax:ValidatorCalloutExtender runat="server" ID="vceHeight" TargetControlID="cvHeight" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="Weight (Kg)" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:TextBox ID="txtWeight" runat="server" CssClass="FormField" />
                <asp:CompareValidator Type="Double" Operator="DataTypeCheck" runat="server" ID="cvWeight" ControlToValidate="txtWeight" ErrorMessage="This is not a valid Weight" Display="None" ValidationGroup="Registration" />
                <ajax:ValidatorCalloutExtender runat="server" ID="vceWeight" TargetControlID="cvWeight" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="Gender" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" CssClass="RadioField">
                <asp:RadioButtonList runat="server" ID="rblGender" RepeatColumns="2">
                    <asp:ListItem Value="Male" Text="Male" />
                    <asp:ListItem Value="Female" Text="Female" />
                </asp:RadioButtonList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" HorizontalAlign="Center"><br /><br /><p class="Attention" style="background-position: center;">Emergency Contact Information</p><br /></asp:TableCell></asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="Name" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top"><asp:TextBox ID="txtECName" runat="server" CssClass="FormField" /></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="Phone Number" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top"><asp:TextBox ID="txtECPN" runat="server" CssClass="FormField" /></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Middle" Font-Bold="true" Text="Relation" CssClass="FormFieldName" />
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top"><asp:TextBox ID="txtECRelation" runat="server" CssClass="FormField" /></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" CssClass="FormFieldName" HorizontalAlign="Center"><p class="question">Do you have any pre existing medical conditions? If so please describe them.</p></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" HorizontalAlign="Center" VerticalAlign="Top">
                <asp:TextBox ID="txtMC" runat="server" CssClass="FormField" TextMode="MultiLine" Rows="3" /><br /><br />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" HorizontalAlign="Center"><br /><p class="Attention" style="background-position: center;">Other Information</p><br /></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" CssClass="FormFieldName" HorizontalAlign="Center"><p class="question">Have you participated in martial arts training before? If so please state style, rank and last time you attended.</p></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" HorizontalAlign="Center" VerticalAlign="Top">
                <asp:TextBox ID="txtPT" runat="server" CssClass="FormField" TextMode="MultiLine" Rows="3" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" CssClass="FormFieldName" HorizontalAlign="Center"><p class="question">How did you find our club?</p></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" HorizontalAlign="Center" VerticalAlign="Top">
                <asp:TextBox ID="txtHDYFUs" runat="server" CssClass="FormField" TextMode="MultiLine" Rows="3" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" CssClass="FormFieldName" HorizontalAlign="Center"><p class="question">What are your reasons for taking martial arts?</p></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" HorizontalAlign="Center" VerticalAlign="Top">
                <asp:TextBox ID="txtRFL" runat="server" CssClass="FormField" TextMode="MultiLine" Rows="3" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" CssClass="FormFieldName" HorizontalAlign="Center"><p class="question">Why did you choose ____________ over our competitors.</p></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" HorizontalAlign="Center" VerticalAlign="Top">
                <asp:TextBox ID="txtWhyBJJ" runat="server" CssClass="FormField" TextMode="MultiLine" Rows="3" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" CssClass="FormFieldName" HorizontalAlign="Center"><p class="question">Were you referred to our club by a current member? if yes can you please give their name.</p></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" HorizontalAlign="Center" VerticalAlign="Top">
                <asp:TextBox ID="txtWYRefered" runat="server" CssClass="FormField" TextMode="MultiLine" Rows="3" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2" HorizontalAlign="Center" VerticalAlign="Top">
                <asp:Button ID="btnRegister" Text="Register" CssClass="FormButton" runat="server" CausesValidation="true" ValidationGroup="Registration" Width="444px" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <br />
</center>

<Corp:ModalPopup runat="server" ID="mpMessage" />

</asp:Content>