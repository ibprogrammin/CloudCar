<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="UpcomingEventsControl.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.EventsModule.UpcomingEventsControl" %>
<%@ Import Namespace="CloudCar.CCFramework.Core" %>

<asp:Repeater runat="server" ID="UpcomingEventsRepeater">
    <ItemTemplate>
        <li>
        	<h3><%# Eval("Title")%></h3>
            <a href='<%# String.Format("/Events/{0}.html", Eval("Permalink"))%>' title='<%# String.Format("{0} - {1}", Eval("Title"), Eval("DateAdded","{0:dddd, dd MMMM yyyy}")) %>'><%# Eval("Location")%> - <%# Eval("DateAdded", "{0:dddd, dd MMMM yyyy}")%></a>
            <p><%# TextFunctions.StripShortString(Eval("Details").ToString, 190)%></p>
    	</li>
    </ItemTemplate>
</asp:Repeater>