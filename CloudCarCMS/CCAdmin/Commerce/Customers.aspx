<%@ Page Title="Customer List" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Customers.aspx.vb" Inherits="CloudCar.CCAdmin.Commerce.Customers" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Customers
        <i class="icon-shopping-cart"></i>
    </h1>
    <hr />
    
    <div class="float-right download-csv">
        Download: 
        <asp:LinkButton ID="DownloadCsvButton" runat="server" Text="" CssClass="csv-icon" />    
    </div>
    

    <asp:UpdatePanel runat="server" ID="upUpdate" ChildrenAsTriggers="true" EnableViewState="true" UpdateMode="Always">
        <ContentTemplate>

    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" Visible="False" />
           
    <asp:DataGrid runat="server" 
            ID="gvCustomers" 
            AutoGenerateColumns="False" 
            DataKeyNames="id" 
            PageSize="12" 
            AllowPaging="true" 
            AllowSorting="True" 
            GridLines="None" 
            CssClass="default-table">
        <PagerStyle HorizontalAlign="Left" PageButtonCount="8" Mode="NumericPages" Position="Bottom" CssClass="default-table-pager" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:BoundColumn DataField="Id" HeaderText="Id" SortExpression="Id" Visible="False" />
            <asp:BoundColumn DataField="FirstName" HeaderText="First Name" SortExpression="FirstName" />
            <asp:BoundColumn DataField="MiddleName" HeaderText="Middle Name" SortExpression="MiddleName" />
            <asp:BoundColumn DataField="LastName" HeaderText="Last Name" SortExpression="LastName" />
            <asp:BoundColumn DataField="Email" HeaderText="Email" SortExpression="Email" />
            <asp:BoundColumn DataField="PhoneNumber" HeaderText="Phone Number" SortExpression="PhoneNumber" />
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