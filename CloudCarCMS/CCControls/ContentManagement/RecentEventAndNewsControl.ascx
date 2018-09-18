<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="RecentEventAndNewsControl.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.RecentEventAndNewsControl" %>

<asp:Repeater ID="rptEntries" runat="server">
    <HeaderTemplate></HeaderTemplate>
    <ItemTemplate>
        <a href="/Home/News.html" title="<%# Eval("EventAndNewsName") %> - <%# Eval("DateAdded","{0:dddd, dd MMMM yyyy}") %>" class="FeaturedProgram">
                   
            <h1><%#Eval("EventAndNewsName")%></h1>
            <p>Date - <%#Eval("EventDate", "{0:dddd, dd MMMM yyyy}")%> <%#Eval("TimeInformation")%> - <%#Eval("Information")%></p>
        </a>
    </ItemTemplate>
    <FooterTemplate></FooterTemplate>
</asp:Repeater>
