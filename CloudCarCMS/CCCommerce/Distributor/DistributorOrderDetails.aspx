<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="DistributorOrderDetails.aspx.vb" Inherits="CloudCar.DistributorOrderDetails" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">

<style type="text/css">
    label { margin-left: 10px; font-size: 14px; }
    p { margin-left: 0px; font-size: 14px; }
</style>

</asp:Content>

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server">
    <h1>Order Details ( <asp:Literal runat="server" ID="litOrderID" /> )</h1>
</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">

<asp:Label runat="server" ID="lblStatus" CssClass="status-message" Visible="false" />

<asp:PlaceHolder runat="server" ID="phOrderDetails" Visible="false">
    
    <br />
    <asp:HyperLink runat="server" NavigateUrl="~/CCAdmin/Commerce/Orders.aspx" Text="&laquo; Back to Orders" style="clear: left; float: left; margin: 10px;" />
    <asp:HiddenField runat="server" ID="hfOrderID" />

    <table cellspacing="0" class="ProductDisplayTable">
        <tr>
            <td>
                <asp:LinkButton id="btnReturnOrder" runat="server" CssClass="BlueButton" style="float: right; width: 140px; position: relative; top: 15px;"><span class="BlueButton">Return</span></asp:LinkButton>
                <asp:LinkButton id="btnPrintOrder" runat="server" CssClass="OrangeButton" style="float: right; width: 120px; position: relative; top: 15px; margin-right: 20px;"><span class="OrangeButton">Print</span></asp:LinkButton>
            </td>
        </tr>
    </table>


    <br />
    <table cellpadding="0" cellspacing="10" width="920">
        <tr>
            <td colspan="2"><h2>Customer Information</h2></td>
            <td colspan="2"><h2>Address</h2></td>
        </tr>
        <tr>
            <td style="width: 140px;"><label>User</label></td>
            <td><asp:Literal runat="server" ID="litUsername" /></td>
            <td rowspan="2" valign="top"><label>Shipping</label></td>
            <td rowspan="2" style="width: 30%;"><p><asp:Literal runat="server" ID="litShippingAddress" /></p></td>
            <td rowspan="2" valign="bottom"></td>
        </tr>
        <tr>
            <td><label>Email</label></td>
            <td><asp:Hyperlink runat="server" ID="hlEmailAddress" /></td>
        </tr>
        <tr>
            <td><label>Name</label></td>
            <td><p><asp:Literal runat="server" ID="litName" /></p></td>
            <td rowspan="2" valign="top"><label>Billing</label></td>
            <td rowspan="2"><p><asp:Literal runat="server" ID="litBillingAddress" /></p></td>
            <td rowspan="2" valign="bottom"></td>
        </tr>
        <tr>
            <td><label>Phone</label></td>
            <td><p><asp:Literal runat="server" ID="litPhoneNumber" /></p></td>
        </tr>
    </table>

    <br />

    <table cellpadding="0" cellspacing="10" width="680">
        <tr>
            <td colspan="3"><h2>Shipping Information</h2></td>
        </tr>
        <tr>
            <td style="width: 140px;"><label>Company</label></td>
            <td><p><asp:Literal runat="server" ID="litShipCompany" /></p></td>
            <td rowspan="3" style="width: 40%;">
                <asp:TextBox runat="server" ID="txtOrderTrackingNumber" Width="200px" /><br />
                <asp:LinkButton id="btnShipOrder" runat="server" CssClass="GreenButton SerifStack" style="width: 216px; position: relative; top: 10px;"><span class="GreenButton">Ship Order</span></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td><label>Service</label></td>
            <td><p><asp:Literal runat="server" ID="litShipService" /></p></td>
        </tr>
        <tr>
            <td><label>Charge</label></td>
            <td><p><asp:Literal runat="server" ID="litShipCharge" /></p></td>
        </tr>
    </table>

    <ajax:TextBoxWatermarkExtender runat="server" ID="tbwOTN" TargetControlID="txtOrderTrackingNumber" WatermarkText="Tracking Number" WatermarkCssClass="Watermark" />

    <br />

    <asp:DataGrid ID="gvOrderItems" runat="server" AutoGenerateColumns="False" DataKeyNames="id" CellPadding="0" CellSpacing="0"
            AllowPaging="False" AllowSorting="False" GridLines="None" CssClass="ProductDisplayTable">
        <Columns>
            <asp:TemplateColumn>
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnShip" Text="Ship" OnCommand="ShipItemButtonCommand" CommandArgument='<%# Eval("ID") %>' CommandName="Ship" ForeColor="YellowGreen" Font-Bold="true" Visible='<%# CBool(Eval("Shipped")) <> true %>' /><br />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn SortExpression="ID" HeaderText="Item(s)">
                <ItemTemplate>
                    <img runat="server" ID="imgItem" src='<%# String.Format("/images/db/{0}/100/{1}.jpg", Eval("ImageID"), Eval("Name"))  %>' alt="" class="ProductDisplayImage" /><br />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Description" SortExpression="Name">
                <ItemTemplate>
                    <asp:Literal runat="server" Text='<%# Eval("Name") %>' ID="litName" /><br />
                    <asp:Label runat="server" ID="lblColour" Text='<%# Eval("Colour") %>' /> / <asp:Label runat="server" ID="lblSize" Text='<%# Eval("Size") %>' /><br />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Tracking Number">
                <ItemTemplate>
                    <asp:TextBox runat="server" ID="txtItemTNumber" Width="200px" Visible='<%# CBool(Eval("Shipped")) <> true %>' style="position: relative; top: 10px;" />
                    <ajax:TextBoxWatermarkExtender runat="server" ID="tbwItemTNumber" TargetControlID="txtItemTNumber" WatermarkText="Tracking #" />
                    <asp:Label runat="server" ID="lblItemTNumber" Text='<%# String.Format("T# ( {0} )", Eval("TrackingNumber")) %>' Visible='<%# CBool(Eval("Shipped")) = true %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="UnitPrice" SortExpression="UnitPrice" HeaderText="Price" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
            <asp:BoundColumn DataField="Quantity" HeaderText="Qty." SortExpression="Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
            <asp:BoundColumn DataField="Price" SortExpression="Price" HeaderText="Total" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
        </Columns>
    </asp:DataGrid>


    <table class="ShoppingCartSummaryTable" cellspacing="0" style="float: right;">
        <tr>
            <td style="border-bottom: none;"><label>Sub Total</label></td>
            <td style="border-bottom: none;" align="right" class="Price"><asp:Literal runat="server" ID="litSubtotal" /></td>
        </tr>
        <tr>
            <td style="border-bottom: none;"><label class="Discount">Discount</label></td>
            <td style="border-bottom: none;" align="right"><asp:Label runat="server" ID="litDiscount" CssClass="Discount" /></td>
        </tr>
        <tr>
            <td style="border-bottom: none;"><label>Shipping</label></td>
            <td style="border-bottom: none;" align="right" class="Price"><asp:Literal runat="server" ID="litShippingCharge" /></td>
        </tr>
        <tr>
            <td><label>Taxes ( <asp:Literal runat="server" ID="litTaxRate" /> )</label></td>
            <td align="right" class="Price"><asp:Literal runat="server" ID="litTaxes" /></td>
        </tr>
        <tr>
            <td style="border-bottom: none;" class="Total">Total</td>
            <td class="Total Price" style="padding-right: 10px; border-bottom: none;" align="right"><asp:Label runat="server" ID="litTotal" CssClass="Price" /></td>
        </tr>
    </table>

</asp:PlaceHolder>

</asp:Content>