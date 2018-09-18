<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="AddContactSmallForm.aspx.vb" Inherits="SMECommerceTemplate.AddContactSmallForm" %>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
    
    <h1>Join our mailinglist!</h1>
    <p>Fill out the form below to recieve the latest updates on our rates.</p>
    <br /><br />
    
    <label>Email Address</label><br />
    <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" TabIndex="1" style="width: 295px;" /><br /><br />
    
    <label>First Name</label><br />
    <asp:TextBox ID="txtFirst" runat="server" MaxLength="50" TabIndex="2" style="width: 295px;" /><br /><br />
    
    <label>Last Name</label><br />
    <asp:TextBox ID="txtLast" runat="server" MaxLength="50" TabIndex="3" style="width: 295px;" /><br /><br />
    
    <label>Middle Name</label><br />
    <asp:TextBox ID="txtMiddle" runat="server" MaxLength="50" TabIndex="4" style="width: 295px;" /><br /><br />
                        
    <label>Home Phone</label><br />
    <asp:TextBox ID="txtHome" runat="server" MaxLength="50" TabIndex="5" style="width: 295px;" /><br /><br />
    
    <br style="clear: both;" />
    <hr />
       
    <div style="margin-left: 40px; margin-right: 20px;">
            
        <div style="width: 470px; float: right;">
            <p>Please select the areas of interest for which you would like to receive occasional email from us.</p>
            
            <asp:Panel ID="contactListsPanel" runat="server" BackColor="White" BorderColor="Green"
                BorderWidth="1px" Height="348px" Width="470px" ScrollBars="Both">
                <asp:CheckBoxList ID="chkListContactLists" runat="server" Width="300px" DataTextField="Name"
                    DataValueField="Id" CellSpacing="5" Font-Names="Calibri" Font-Size="11pt">
                </asp:CheckBoxList>
            </asp:Panel>
        </div>
        
        <label>Address</label><br />
        <asp:TextBox ID="txtAddr1" runat="server" MaxLength="50" TabIndex="6" style="width: 295px;" /><br /><br />
        <asp:TextBox ID="txtAddr2" runat="server" MaxLength="50" TabIndex="7" Enabled="false" Visible="false" />
        <asp:TextBox ID="txtAddr3" runat="server" MaxLength="50" TabIndex="8" Enabled="false" Visible="false" />
        
        <label>City</label><br />
        <asp:TextBox ID="txtCity" runat="server" MaxLength="50" TabIndex="9" style="width: 295px;" /><br /><br />
        
        <label>State/Province (US/Canada)</label><br />
        <asp:DropDownList ID="dropDownState" runat="server" DataTextField="Name" DataValueField="Code" TabIndex="10" style="width: 290px;" /><br /><br />
        
        <label>State/Province (Other)</label><br />
        <asp:TextBox ID="txtOtherState" runat="server" MaxLength="50" TabIndex="11" style="width: 295px;" /><br /><br />
        
        <label>Zip/Postal Code</label><br />
        <asp:TextBox ID="txtZip" runat="server" MaxLength="25" TabIndex="12" style="width: 295px;" /><br /><br />
        
        <asp:TextBox ID="txtSubZip" runat="server" MaxLength="25" TabIndex="13" Enabled="false" Visible="false" />

        <label>Country</label><br />
        <asp:DropDownList ID="dropDownCountry" runat="server" DataTextField="Name" DataValueField="Code" TabIndex="14" style="width: 290px;" /><br /><br />

        <asp:Button ID="btnAdd" runat="server" Text="Sign Up" OnClick="btnAdd_Click" CssClass="Green" />
        <asp:CustomValidator ID="customValidator" runat="server" Display="None" OnServerValidate="customValidator_ServerValidate" />

    </div>

</asp:Content>