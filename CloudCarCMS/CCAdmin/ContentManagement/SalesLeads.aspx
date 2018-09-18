<%@ Page Title="Leads" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="SalesLeads.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.SalesLeads" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Leads
        <i class="icon-thumbs-up"></i>
    </h1><hr />
            
    <asp:PlaceHolder runat="server" ID="phDetails" Visible="false">

        <label>Name</label>
        <div class="display-message"><asp:Literal runat="server" ID="litName" /></div>
        <br class="clear-both" /><br />
            
        <label>Email</label>
        <div class="display-message"><asp:Literal runat="server" ID="litEmail" /></div>
        <br class="clear-both" /><br />
            
        <label>Question/Comment</label>
        <div class="display-message"><asp:Literal runat="server" ID="litInquiry" /></div>
        <br class="clear-both" /><br />
            
        <label>Date</label>
        <div class="display-message"><asp:Literal runat="server" id="litDate" /></div>
        <br class="clear-both" />

        <hr />
        
    </asp:PlaceHolder>    
        
    <asp:DataGrid runat="server" ID="gvSalesLads" DataKeyField="id" AllowCustomPaging="false" DataKeyNames="id" AllowSorting="True" AutoGenerateColumns="False" 
            PageSize="10" AllowPaging="true" GridLines="None" CssClass="default-table" CellPadding="0" CellSpacing="0">
        <PagerStyle HorizontalAlign="Right" PageButtonCount="8" Mode="NumericPages" Position="Bottom" CssClass="default-table-pager" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:TemplateColumn HeaderText="Name">
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnSelect" OnCommand="btnSelect_Command" CommandArgument='<%# Eval("id") %>' CommandName="Select" Text='<%# Eval("Name") %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Email">
                <ItemTemplate>
                    <asp:HyperLink runat="server" Text='<%# Eval("email") %>' NavigateUrl='<%# String.Format("mailto:{0}", Eval("email")) %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn HeaderText="Question/Comment" DataField="inquiry" />
            <asp:BoundColumn HeaderText="Date Sent" DataField="datesent" DataFormatString="{0:d}" />
            <asp:TemplateColumn HeaderText="Viewed">
                <ItemTemplate>
                    <asp:CheckBox runat="server" ID="ckbChecked" Checked='<%# Eval("checked") %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn>
                <ItemTemplate>
                    <asp:LinkButton runat="server" ID="btnDelete" OnCommand="btnDelete_Command" CommandArgument='<%# Eval("id") %>' CommandName="Delete" Text="Delete" />
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>

</asp:Content>