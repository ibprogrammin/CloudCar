<%@ Page Title="Promo Codes" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="PromoCodes.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.PromoCodes" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Promo Codes
        <i class="icon-certificate"></i>
    </h1><hr />
    
    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" Visible="false" />
            
    <asp:PlaceHolder runat="server" ID="phDetails">
        
        <asp:HiddenField runat="server" ID="hfPromoCodeID" />

        <div class="tab-container">
	        <ul class="tabs">
	            <li class="tab"><a href="#tab-details">Details</a></li>
	        </ul>
            <div id="tab-details">
            
                <label>Code (Max. 8 Characters)</label>
                <asp:TextBox runat="server" ID="txtCode" MaxLength="8" CssClass="form-text-box" />
                <br class="clear-both" /><br />
            
                <label>Sales Rep.</label>
                <asp:TextBox runat="server" ID="txtSalesRep" CssClass="form-text-box" />
                <br class="clear-both" /><br />
            
                <label style="height: 110px;">Discount</label>
                <asp:TextBox runat="server" ID="txtDiscount" CssClass="form-text-box" /><br />

                <b>Note:</b> The Discount can be in dollars or a percentage, use a numerical value only, do not enter a dollar or percentage symbol.<br /><br />
                <asp:CheckBox runat="server" ID="ckbFixed" Text="" style="display: block; float: left;" />
                <label class="checkbox-label" style="width: 420px; margin-top: -10px;">&nbsp;Check if discount is a dollar amount</label>
                <br class="clear-both" />
            
            </div>
        </div>
    
        <br class="clear-both" />

        <asp:Button runat="server" ID="btnAdd" Text="Save" CausesValidation="true" ValidationGroup="ValidateForm" CssClass="SaveButton" />
        <asp:Button runat="server" ID="btnClear" Text="Clear" CausesValidation="false" CssClass="DeleteButton" />

        <br class="clear-both" /><hr />

        <asp:RequiredFieldValidator ID="rfvDiscount" runat="server" ControlToValidate="txtDiscount" Display="None" ErrorMessage="Please enter a discount amount for this Promo Code" ValidationGroup="ItemValidation" />
        <ajax:ValidatorCalloutExtender ID="vceDiscount" runat="server" TargetControlID="rfvDiscount" PopupPosition="TopLeft" />
        <ajax:FilteredTextBoxExtender ID="ftbDiscount" runat="server" TargetControlID="txtDiscount" FilterType="Custom, Numbers" ValidChars="." />
        
        <asp:RequiredFieldValidator ID="rfvSalesRep" runat="server" ControlToValidate="txtSalesRep" Display="None" ErrorMessage="Please enter a Sales Rep to associate with this Promo Code" ValidationGroup="ItemValidation" />
        <ajax:ValidatorCalloutExtender ID="vceSalesRep" runat="server" TargetControlID="rfvSalesRep" PopupPosition="TopLeft" />
        
    </asp:PlaceHolder>    

    <asp:DataGrid runat="server" 
            ID="dgPromoCodes" 
            DataKeyField="id" 
            AllowCustomPaging="false" 
            DataKeyNames="id" 
            AllowSorting="True" 
            AutoGenerateColumns="False" 
            PageSize="20" 
            AllowPaging="true" 
            GridLines="None" 
            CssClass="default-table">
        <PagerStyle HorizontalAlign="Right" PageButtonCount="8" Mode="NumericPages" Position="Bottom" CssClass="default-table-pager" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:TemplateColumn HeaderText="Code">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnSelect" OnCommand="btnSelect_Command" CommandArgument='<%# Eval("id") %>' CommandName="Select" Text='<%# Eval("Code") %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Dollar Amount" itemstyle-horizontalalign="center" HeaderStyle-HorizontalAlign="center">
                <ItemTemplate>
                    <asp:CheckBox runat="server" ID="ckbChecked" Checked='<%# Eval("FixedAmount") %>' style="float: none !important; display: block; width: 20px;" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Discount" itemstyle-horizontalalign="center" HeaderStyle-HorizontalAlign="center">
                <ItemTemplate>
                    <%# If(CBool(Eval("FixedAmount")), String.Format("${0}", CDec(Eval("Discount"))), String.Format("{0}%", CDec(Eval("Discount"))))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn HeaderText="Sales Rep" DataField="SalesRep" itemstyle-horizontalalign="center" HeaderStyle-HorizontalAlign="center" />
            <asp:TemplateColumn ItemStyle-HorizontalAlign="Right" ItemStyle-Width="6%">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnDelete" OnClientClick="return confirm('Are you sure you want to delete this Promo Code?');" CausesValidation="false" OnCommand="btnDelete_Command" CommandArgument='<%# Eval("ID") %>' CommandName="Delete" Text="" CssClass="icon-trash delete-icon" />
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

</asp:Content>