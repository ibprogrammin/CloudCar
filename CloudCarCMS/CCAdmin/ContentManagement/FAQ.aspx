<%@ Page Title="Frequently Asked Questions" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Admin.Master" CodeBehind="FAQ.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.FAQ" %>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <h1 class="form-heading-style">
        FAQ
        <i class="icon-comment"></i>
        <asp:Button id="btnAddFaq" runat="server" CssClass="SaveButton heading-button-new" CausesValidation="true" Text="New" />
    </h1><hr />

    <asp:Label ID="lblStatus" runat="server" Text="" CssClass="status-message" Visible="false" />
    
    <asp:PlaceHolder ID="phAddFaq" runat="server" Visible="false">
    
    <div class="tab-container">
	    <ul class="tabs">
	        <li class="tab"><a href="#tab-details">Details</a></li>
	    </ul>
        <div id="tab-details">

            <label>Question</label>        
            <asp:TextBox ID="txtQuestion" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-text-area" />
            <asp:RequiredFieldValidator ID="rfvQ" runat="server" ControlToValidate="txtQuestion"
                ErrorMessage="Please enter a Question." SetFocusOnError="true" Display="None" ValidationGroup="FAQ" />
            <ajax:ValidatorCalloutExtender ID="vceQ" runat="server" TargetControlID="rfvQ" />
            <br class="clear-both" /><br />
        
            <label>Answer</label>
            <asp:TextBox ID="txtAnswer" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-text-area" />
            <asp:RequiredFieldValidator ID="rfvA" runat="server" ControlToValidate="txtAnswer"
                ErrorMessage="Please Enter an Answer." SetFocusOnError="true" Display="None" ValidationGroup="FAQ" />
            <ajax:ValidatorCalloutExtender ID="vceA" runat="server" TargetControlID="rfvA" />
            <br class="clear-both" /><br />
            
            <label>Order</label>
            <asp:TextBox ID="txtOrder" runat="server" TextMode="SingleLine" Text="1" CssClass="form-text-box" />
            <br class="clear-both" /><br />
                
        </div>
    </div>

    <br class="clear-both" />

    <asp:Button runat="server" ID="btnAdd" Text="Save" CausesValidation="true" ValidationGroup="ValidateForm" CssClass="SaveButton" />
    <asp:Button runat="server" ID="btnCancel" Text="Clear" CausesValidation="false" CssClass="DeleteButton" />

    <br class="clear-both" /><hr />

    </asp:PlaceHolder>

    <asp:GridView runat="server" 
            ID="gvFAQ" 
            AutoGenerateColumns="False" 
            DataSourceID="FaqDataSource" 
            DataKeyNames="FaqID" 
            AllowSorting="true" 
            AllowPaging="true" 
            CssClass="default-table">
        <PagerStyle HorizontalAlign="Right" CssClass="default-table-pager" />
        <PagerSettings PageButtonCount="20" Mode="NumericFirstLast" Position="Bottom" />
        <HeaderStyle CssClass="default-table-header" />
        <Columns>
            <asp:BoundField DataField="Question" HeaderText="Question" SortExpression="Question" ItemStyle-Width="325px" />
            <asp:BoundField DataField="Answer" HeaderText="Answer" SortExpression="Answer" ItemStyle-Width="325px" />
            <asp:BoundField DataField="OrderNumber" HeaderText="Sort Order" SortExpression="OrderNumber" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="lbDelete" runat="server" CommandArgument='<%# Eval("FaqID") %>' CommandName="DeleteFAQ" Text="" CssClass="icon-trash delete-icon" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <asp:SqlDataSource ID="FaqDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:MainConnectionString %>"
        SelectCommand="SELECT [FaqID], [Question], [Answer], [OrderNumber] FROM [Faq] ORDER BY [OrderNumber]" />

</asp:Content>