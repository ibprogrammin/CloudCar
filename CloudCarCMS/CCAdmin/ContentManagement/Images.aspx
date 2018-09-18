<%@ Page title="Images" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Images.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.Images" %>
<%@ Register TagPrefix="SM" Namespace="CloudCar.CCControls" Assembly="CloudCarFramework" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        Images
        <i class="icon-instagram"></i>
        <!--img src="/CCTemplates/Admin/Images/icons/cc.images.icon.dark.png" alt="Images" /-->
    </h1><hr />
    
    <div class="search-bar" style="height: 55px;">
        <asp:Button id="btnUpload" runat="server" CssClass="SaveButton" Text="Save" style="float: right; margin-top: 5px;" />
        <div class="form-file-upload-display">
            <div class="form-fake-upload">
	            <input type="text" name="imagefilename" readonly="readonly" />
            </div>
            <asp:FileUpload runat="server" ID="fuImage" ToolTip="Select the image to upload" size="20" CssClass="form-real-upload" onchange="this.form.imagefilename.value = this.value;" /><br />
        </div>
    </div>

    <asp:Label ID="lblStatus" runat="server" Text="" CssClass="status-message" Visible="false" />
    
    <SM:DataPagerRepeater runat="server" ID="rptImages" DataSourceID="dsImages" PersistentDataSource="true">
        <HeaderTemplate>
            <table class="default-table">
                <thead class="default-table-header">
                    <tr>
                        <td></td>
                        <td>Media</td>
                        <td>Location</td>
                        <td>Size</td>
                        <td></td>
                    </tr>
                </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><asp:Image runat="server" class="trigger" ImageUrl='<%# String.Format("/images/db/{0}/83/{1}", Eval("PictureID"), Eval("PictureFileName")) %>' style="" /></td>
                <td><%# Eval("PictureContentType")%></td>
                <td><a href="<%# String.Format("/images/db/{0}/full/{1}", Eval("PictureID"), Eval("PictureFileName")) %>" target="_blank"><%# String.Format("/images/db/{0}/full/{1}", Eval("PictureID"), Eval("PictureFileName")) %></a></td>
                <td><%# String.Format("{0} Kb",CInt(Eval("PictureContentLength")) / 1000)%></td>
                <td><asp:LinkButton id="lbDelete" runat="server" CommandArgument='<%# Eval("PictureID") %>' CommandName="DeleteImage" Enabled='<%# HasRelationship(CType(Eval("PictureID"), Integer)) %>' Text="" class="icon-trash" style="color: #BF0000; font-size: 24px;" /></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </SM:DataPagerRepeater>
    <br />        
    <asp:DataPager ID="ImagesDataPager" runat="server" PagedControlID="rptImages" PageSize="15" style="font-size: 16px;">
        <Fields>
            <asp:NextPreviousPagerField ButtonType="Link" ShowPreviousPageButton="true" ShowNextPageButton="false" ShowFirstPageButton="true" ShowLastPageButton="false" PreviousPageText="Prev" FirstPageText="&nbsp;&laquo;&nbsp;" />
            <asp:NumericPagerField ButtonCount="10" ButtonType="Link" RenderNonBreakingSpacesBetweenControls="true" />
            <asp:NextPreviousPagerField ButtonType="Link" ShowNextPageButton="true" ShowLastPageButton="true" ShowFirstPageButton="false" ShowPreviousPageButton="false" NextPageText="Next" LastPageText="&nbsp;&raquo;&nbsp;" />
        </Fields>
    </asp:DataPager>

    <asp:SqlDataSource ID="dsImages" runat="server" ConnectionString="<%$ ConnectionStrings:MainConnectionString %>" 
        SelectCommand="SELECT [PictureID], [PictureFileName], [PictureContentType], [PictureContentLength] FROM [Picture]" />

</asp:Content>