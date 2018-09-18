<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="TestimonialControl.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.TestimonialControl" %>
<%@ Import Namespace="CloudCar.CCFramework.Core" %>

<asp:Repeater ID="TestimonialRepeater" runat="server">
    <HeaderTemplate>
    </HeaderTemplate>
    <ItemTemplate>
        
        <section class="testimonial-bubble">
            <%# String.Format("<img src=""/images/db/{0}/full/{1}"" alt="""" style=""display: none;"" />", Eval("ImageId"), PictureController.GetPictureFilename(CInt(Eval("ImageId"))))%>
        
            <blockquote>
                &quot;<%#Eval("Quote")%>&quot;
            </blockquote>
            <em>~ <%#Eval("Author")%></em>
        </section>

    </ItemTemplate>
    <FooterTemplate>
    </FooterTemplate>
</asp:Repeater>






