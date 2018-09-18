<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="Schedule.aspx.vb" Inherits="CloudCar.Schedule1" title="Untitled Page" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<asp:Content ID="Content3" ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
    
<rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" Height="600px" Width="800px">
    <LocalReport ReportPath="Reports\Schedule.rdlc">
        <DataSources>
            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="ScheduleFull" />
        </DataSources>
    </LocalReport>
</rsweb:ReportViewer>

<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
    SelectMethod="GetScheduleForReport" TypeName="CloudCar.CCFramework.ContentManagement.CalendarModule.ScheduleController">
</asp:ObjectDataSource>

</asp:Content>