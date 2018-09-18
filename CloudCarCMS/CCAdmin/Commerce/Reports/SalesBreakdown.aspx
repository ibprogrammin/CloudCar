<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="SalesBreakdown.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.Reports.SalesBreakdown" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1>Monthly Sales Breakdown</h1>
    <br />
    <table class="ProductDisplayTable" width="960" cellspacing="0">
        <tr>
            <td><h3>Select Year</h3></td>
            <td><asp:DropDownList runat="server" ID="ddlYearSelection" /></td>
            <td><h3>Select Type</h3></td>
            <td><asp:DropDownList runat="server" ID="ddlReportType" /></td>
            <td><asp:LinkButton ID="btnGo" runat="server" CssClass="OrangeButton" style="position: relative; width: 140px; top: 15px;"><span class="OrangeButton">Go</span></asp:LinkButton></td>
            <td style="width: 240px;"></td>
        </tr>
    </table>
    
    Download Report: <asp:LinkButton runat="server" ID="btnDownloadCsv" Text=".csv" />
    
    <br style="clear: both;" /><br />
   
    <h2><asp:Literal runat="server" ID="litYear" /></h2>
   
    <div style="margin-bottom: 20px; padding: 30px;">
    
        <asp:ListView ID="lvSalesMonths" runat="server" OnItemDataBound="lvSalesMonths_ItemDataBound">
            <LayoutTemplate>
                <div id="itemPlaceholder" runat="server"></div>
            </LayoutTemplate>
            <ItemTemplate>
                <div style="background-color: #F7F7F7; padding: 30px;">
                    <asp:HiddenField runat="server" ID="hfMonth" Value='<%# Eval("Month") %>' />
                    <h3><asp:Literal ID="litMonth" runat="server" Text='<%# MonthName(CInt(Eval("Month"))) %>' /></h3>
                    
                    <asp:DataGrid runat="server" ID="dgSalesBreakdown" AutoGenerateColumns="false" AllowSorting="true" GridLines="None" 
                            CssClass="ProductDisplayTable" CellPadding="0" CellSpacing="0" style="width: 840px">
                        <Columns>
                            <asp:TemplateColumn HeaderText="Branch" HeaderStyle-Font-Size="14px" ItemStyle-Width="160px">
                                <ItemTemplate>
                                    <%#GetBranchCity(CStr(Eval("Code")))%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="Rep" SortExpression="Rep" HeaderText="Sales Rep" ItemStyle-Width="168px" />
                            <asp:BoundColumn DataField="Cost" SortExpression="Cost" DataFormatString="{0:C}" HeaderText="Cost" ItemStyle-Width="168px" />
                            <asp:BoundColumn DataField="Sales" SortExpression="Sales" DataFormatString="{0:C}" HeaderText="Sales" ItemStyle-Width="168px" />
                            <asp:BoundColumn DataField="Discounts" SortExpression="Discount" DataFormatString="{0:C}" HeaderText="Discounts" ItemStyle-Width="168px" />
                            <asp:BoundColumn DataField="Profit" SortExpression="Profit" DataFormatString="{0:C}" HeaderText="Profit" ItemStyle-Width="168px" />
                        </Columns>
                    </asp:DataGrid>
                    
                    <table class="ProductDisplayTable" cellpadding="0" cellspacing="0" style="width: 840px">
                        <tr>
                            <td style="width: 168px;"><h4 style="font-weight: bold;">Totals</h4></td>
                            <td style="width: 168px;"></td>
                            <td style="width: 168px;"><asp:Literal runat="server" ID="litCost" /></td>
                            <td style="width: 168px;"><asp:Literal runat="server" ID="litSales" /></td>
                            <td style="width: 168px;"><asp:Literal runat="server" ID="litDiscounts" /></td>
                            <td style="width: 168px;"><asp:Literal runat="server" ID="litProfit" /></td>
                        </tr>
                    </table>

                </div>
            </ItemTemplate>
        </asp:ListView>
        
        <table class="ProductDisplayTable" cellpadding="0" cellspacing="0" style="width: 840px; margin-left: 35px;">
            <tr>
                <td style="width: 168px;"><h4 style="font-weight: bold;">Yearly Totals</h4></td>
                <td style="width: 168px;"></td>
                <td style="width: 168px;"><asp:Literal runat="server" ID="litYearlyCost" /></td>
                <td style="width: 168px;"><asp:Literal runat="server" ID="litYearlySales" /></td>
                <td style="width: 168px;"><asp:Literal runat="server" ID="litYearlyDiscounts" /></td>
                <td style="width: 168px;"><asp:Literal runat="server" ID="litYearlyProfit" /></td>
            </tr>
        </table>
        
        <asp:ListView ID="lvBranchMonths" runat="server" OnItemDataBound="lvBranchMonths_ItemDataBound" Visible="false">
            <LayoutTemplate>
                <div id="itemPlaceholder" runat="server"></div>
            </LayoutTemplate>
            <ItemTemplate>
                <div style="background-color: #F7F7F7; padding: 30px;">
                    <asp:HiddenField runat="server" ID="hfMonth" Value='<%# Eval("Month") %>' />
                    <h3><asp:Literal ID="litMonth" runat="server" Text='<%# MonthName(CInt(Eval("Month"))) %>' /></h3>
                    
                    <asp:DataGrid runat="server" ID="dgSalesBreakdown" AutoGenerateColumns="false" AllowSorting="true" GridLines="None" 
                            CssClass="ProductDisplayTable" CellPadding="0" CellSpacing="0" style="width: 840px">
                        <Columns>
                            <asp:BoundColumn DataField="Branch" SortExpression="Branch" HeaderText="Branch" ItemStyle-Width="168px" />
                            <asp:BoundColumn DataField="Cost" SortExpression="Cost" DataFormatString="{0:C}" HeaderText="Cost" ItemStyle-Width="168px" />
                            <asp:BoundColumn DataField="Price" SortExpression="Price" DataFormatString="{0:C}" HeaderText="Sales" ItemStyle-Width="168px" />
                            <asp:BoundColumn DataField="Discount" SortExpression="Discount" DataFormatString="{0:C}" HeaderText="Discounts" ItemStyle-Width="168px" />
                            <asp:BoundColumn DataField="Profit" SortExpression="Profit" DataFormatString="{0:C}" HeaderText="Profit" ItemStyle-Width="168px" />
                        </Columns>
                    </asp:DataGrid>
                    
                    <table class="ProductDisplayTable" cellpadding="0" cellspacing="0" style="width: 840px">
                        <tr>
                            <td style="width: 168px;"><h4 style="font-weight: bold;">Totals</h4></td>
                            <td style="width: 168px;"><asp:Literal runat="server" ID="litCost" /></td>
                            <td style="width: 168px;"><asp:Literal runat="server" ID="litSales" /></td>
                            <td style="width: 168px;"><asp:Literal runat="server" ID="litDiscounts" /></td>
                            <td style="width: 168px;"><asp:Literal runat="server" ID="litProfit" /></td>
                        </tr>
                    </table>

                </div>
            </ItemTemplate>
        </asp:ListView>
        
    </div>

</asp:Content>