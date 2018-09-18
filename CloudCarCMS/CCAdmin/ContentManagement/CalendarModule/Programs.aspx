<%@ Page Title="Programs" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Programs.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.CalendarModule.Programs" %>
<%@ Import Namespace="CloudCar.CCFramework.Core" %>
<%@ Import Namespace="CloudCar.CCFramework.ContentManagement.CalendarModule" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">


    <asp:Button runat="server" UseSubmitBehavior="true" PostBackUrl="~/CCAdmin/ContentManagement/CalendarModule/ProgramDetails.aspx" CssClass="SaveButton" style="float: right; width: 280px; margin-top: 30px;" Text="Add Program" />

    <img src="/CCTemplates/Admin/Images/icons/order.icon.png" alt="View Programs" width="75" height="75" class="HeadingIcon" /><br />
    <h1 class="form-heading-style">
        Programs
    </h1><hr />

    <asp:UpdatePanel runat="server" ID="upUpdate" ChildrenAsTriggers="true" EnableViewState="true" UpdateMode="Always">
        <ContentTemplate>

    <br /><asp:Label runat="server" ID="lblStatus" CssClass="status-message" Visible="False" /><br />
    
    <asp:DataGrid ID="gvPrograms" runat="server" AutoGenerateColumns="False" DataKeyNames="id" PageSize="10" 
            AllowPaging="true" AllowSorting="True" GridLines="None" CssClass="default-table" CellPadding="0" CellSpacing="0">
        <PagerStyle HorizontalAlign="Right" PageButtonCount="8" Mode="NumericPages" Position="Bottom" CssClass="default-table-pager" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:TemplateColumn HeaderText="Icon">
                <ItemTemplate>
                    <asp:Image runat="server" ImageUrl='<%# PictureController.GetPictureLink(CInt(Eval("IconImage")),30)  %>' style="margin-bottom: -10px;" />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn SortExpression="Id" HeaderText="Program">
                <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%# "~/CCAdmin/ContentManagement/CalendarModule/ProgramDetails.aspx?Program=" & Eval("ID").ToString  %>' Text='<%# Eval("Name") %>' />
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Instructors">
                <ItemTemplate>
                    <%# InstructorController.GetProgramInstructors(Integer.Parse(Eval("Id").ToString())).Count().ToString%>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Dates">
                <ItemTemplate>
                    <%# ScheduleController.GetFutureProgramSchedules(Integer.Parse(Eval("Id").ToString())).Count().ToString%>
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