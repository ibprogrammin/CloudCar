<%@ Page Title="Settings" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Settings.aspx.vb" Inherits="CloudCar.CCAdmin.Settings" %>
<%@ Import Namespace="CloudCar.CCFramework.Core" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Settings
        <i class="icon-gears"></i>
    </h1>
    <hr />
    
    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" Visible="false" />
    
    <div class="tab-container">
	    
	    <asp:Repeater runat="server" ID="RepeaterTabs">
	        <HeaderTemplate>
	            <ul class="tabs">
	        </HeaderTemplate>
            <ItemTemplate>
                <li class="tab"><a href="#tab-<%#Container.DataItem.ToString().Replace(Container.DataItem.ToString.SubString(0,3), "").Replace(" ","-")%>"><%#Container.DataItem.ToString().Replace(Container.DataItem.Tostring.SubString(0,3), "")%></a></li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
	
        <asp:Repeater runat="server" ID="RepeaterTabContent">
            <ItemTemplate>
                <div id="tab-<%#Container.DataItem.ToString().Replace(Container.DataItem.Tostring().SubString(0,3), "").Replace(" ","-")%>">

                    <asp:HiddenField runat="server" ID="hfCategory" Value='<%#Container.DataItem %>' />
        
                    <asp:Repeater runat="server" ID="rptCategoryItems" 
                            DataSource='<%# if(Roles.IsUserInRole(Membership.GetUser.Username, "Super User"), SettingController.GetSettingByCategory(CType(Container.DataItem, String)), SettingController.GetReadableSettingByCategory(CType(Container.DataItem, String))) %>'>
                        <ItemTemplate>
                            <label><%#Eval("Display")%></label>
                            <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("ID") %>' />
                            <asp:HiddenField runat="server" ID="hfKey" Value='<%# Eval("Key") %>' />
                            <asp:HiddenField runat="server" ID="hfOldValue" Value='<%# Eval("Value") %>' />
                            <asp:TextBox runat="server" ID="txtValue" Text='<%# Eval("Value") %>' CssClass="display-message" />
                        </ItemTemplate>
                        <SeparatorTemplate>
                            <br class="clear-both" /><br />
                        </SeparatorTemplate>
                        <FooterTemplate>
                            <br class="clear-both" />
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </ItemTemplate>
        </asp:Repeater>

    </div>

    <asp:Repeater runat="server" ID="rptSettings">
        <ItemTemplate>
            <fieldset>
                <h2 class="form-heading-style"><%#Container.DataItem%></h2><br />
        
                <asp:HiddenField runat="server" ID="hfCategory" Value='<%#Container.DataItem %>' />
        
                <asp:Repeater runat="server" ID="rptCategoryItems" 
                        DataSource='<%# if(Roles.IsUserInRole(Membership.GetUser.Username, "Super User"), SettingController.GetSettingByCategory(CType(Container.DataItem, String)), SettingController.GetReadableSettingByCategory(CType(Container.DataItem, String))) %>'>
                    <ItemTemplate>
                        <label><%# Eval("Display")%></label>
                        <asp:HiddenField runat="server" ID="hfID" Value='<%# Eval("ID") %>' />
                        <asp:HiddenField runat="server" ID="hfKey" Value='<%# Eval("Key") %>' />
                        <asp:HiddenField runat="server" ID="hfOldValue" Value='<%# Eval("Value") %>' />
                        <asp:TextBox runat="server" ID="txtValue" Text='<%# Eval("Value") %>' CssClass="display-message" />
                        
                        <br class="clear-both" /><br />
                    </ItemTemplate>
                </asp:Repeater>
            
            </fieldset>
            
            <br class="clear-both" />
        </ItemTemplate>
    </asp:Repeater>
    
    <br class="clear-both" />

    <asp:Button id="SaveButton" runat="server" CssClass="SaveButton" CausesValidation="true" ValidationGroup="Validate" Text="Save" />

    <br class="clear-both" />

</asp:Content>