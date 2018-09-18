<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="DailyDiscountReport.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.Reports.DailyDiscountReport" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Daily Discount Report
        <i class="icon-bar-chart"></i>
    </h1>
    <br />
    
    <table class="ProductDisplayTable" width="960" cellspacing="0">
        <tr>
            <td><h3>Start Date</h3></td>
            <td><asp:TextBox ID="txbInitial" CssClass="TextBox" runat="server"  Width="180px" /></td>
            <td><h3>Finish Date</h3></td>
            <td><asp:TextBox ID="txbFinal" CssClass="TextBox" runat="server" Width="180px" /></td>
            <td><h3>Select Type</h3></td>
            <td><asp:DropDownList runat="server" ID="ddlReportType" /></td>
            <td><asp:LinkButton ID="btnGo" runat="server" CssClass="OrangeButton" style="position: relative; width: 140px; top: 15px;"><span class="OrangeButton">Go</span></asp:LinkButton></td>
        </tr>
    </table>
    
    <ajax:CalendarExtender ID="cxtInitial" runat="server" TargetControlID="txbInitial" />
    <ajax:CalendarExtender ID="cxtFinal" runat="server"  TargetControlID="txbFinal" />
    
    Download Report: <asp:LinkButton runat="server" ID="btnDownloadCsv" Text=".csv" />
    
    <br style="clear: both;" /><br />
   
    <h2><asp:Literal runat="server" ID="litDay" /></h2>
   
    <div style="margin-bottom: 20px; padding: 30px;">
    
        <asp:ListView ID="lvSalesDays" runat="server" OnItemDataBound="lvSalesDays_ItemDataBound">
            <LayoutTemplate>
                <div id="itemPlaceholder" runat="server"></div>
            </LayoutTemplate>
            <ItemTemplate>
                <div style="background-color: #F7F7F7; padding: 30px;">
                    <asp:HiddenField ID="hDate" runat="server" Value='<%#Eval("OrderDate")%>' />
                    <div style="float: right;"><h2 class="Green"><asp:Literal runat="server" ID="litDailyTotal" /></h2></div>
                    <h2><asp:Literal ID="litDate" runat="server" Text='<%#CType(Eval("OrderDate"), DateTime).ToString("D")%>' /></h2>
                    
                    <asp:DataGrid runat="server" ID="dgSalesBreakdown" AutoGenerateColumns="false" AllowSorting="true" GridLines="None" 
                            CssClass="ProductDisplayTable" CellPadding="0" CellSpacing="0" style="width: 840px;">
                        <Columns>
                            <asp:BoundColumn DataField="SalesRep" SortExpression="SalesRep" HeaderText="Sales Rep" ItemStyle-Width="168px" />
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
        
        <asp:ListView ID="lvBranchDays" runat="server" OnItemDataBound="lvBranchDays_ItemDataBound" Visible="false">
            <LayoutTemplate>
                <div id="itemPlaceholder" runat="server"></div>
            </LayoutTemplate>
            <ItemTemplate>
                <div style="background-color: #F7F7F7; padding: 30px;">
                    <asp:HiddenField ID="hDate" runat="server" Value='<%#Eval("OrderDate")%>' />
                    <div style="float: right;"><h2 class="Green"><asp:Literal runat="server" ID="litDailyTotal" /></h2></div>
                    <h2><asp:Literal ID="litDate" runat="server" Text='<%#CType(Eval("OrderDate"), DateTime).ToString("D")%>' /></h2>
                    
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
        
        <table class="ProductDisplayTable" cellpadding="0" cellspacing="0" style="width: 840px; margin-left: 35px;">
            <tr>
                <td style="width: 168px;"><h4 style="font-weight: bold;">Yearly Totals</h4></td>
                <td style="width: 168px;"><asp:Literal runat="server" ID="litYearlyCost" /></td>
                <td style="width: 168px;"><asp:Literal runat="server" ID="litYearlySales" /></td>
                <td style="width: 168px;"><asp:Literal runat="server" ID="litYearlyDiscounts" /></td>
                <td style="width: 168px;"><asp:Literal runat="server" ID="litYearlyProfit" /></td>
            </tr>
        </table>
        
    </div>

</asp:Content>