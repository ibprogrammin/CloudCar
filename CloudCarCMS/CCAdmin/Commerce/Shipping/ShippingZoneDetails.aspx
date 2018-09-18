<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="ShippingZoneDetails.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.Shipping.ShippingZoneDetails" %>
<%@ Import Namespace="CloudCar.CCFramework.Commerce" %>
<%@ Import Namespace="CloudCar.CCFramework.Core" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Shipping Zones
        <i class="icon-truck"></i>
    </h1>
    <hr />
    
    <asp:LinkButton runat="server" ID="btnDisplayUnusedPostalCodes" Text="View Unused Postal Codes" /> - May take a few seconds to process
    <br class="clear-both" /><br />

    <asp:Label runat="server" ID="lblPrefixList" CssClass="status-message" Visible="false" />
    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" Visible="false" />

    <asp:HiddenField runat="server" ID="hfZoneID" />
    
    <div class="tab-container">
	    <ul class="tabs">
		    <li class="tab"><a href="#tab-list">Zone List</a></li>
            <li class="tab"><a href="#tab-new">New Zone</a></li>
		    <li class="tab"><a href="#tab-test">Test Zone</a></li>
	    </ul>
        <div id="tab-list">
            <label>Distributor</label>
            <asp:DropDownList runat="server" ID="ddlSelectDistributor" DataTextField="UserName" DataValueField="UserID" AutoPostBack="true" />
            <br class="clear-both" /><br />

            <asp:GridView runat="server" ID="ZonesGridView" DataKeyField="id" AllowCustomPaging="false" DataKeyNames="id" AllowSorting="True" AutoGenerateColumns="False" 
                    PageSize="12" AllowPaging="true" GridLines="None" CssClass="default-table">
                <PagerStyle HorizontalAlign="Right" CssClass="default-table-pager" />
                <HeaderStyle CssClass="default-table-header" />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="btnSelectZone" OnCommand="btnSelectZone_Command" CommandArgument='<%# Eval("id") %>' CommandName="Select" Text="" cssclass="icon-edit edit-icon" PostBackUrl="~/CCAdmin/Commerce/Shipping/ShippingZoneDetails.aspx#tab-new" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Prefix Low" DataField="PrefixLow" />
                    <asp:BoundField HeaderText="Prefix High" DataField="PrefixHigh" />
                    <asp:TemplateField HeaderText="Province">
                        <ItemTemplate>
                            <asp:Literal ID="Literal1" runat="server" Text='<%# ProvinceController.GetProvinceName(CType(Eval("ProvinceID"), Integer)) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Distributor">
                        <ItemTemplate>
                            <asp:Literal ID="Literal2" runat="server" Text='<%# RegisteredUserController.GetUserNameByID(CType(Eval("DistributorUserID"), Integer)) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Zone" DataField="Zone" ItemStyle-CssClass="align-center" HeaderStyle-CssClass="align-center" />
                    <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="btnDeleteZone" OnClientClick="return confirm('Are you sure you want to delete this Zone?');" OnCommand="btnDeleteZone_Command" CommandArgument='<%# Eval("ID") %>' CommandName="Delete" Text="" CssClass="icon-trash delete-icon" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div id="tab-new">
            <label>Prefix Low</label>
            <asp:TextBox runat="server" ID="txtPrefixLow" CssClass="form-text-box" />
            <br class="clear-both" /><br />
        
            <label>Prefix High</label>
            <asp:TextBox runat="server" ID="txtPrefixHigh" CssClass="form-text-box" />
            <br class="clear-both" /><br />
        
            <label>Zone</label>
            <asp:TextBox runat="server" ID="txtZoneSZ" CssClass="form-text-box" />
            <br class="clear-both" /><br />
        
            <label>Province</label>
            <asp:DropDownList runat="server" ID="ddlProvine" DataTextField="Name" DataValueField="ID" />
            <br class="clear-both" /><br />

            <label>Distributor</label>
            <asp:DropDownList runat="server" ID="ddlDistributor" DataTextField="UserName" DataValueField="UserID" EnableViewState="true" />
            <br class="clear-both" /><br />
            
            <br class="clear-both" />
    
            <asp:Button id="btnAddZone" runat="server" CssClass="SaveButton" ValidationGroup="Zone" CausesValidation="true" Text="Save" PostBackUrl="~/CCAdmin/Commerce/Shipping/ShippingZoneDetails.aspx#tab-new" />
            <asp:Button id="btnClearZone" runat="server" CssClass="DeleteButton" Text="Clear" PostBackUrl="~/CCAdmin/Commerce/Shipping/ShippingZoneDetails.aspx#tab-new" />
            
            <br class="clear-both" />
        </div>
        <div id="tab-test">
            
            <label>Test Postal Code</label>
            <asp:TextBox runat="server" ID="txtTestPostalCode" CssClass="form-text-box" />
            <br class="clear-both" /><br />

            <asp:Button id="btnTestPostalCode" runat="server" CssClass="SaveButton" Text="Run Test" style="margin-left: 250px;" PostBackUrl="~/CCAdmin/Commerce/Shipping/ShippingZoneDetails.aspx#tab-test" />
            <br class="clear-both" /><br />

            <asp:Label runat="server" ID="lblTestPostalCode" CssClass="status-message" Visible="false" />

        </div>
    </div>

    <asp:RequiredFieldValidator ID="rfvPrefixLow" runat="server" ControlToValidate="txtPrefixLow" Display="None" ErrorMessage="Please enter a prefix" ValidationGroup="Zone" />
    <ajax:ValidatorCalloutExtender ID="vcePrefixLow" runat="server" TargetControlID="rfvPrefixLow" PopupPosition="TopLeft" />
    
    <asp:RequiredFieldValidator ID="rfvPrefixHigh" runat="server" ControlToValidate="txtPrefixHigh" Display="None" ErrorMessage="Please enter a prefix" ValidationGroup="Zone" />
    <ajax:ValidatorCalloutExtender ID="vcePrefixHigh" runat="server" TargetControlID="rfvPrefixHigh" PopupPosition="TopLeft" />
    
    <asp:RequiredFieldValidator ID="rfvZoneSZ" runat="server" ControlToValidate="txtZoneSZ" Display="None" ErrorMessage="Please enter a zone" ValidationGroup="Zone" />
    <ajax:ValidatorCalloutExtender ID="vceZoneSZ" runat="server" TargetControlID="rfvZoneSZ" PopupPosition="TopLeft" />
    
    <asp:RequiredFieldValidator ID="rfvProvine" runat="server" ControlToValidate="ddlProvine" Display="None" ErrorMessage="Please select a province" ValidationGroup="Zone" />
    <ajax:ValidatorCalloutExtender ID="vceProvine" runat="server" TargetControlID="rfvProvine" PopupPosition="TopLeft" />
    
    <asp:RequiredFieldValidator ID="rfvDistributor" runat="server" ControlToValidate="ddlDistributor" Display="None" ErrorMessage="Please select a distributor" ValidationGroup="Zone" />
    <ajax:ValidatorCalloutExtender ID="vceDistributor" runat="server" TargetControlID="rfvDistributor" PopupPosition="TopLeft" />

</asp:Content>