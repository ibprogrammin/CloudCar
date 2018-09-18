<%@ Page Title="Branch Sales" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="MonthlyBranchReport.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.Reports.MonthlyBranchReport" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Branch Sales
        <i class="icon-bar-chart"></i>
    </h1><hr />
    
    <div class="search-bar">
        <span style="width: 180px;">Select Year</span>
        <asp:DropDownList runat="server" ID="ddlYearSelection" />
        <asp:Button ID="btnGo" runat="server" CssClass="SaveButton float-right" style="width: 140px;" text="Go" />
    </div>
    
    <br class="clear-both" /><br />
   
    <h2 style="margin: 0px;"><asp:Literal runat="server" ID="litYear" /></h2>
    Download Report: <asp:LinkButton runat="server" ID="btnDownloadCsv" Text=".csv" />
   
    <div style="margin: 0px -20px; margin-left: -40px; padding: 20px 20px;">
        
        <asp:ListView ID="lvBranchMonths" runat="server" OnItemDataBound="lvBranchMonths_ItemDataBound" Visible="false">
            <LayoutTemplate>
                <div id="itemPlaceholder" runat="server"></div>
            </LayoutTemplate>
            <ItemTemplate>
                <div style="background-color: #F7F7F7; padding: 20px 20px;">
                    <asp:HiddenField runat="server" ID="hfMonth" Value='<%# Eval("Month") %>' />
                    <h3 style="margin-left: 10px;"><asp:Literal ID="litMonth" runat="server" Text='<%# MonthName(CInt(Eval("Month"))) %>' /></h3>
                    
                    <asp:DataGrid runat="server" ID="dgSalesBreakdown" AutoGenerateColumns="false" AllowSorting="true" GridLines="None" 
                            CssClass="default-table">
                        <HeaderStyle CssClass="default-table-header" />
                        <Columns>
                            <asp:BoundColumn DataField="Branch" SortExpression="Branch" HeaderText="Branch" ItemStyle-Width="148px" />
                            <asp:BoundColumn DataField="SalesRep" SortExpression="SalesRep" HeaderText="Rep" ItemStyle-Width="148px" />
                            <asp:BoundColumn DataField="OrderId" SortExpression="OrderId" HeaderText="Order" ItemStyle-Width="68px" />
                            <asp:BoundColumn DataField="Cost" SortExpression="Cost" DataFormatString="{0:C}" HeaderText="Cost" ItemStyle-Width="148px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                            <asp:BoundColumn DataField="Price" SortExpression="Price" DataFormatString="{0:C}" HeaderText="Sales" ItemStyle-Width="148px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                            <asp:BoundColumn DataField="Discount" SortExpression="Discount" DataFormatString="{0:C}" HeaderText="Discounts" ItemStyle-Width="148px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                            <asp:BoundColumn DataField="Shipping" SortExpression="Shipping" DataFormatString="{0:C}" HeaderText="Shipping" ItemStyle-Width="148px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                            <asp:BoundColumn DataField="Profit" SortExpression="Profit" DataFormatString="{0:C}" HeaderText="Profit" ItemStyle-Width="148px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                        </Columns>
                    </asp:DataGrid>
                    
                    <table class="default-table" style="background-color: #EEE; padding: 0px;">
                        <tr>
                            <td style="width: 148px;">Total</td>
                            <td style="width: 148px;">&nbsp;</td>
                            <td style="width: 68px;">&nbsp;</td>
                            <td style="width: 148px; text-align: right;"><asp:Literal runat="server" ID="litCost" /></td>
                            <td style="width: 148px; text-align: right;"><asp:Literal runat="server" ID="litSales" /></td>
                            <td style="width: 148px; text-align: right;"><asp:Literal runat="server" ID="litDiscounts" /></td>
                            <td style="width: 148px; text-align: right;"><asp:Literal runat="server" ID="litShipping" /></td>
                            <td style="width: 148px; text-align: right;"><asp:Literal runat="server" ID="litProfit" /></td>
                        </tr>
                    </table>

                </div>
            </ItemTemplate>
        </asp:ListView>
        
        <table class="default-table" style="margin-left: 20px; margin-right: 20px; padding: 0px;">
            <tr>
                <td style="width: 148px;">Yearly Total</td>
                <td style="width: 148px;">&nbsp;</td>
                <td style="width: 68px;">&nbsp;</td>
                <td class="align-right" style="width: 148px;"><asp:Literal runat="server" ID="litYearlyCost" /></td>
                <td class="align-right" style="width: 148px;"><asp:Literal runat="server" ID="litYearlySales" /></td>
                <td class="align-right" style="width: 148px;"><asp:Literal runat="server" ID="litYearlyDiscounts" /></td>
                <td class="align-right" style="width: 148px;"><asp:Literal runat="server" ID="litYearlyShipping" /></td>
                <td class="align-right" style="width: 148px;"><asp:Literal runat="server" ID="litYearlyProfit" /></td>
            </tr>
        </table>
        
    </div>

</asp:Content>