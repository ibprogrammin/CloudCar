<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableViewState="true" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Users.aspx.vb" Inherits="CloudCar.CCAdmin.Users" MaintainScrollPositionOnPostback="true" %>

<asp:Content ContentPlaceHolderID="head" runat="server">

<script type="text/javascript">
    var txtUserName;

    function pageLoad() {
        txtUsername = $get("<%=txtUsername.ClientID %>");
    }

    function CheckUserName() {
        if (txtUsername.value.length > 0) {
            $get("spanUsername").style.fontFamily = "Arial";
            $get("spanUsername").innerHTML = "<br /> Checking availability . . .";
            $get("spanUsername").style.color = "red";
            $get("spanUsername").style.fontWeight = "bold";
            CloudCar.UserService.UserNameExists(txtUsername.value, OnCheckUserName);
        }
        else {
            $get("spanUsername").innerHTML = "";
        }
    }

    function OnCheckUserName(unavailable) {
        if (unavailable == true) {
            $get("spanUsername").style.fontFamily = "Arial";
            $get("spanUsername").innerHTML = "<br /> <b style='font-size: 14px;'>X</b>";
            $get("spanUsername").style.color = "red";
            $get("spanUsername").style.fontWeight = "bold";
            txtUsername.className = "TextBox TextNotAvailable";
            txtUsername.focus();
            txtUsername.select();
        }
        else if (unavailable != true) {
            $get("spanUsername").style.fontFamily = "Arial";
            $get("spanUsername").innerHTML = "<br /> <b style='font-size: 14px;'>&radic;</b>";
            $get("spanUsername").style.color = "green";
            $get("spanUsername").style.fontWeight = "bold";
            txtUsername.className = "TextBox TextAvailable";
        }
    }
</script>

</asp:Content>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">
    
    <asp:PlaceHolder ID="phAddUser" runat="server" Visible="false">   
    
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server" >
        <Services>
            <asp:ServiceReference Path="~/services/UserService.asmx" />
        </Services>
    </asp:ScriptManagerProxy>

    <asp:UpdatePanel UpdateMode="Conditional" ID="upLogin" runat="server" >
        <ContentTemplate>

            <h1 class="form-heading-style">
                Create User
                <i class="icon-user"></i>
            </h1>
            <hr />
            
            <h5>Fill out this form below to create a new user profile.</h5><br />
            
            <asp:ValidationSummary id="vsValidation" runat="server" displaymode="BulletList" ValidationGroup="Registration" CssClass="error-display" 
                HeaderText="<b>Oh no! you broke it! hehehe... Just kidding!</b><br />" />
           
            <asp:Label runat="server" ID="StatusLabel" Visible="False" CssClass="status-message" />
           
            <fieldset>
                
                <h2 class="form-heading-style">Details</h2><br />
            
                <label>Username</label>
                <asp:TextBox id="txtUsername" runat="server" value="" ValidationGroup="Registration" onblur="CheckUserName()" TabIndex="31" style="width: 650px;" />
                <div id="spanUsername" style="position: relative; width: 400px; top: -50px; height: 0px; margin-right: 20px; float: left; text-align: right;"></div>
                <br style="clear: both;"/><br />
                
                <label>Password</label>
                <asp:TextBox id="txtPassword" runat="server" value="" TextMode="SingleLine" ValidationGroup="Registration" TabIndex="32" style="width: 650px;" />
                <br style="clear: both;"/><br />
                
                <label>Confirm Password</label>
                <asp:TextBox id="txtRePassword" runat="server" value="" TextMode="SingleLine" ValidationGroup="Registration" TabIndex="33" style="width: 650px;" />
                <br style="clear: both;"/><br />
                
                <label>Password Question</label>
                <asp:TextBox id="txtPWQuestion" runat="server" value="" ValidationGroup="Registration" TabIndex="34" style="width: 650px;" />
                <br style="clear: both;"/><br />
                
                <label>Password Answer</label>
                <asp:TextBox id="txtPWAnswer" runat="server" value="" ValidationGroup="Registration" TabIndex="35" style="width: 650px;" />
                <br style="clear: both;"/><br />
                
                <label>Email</label>
                <asp:TextBox id="txtEmail" runat="server" value="" ValidationGroup="Registration" TabIndex="36" style="width: 650px;" />
                <br style="clear: both;"/><br />
            
            </fieldset>

            <br class="clear-both" />

            <fieldset>
            
                <h2 class="form-heading-style">Personal Information</h2><br />
                
                <label>First Name</label>
                <asp:TextBox id="txtFirstName" runat="server" value="" ValidationGroup="Registration" TabIndex="37" style="width: 650px;" />
                <br style="clear: both;"/><br />
                
                <label>Middle Name</label>
                <asp:TextBox id="txtMiddleName" runat="server" value="" ValidationGroup="Registration" TabIndex="38" style="width: 650px;" />
                <br style="clear: both;"/><br />
                
                <label>Last Name</label>
                <asp:TextBox id="txtLastName" runat="server" value="" ValidationGroup="Registration" TabIndex="39" style="width: 650px;" />
                <br style="clear: both;"/><br />
                
                <label>Phone</label>
                <asp:TextBox id="txtPhone" runat="server" value="" ValidationGroup="Registration" TabIndex="40" style="width: 650px;" />
                <br style="clear: both;"/><br />
                
                <label>Birth Date</label>
                <asp:TextBox id="txtBirthDate" runat="server" value="" ValidationGroup="Registration" TabIndex="41" style="width: 650px;" />
                <br style="clear: both;"/><br />
                    
            </fieldset>
            
            <br class="clear-both" />
            
            <fieldset>
            
                <h2 class="form-heading-style">Address</h2><br />
                
                <label>Address</label>
                <asp:TextBox id="txtAddress" runat="server" value="" ValidationGroup="Registration" TabIndex="42" style="width: 650px;" />
                <br style="clear: both;"/><br />
                
                <label>City</label>
                <asp:TextBox id="txtCity" runat="server" value="" ValidationGroup="Registration" TabIndex="43" style="width: 650px;" />
                <br style="clear: both;"/><br />
                
                <label>Country</label>
                <asp:DropDownList ID="ddlCountry" runat="server" DataTextField="Name" DataValueField="ID" AutoPostBack="true" ValidationGroup="Registration" TabIndex="44" style="width: 670px;" />
                <br style="clear: both;"/><br />
                
                <label>Province/State</label>
                <asp:DropDownList ID="ddlProvince" runat="server" DataTextField="Name" DataValueField="ID" ValidationGroup="Registration" TabIndex="45" style="width: 670px;">
                    <asp:ListItem Text="Province" Value="" />
                </asp:DropDownList>
                <br style="clear: both;"/><br />
                
                <label>Postal Code/Zip Code</label>
                <asp:TextBox id="txtPC" runat="server" value="" ValidationGroup="Registration" TabIndex="46" style="width: 650px;" />
                <br style="clear: both;"/><br />
                
            </fieldset>
            
            <br class="clear-both" />
            
            <asp:Button id="btnRegister" runat="server" CssClass="SaveButton" CausesValidation="true" ValidationGroup="Registration" Text="Register" />
            <asp:Button id="btnCancel" runat="server" CssClass="DeleteButton" Text="Clear" />

            <br class="clear-both" /><br />

            <ajax:TextBoxWatermarkExtender ID="tbweUsername" runat="server" TargetControlID="txtUsername" WatermarkText="Username" />
            <ajax:TextBoxWatermarkExtender ID="tbwePassword" runat="server" TargetControlID="txtPassword" WatermarkText="Password" />
            <ajax:TextBoxWatermarkExtender ID="tbweConfirmPassword" runat="server" TargetControlID="txtRePassword" WatermarkText="Confirm Password" />
            <ajax:TextBoxWatermarkExtender ID="tbwePWQuestion" runat="server" TargetControlID="txtPWQuestion" WatermarkText="Password Question" />
            <ajax:TextBoxWatermarkExtender ID="tbwePWAnswer" runat="server" TargetControlID="txtPWAnswer" WatermarkText="Password Answer" />
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

            <asp:RequiredFieldValidator id="rfvUsername" runat="server" controltovalidate="txtUsername" errormessage="You forgot to enter your username silly!" SetFocusOnError="true" display="None" ValidationGroup="Registration"/>    
            <asp:RequiredFieldValidator id="rfvPassword" runat="server" controltovalidate="txtPassword" errormessage="You forgot to enter your password silly!" SetFocusOnError="true" display="None" ValidationGroup="Registration"/>
            <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtRePassword" ErrorMessage="You forgot to enter your confirmation password silly!" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
            <asp:RequiredFieldValidator ID="rfvPasswordQuestion" runat="server" ControlToValidate="txtPWQuestion" ErrorMessage="You forgot to enter your password question silly!" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
            <asp:RequiredFieldValidator ID="rfvPasswordAnswer" runat="server" ControlToValidate="txtPWAnswer" ErrorMessage="You forgot to enter your password answer silly!" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
            <asp:RequiredFieldValidator ID="rfvEmailAddress" runat="server" ControlToValidate="txtEmail" ErrorMessage="You forgot to enter your email address silly!" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
            <asp:RequiredFieldValidator id="rfvAddress" runat="server" controltovalidate="txtAddress" errormessage="You forgot to enter your address silly!" SetFocusOnError="true" display="None" ValidationGroup="Registration"/>
            <asp:RequiredFieldValidator ID="rfvCity" runat="server" ControlToValidate="txtCity" ErrorMessage="You forgot to enter your city silly!" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
            <asp:RequiredFieldValidator ID="rfvCountry" runat="server" ControlToValidate="ddlCountry" ErrorMessage="You forgot to select your country silly!" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
            <asp:RequiredFieldValidator ID="rfvProvince" runat="server" ControlToValidate="ddlProvince" ErrorMessage="You forgot to select your province silly!" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
            <asp:RequiredFieldValidator ID="rfvPC" runat="server" ControlToValidate="txtPC" ErrorMessage="You forgot to enter your postal code/zip silly!" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
            <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName" ErrorMessage="You forgot to enter your first name!" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
            <asp:RequiredFieldValidator ID="rfvMiddleName" runat="server" ControlToValidate="txtMiddleName" ErrorMessage="You forgot to enter your middle name!" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
            <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName" ErrorMessage="You forgot to enter your last name!" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />
            <asp:RequiredFieldValidator ID="rfvPhoneNumber" runat="server" ControlToValidate="txtPhone" ErrorMessage="You forgot to enter your phone number!" SetFocusOnError="true" Display="None" ValidationGroup="Registration" />

            <asp:RegularExpressionValidator ID="revEmailAdressFormat" runat="server" ControlToValidate="txtEmail" ErrorMessage="Your Email address is not in the correct format." SetFocusOnError="true"
                Display="None" ValidationGroup="Registration" ValidationExpression="^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$" />

            <asp:CompareValidator ID="cvConfirmPassword" ControlToCompare="txtPassword" ControlToValidate="txtRePassword" runat="server" ErrorMessage="Oops! The password and confirmation password fields don't match." SetFocusOnError="true" Display="None" ValidationGroup="Registration" />

        </ContentTemplate>
    </asp:UpdatePanel>
    
    </asp:PlaceHolder>
    
    <asp:PlaceHolder ID="phUserProfile" runat="server" Visible="false">
        
        <h1 class="form-heading-style">
            View User
            <i class="icon-user"></i>
        </h1><hr />

        <table class="default-table">
            <tr>
                <td style="font-weight: bold;">Username</td>
                <td><asp:Label ID="lblUserName" runat="server" /></td>
            </tr>
            <tr>
                <td style="font-weight: bold;">Email</td>
                <td><asp:Label ID="lblEmail" runat="server" /></td>
            </tr>
            <tr>
                <td style="font-weight: bold;">Name</td>
                <td><asp:Label ID="lblName" runat="server" /></td>
            </tr>
            <tr>
                <td style="font-weight: bold;">Address</td>
                <td><asp:Label ID="lblAddress" runat="server" /></td>
            </tr>
            <tr>
                <td style="font-weight: bold;">Phone</td>
                <td><asp:Label ID="lblPhone" runat="server" /></td>
            </tr>
            <tr>
                <td style="font-weight: bold;">Birth Date</td>
                <td><asp:Label ID="lblBirthDate" runat="server" /></td>
            </tr>
        </table>
        
        <br class="clear-both" />

        <asp:Button id="btnBack" runat="server" Text="Return" CssClass="BlueButton" />

        <br />
    </asp:PlaceHolder>

    <asp:PlaceHolder ID="phUserList" runat="server" Visible="false">

        <h1 class="form-heading-style">
            Users
            <i class="icon-user"></i>
        </h1>
        <hr />
        
        <div class="search-bar">
            <asp:TextBox runat="server" ID="UserSearchTextBox" style="width: 535px;" />
            <asp:Button id="SearchButton" runat="server" CssClass="OrangeButton" CausesValidation="true" style="width: 110px;" Text="Go" />
            <asp:Button id="btnAddUser" runat="server" CssClass="SaveButton" CausesValidation="true" Text="Create" />
            
            <ajax:TextBoxWatermarkExtender runat="server" ID="UserSearchWatermark" TargetControlID="UserSearchTextBox" WatermarkText="Search Users" WatermarkCssClass="Watermark" />
        </div>
        
        <asp:Label ID="lblStatus" runat="server" Text="" CssClass="status-message" Visible="false" />

        <asp:DataGrid runat="server" 
                ID="gvUsers" 
                AutoGenerateColumns="False" 
                AllowSorting="true" 
                DataSourceID="dsUsers" 
                DataKeyField="UserName" 
                GridLines="None" 
                AllowPaging="true" 
                PageSize="20" 
                CssClass="default-table">
            <PagerStyle HorizontalAlign="Right" PageButtonCount="8" Mode="NumericPages" Position="Bottom" CssClass="default-table-pager" />
            <HeaderStyle CssClass="default-table-header" />
            <Columns>
                <asp:TemplateColumn HeaderText="User" SortExpression="UserName">
                    <ItemTemplate>
                        <br />
                        <asp:LinkButton ID="lbSelect" runat="server" OnClick="lbSelect_Click"
                            CausesValidation="False" CommandArgument='<%# Eval("UserName") %>' CommandName="Select" Text='<%# Eval("UserName") %>' /><br />
                        <%# Eval("Name") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Last Login" SortExpression="LastActivityDate">
                    <ItemTemplate>
                        <br />
                        <asp:Literal runat="server" Text='<%# String.Format("{0:dddd, MMM dd, yyyy}", Eval("LastActivityDate")) %>' />
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Settings" ItemStyle-Width="20%">
                    <ItemTemplate>
                        
                        <br />
                        <asp:CheckBox runat="server" ID="EnableUserCheckBox" OnCheckedChanged="EnableUserCheckBoxChecked"
                            OnDataBinding="EnableUserCheckBoxDataBinding" UserName='<%# Eval("UserName") %>' AutoPostBack="true" Text="" CssClass="default-table-check-box" style="margin-top: -1px !important;" />
                        <span style="font-weight: bold; margin-left: 20px; font-size: 18px;">Enabled</span>
                        <br style="clear: both;" />
                        
                        <asp:CheckBox runat="server" ID="chkLockedOut" OnCheckedChanged="LockedOutCheckBoxCheckChanged"
                            OnDataBinding="LockedOutCheckBoxDataBinding" UserName='<%# Eval("UserName") %>' AutoPostBack="true" CssClass="default-table-check-box" style="margin-top: -1px !important;" />
                        <span style="font-weight: bold; margin-left: 20px; font-size: 18px;">Locked</span>
                        <br style="clear: both;" />
                        
                        <asp:CheckBox runat="server" ID="chkAdmin" OnCheckedChanged="AdminCheckBoxCheckChanged"
                            OnDataBinding="AdminCheckBoxDataBinding" UserName='<%# Eval("UserName") %>' AutoPostBack="true" CssClass="default-table-check-box" style="margin-top: -1px !important;" />
                        <span style="font-weight: bold; margin-left: 20px; font-size: 18px;">Admin</span>
                        
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Price Level">
                    <ItemTemplate>
                        <br />
                        <asp:DropDownList runat="server" OnSelectedIndexChanged="PriceLevelDropDownIndexChanged" OnDataBinding="PriceLevelDropDownDataBinding" UserName='<%# Eval("UserName") %>' AutoPostBack="true">
                            <asp:ListItem Text="Retail" Value="0" />
                            <asp:ListItem Text="Wholesale" Value="1" />
                            <asp:ListItem Text="Associate" Value="2" />
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <ItemTemplate>
                        <asp:Literal runat="server" Text='<%# String.Format("{0}?", Eval("PasswordQuestion")) %>' /><br />
                        <asp:TextBox ID="txtPwdAnswer" runat="server" />
                        <ajax:TextBoxWatermarkExtender runat="server" ID="tbwPwdAnswer" TargetControlID="txtPwdAnswer" WatermarkText="Answer" />
                        <br style="clear: both;" />
                        <asp:LinkButton ID="lbResetPassword" runat="server" OnCommand="btnResetPassword_Command" CommandArgument='<%# Eval("UserName") %>' CommandName="ResetPassword" Text="Reset Password" /><br />
                        <asp:LinkButton ID="lbDelete" runat="server" OnCommand="btnDeleteUser_Command" CommandArgument='<%# Eval("UserName") %>' CommandName="DeleteUser" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this User? This cannot be undone.');" />
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
        
        <asp:SqlDataSource ID="dsUsers" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MainConnectionString %>" DataSourceMode="DataSet" 
            SelectCommand="SELECT U.UserName, U.LastActivityDate, M.PasswordQuestion,
                        Name = S.FirstName + ' ' + S.MiddleName + ' ' + S.LastName,
                        FirstName = S.FirstName, MiddleName = S.MiddleName, LastName = S.LastName
                        FROM aspnet_Users AS U 
                        LEFT OUTER JOIN aspnet_Membership AS M 
                        ON M.UserId = U.UserId 
                        LEFT OUTER JOIN RegisteredUser As R
                        ON R.USERNAME = U.UserName
                        LEFT OUTER JOIN SimpleUser AS S
                        ON S.ID = R.UserID
                        GROUP BY U.UserName, U.LastActivityDate, M.PasswordQuestion, S.FirstName, S.MiddleName, S.LastName
                        ORDER BY U.UserName"
            FilterExpression="UserName LIKE '%{0}%' OR Name LIKE '%{0}%'">
            <FilterParameters>
                <asp:ControlParameter ControlID="UserSearchTextBox" Name="UserNameParam" ConvertEmptyStringToNull="True" />
            </FilterParameters>
        </asp:SqlDataSource>
            
        <br /><br />
        
    </asp:PlaceHolder>

</asp:Content>