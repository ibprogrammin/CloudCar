<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EventsControl.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.EventsModule.EventsControl" %>

<asp:Repeater runat="server" ID="EventsRepeater">
    <ItemTemplate>
        <h3><%#Eval("Title")%></h3>
        <%# IIf(CInt(Eval("ImageId")) = Nothing, "", String.Format("<img src=""/images/db/{0}/107/{1}.jpg"" alt=""{1}"" style=""float:left;padding:5px;border:1px solid #999;margin:10px; margin-top: 0px;"" />", Eval("ImageId"), Eval("Title")))%>
        <p>
            <b>Date -</b> <%# Eval("EventDate", "{0:dddd, dd MMMM yyyy}")%> at <%#Eval("Time", "{0:h:mm tt}")%><br/>
            <b>Location -</b> <%#Eval("Location")%>
        </p>
        <p><%# Eval("Details")%></p>
    </ItemTemplate>
    <SeparatorTemplate>
        <br />
    </SeparatorTemplate>
</asp:Repeater>