<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="NewsControl.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.NewsModule.NewsControl" %>

<asp:Repeater runat="server" ID="NewsRepeater">
    <ItemTemplate>
        <h3><a href="<%# String.Format("/News/{0}.html", Eval("Permalink")) %>"><%#Eval("Title")%></a></h3>
        <h4><%# Eval("SubTitle")%></h4>
        <%# IIf(Eval("ImageId") Is Nothing, "", String.Format("<img src=""/images/db/{0}/107/{1}.jpg"" alt=""{1}"" style=""float:left;padding:5px;border:1px solid #999;margin:10px; margin-top: 0px;"" />", Eval("ImageId"), Eval("Title")))%>
        <p>
            <i><%# Eval("PublishDate", "{0:dddd, dd MMMM yyyy}")%></i><br/>
        </p>
        <p><%# Eval("Details")%></p>
    </ItemTemplate>
    <SeparatorTemplate>
        <br />
    </SeparatorTemplate>
</asp:Repeater>