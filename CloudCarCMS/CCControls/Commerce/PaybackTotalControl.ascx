<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="PaybackTotalControl.ascx.vb" Inherits="CloudCar.CCControls.Commerce.PaybackTotalControl" %>

<table class="default-table">
    <tr style="height: 20px;">
        <td><b>Cost of Website</b></td>
        <td class="Price"><asp:Literal runat="server" ID="litCostOfWebsite" /></td>
    </tr>
    <tr style="height: 20px;">
        <td><b>Cost of Goods Sold</b></td>
        <td class="Price"><asp:Literal runat="server" ID="litCostOfGoodsSold" /></td>
    </tr>
    <tr style="height: 20px;">
        <td><b>Total Sales</b></td>
        <td class="Price"><asp:Literal runat="server" ID="litTotalSales" /></td>
    </tr>
    <tr style="height: 20px;">
        <td><b>Total Profit from Sales</b></td>
        <td class="Price"><asp:Literal runat="server" ID="litTotalProfitFromSales" /></td>
    </tr>
    <tr style="height: 60px; background-color: #E6FFD2;">
        <td class="Total">Remaining Balance</td>
        <td class="TotalPrice"><asp:Literal runat="server" ID="litAmountRemaining" /></td>
    </tr>
</table>