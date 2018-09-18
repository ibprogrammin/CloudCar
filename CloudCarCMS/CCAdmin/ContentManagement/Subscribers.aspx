<%@ Page Title="Subscribers" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Subscribers.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.Subscribers" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Subscribers
        <i class="icon-pencil"></i>
    </h1><hr />
    
    <div class="float-right download-csv">
        Download: 
        <asp:LinkButton ID="DownloadCsvButton" runat="server" Text="" CssClass="csv-icon" />    
    </div>

    <asp:GridView runat="server" 
            ID="SubscribersGridView" 
            AllowSorting="True" 
            AllowPaging="true" 
            AutoGenerateColumns="False" 
            DataKeyNames="ID" 
            GridLines="None" 
            CssClass="default-table">
        <PagerStyle HorizontalAlign="Right" CssClass="default-table-pager" />
        <PagerSettings PageButtonCount="20" Mode="NumericFirstLast" Position="Bottom" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:TemplateField HeaderText="Subscriber Email">
                <ItemTemplate>
                    <asp:HyperLink runat="server" Text='<%# Eval("Email") %>' NavigateUrl='<%# String.Format("mailto:{0}", Eval("Email")) %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Date Added" DataField="DateAdded" />
            <asp:CheckBoxField HeaderText="OptOut" DataField="OptOut" />
        </Columns>
    </asp:GridView>

</asp:Content>