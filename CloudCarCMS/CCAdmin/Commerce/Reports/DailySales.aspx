<%@ Page Title="Daily Sales Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="DailySales.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.Reports.DailySales"%>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Daily Sales
        <i class="icon-bar-chart"></i>
    </h1><hr />
    
    <div id="daily-sales-chart"></div>
    <br class="clear-both" />

    <div class="search-bar">
        <span>Start</span>
        <asp:TextBox ID="txbInitial" runat="server" CssClass="form-date-pick" />
        <span>End</span>
        <asp:TextBox ID="txbFinal" runat="server" CssClass="form-date-pick" />
        <asp:Button ID="btnGo" runat="server" CssClass="SaveButton float-right" style="width: 180px;" text="Go" />
    </div>
    
    <span class="float-right download-csv">Download: <asp:LinkButton runat="server" ID="DownloadCsvButton" Text="" CssClass="csv-icon" /></span>
    <br class="clear-both" />

    <ajax:CalendarExtender ID="cxtInitial" runat="server" TargetControlID="txbInitial" />
    <ajax:CalendarExtender ID="cxtFinal" runat="server"  TargetControlID="txbFinal" />

    <asp:ListView ID="lvwDates" runat="server">
        <LayoutTemplate>
            <div id="itemPlaceholder" runat="server"></div>
        </LayoutTemplate>
        <ItemTemplate>
            <asp:HiddenField ID="hDate" runat="server" Value='<%#Eval("OrderDate")%>' />
            <div class="float-right" style="margin-right: 60px;">
                <h2 class="Green"><asp:Literal runat="server" ID="litDailyTotal" /></h2>
            </div>
            <h2 style="margin-left: 40px;">
                <asp:Literal ID="litDate" runat="server" Text='<%#CType(Eval("OrderDate"), DateTime).ToString("D")%>' />
            </h2>
            
            <div style="background-color: #FDFDFD; margin-bottom: 20px; margin-right: 0px; padding: 0px; border: 1px solid #eee;">
                <asp:ListView ID="lvwOrders" runat="server" OnItemDataBound="lvwOrders_ItemDataBound">
                    <LayoutTemplate>
                        <div id="itemPlaceholder" runat="server"></div>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <div style="padding: 30px; padding-top: 10px;">
                            <h3>Order (<asp:Literal ID="litOrderId" runat="server" Text='<%#Eval("OrderId") %>' />)</h3>
                            <table class="default-table" style="width: 100%;">
                            <asp:ListView ID="lvwProducts" runat="server">
                                <LayoutTemplate>
                                        <tbody>
                                            <tr class="default-table-header">
                                                <td>Product</td>
                                                <td class="align-center">Quantity</td>
                                                <td class="align-right">Cost</td>
                                                <td class="align-right">Price</td>
                                                <td class="align-right">Profit</td>
                                                <td class="align-right" style="width: 20%;">Total</td>
                                            </tr>
                                            <asp:PlaceHolder id="itemPlaceholder" runat="server" />
                                            <tr>
                                                <td colspan="5"></td>
                                                <td class="align-right" style="border-top: 4px solid #888;">
                                                    <h3 class="Green" style="text-align:right;"><b><asp:Literal ID="litOrderTotal" runat="server"/></b></h3>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><asp:Literal ID="litProductName" runat="server" Text='<%#Eval("Name") %>' /></td>
                                        <td class="align-center"><asp:Literal ID="litProductQty" runat="server" Text='<%#Eval("Quantity") %>' /></td>
                                        <td class="align-right"><asp:Literal ID="litCost" runat="server" Text='<%#CType(Eval("Cost"), Double).ToString("C")%>' /></td>
                                        <td class="align-right"><asp:Literal ID="litPrice" runat="server" Text='<%#CType(Eval("Price"), Double).ToString("C")%>' /></td>
                                        <td class="align-right"><asp:Literal ID="litProfit" runat="server" Text='<%#CType(Eval("Profit"), Double).ToString("C")%>' /></td>
                                        <td class="align-right"><asp:Literal ID="litTotal" runat="server" Text='<%#CType(Eval("Total"), Double).ToString("C") %>' /></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </ItemTemplate>
    </asp:ListView>
    
    <asp:SqlDataSource ID="DailySalesByOrderDS" runat="server" />

</asp:Content>