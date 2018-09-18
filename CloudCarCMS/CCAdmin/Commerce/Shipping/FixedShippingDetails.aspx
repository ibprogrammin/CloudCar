<%@ Page Title="" Language="vb" AutoEventWireup="false" MaintainScrollPositionOnPostback="true" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="FixedShippingDetails.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.Shipping.FixedShippingDetails" %>

<%@ Import Namespace="CloudCar.CCFramework.Commerce" %>
<%@ Import Namespace="CloudCar.CCFramework.Core" %>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">

    <h1>Shipping Zones</h1>

    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" Visible="false" />
    <br /><br />

    <asp:HiddenField runat="server" ID="hfZoneID" />
    <fieldset>
        <p>
            <label>Prefix Low</label><br />
            <asp:TextBox runat="server" ID="txtPrefixLow" />
        </p>
        <p>
            <label>Prefix High</label><br />
            <asp:TextBox runat="server" ID="txtPrefixHigh" />
        </p>
        <p>
            <label>Zone</label><br />
            <asp:TextBox runat="server" ID="txtZoneSZ" />
        </p>
        <p>
            <label>Province</label><br />
            <asp:DropDownList runat="server" ID="ddlProvine" DataTextField="Name" DataValueField="ID" />
        </p>
        <p>
            <label>Distributor</label><br />
            <asp:DropDownList runat="server" ID="ddlDistributor" DataTextField="UserName" DataValueField="UserID" />
        </p>
    </fieldset>
    <br />
    <asp:LinkButton id="btnAddZone" runat="server" CssClass="GreenButton" ValidationGroup="Zone" CausesValidation="true" style="width: 250px; float: left;"><span class="GreenButton">Save</span></asp:LinkButton>
    <asp:LinkButton id="btnClearZone" runat="server" CssClass="RedButton" style="margin-left: 20px; width: 250px; float: left;"><span class="RedButton">Clear</span></asp:LinkButton>

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

    <asp:DataGrid runat="server" ID="dgZones" DataKeyField="id" AllowCustomPaging="false" DataKeyNames="id" AllowSorting="True" AutoGenerateColumns="False" 
            PageSize="10" AllowPaging="true" GridLines="None" CssClass="ProductDisplayTable">
        <PagerStyle CssClass="PagerStyle" PageButtonCount="8" Mode="NumericPages" Position="Bottom" HorizontalAlign="Right" />
        <Columns>
            <asp:TemplateColumn>
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnSelectZone" OnCommand="btnSelectZone_Command" CommandArgument='<%# Eval("id") %>' CommandName="Select" Text="Edit" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn HeaderText="Prefix Low" DataField="PrefixLow" />
            <asp:BoundColumn HeaderText="Prefix High" DataField="PrefixHigh" />
            <asp:BoundColumn HeaderText="Zone" DataField="Zone" />
            <asp:TemplateColumn HeaderText="Province">
                <ItemTemplate>
                    <asp:Literal runat="server" Text='<%# ProvinceController.GetProvinceName(Eval("ProvinceID")) %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Distributor">
                <ItemTemplate>
                    <asp:Literal runat="server" Text='<%# RegisteredUserController.GetUserNameByID(Eval("DistributorUserID")) %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnDeleteZone" OnClientClick="return confirm('Are you sure you want to delete this Zone?');" OnCommand="btnDeleteZone_Command" CommandArgument='<%# Eval("ID") %>' CommandName="Delete" Text="Delete" />
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

    <br style="clear: both;" /><br />
    
    <h1>Shipping Rates</h1>

    <p>These are the product that will be used when the user sets a given number of rooms.</p><br /><br />

    <asp:HiddenField runat="server" ID="hfRateID" />
    <fieldset>
        <p>
            <label>Weight (lbs.)</label><br />
            <asp:TextBox runat="server" ID="txtWeightLBS" />
        </p>
        <p>
            <label>Weight (Kg)</label><br />
            <asp:TextBox runat="server" ID="txtWeightKG" />
        </p>
        <p>
            <label>Zone</label><br />
            <asp:TextBox runat="server" ID="txtZoneSR" />
        </p>
        <p>
            <label>Cost</label><br />
            <asp:TextBox runat="server" ID="txtCost" />
        </p>
    </fieldset>
    <br />
    <asp:LinkButton id="btnAddRate" runat="server" CssClass="GreenButton" ValidationGroup="Product" CausesValidation="true" style="width: 250px; float: left;"><span class="GreenButton">Save</span></asp:LinkButton>
    <asp:LinkButton id="btnClearRate" runat="server" CssClass="RedButton" style="margin-left: 20px; width: 250px; float: left;"><span class="RedButton">Clear</span></asp:LinkButton>

    <asp:RequiredFieldValidator ID="rfvWeightLBS" runat="server" ControlToValidate="txtWeightLBS" Display="None" ErrorMessage="Please enter a weight in pounds" ValidationGroup="Product" />
    <ajax:ValidatorCalloutExtender ID="vceWeightLBS" runat="server" TargetControlID="rfvWeightLBS" PopupPosition="TopLeft" />  
    
    <asp:RequiredFieldValidator ID="rfvWeightKG" runat="server" ControlToValidate="txtWeightKG" Display="None" ErrorMessage="Please enter a weight in kilograms" ValidationGroup="Product" />
    <ajax:ValidatorCalloutExtender ID="vceWeightKG" runat="server" TargetControlID="rfvWeightKG" PopupPosition="TopLeft" />  
    
    <asp:RequiredFieldValidator ID="rfvZoneSR" runat="server" ControlToValidate="txtZoneSR" Display="None" ErrorMessage="Please enter a zone number" ValidationGroup="Product" />
    <ajax:ValidatorCalloutExtender ID="vceZoneSR" runat="server" TargetControlID="rfvZoneSR" PopupPosition="TopLeft" />  
    
    <asp:RequiredFieldValidator ID="rfvCost" runat="server" ControlToValidate="txtCost" Display="None" ErrorMessage="Please enter the cost" ValidationGroup="Product" />
    <ajax:ValidatorCalloutExtender ID="vceCost" runat="server" TargetControlID="rfvCost" PopupPosition="TopLeft" />    
    
    <asp:DataGrid runat="server" ID="dgRates" DataKeyField="id" AllowCustomPaging="false" DataKeyNames="id" AllowSorting="True" 
            AutoGenerateColumns="False" PageSize="10" AllowPaging="true" GridLines="None" CssClass="ProductDisplayTable">
        <PagerStyle CssClass="PagerStyle" PageButtonCount="8" Mode="NumericPages" Position="Bottom" HorizontalAlign="Right" />
        <Columns>
            <asp:TemplateColumn>
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnSelectRate" OnCommand="btnSelectRate_Command" CommandArgument='<%# Eval("id") %>' CommandName="Select" Text="Edit" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn HeaderText="Weight (lbs.)" DataField="WeightLBS" />
            <asp:BoundColumn HeaderText="Weight (Kg)" DataField="WeightKGS" />
            <asp:BoundColumn HeaderText="Zone" DataField="Zone" />
            <asp:BoundColumn HeaderText="Cost" DataField="Cost" />
            <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnDeleteRate" OnClientClick="return confirm('Are you sure you want to delete this Rate?');" OnCommand="btnDeleteRate_Command" CommandArgument='<%# Eval("ID") %>' CommandName="Delete" Text="Delete" />
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    

    <br style="clear: both;" /><br />

</asp:Content>