<%@ Page Title="Sales Request" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="SalesRequest.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.SalesRequest" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Sales Request
    </h1><hr />
    <hr />
    
    <asp:DataGrid runat="server" ID="dgSalesRequests" AutoGenerateColumns="false" DataKeyField="ID" AllowSorting="true" AllowPaging="true" 
            GridLines="None" CssClass="default-table" CellPadding="0" CellSpacing="0">
        <PagerStyle HorizontalAlign="Right" PageButtonCount="8" Mode="NumericPages" Position="Bottom" CssClass="default-table-pager" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:TemplateColumn HeaderText="Sales Rep">
                <ItemTemplate>
                    <a href="mailto:<%# Eval("SalesRepEmail") %>"><%#Eval("SalesRepName")%></a>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Customer">
                <ItemTemplate>
                    <a href="mailto:<%# Eval("CustomerEmail") %>"><%#Eval("CustomerName")%></a>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Product">
                <ItemTemplate>
                    <asp:Literal runat="server" Text='<%# Eval("Product.Name") %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="RequestKey" HeaderText="Key" ItemStyle-HorizontalAlign="Left" />
            <asp:TemplateColumn HeaderText="Redeemed">
                <ItemTemplate>
                    <asp:CheckBox runat="server" Checked='<%# Eval("Redeemed") %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="8%">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnSendNotice" OnClientClick="return confirm('Are you sure you want to reissue an email to the customer?');" OnCommand="btnSendNotice_Command" CommandArgument='<%# Eval("ID") %>' CommandName="Send" Text="Notify" />
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

</asp:Content>