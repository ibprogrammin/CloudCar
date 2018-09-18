<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="MonthlySales.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.Reports.MonthlySales" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">


    <h1 class="form-heading-style">
        Monthly Sales
        <i class="icon-bar-chart"></i>
    </h1><hr />
    
    <div id="monthly-sales-chart"></div>
    <br class="clear-both" />

    <span class="float-right download-csv">Download: <asp:LinkButton runat="server" ID="DownloadCsvButton" Text="" CssClass="csv-icon" /></span>
    
    <asp:Repeater runat="server" ID="MonthlySalesRepeater">
        <HeaderTemplate>
            <table class="default-table">
                <thead>
                    <tr>
                        <td>Month</td>
                        <td class="align-center">Orders</td>
                        <td class="align-center">Items</td>
                        <td class="align-right">Sales</td>
                        <td class="align-right">COGS</td>
                        <td class="align-right">Shipping (Cost)</td>
                        <td class="align-right">Discount</td>
                        <td class="align-right">Taxes</td>
                        <td class="align-right">Gross Profit</td>
                        <td class="align-right">Margin</td>
                    </tr>
                </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><%# String.Format("{0} {1}", MonthName(CInt(Eval("Month"))), Eval("Year"))%></td>
                <td class="align-center"><%# Eval("Orders")%></td>
                <td class="align-center"><%# Eval("Items")%></td>
                <td class="align-right"><%# String.Format("{0:C}", Eval("Sales"))%></td>
                <td class="align-right"><%# String.Format("{0:C}", Eval("CostOfGoodsSold"))%></td>
                <td class="align-right"><%# String.Format("{0:C}", Eval("Shipping"))%> (<%# String.Format("{0:C}", Eval("ShippingCost"))%>)</td>
                <td class="align-right"><%# String.Format("{0:C}", Eval("Discount"))%></td>
                <td class="align-right"><%# String.Format("{0:C}", Eval("Taxes"))%></td>
                <td class="align-right"><%# String.Format("{0:C}", Eval("Profit"))%></td>
                <td class="align-right"><%# String.Format("{0:F2}%", Eval("ProfitMargin"))%></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
    

</asp:Content>