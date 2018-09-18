<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="RecentNewsControl.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.NewsModule.RecentNewsControl" %>
<%@ Import Namespace="CloudCar.CCFramework.Core" %>

<asp:Repeater ID="NewsRepeater" runat="server">
    <HeaderTemplate>
        <h3>The Latest</h3>
    </HeaderTemplate>
    <ItemTemplate>
        <h4><a href='<%# String.Format("/News/{0}.html", Eval("Permalink"))%>' title='<%# String.Format("{0} - {1}", Eval("Title"), Eval("PublishDate","{0:dddd, dd MMMM yyyy}")) %>'><%# Eval("Title")%></a></h4>
        <p><%# TextFunctions.StripShortString(Eval("Summary").ToString, 190)%></p>
            
        <%# IIf(Eval("ImageId") Is Nothing, "", String.Format("<div class=""newsimg""><img src=""/images/db/{0}/107/{1}.jpg"" alt=""{1}"" /></div>", Eval("ImageId"), Eval("Permalink")))%>
    </ItemTemplate>
    <AlternatingItemTemplate>
        <h4><a href='<%# String.Format("/News/{0}.html", Eval("Permalink"))%>' title='<%# String.Format("{0} - {1}", Eval("Title"), Eval("PublishDate","{0:dddd, dd MMMM yyyy}")) %>'><%# Eval("Title")%></a></h4>
        <p><%# TextFunctions.StripShortString(Eval("Summary").ToString, 190)%></p>
            
        <%# IIf(Eval("ImageId") Is Nothing, "", String.Format("<div class=""newsimg""><img src=""/images/db/{0}/107/{1}.jpg"" alt=""{1}"" /></div>", Eval("ImageId"), Eval("Permalink")))%>
    </AlternatingItemTemplate>
    <FooterTemplate>
        
    </FooterTemplate>
</asp:Repeater>
