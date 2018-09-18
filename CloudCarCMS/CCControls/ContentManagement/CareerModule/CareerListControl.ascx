<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CareerListControl.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.CareerModule.CareerListControl" %>

<%@ Import Namespace="CloudCar.CCFramework.ContentManagement.CareerModule" %>
<%@ Register TagPrefix="SM" Namespace="CloudCar.CCControls" Assembly="CloudCarFramework" %>

<SM:DataPagerRepeater runat="server" ID="CareersRepeater" PersistentDataSource="true" TotalRows="10">
    <ItemTemplate>
        <h2><%# Eval("Title")%></h2>
        <p><b>Department: </b><%# CareerController.GetDepartmentLabel(CInt(Eval("Department")))%></p>
        <%# Eval("Description")%>
    </ItemTemplate>
    <SeparatorTemplate>
        <br class="clear-both" />
    </SeparatorTemplate>
</SM:DataPagerRepeater>