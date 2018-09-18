<%@ Page Title="Product Sales" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="ProductSalesReport.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.Reports.ProductSalesReport" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Product Sales
        <i class="icon-bar-chart"></i>
    </h1><hr />

    <div class="search-bar" >
        <span style="width: 180px;">Select Year</span>
        <asp:DropDownList runat="server" ID="ddlYearSelection" style="width: 220px;" />
        <asp:Button ID="btnGo" runat="server" CssClass="SaveButton float-right" style="width: 220px;" text="Go" />
    </div>
    
    <span class="float-right download-csv">Download: <asp:LinkButton runat="server" ID="btnDownloadCsv" Text="" CssClass="csv-icon" /></span> 

    <asp:Literal runat="server" ID="litYear" Visible="False" />
    
    <asp:ListView ID="lvBranchMonths" runat="server" OnItemDataBound="lvBranchMonths_ItemDataBound" Visible="false">
        <LayoutTemplate>
            <div id="itemPlaceholder" runat="server"></div>
        </LayoutTemplate>
        <ItemTemplate>

                <asp:HiddenField runat="server" ID="hfMonth" Value='<%# Eval("Month") %>' />
                <h3 style="margin-left: 30px;"><asp:Literal ID="litMonth" runat="server" Text='<%# MonthName(CInt(Eval("Month"))) & " " & CStr(Eval("Year")) %>' /></h3>
                    
                <asp:DataGrid runat="server" 
                        ID="dgSalesBreakdown" 
                        AutoGenerateColumns="false" 
                        AllowSorting="true" 
                        GridLines="None" 
                        CssClass="default-table">
                    <HeaderStyle CssClass="default-table-header" />
                    <Columns>
                        <asp:BoundColumn DataField="Product" SortExpression="Product" HeaderText="Product" ItemStyle-Width="148px" />
                        <asp:BoundColumn DataField="Quantity" SortExpression="Quantity" HeaderText="Quantity" ItemStyle-Width="148px" ItemStyle-HorizontalAlign="center" HeaderStyle-HorizontalAlign="Center" />
                        <asp:BoundColumn DataField="Cost" SortExpression="Cost" HeaderText="Cost" ItemStyle-Width="148px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                        <asp:BoundColumn DataField="Price" SortExpression="Price" HeaderText="Price" ItemStyle-Width="148px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                        <asp:BoundColumn DataField="Total" SortExpression="Total" HeaderText="Total" ItemStyle-Width="148px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                    </Columns>
                </asp:DataGrid>
        </ItemTemplate>
    </asp:ListView>

</asp:Content>