<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="AddProductReviewControl.ascx.vb" Inherits="CloudCar.CCControls.Commerce.AddProductReviewControl" %>

<h2 id="review-title">Write a review</h2>

<asp:Label runat="server" ID="MessageLabel" CssClass="status-message" Visible="false" />
<asp:ValidationSummary runat="server" ID="vsValidation" CssClass="status-message" ValidationGroup="Review" HeaderText="Oops! Your forgot a few things silly!" />

<b>Your Name:</b><br />
<asp:TextBox runat="server" ID="NameTextBox" />
<br /><br />

<b>Email</b><br />
<asp:TextBox runat="server" ID="EmailTextBox" />
<br /><br />
    
<b>Avatar URL</b><br />
<asp:TextBox runat="server" ID="UrlTextBox" />
<br /><br />

<b>Your Review:</b>
<asp:TextBox runat="server" ID="CommentTextBox" TextMode="MultiLine" Rows="8" Columns="40" style="width: 98%;" />
<span style="font-size: 11px;"><span style="color: #FF0000;">Note:</span> HTML is not translated!</span><br>
<br /><br />

<b>Rating:</b><br />
<ajax:Rating runat="server" 
    ID="ProductRatingControl" 
    MaxRating="5" 
    CurrentRating="4" 
    EmptyStarCssClass="star-empty" 
    FilledStarCssClass="star-filled" 
    CssClass="rating-star" 
    StarCssClass="rating-item" 
    WaitingStarCssClass="star-waiting" />
<br style="clear: both;" /><br />

<div class="buttons">
    <div class="right">
        <asp:Button runat="server" ID="CreateReviewbutton" CssClass="button" Text="Continue" CausesValidation="true" ValidationGroup="Review" />
    </div>
</div>

<asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="NameTextBox" Display="None" ErrorMessage="Please enter your name." SetFocusOnError="true" ValidationGroup="Review" />
<asp:RequiredFieldValidator runat="server" ID="rfvEmail" ControlToValidate="EmailTextBox" Display="None" ErrorMessage="Please enter your email." SetFocusOnError="true" ValidationGroup="Review" />
<asp:RequiredFieldValidator runat="server" ID="rfvComment" ControlToValidate="CommentTextBox" Display="None" ErrorMessage="Please enter your review." SetFocusOnError="true" ValidationGroup="Review" />