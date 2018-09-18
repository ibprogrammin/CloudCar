<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="BoxCalculator.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.ClientCustom.BoxCalculator" %>

<asp:Content ID="Content4" ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">

    <h1>Box Calculator Configuration</h1>

    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" Visible="false" />
    
    <br /><br />
    <h2>Room</h2>
    
    <p>These are the rooms the user has to choose from on the Box Calculator.</p>

    <asp:HiddenField runat="server" ID="hfRoomID" />
    <table class="ProductDisplayTable" width="960" cellspacing="0">
        <tr>
            <td style="width: 340px;"><asp:TextBox runat="server" ID="txtName" style="width: 320px;" /></td>
            <td><asp:LinkButton id="btnAddRoom" runat="server" CssClass="GreenButton" ValidationGroup="Room" CausesValidation="true" style="position: relative; width: 250px; top: 15px;"><span class="GreenButton">Add</span></asp:LinkButton></td>
            <td><asp:LinkButton id="btnClearRoom" runat="server" CssClass="RedButton" style="position: relative; width: 250px; top: 15px;"><span class="RedButton">Clear</span></asp:LinkButton></td>
        </tr>
    </table>

    <ajax:TextBoxWatermarkExtender runat="server" ID="tbweRoomName" WatermarkCssClass="Watermark" WatermarkText="Name" TargetControlID="txtName" />
    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" Display="None" ErrorMessage="Please enter aa room name" ValidationGroup="Room" />
    <ajax:ValidatorCalloutExtender ID="vceName" runat="server" TargetControlID="rfvName" PopupPosition="TopLeft" />

    <asp:DataGrid runat="server" ID="dgRooms" DataKeyField="id" AllowCustomPaging="false" DataKeyNames="id" AllowSorting="True" AutoGenerateColumns="False" 
            PageSize="20" AllowPaging="true" GridLines="None" CssClass="ProductDisplayTable">
        <PagerStyle CssClass="PagerStyle" PageButtonCount="8" Mode="NumericPages" Position="Bottom" HorizontalAlign="Right" />
        <Columns>
            <asp:TemplateColumn>
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnSelectRoom" OnCommand="btnSelectRoom_Command" CommandArgument='<%# Eval("id") %>' CommandName="Select" Text="Edit" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn HeaderText="Name" DataField="RoomName" />
            <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnDeleteRoom" OnClientClick="return confirm('Are you sure you want to delete this Room?');" OnCommand="btnDeleteRoom_Command" CommandArgument='<%# Eval("ID") %>' CommandName="Delete" Text="Delete" />
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

    <br style="clear: both;" /><br />
    
    <h2>Room Product</h2>

    <p>These are the product that will be used when the user sets a given number of rooms.</p>

    <asp:HiddenField runat="server" ID="hfRoomProductID" />
    <fieldset>
        <p>
            <label>Room</label><br />
            <asp:DropDownList runat="server" ID="ddlRoom" DataTextField="RoomName" DataValueField="ID" />
        </p>
        <p>
            <label>Product</label><br />
            <asp:DropDownList runat="server" ID="ddlProduct" DataTextField="Name" DataValueField="ID" />
        </p>
        <p>
            <label>Quantity per Room</label><br />
            <asp:TextBox runat="server" ID="txtQuantity" />
        </p>
        <p>
            <label>Reoccurs each Room</label><br />
            <asp:CheckBox runat="server" ID="ckbReoccurs" />
        </p>
    </fieldset>
    <br />
    <asp:LinkButton id="btnAddRoomProduct" runat="server" CssClass="GreenButton" ValidationGroup="ItemValidation" CausesValidation="true" style="width: 250px; float: left;"><span class="GreenButton">Submit</span></asp:LinkButton>
    <asp:LinkButton id="btnClearRoomProduct" runat="server" CssClass="RedButton" style="margin-left: 20px; width: 250px; float: left;"><span class="RedButton">Clear</span></asp:LinkButton>

    <asp:RequiredFieldValidator ID="rfvProduct" runat="server" ControlToValidate="ddlProduct" Display="None" ErrorMessage="Please select a product" ValidationGroup="ItemValidation" />
    <ajax:ValidatorCalloutExtender ID="vceProduct" runat="server" TargetControlID="rfvProduct" PopupPosition="TopLeft" />    
    
    <asp:DataGrid runat="server" ID="dgRoomProducts" DataKeyField="id" AllowCustomPaging="false" DataKeyNames="id" AllowSorting="True" AutoGenerateColumns="False" 
            PageSize="20" AllowPaging="true" GridLines="None" CssClass="ProductDisplayTable">
        <PagerStyle CssClass="PagerStyle" PageButtonCount="8" Mode="NumericPages" Position="Bottom" HorizontalAlign="Right" />
        <Columns>
            <asp:TemplateColumn>
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnSelectRoomProduct" OnCommand="btnSelectRoomProduct_Command" CommandArgument='<%# Eval("id") %>' CommandName="Select" Text="Edit" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <ItemTemplate>
                    <%#GetRoomName(Eval("RoomID"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <ItemTemplate>
                    <%#GetProductName(Eval("ProductID"))%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn HeaderText="Quantity" DataField="Quantity" />
            <asp:BoundColumn HeaderText="Reoccurs" DataField="Reoccurs" />
            <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnDeleteRoomProduct" OnClientClick="return confirm('Are you sure you want to delete this Room Product?');" OnCommand="btnDeleteRoomProduct_Command" CommandArgument='<%# Eval("ID") %>' CommandName="Delete" Text="Delete" />
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

    <br style="clear: both;" /><br />

</asp:Content>