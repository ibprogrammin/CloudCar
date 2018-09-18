<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="DatabaseImageControl.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.DatabaseImageControl" %>

<asp:HiddenField runat="server" ID="hfImageID" />
<img runat="server" ID="imgRotatorImage" alt="Image Preview" visible="false" src="" style="background-color: #F4F4F4; padding: 4px; border: 1px solid #999; float: right; margin-top: 20px;" />
<asp:TextBox runat="server" ID="txtImageLocation" ReadOnly="true" /><br />
<asp:FileUpload runat="server" ID="fuImage" ToolTip="Select the image for this rotator item." size="30" />

<asp:LinkButton ID="btnGetImage" runat="server" Text="Select Image" OnCommand="btnGetImage_Command" />

<asp:Panel runat="server" ID="panImages">


<div style="margin-right: 20px; padding: 5px; border: 1px solid #ADCAD8; background-color: #EEFAFF;">
    <asp:Button id="btnUpload" runat="server" CssClass="Orange" Text="Upload Image" style="float: right;" />
    <asp:FileUpload runat="server" ID="FileUpload1" />
</div>

<asp:Label ID="lblStatus" runat="server" Text="" CssClass="status-message" Visible="false" />
<br />


<asp:Repeater runat="server" ID="rptImages" DataSourceID="dsImages">
    <HeaderTemplate>
        <div style="border: 1px solid #DDD; display: table; margin-right: 20px;">
    </HeaderTemplate>
    <ItemTemplate>
        
        <div class="bubble-info" style="width: 110px; height: 110px; padding: 0px; margin: 9px; float: left; display: table-cell; vertical-align: middle; text-align: center; border: 1px solid #AAA; background: #FBFBFB;">
            <asp:Image ID="imgImage" runat="server" class="trigger" ImageUrl='<%# String.Format("/images/db/{0}/80/{1}", Eval("PictureID"), Eval("PictureFileName")) %>' style="margin: auto; padding: 8px; cursor: pointer; vertical-align: middle;" />
        
            <asp:Panel ID="panDetails" runat="server" CssClass="popup">
                <h2>Image Details</h2>
                <asp:LinkButton id="lbDelete" runat="server" CommandArgument='<%# Eval("PictureID") %>' CommandName="DeleteImage" Enabled='<%# HasRelationship(Eval("PictureID")) %>' Text="Delete" style="float: right; color: Red; padding-right: 10px;" />
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# String.Format("/images/db/{0}/full/{1}", Eval("PictureID"), Eval("PictureFileName")) %>' Text='<%# Eval("PictureFileName") %>' /><br /><br />
                <asp:TextBox ID="TextBox1" runat="server" ReadOnly="true" Text='<%# String.Format("/images/db/{0}/full/{1}", Eval("PictureID"), Eval("PictureFileName")) %>' style="width: 420px;" />
            </asp:Panel>
        </div>

    </ItemTemplate>
    <FooterTemplate>
        </div>
    </FooterTemplate>
</asp:Repeater>

<asp:SqlDataSource ID="dsImages" runat="server" ConnectionString="<%$ ConnectionStrings:MainConnectionString %>" SelectCommand="SELECT [PictureID], [PictureFileName], [PictureContentType], [PictureContentLength] FROM [Picture]" />






<script type="text/javascript" language="javascript">

$(function () {
  $('.bubbleInfo').each(function () {
    // options
    var distance = 55;
    var time = 250;
    var hideDelay = 500;

    var hideDelayTimer = null;

    // tracker
    var beingShown = false;
    var shown = false;
    
    var trigger = $('.trigger', this);
    var popup = $('.popup', this).css('opacity', 0);

    // set the mouseover and mouseout on both element
    $([trigger.get(0), popup.get(0)]).mouseover(function () {
      // stops the hide event if we move from the trigger to the popup element
      if (hideDelayTimer) clearTimeout(hideDelayTimer);

      // don't trigger the animation again if we're being shown, or already visible
      if (beingShown || shown) {
        return;
      } else {
        beingShown = true;

        // reset position of popup box
        popup.css({
          top: -100,
          left: -33,
          display: 'block' // brings the popup back in to view
        })

        // (we're using chaining on the popup) now animate it's opacity and position
        .animate({
          top: '-=' + distance + 'px',
          opacity: 1
        }, time, 'swing', function() {
          // once the animation is complete, set the tracker variables
          beingShown = false;
          shown = true;
        });
      }
    }).mouseout(function () {
      // reset the timer if we get fired again - avoids double animations
      if (hideDelayTimer) clearTimeout(hideDelayTimer);
      
      // store the timer so that it can be cleared in the mouseover if required
      hideDelayTimer = setTimeout(function () {
        hideDelayTimer = null;
        popup.animate({
          top: '-=' + distance + 'px',
          opacity: 0
        }, time, 'swing', function () {
          // once the animate is complete, set the tracker variables
          shown = false;
          // hide the popup entirely after the effect (opacity alone doesn't do the job)
          popup.css('display', 'none');
        });
      }, hideDelay);
    });
  });
});

</script>




</asp:Panel>