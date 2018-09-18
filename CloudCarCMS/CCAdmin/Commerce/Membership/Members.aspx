<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="Members.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.Membership.Members" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server"></asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SecondContentPlaceHolder" runat="server"></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FirstContentPlaceHolder" runat="server"></asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server">

<h2>Current Members</h2>

<br /><asp:Label ID="lblStatus" runat="server" Text="" CssClass="StoreStatusMessage" Visible="false" /><br />

<asp:GridView runat="server" ID="gvUsers" AutoGenerateColumns="False" AllowSorting="true"
    DataSourceID="dsUsers" DataKeyNames="UserName" Width="100%" GridLines="None" BorderColor="#B30001" 
    BorderStyle="Solid" BorderWidth="1" CellPadding="4" CellSpacing="4" AllowPaging="true" PageSize="20">
    <HeaderStyle CssClass="GridHeader" />
    <RowStyle VerticalAlign="Top" BackColor="#8C826C" />
    <AlternatingRowStyle BackColor="#706755" />
    <Columns>
        <asp:TemplateField HeaderText="User Name" SortExpression="UserName">
            <ItemTemplate>
                <asp:LinkButton ID="lbSelect" runat="server" CssClass="GridButton" OnClick="lbSelect_Click" CausesValidation="False" 
                    CommandArgument='<%# Eval("UserName") %>' CommandName="Select" Text='<%# Eval("UserName") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="JoinDate" HeaderText="Date Joined" SortExpression="JoinDate" DataFormatString="{0:MMM dd, yyyy}" ItemStyle-ForeColor="#E3E1B8" />
        <asp:BoundField DataField="BillFrequency" HeaderText="Bill Frequency" SortExpression="BillFrequency" ItemStyle-ForeColor="#E3E1B8" />
        <asp:BoundField DataField="LastBillDate" HeaderText="Last Bill Date" SortExpression="Last Bill Date" DataFormatString="{0:MMM dd, yyyy}" ItemStyle-ForeColor="#E3E1B8" />
        <asp:BoundField DataField="NextBillDate" HeaderText="Next Bill Date" SortExpression="Next Bill Date" DataFormatString="{0:MMM dd, yyyy}" ItemStyle-ForeColor="#E3E1B8" />
        <asp:TemplateField>
            <ItemTemplate>
                <asp:LinkButton ID="lbSuspend" runat="server" CommandArgument='<%# Eval("UserID") %>' CommandName="SuspendMembership" CssClass="GridButton" Text="Suspend Membership" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

</asp:Content>