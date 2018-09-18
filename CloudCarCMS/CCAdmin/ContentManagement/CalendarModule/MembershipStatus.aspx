<%@ Page Title="Membership Status" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="MembershipStatus.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.CalendarModule.MembershipStatus" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <img src="/CCTemplates/Admin/Images/icons/user.icon.png" alt="Enable User Membership" width="75" height="75" class="HeadingIcon" /><br />
    <h1 class="form-heading-style">Membership Status</h1><br /><hr /><br />
    
    <asp:Label ID="lblStatus" runat="server" Text="" CssClass="status-message" Visible="false" />

    <asp:DataGrid runat="server" ID="UsersDataGrid" AutoGenerateColumns="False" AllowSorting="true" DataSourceID="dsUsers" DataKeyField="UserName" 
            GridLines="None" CellPadding="0" CellSpacing="0" AllowPaging="true" PageSize="20" CssClass="default-table">
        <PagerStyle HorizontalAlign="Right" PageButtonCount="8" Mode="NumericPages" Position="Bottom" CssClass="default-table-pager" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:TemplateColumn HeaderText="User" SortExpression="UserName">
                <ItemTemplate>
                    <asp:Literal runat="server" Text='<%# Eval("UserName") %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Email" SortExpression="Email">
                <ItemTemplate>
                    <asp:HyperLink runat="server" Text='<%# Eval("Email") %>' NavigateUrl='<%# String.Format("mailto:{0}", Eval("Email")) %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Last Login">
                <ItemTemplate>
                    <asp:Literal runat="server" Text='<%# String.Format("{0:F}", Eval("LastActivityDate")) %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Membership Status" ItemStyle-Width="25%">
                <ItemTemplate>

                    <asp:CheckBox runat="server" ID="MembershipEnabledCheckBox" OnCheckedChanged="MembershipEnabledCheckBoxCheckChanged"
                        OnDataBinding="MembershipEnabledCheckBoxDataBinding" UserName='<%# Eval("UserName") %>' AutoPostBack="true" Text="" CssClass="default-table-check-box" style="margin-top: 0px; display: block; clear: both;" />
                    <span style="margin-left: 20px; font-size: 18px;">Active</span>
                    <br style="clear: both;" />
                    
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid>
    
    <asp:SqlDataSource ID="dsUsers" runat="server" ConnectionString="<%$ ConnectionStrings:MainConnectionString %>" DataSourceMode="DataSet" 
        SelectCommand="SELECT U.UserName, U.LastActivityDate, M.Email FROM aspnet_Users AS U LEFT OUTER JOIN aspnet_Membership AS M ON M.UserId = U.UserId GROUP BY U.UserName, U.LastActivityDate, M.Email ORDER BY U.UserName"/>
        
    <br /><br />

</asp:Content>