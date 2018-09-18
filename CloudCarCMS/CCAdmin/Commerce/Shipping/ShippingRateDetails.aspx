<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="ShippingRateDetails.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.Shipping.ShippingRateDetails" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Shipping Rates
        <i class="icon-truck"></i>
    </h1>
    <hr />

    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" Visible="false" />

    <asp:HiddenField runat="server" ID="hfRateID" />
    
    <div class="tab-container">
	    <ul class="tabs">
		    <li class="tab"><a href="#tab-list">Rate List</a></li>
            <li class="tab"><a href="#tab-new">New Rate</a></li>
	    </ul>
        <div id="tab-list">
            <asp:GridView runat="server" 
                    ID="RatesGridView" 
                    DataKeyField="id" 
                    AllowCustomPaging="false" 
                    DataKeyNames="id" 
                    AllowSorting="True" 
                    AutoGenerateColumns="False" 
                    PageSize="12" 
                    AllowPaging="true" 
                    GridLines="None" 
                    CssClass="default-table">
                <PagerStyle 
                    HorizontalAlign="Right" 
                    CssClass="default-table-pager" />
                <HeaderStyle 
                    CssClass="default-table-header" />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="btnSelectRate" PostBackUrl="~/CCAdmin/Commerce/Shipping/ShippingRateDetails.aspx#tab-new" OnCommand="btnSelectRate_Command" CommandArgument='<%# Eval("id") %>' CommandName="Select" Text="" CssClass="icon-edit edit-icon" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Weight (lbs.)" DataField="WeightLBS" ItemStyle-CssClass="align-center" HeaderStyle-CssClass="align-center" />
                    <asp:BoundField HeaderText="Weight (Kg)" DataField="WeightKGS" ItemStyle-CssClass="align-center" HeaderStyle-CssClass="align-center" />
                    <asp:BoundField HeaderText="Zone" DataField="Zone" ItemStyle-CssClass="align-center" HeaderStyle-CssClass="align-center" />
                    <asp:BoundField HeaderText="Cost" DataField="Cost" ItemStyle-CssClass="align-right" HeaderStyle-CssClass="align-right" />
                    <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="btnDeleteRate" OnClientClick="return confirm('Are you sure you want to delete this Rate?');" OnCommand="btnDeleteRate_Command" CommandArgument='<%# Eval("ID") %>' CommandName="Delete" Text="" CssClass="icon-trash delete-icon" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
    
            <br class="clear-both" />
        </div>
        <div id="tab-new">
            <label>Weight (lbs.)</label>
            <asp:TextBox runat="server" ID="txtWeightLBS" />
            <br class="clear-both" /><br />

            <label>Weight (Kg)</label>
            <asp:TextBox runat="server" ID="txtWeightKG" />
            <br class="clear-both" /><br />

            <label>Zone</label>
            <asp:TextBox runat="server" ID="txtZoneSR" />
            <br class="clear-both" /><br />

            <label>Cost</label>
            <asp:TextBox runat="server" ID="txtCost" />
            <br class="clear-both" /><br />
    
            <asp:Button id="btnAddRate" runat="server" CssClass="SaveButton" ValidationGroup="Product" CausesValidation="true" Text="Save" PostBackUrl="~/CCAdmin/Commerce/Shipping/ShippingRateDetails.aspx#tab-new" />
            <asp:Button id="btnClearRate" runat="server" CssClass="DeleteButton" Text="Clear" PostBackUrl="~/CCAdmin/Commerce/Shipping/ShippingRateDetails.aspx#tab-new" />

            <br class="clear-both" />
        </div>
    </div>

    <asp:RequiredFieldValidator ID="rfvWeightLBS" runat="server" ControlToValidate="txtWeightLBS" Display="None" ErrorMessage="Please enter a weight in pounds" ValidationGroup="Product" />
    <ajax:ValidatorCalloutExtender ID="vceWeightLBS" runat="server" TargetControlID="rfvWeightLBS" PopupPosition="TopLeft" />  
    
    <asp:RequiredFieldValidator ID="rfvWeightKG" runat="server" ControlToValidate="txtWeightKG" Display="None" ErrorMessage="Please enter a weight in kilograms" ValidationGroup="Product" />
    <ajax:ValidatorCalloutExtender ID="vceWeightKG" runat="server" TargetControlID="rfvWeightKG" PopupPosition="TopLeft" />  
    
    <asp:RequiredFieldValidator ID="rfvZoneSR" runat="server" ControlToValidate="txtZoneSR" Display="None" ErrorMessage="Please enter a zone number" ValidationGroup="Product" />
    <ajax:ValidatorCalloutExtender ID="vceZoneSR" runat="server" TargetControlID="rfvZoneSR" PopupPosition="TopLeft" />  
    
    <asp:RequiredFieldValidator ID="rfvCost" runat="server" ControlToValidate="txtCost" Display="None" ErrorMessage="Please enter the cost" ValidationGroup="Product" />
    <ajax:ValidatorCalloutExtender ID="vceCost" runat="server" TargetControlID="rfvCost" PopupPosition="TopLeft" />    

</asp:Content>