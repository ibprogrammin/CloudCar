<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AddCommentControl.ascx.vb" Inherits="CloudCar.CCControls.Blogging.AddCommentControl" %>

<h3>Leave your comments</h3>
<h5>Feel free to leave a comment about this blog post.</h5>

<br />

<div style="margin: 10px; width: 580px;">
    <!--%=Html.ValidationSummary("Oops! You forgot a few things silly!", New With {.class = "SansStack", .style = "color: red; line-height: 1.4em;"})%-->
</div>

<fieldset>
    <asp:HiddenField runat="server" ID="BlogIdHiddenField" />

    <label>Name</label><br />
    <span class="it2">
        <asp:TextBox runat="server" ID="NameTextBox" />
    </span>
    <br class="clear" />
    
    <!-- TODO Add Validators -->

    <label>Email</label><br />
    <span class="it2">
        <asp:TextBox runat="server" ID="EmailTextBox" />
    </span>
    <br class="clear" />

    <label>Website</label><br />
    <span class="it2">
        <asp:TextBox runat="server" ID="UrlTextBox" />
    </span>
    <br class="clear" />

    <label>Comments</label><br />
    <span class="textarea1">
        <asp:TextBox runat="server" ID="CommentTextBox" TextMode="MultiLine" Row="4" Columns="5" />
    </span>
    <br class="clear" />

    <span style="margin-left: 10px;">
        
    </span>
            
    <p><asp:Button runat="server" ID="CreateCommentButton" CssClass="addurreview_but1" /></p>
</fieldset>