<%@ Page Title="Bookings" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Bookings.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.CalendarModule.Bookings" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <img src="/CCTemplates/Admin/Images/icons/order.icon.png" alt="View Bookings" width="75" height="75" class="HeadingIcon" /><br />
    <h1 class="form-heading-style">
        Bookings
        <i class="icon-book"></i>
    </h1><hr />

    <asp:UpdatePanel runat="server" ID="upUpdate" ChildrenAsTriggers="true" EnableViewState="true" UpdateMode="Always">
        <ContentTemplate>

    <br /><asp:Label runat="server" ID="lblStatus" CssClass="status-message" Visible="False" /><br />
        
           
    <asp:DataGrid ID="gvBookings" runat="server" AutoGenerateColumns="False" DataKeyNames="id" PageSize="10" 
            AllowPaging="true" AllowSorting="True" GridLines="None" CssClass="default-table" CellPadding="0" CellSpacing="0">
        <PagerStyle HorizontalAlign="Right" PageButtonCount="8" Mode="NumericPages" Position="Bottom" CssClass="default-table-pager" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:BoundColumn DataField="Name" HeaderText="Name" SortExpression="Name" />
            <asp:BoundColumn DataField="Program" HeaderText="Program" SortExpression="Program" />
            <asp:BoundColumn DataField="Date" HeaderText="Date" SortExpression="Date" DataFormatString="{0:d}" />
            <asp:BoundColumn DataField="Spaces" HeaderText="Spaces" SortExpression="Spaces" />
            <asp:BoundColumn DataField="Status" HeaderText="Status" SortExpression="Status" />
            <asp:BoundColumn DataField="Approved" HeaderText="Approved" SortExpression="Approved" />
        </Columns>
    </asp:DataGrid>

    <br />
    <br />
    
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress AssociatedUpdatePanelID="upUpdate" runat="server" ID="upProgress" DynamicLayout="false" >
        <ProgressTemplate>
            <div style="display: table-cell; position: fixed; top: 50%; left: 40%; width: 400px; height: 80px; background-color: #FFF; border: 1px solid #F5F5F5; vertical-align: middle;"><h3 class="BoldDark" style="text-align: center; position: relative; top: 32px;">Loading please wait...</h3></div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>