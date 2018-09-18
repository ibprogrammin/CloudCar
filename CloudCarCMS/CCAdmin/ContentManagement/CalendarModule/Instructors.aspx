<%@ Page Title="Instructors" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Instructors.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.CalendarModule.Instructors" %>
<%@ Import Namespace="CloudCar.CCFramework.Core" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <asp:Button runat="server" UseSubmitBehavior="true" PostBackUrl="~/CCAdmin/ContentManagement/CalendarModule/InstructorDetails.aspx" CssClass="SaveButton" style="float: right; width: 280px; margin-top: 30px;" Text="Add Instructor" />

    <img src="/CCTemplates/Admin/Images/icons/order.icon.png" alt="View Instructors" width="75" height="75" class="HeadingIcon" /><br />
    <h1 class="form-heading-style">
        Instructors
    </h1><hr />

    <asp:UpdatePanel runat="server" ID="upUpdate" ChildrenAsTriggers="true" EnableViewState="true" UpdateMode="Always">
        <ContentTemplate>

    <br /><asp:Label runat="server" ID="lblStatus" CssClass="status-message" Visible="false" /><br />
    
    <asp:DataGrid ID="gvInstructors" runat="server" AutoGenerateColumns="False" DataKeyNames="id" PageSize="10" 
            AllowPaging="true" AllowSorting="True" GridLines="None" CssClass="default-table" CellPadding="0" CellSpacing="0">
        <PagerStyle HorizontalAlign="Right" PageButtonCount="8" Mode="NumericPages" Position="Bottom" CssClass="default-table-pager" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:TemplateColumn>
                <ItemTemplate>
                    <asp:Image runat="server" ImageUrl='<%# String.Format("/images/db/{0}/40/{1}.jpg", Eval("ProfileImageId"), PictureController.GetPicture(CInt(Eval("ProfileImageId"))).PictureFileName) %>' style="margin-bottom: -15px;" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Instructor">
                <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%# String.Format("~/CCAdmin/ContentManagement/CalendarModule/InstructorDetails.aspx?Instructor={0}", Eval("ID"))  %>' Text='<%# Eval("Name") %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
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