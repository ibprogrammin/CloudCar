<%@ Page Title="Blogs" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="Blogs.aspx.vb" Inherits="CloudCar.CCAdmin.Blogging.Blogs" ValidateRequest="false" %>
<%@ Register TagPrefix="SM" Namespace="CloudCar.CCControls.Validators" Assembly="CloudCarFramework" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">
    
    <div class="edit-buttons-panel">
        <asp:Button runat="server" id="NewButton" CssClass="SaveButton" CausesValidation="false" style="float: right; width: 140px; position:relative; z-index: 9;" Text="New" ToolTip="Create a new page" />
        <asp:Button runat="server" ID="DeleteButton" Text="Delete" CausesValidation="false" CssClass="DeleteButton" OnClientClick="return confirm('Are you sure you wish to delete the current blog post and all associated comments? Your changes cannot be undone.');" style="width: 140px; float: right; margin-right: 10px; margin-left: 0px;" Enabled="False" />
        <asp:Button runat="server" ID="SaveButton" Text="Save" CausesValidation="true" ValidationGroup="ValidateForm" CssClass="SaveButton" style="width: 140px; float: right; margin-left: 0px; margin-right: 10px;" Enabled="False" />
        <asp:Button runat="server" id="BackButton" CssClass="BlueButton" CausesValidation="false" style="float: right; margin-left: 0px; margin-right: 10px; width: 45px; z-index: 8;" Text="" ToolTip="Return to page list" Enabled="False" />
    </div>

    <h1 class="form-heading-style">
        Blog
        <i class="icon-rss"></i>
    </h1>
    <hr />

    <asp:Label runat="server" ID="lblStatus" CssClass="status-message" Visible="false" />
    
    <asp:PlaceHolder runat="server" ID="PostsListPlaceHolder" Visible="False">
        
        <asp:GridView runat="server" 
                ID="PostsList" 
                AutoGenerateColumns="False" 
                DataKeyNames="Id" 
                AllowSorting="true" 
                AllowPaging="true" 
                GridLines="None" 
                CssClass="default-table" 
                CellPadding="0" 
                CellSpacing="0" 
                PageSize="12">
            <PagerStyle HorizontalAlign="Right" CssClass="default-table-pager" />
            <PagerSettings PageButtonCount="20" Mode="NumericFirstLast" Position="Bottom" />
            <HeaderStyle CssClass="default-table-header" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <i class="icon-file-text"></i>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Post" HeaderStyle-CssClass="TableHeaderText">
                    <ItemTemplate>
                        <asp:LinkButton ID="SelectPostButton" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="SelectPost" Text='<%# Eval("Title") %>' /><br />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="URL" HeaderStyle-CssClass="TableHeaderText">
                    <ItemTemplate>
                        <span class="Highlight"><%# String.Format("/Blog/{0}.html", Eval("Permalink")) %></span> (<asp:HyperLink runat="server" NavigateUrl='<%# String.Format("/Blog/{0}.html", Eval("Permalink")) %>' Text="View" />)
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <asp:LinkButton ID="DeletePostButton" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="DeletePost" Text="" CssClass="icon-trash delete-icon" ToolTip="Delete the current page permanently" OnClientClick="return confirm('Are you sure you want to delete this blog post permanently?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

    </asp:PlaceHolder>
    
    <asp:PlaceHolder runat="server" ID="PostEditPlaceHolder" Visible="False">
    
    <asp:HiddenField runat="server" ID="hfBlogID" />

    <div class="tab-container">
	    <ul class="tabs">
		    <li class="tab"><a href="#tab-details">Post Details</a></li>
		    <li class="tab"><a href="#tab-summary">Summary</a></li>
            <li class="tab"><a href="#tab-content">Content</a></li>
            <li class="tab"><a href="#tab-seo">SEO</a></li>
		    <li class="tab"><a href="#tab-comments">Comments</a></li>
	    </ul>
        <div id="tab-details">
            <asp:Literal runat="server" ID="litCurrentPage" Visible="false" />
            <asp:Literal runat="server" ID="litAuthor" Visible="false" />
    
            <label for="BlogTitleTextBox">Title</label>
            <asp:TextBox runat="server" ID="BlogTitleTextBox" CssClass="form-text-box" />
            <br class="clear-both" /><br />
        
            <label for="BlogAuthorDropDown">Author ( <a href="/CCAdmin/Blogging/Authors.aspx" title="Add an author">New</a> )</label>
            <asp:DropDownList runat="server" ID="BlogAuthorDropDown" DataTextField="Name" DataValueField="ID" CssClass="form-select-box" />
            <br class="clear-both" /><br />
    
            <label for="BlogSubHeadingTextBox">Sub-Heading</label>
            <asp:TextBox runat="server" ID="BlogSubHeadingTextBox" CssClass="form-text-box" />
            <br class="clear-both" /><br />
            
            <label for="BlogHeadingImageTextBox">Heading Image URL <br />(<a href="/CCAdmin/ContentManagement/Images.aspx" title="" target="_blank">Upload Image</a>)</label>
            <asp:TextBox ID="BlogHeadingImageTextBox" runat="server" CssClass="form-text-box" />
            <br class="clear-both" /><br />
        
            <label for="BlogThumbnailImageTextBox">Thumbnail Image URL <br />(<a href="/CCAdmin/ContentManagement/Images.aspx" title="" target="_blank">Upload Image</a>)</label>
            <asp:TextBox ID="BlogThumbnailImageTextBox" runat="server" CssClass="form-text-box" />
            <br class="clear-both" /><br />

            <label for="BlogSetLiveCheckBox">Make Post Live</label>
            <asp:CheckBox runat="server" ID="BlogSetLiveCheckBox" CssClass="form-check-box" />
            <br class="clear-both" />
                
            <a href="#" class="next-tab" rel="1">next</a>
            <br class="clear-both" />
        </div>
        <div id="tab-summary">
            <asp:TextBox ID="BlogSummaryTextBox" runat="server" TextMode="MultiLine" Rows="5" class="form-text-area-full" />
            <br class="clear-both" /><br />
                
            <a href="#" class="prev-tab" rel="0">prev</a>
            <a href="#" class="next-tab" rel="2">next</a>
            <br class="clear-both" />
        </div>
        <div id="tab-content">
            <textarea runat="server" id="BlogContentTextArea" class="ck-editor" />
            <br class="clear-both" />
                
            <a href="#" class="prev-tab" rel="1">prev</a>
            <a href="#" class="next-tab" rel="3">next</a>
            <br class="clear-both" />
        </div>  
        <div id="tab-seo">
            <label for="BlogPermalinkTextBox">Permalink</label>
            <asp:TextBox runat="server" ID="BlogPermalinkTextBox" CssClass="form-text-box" />
            <br class="clear-both" /><br />
        
            <label for="BlogKeywordsTextBox">Keywords</label>
            <asp:TextBox runat="server" ID="BlogKeywordsTextBox" CssClass="form-text-box" />
            <br class="clear-both" /><br />
    
            <label for="BlogDescriptionTextBox">Description <br />(max. 260 characters)</label>
            <asp:TextBox runat="server" ID="BlogDescriptionTextBox" TextMode="MultiLine" Rows="5" MaxLength="260" CssClass="form-text-area" />
            <br class="clear-both" /><br />
    
            <label for="BlogCategoryTextBox">Category</label>
            <asp:TextBox runat="server" ID="BlogCategoryTextBox" CssClass="form-text-box" />
            <br class="clear-both" /><br />
            
            <asp:ScriptManagerProxy ID="smpProxy" runat="server" />

            <ajax:AutoCompleteExtender runat="server" 
                ID="aceCategories" 
                TargetControlID="BlogCategoryTextBox" 
                ServiceMethod="GetCategories" 
                ServicePath="~/services/BlogCategoryService.asmx" 
                MinimumPrefixLength="1" 
                EnableCaching="true" 
                CompletionSetCount="10" 
                CompletionInterval="1000" />
                
            <br class="clear-both" />
                
            <a href="#" class="prev-tab" rel="2">prev</a>
            <a href="#" class="next-tab" rel="4">next</a>
            <br class="clear-both" />
        </div>
        <div id="tab-comments">
            <asp:GridView runat="server"
                    ID="gvwCommentsDetails" 
                    AutoGenerateColumns="false" 
                    AllowPaging="true" 
                    AllowSorting="true" 
                    GridLines="None" 
                    CssClass="default-table" 
                    CellPadding="0" 
                    CellSpacing="0">
                <PagerStyle HorizontalAlign="Right" CssClass="default-table-pager" />
                <PagerSettings PageButtonCount="20" Mode="NumericFirstLast" Position="Bottom" />
                <HeaderStyle CssClass="default-table-header" />
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Name" />
                    <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="EventAndNewsName" />
                    <asp:BoundField DataField="Comment" HeaderText="Comment" />    
                    <asp:TemplateField HeaderText="Approved">
                        <ItemTemplate>
                            <asp:CheckBox ID="ApprovedCheckBox" runat="server" checked='<%# Eval("Approved") %>' CommentID='<%# Eval("CommentID") %>' OnCheckedChanged="ApprovedCheckBoxCheckChanged" AutoPostBack="True"/>
                        </ItemTemplate>
                    </asp:TemplateField>      
                </Columns> 
            </asp:GridView>
            <br class="clear-both" />
            
            <a href="#" class="prev-tab" rel="3">prev</a>
            <br class="clear-both" />
        </div>
    </div>
    

    <div id="dv" style="position: absolute; background-color: Gray; padding: 10px; display: none;" runat="server">
        <ajax:AsyncFileUpload ID="FileUploadControl" runat="server" Style="display: none" Width="300px" UploaderStyle="Modern" />
        <asp:Button ID="btnUpload" runat="server" Text="Upload" Style="display: none" OnClick="btnUpload_Click" CssClass="Green" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Style="display: none" OnClientClick="hide();" CssClass="Red" />
    </div>

    <asp:RequiredFieldValidator ID="rfvBT" runat="server" ControlToValidate="BlogTitleTextBox" ErrorMessage="You should create a title for this blog." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vceBT" runat="server" TargetControlID="rfvBT" />
    
    <asp:RequiredFieldValidator ID="rfvBC" runat="server" ControlToValidate="BlogContentTextArea" ErrorMessage="You should add some content to this blog entry." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vceBC" runat="server" TargetControlID="rfvBC" />
    
    <asp:RequiredFieldValidator ID="rfvCS" runat="server" ControlToValidate="BlogSummaryTextBox" ErrorMessage="You should add a content summary to this blog entry." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vceCS" runat="server" TargetControlID="rfvCS" />
    
    <asp:RequiredFieldValidator ID="rfvPL" runat="server" ControlToValidate="BlogPermalinkTextBox" ErrorMessage="You should create a permalink for this blog entry." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vcePL" runat="server" TargetControlID="rfvPL" />
    <SM:PermalinkValidator ID="pvPermalink" runat="server" ControlToValidate="BlogPermalinkTextBox" PermalinkType="Blog" ErrorMessage="The permalink you have choosen is not unique." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vcePermalinkValidator" runat="server" TargetControlID="pvPermalink" />
    
    <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ControlToValidate="BlogCategoryTextBox" ErrorMessage="You should pick a category for this blog entry." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vceCategory" runat="server" TargetControlID="rfvCategory" />
    
    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="BlogTitleTextBox" runat="server" ValidationExpression="^[\s\S]{0,255}$" ErrorMessage="Your title should be no more than 255 characters in length" SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" TargetControlID="RegularExpressionValidator1" />
    
    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="BlogSubHeadingTextBox" runat="server" ValidationExpression="^[\s\S]{0,255}$" ErrorMessage="Your sub heading should be no more than 255 characters in length" SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" TargetControlID="RegularExpressionValidator2" />
    
    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="BlogKeywordsTextBox" runat="server" ValidationExpression="^[\s\S]{0,255}$" ErrorMessage="Your keywords should be no more than 255 characters in length" SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" TargetControlID="RegularExpressionValidator3" />
    
    <asp:RegularExpressionValidator ID="revDescriptionLength" ControlToValidate="BlogDescriptionTextBox" runat="server" ValidationExpression="^[\s\S]{0,260}$" ErrorMessage="Your description should be no more than 260 characters in length" SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vceDescriptionLength" runat="server" TargetControlID="revDescriptionLength" />
    
    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ControlToValidate="BlogPermalinkTextBox" runat="server" ValidationExpression="^[\s\S]{0,255}$" ErrorMessage="Your permalink should be no more than 255 characters in length" SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" TargetControlID="RegularExpressionValidator4" />
    
    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" ControlToValidate="BlogCategoryTextBox" runat="server" ValidationExpression="^[\s\S]{0,255}$" ErrorMessage="Your category should be no more than 255 characters in length" SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server" TargetControlID="RegularExpressionValidator5" />


    </asp:PlaceHolder>
    
</asp:Content>

<asp:Content ContentPlaceHolderID="cphScripts" runat="server">

<script language="javascript" type="text/javascript">

    function show() {
        document.getElementById("<%=dv.ClientID%>").style.display = '';
        document.getElementById("<%=FileUploadControl.ClientID%>").style.display = '';
        document.getElementById("<%=btnUpload.ClientID%>").style.display = '';
        document.getElementById("<%=btnCancel.ClientID%>").style.display = '';

        showFloatDiv();
    }

    function hide() {
        document.getElementById("<%=dv.ClientID%>").style.display = "none";
        document.getElementById("<%=FileUploadControl.ClientID%>").style.display = "none";
        document.getElementById("<%=btnUpload.ClientID%>").style.display = "none";
        document.getElementById("<%=btnCancel.ClientID%>").style.display = "none";
    }

    function showFloatDiv() {
        if (!e) {
            var e = window.event || arguments.callee.caller.arguments[0];
        }

        var scrolledV = scrollV();
        var scrolledH = (navigator.appName == 'Netscape') ? document.body.scrollLeft : document.body.scrollLeft;

        tempX = (navigator.appName == 'Netscape') ? e.clientX : event.clientX;
        tempY = (navigator.appName == 'Netscape') ? e.clientY : event.clientY;

        document.getElementById("<%=dv.ClientID%>").style.left = (tempX + scrolledH) + 'px';
        document.getElementById("<%=dv.ClientID%>").style.top = (tempY + scrolledV) + 'px';
        document.getElementById("<%=dv.ClientID%>").style.display = "";
    }

    function scrollV() {
        var scrolledV;
        if (window.pageYOffset) {
            scrolledV = window.pageYOffset;
        }
        else if (document.documentElement && document.documentElement.scrollTop) {
            scrolledV = document.documentElement.scrollTop;
        }
        else if (document.body) {
            scrolledV = document.body.scrollTop;
        }
        return scrolledV;
    }

</script>

</asp:Content>