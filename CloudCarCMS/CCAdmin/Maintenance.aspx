<%@ Page Title="Site Maintenance" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Maintenance.aspx.vb" Inherits="CloudCar.CCAdmin.Maintenance" %>
<%@ Import Namespace="CloudCar.CCFramework.Commerce" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Maintenance
        <i class="icon-wrench"></i>
    </h1>
    <hr />
    
    <asp:Label runat="server" ID="StatusMessageLabel" CssClass="status-message" Visible="false" />
    
    <fieldset>
                
        <h2 class="form-heading-style">Shopping Carts</h2><br /><br />
            
        <label>Total</label>
        <asp:Label runat="server" ID="TotalShoppingCartsLabel" CssClass="display-message" style="width: 290px;" />
        
        <br style="clear: left;" /><br />
        
        <label>Registered</label>
        <asp:Label runat="server" ID="RegisteredShoppingCartsLabel" CssClass="display-message" style="width: 290px;" />
        
        <br style="clear: left;" /><br />
        
        <label>Unregistered</label>
        <asp:Label runat="server" ID="UnregisteredShoppingCartsLabel" CssClass="display-message" style="width: 290px;" />
        
        <br style="clear: left;" /><br /><br />
        
        <p><b>Note:</b> Click the button below to delete all the unregistered shopping carts from the database.</p>
        
        <br style="clear: left;" />
        
        <asp:Button runat="server" ID="DeleteUnregisteredShoppingCartsButton" Text="Delete" OnClientClick="return confirm('Are you sure you wish to delete all of the unregistered shopping carts? Your changes cannot be undone.');" CssClass="DeleteButton" />
        
        <br style="clear: left;" /><br />
        
    </fieldset>
    
    <br class="clear-both" />
    
    <fieldset>
                
        <h2 class="form-heading-style">Images</h2><br /><br />
            
        <label>Total</label>
        <asp:Label runat="server" ID="TotalImagesLabel" CssClass="display-message" style="width: 290px;" />
        
        <br style="clear: left;" /><br />
        
        <label>Linked</label>
        <asp:Label runat="server" ID="UsedImagesLabel" CssClass="display-message" style="width: 290px;" />
        
        <br style="clear: left;" /><br />
        
        <label>Unused</label>
        <asp:Label runat="server" ID="UnusedImagesLabel" CssClass="display-message" style="width: 290px;" />
        
        <br style="clear: left;" /><br />
        
        <label>Wasted Space (MB)</label>
        <asp:Label runat="server" ID="WaistedBytesLabel" CssClass="display-message" style="width: 290px;" />
        
        <br style="clear: left;" /><br /><br />
        
        <p><b>Note:</b> Click the button below to delete all the unused images from the database. If you would like to choose which images you would like to delete, click the checkbox next to the file name, then hit the delete button at the bottom of the list.</p>
        
        <br style="clear: left;" />
        
        <asp:Button runat="server" ID="DeleteUnusedImagesButton" Text="Delete All" OnClientClick="return confirm('Are you sure you wish to delete all of the unused images? Your changes cannot be undone.');" CssClass="DeleteButton" />
        
        <br class="clear-both" /><br />
        
        <asp:Repeater runat="server" ID="UnusedImagesRepeater">
            <HeaderTemplate>
                <div style="margin-left: 10px; margin-right: 20px; padding: 10px; border: 1px solid #DDD; background: #FEFEFE;">
                    <h3 style="margin-left: 0px; padding-left: 0px;">Unused Images</h3>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Checkbox runat="server" Checked="false" style="font-size: 10px; margin: 0px; padding: 0px; width: 280px; float: left;" PictureID='<%# Eval("PictureID") %>' Text='<%#Eval("PictureFileName") %>' />
            </ItemTemplate>
            <FooterTemplate>
                    <br class="clear-both" /><br />
                    <asp:Button runat="server" ID="DeleteSelectedImagesButton" Text="Delete" OnClientClick="return confirm('Are you sure you wish to delete these images? Your changes cannot be undone.');" CssClass="DeleteButton" OnClick="DeleteSelectedImagesButtonClick" />
                    <br class="clear-both" /><br />
                </div>
            </FooterTemplate>
        </asp:Repeater>
        
        <br class="clear-both" /><br />
        
    </fieldset>

    <br class="clear-both" />

    <fieldset>
                
        <h2 class="form-heading-style">Addresses</h2><br /><br />
            
        <label>Total</label>
        <asp:Label runat="server" ID="TotalAddressesLabel" CssClass="display-message" style="width: 290px;" />
        
        <br style="clear: left;" /><br />
        
        <label>Linked</label>
        <asp:Label runat="server" ID="LinkedAddressesLabel" CssClass="display-message" style="width: 290px;" />
        
        <br style="clear: left;" /><br />
        
        <label>Unused</label>
        <asp:Label runat="server" ID="UnusedAddressesLabel" CssClass="display-message" style="width: 290px;" />
        
        <br style="clear: left;" /><br />
        
        <br style="clear: left;" /><br /><br />
        
        <p style="width: 650px;"><b>Note:</b> Click the button below to delete all the unused addresses from the database. If you would like to choose which addresses you would like to delete, click the checkbox next to the file name, then hit the delete button at the bottom of the list.</p>
        
        <br style="clear: left;" />
        
        <asp:Button runat="server" ID="DeleteUnusedAddressesButton" Text="Delete All" OnClientClick="return confirm('Are you sure you wish to delete all of the unused addresses? Your changes cannot be undone.');" CssClass="DeleteButton" />
        
        <br style="clear: both;" /><br />
        
        
        <asp:Repeater runat="server" ID="UnusedAddressRepeater">
            <HeaderTemplate>
                <div style="margin-left: 10px; margin-right: 20px; padding: 10px; border: 1px solid #DDD; background: #FEFEFE;">
                    <h3 style="margin-left: 0px; padding-left: 0px;">Unused Addresses</h3>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Checkbox runat="server" Checked="false" style="font-size: 10px; margin: 0px; padding: 0px; width: 280px; float: left;" AddressID='<%# Eval("ID") %>' Text='<%#String.Format("{0}, {1}, {2}, {3}, {4}", Eval("Address"), Eval("City"), ProvinceController.GetProvinceName(CInt(Eval("ProvStateID"))), ProvinceController.GetProvinceCountryName(CInt(Eval("ProvStateID"))), Eval("PCZIP")) %>' />
            </ItemTemplate>
            <FooterTemplate>
                    <br style="clear: both;" /><br />
                    <asp:Button runat="server" ID="DeleteSelectedAddressButton" Text="Delete" OnClientClick="return confirm('Are you sure you wish to delete these addresses? Your changes cannot be undone.');" CssClass="DeleteButton" OnClick="DeleteSelectedAddressButtonClick" />
                    <br style="clear: both;" /><br />
                </div>
            </FooterTemplate>
        </asp:Repeater>
        
        <br class="clear-both" /><br />
        
    </fieldset>

</asp:Content>