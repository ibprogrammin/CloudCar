<%@ Page Title="User Profile" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="UserProfile.aspx.vb" Inherits="CloudCar.CCAuthentication.UserProfile" %>

<asp:Content ContentPlaceHolderID="head" runat="server">

<style type="text/css">
    label { width: 140px; display: block; float: left; }
    input { width: 290px;}
    select { width: 312px; }
</style>

</asp:Content>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

<asp:PlaceHolder runat="server" ID="phMessage" Visible="false">
    <h1 class="form-heading-style">
        SORRY!
        <i class="icon-exclamation-sign"></i>
    </h1><hr />
    <h3>You must login to view your User Profile!</h3>
    <br /><br /><br /><br /><br /><br /><br />    
</asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="phUserProfile" Visible="false">

<asp:UpdatePanel UpdateMode="Conditional" ID="upLogin" runat="server" >
    <ContentTemplate>
        
    <h1 class="form-heading-style">
        User Profile
        <i class="icon-user"></i>
    </h1><hr />
    
    <asp:Label runat="server" ID="lblStatus" Visible="false" CssClass="error-display" />

    <p>Thank you for joining us! Feel free to change your account settings and personal details.</p><br />
    <asp:ValidationSummary id="vsValidation" runat="server" displaymode="BulletList" ValidationGroup="Registration" CssClass="ErrorDisplay" 
        HeaderText="<b>Please complete required fields!</b><br />" />
    <br />

    <div style="float: right; width: 360px;">
    
    <div class="account-panel">
        <h2>Account Panel</h2>
        <p>Has your <b>password</b> been compromised or do you want to make it a little more secure?</p><br/>
        <asp:Button ID="Button1" runat="server" PostBackUrl="/Change-Password.html" title="Click to change your password" Text="Change Password" CssClass="SaveButton account-panel-button" />
    </div>
        
    <asp:Panel runat="server" ID="pnlDistributor" Visible="false" CssClass="account-panel">
        <h2>Distributor Panel</h2>
        <p>You have <b>orders</b> that may be pending shippment! Please check the status immediately.</p><br />
        <asp:Button runat="server" ID="hlDistributorOrders" CssClass="SaveButton account-panel-button" PostBackUrl="/CCCommerce/Distributor/DistributorOrders.aspx" Text="View Orders" />
    </asp:Panel>
        
    <asp:PlaceHolder runat="server" ID="phAvailableDownloads" Visible="false">
        <div class="account-panel">
        <h2>Available Downloads</h2>
        <p>Here is a list of the products available for you to download.</p><br/>
        <asp:DataGrid ID="gvDownloadProducts" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" CellPadding="0" CellSpacing="0"
                AllowPaging="False" AllowSorting="False" GridLines="None" CssClass="ProductDisplayTable">
            <Columns>
                <asp:BoundColumn DataField="Name" HeaderText="Product" SortExpression="Name" />
                <asp:TemplateColumn SortExpression="ID" HeaderText="Link">
                    <ItemTemplate>
                        <a href='<%# Eval("Link") %>' title='<%# Eval("Filename") %>'><%#Eval("FileName")%></a>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
        </div>
    </asp:PlaceHolder>
        
    </div>
    
    <h2>Credentials</h2><br />
        
    <label>User Name</label>
    <asp:TextBox id="txtUsername" runat="server" value="" ValidationGroup="Registration" TabIndex="31" Enabled="false" />
    <br class="clear-left" /><br />

    <label>Email</label>
    <asp:TextBox id="txtEmail" runat="server" value="" ValidationGroup="Registration" TabIndex="36" />
    <br class="clear-left" /><br />

    <br/>

    <h2>Personal Details</h2><br />
        
    <label>First Name</label>
    <asp:TextBox id="txtFirstName" runat="server" value="" ValidationGroup="Registration" TabIndex="37" />
    <br class="clear-left" /><br />
        
    <label>Middle Name</label>
    <asp:TextBox id="txtMiddleName" runat="server" value="" ValidationGroup="Registration" TabIndex="38" />
    <br class="clear-left" /><br />
        
    <label>Last Name</label>
    <asp:TextBox id="txtLastName" runat="server" value="" ValidationGroup="Registration" TabIndex="39" />
    <br class="clear-left" /><br />
        
    <label>Phone</label>
    <asp:TextBox id="txtPhone" runat="server" value="" ValidationGroup="Registration" TabIndex="40" />
    <br class="clear-left" /><br />
        
    <label>Birth Date</label>
    <asp:TextBox id="txtBirthDate" runat="server" value="" ValidationGroup="Registration" TabIndex="41" />
    <br class="clear-left" /><br />


    <br />

    <h2>Address</h2><br />
        
    <label>Address</label>
    <asp:TextBox id="txtAddress" runat="server" value="" ValidationGroup="Registration" TabIndex="42" />
    <br class="clear-left" /><br />
        
    <label>City</label>
    <asp:TextBox id="txtCity" runat="server" value="" ValidationGroup="Registration" TabIndex="43" />
    <br class="clear-left" /><br />
        
    <label>Country</label>
    <asp:DropDownList ID="ddlCountry" runat="server" DataTextField="Name" DataValueField="ID" AutoPostBack="true" ValidationGroup="Registration" TabIndex="44" />
    <br class="clear-left" /><br />
        
    <label>Province</label>
    <asp:DropDownList ID="ddlProvince" runat="server" DataTextField="Name" DataValueField="ID" ValidationGroup="Registration" TabIndex="45">
        <asp:ListItem Text="Province/State" Value="" />
    </asp:DropDownList>
    <br class="clear-left" /><br />
        
    <label>Postal Code</label>
    <asp:TextBox id="txtPC" runat="server" value="" ValidationGroup="Registration" TabIndex="46" />
    <br class="clear-left" />
    
    <asp:Button id="btnUpdate" runat="server" Text="Save" CausesValidation="true" ValidationGroup="Registration" TabIndex="47" CssClass="SaveButton" style="margin-left: 150px; width: 310px; margin-top: 20px;" />

    <br class="clear-left" /><br />


    <ajax:TextBoxWatermarkExtender ID="tbweUsername" runat="server" TargetControlID="txtUsername" WatermarkText="Username" />
    <% 'ajax:TextBoxWatermarkExtender ID="tbwePassword" runat="server" TargetControlID="txtPassword" WatermarkText="Password" />
        '<ajax:TextBoxWatermarkExtender ID="tbweConfirmPassword" runat="server" TargetControlID="txtConfirmPassword" WatermarkText="Confirm Password" />
        '<ajax:TextBoxWatermarkExtender ID="tbwePWQuestion" runat="server" TargetControlID="txtPWQuestion" WatermarkText="Password Question" />
        '<ajax:TextBoxWatermarkExtender ID="tbwePWAnswer" runat="server" TargetControlID="txtPWAnswer" WatermarkText="Password Answer" / %>
    <ajax:TextBoxWatermarkExtender ID="tbweEmail" runat="server" TargetControlID="txtEmail" WatermarkText="Email Address" />
    <ajax:TextBoxWatermarkExtender ID="tbweFirstName" runat="server" TargetControlID="txtFirstName" WatermarkText="First Name" />
    <ajax:TextBoxWatermarkExtender ID="tbweMiddleName" runat="server" TargetControlID="txtMiddleName" WatermarkText="Middle Name" />
    <ajax:TextBoxWatermarkExtender ID="tbweLastName" runat="server" TargetControlID="txtLastName" WatermarkText="Last Name" />
    <ajax:TextBoxWatermarkExtender ID="tbwePhone" runat="server" TargetControlID="txtPhone" WatermarkText="Phone Number" />
    <ajax:TextBoxWatermarkExtender ID="tbweBirthDate" runat="server" TargetControlID="txtBirthDate" WatermarkText="Birth Date" />
    <ajax:TextBoxWatermarkExtender ID="tbweAddress" runat="server" TargetControlID="txtAddress" WatermarkText="Address" />
    <ajax:TextBoxWatermarkExtender ID="tbweCity" runat="server" TargetControlID="txtCity" WatermarkText="City" />
    <ajax:TextBoxWatermarkExtender ID="tbwePC" runat="server" TargetControlID="txtPC" WatermarkText="Postal Code/Zip" />  

    <ajax:CalendarExtender ID="ceBirthDate" runat="server" TargetControlID="txtBirthDate" PopupPosition="BottomLeft" />

    <asp:RequiredFieldValidator id="rfvUsername" runat="server" controltovalidate="txtUsername" errormessage="You forgot to enter your username!" SetFocusOnError="true" display="None" ValidationGroup="Registration"/>    
    <%  'asp:RequiredFieldValidator id="rfvPassword" runat="server" controltovalidate="txtPassword" errormessage="You forgot to enter your password!" SetFocusOnError="true" display="None" ValidationGroup="Registration"/>
        '<asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword" ErrorMessage="You forgot to enter your confirmation password!" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
        '<asp:RequiredFieldValidator ID="rfvPasswordQuestion" runat="server" ControlToValidate="txtPWQuestion" ErrorMessage="You forgot to enter your password question!" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
        '<asp:RequiredFieldValidator ID="rfvPasswordAnswer" runat="server" ControlToValidate="txtPWAnswer" ErrorMessage="You forgot to enter your password answer!" SetFocusOnError="true" Display="None" ValidationGroup="Registration" / %>
    <asp:RequiredFieldValidator ID="rfvEmailAddress" runat="server" ControlToValidate="txtEmail" ErrorMessage="You forgot to enter your email address!" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
    <asp:RequiredFieldValidator id="rfvAddress" runat="server" controltovalidate="txtAddress" errormessage="You forgot to enter your address!" SetFocusOnError="true" display="None" ValidationGroup="Registration"/>
    <asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity" ErrorMessage="You forgot to enter your city!" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
    <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ControlToValidate="ddlCountry" ErrorMessage="You forgot to select your country!" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
    <asp:RequiredFieldValidator ID="rfvProvince" runat="server" ControlToValidate="ddlProvince" ErrorMessage="You forgot to select your province!" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
    <asp:RequiredFieldValidator ID="rfvPC" runat="server" ControlToValidate="txtPC" ErrorMessage="You forgot to enter your postal code!" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />

    <asp:RegularExpressionValidator ID="revEmailAdressFormat" runat="server" ControlToValidate="txtEmail" ErrorMessage="Your Email address is not in the correct format." SetFocusOnError="true"
        Display="None" ValidationGroup="Registration" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" />

    <% 'asp:CompareValidator ID="cvConfirmPassword" ControlToCompare="txtPassword" ControlToValidate="txtConfirmPassword" runat="server" ErrorMessage="Oops! The password and confirmation password fields don't match." SetFocusOnError="true" Display="None" ValidationGroup="Registration" /%>

    </ContentTemplate>
</asp:UpdatePanel>
    
</asp:PlaceHolder>

</asp:Content>

<asp:Content ContentPlaceHolderID="cphBeforeContent" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="cphAfterContent" runat="server">
    <div class="bread-crumb-holder"><a href="/" title="Home">Home</a> <i class="icon-caret-right"></i> Account</div>
</asp:Content>