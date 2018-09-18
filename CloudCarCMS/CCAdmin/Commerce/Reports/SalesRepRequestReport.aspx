<%@ Page Title="Sales Rep Report" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="SalesRepRequestReport.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.Reports.SalesRepRequestReport" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Sales Rep Request Breakdown
        <i class="icon-bar-chart"></i>
    </h1><br />

    <table class="default-table" width="960" cellspacing="0">
        <tr>
            <td style="width: 520px;"></td>
            <td valign="top"><h3 style="width: 120px;">Select Year</h3></td>
            <td><asp:DropDownList runat="server" ID="ddlYearSelection" /></td>
            <td><asp:Button ID="btnGo" runat="server" CssClass="GreenButton" Text="Go" /></td>
        </tr>
    </table>
    
    <br style="clear: both;" /><br />
   
    <asp:PlaceHolder runat="server" ID="phDisplay" Visible="false">
        <h2 style="margin: 0px;"><asp:Literal runat="server" ID="litYear" /></h2>
        Download Report: <asp:LinkButton runat="server" ID="btnDownloadCsv" Text=".csv" />
    </asp:PlaceHolder>
   
    <div style="margin: 0px -20px; margin-left: -40px; padding: 20px 20px;">
        
        <asp:ListView ID="lvMonthData" runat="server" OnItemDataBound="lvMonthData_ItemDataBound" Visible="false">
            <LayoutTemplate>
                <div id="itemPlaceholder" runat="server"></div>
            </LayoutTemplate>
            <ItemTemplate>
                <div style="background-color: #F7F7F7; padding: 20px 20px;">
                    <asp:HiddenField runat="server" ID="hfMonth" Value='<%# Eval("Month") %>' />
                    <h3 style="margin-left: 0px;"><asp:Literal ID="litMonth" runat="server" Text='<%# MonthName(CInt(Eval("Month"))) %>' /></h3>
                    
                    <asp:DataGrid runat="server" ID="dgSalesBreakdown" AutoGenerateColumns="false" AllowSorting="true" GridLines="None" 
                            CssClass="ProductDisplayTable" CellPadding="0" CellSpacing="0">
                        <Columns>
                            <asp:BoundColumn DataField="Branch" SortExpression="Branch" HeaderText="Branch" ItemStyle-Width="148px" />
                            <asp:BoundColumn DataField="SalesRep" SortExpression="SalesRep" HeaderText="Sales Rep" ItemStyle-Width="148px" />
                            <asp:BoundColumn DataField="Product" SortExpression="Product" HeaderText="Product" ItemStyle-Width="148px" />
                            <asp:BoundColumn DataField="ActualQuantity" SortExpression="ActualQuantity" HeaderText="Quantity" ItemStyle-Width="148px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                        </Columns>
                    </asp:DataGrid>
                </div>
            </ItemTemplate>
        </asp:ListView>
        
    </div>

</asp:Content>