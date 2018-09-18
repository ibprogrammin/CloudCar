<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="ContentPageDetails.aspx.vb" Inherits="CloudCar.CCAdmin.Archive.ContentPageDetails" ValidateRequest="false" %>

<%@ Register TagPrefix="SM" Namespace="CloudCar.CCControls.Validators" Assembly="CloudCarFramework" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server" ></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
    

<div class="DefaultContent">
    <h2>Content Pages</h2>
    <asp:Label runat="server" ID="lblStatus" CssClass="StoreStatusMessage" Visible="false" /><br style="clear: both;" />
    
    <asp:ListBox runat="server" ID="lbContentPages" DataValueField="id" DataTextField="ContentTitle" AutoPostBack="true" style="width: 460px; float: left;" Rows="3" />
    <asp:Button runat="server" ID="btnClear" Text="New" CausesValidation="false" CssClass="NewButton Green" style="height: 100px;" />
        
    <asp:HiddenField runat="server" ID="hfPageID" />
    
    <fieldset>
        <h2>Page Content</h2>
        <asp:Literal ID="litCurrentPage" runat="server" Visible="false" />
        <p>
            <label for="txtPageTitle">Page Title</label><br />
            <asp:TextBox runat="server" ID="txtPageTitle" style="width: 930px;" />
        </p>
        <p>
            <label for="txtPageTitle">Content Title</label><br />
            <asp:TextBox runat="server" ID="txtContentTitle" style="width: 930px;" />
        </p>
        <p>
            <label for="txtPageContent">Page Content</label><br />
            <obout:Editor ID="txtPageContent" runat="server" Width="940" Height="620" Submit="false" Appearance="full" ShowQuickFormat="false" StyleFile="/styles/main.styles.css">
                <AddCssFiles>
                    <Obout:CssFile Path="/styles/main.styles.min.css" />
                </AddCssFiles>
            </obout:Editor>
        </p>
        <p>
            <label for="txtPageTitle">Script</label><br />
            <asp:TextBox runat="server" ID="txtScript" TextMode="MultiLine" style="width: 930px; height: 220px;" />
        </p>
        <p>
            <label for="ckbSubMenu">Display in sub menu</label>
            <asp:CheckBox runat="server" id="ckbSubMenu" />
        </p>
    </fieldset>
    
    <fieldset style="width: 470px; float: left;">
        <br /><h2>SEO Content</h2>
        <p style="width: 420px;">
            <asp:HiddenField runat="server" ID="hfPermalink" Value="" />
            <label for="txtPermalink">Permalink</label><br />
            <asp:TextBox runat="server" ID="txtPermalink" style="width: 410px;" />
        </p>
        <p style="width: 420px;">
            <label for="txtKeywords">Keywords</label><br />
            <asp:TextBox runat="server" ID="txtKeywords" style="width: 410px;" />
        </p>
        <p style="width: 420px;">
            <label for="txtDescription">Description</label><br />
            <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" Rows="4" style="width: 410px;" />
        </p>
    </fieldset>
    
    <fieldset style="width: 450px; float: left; margin-left: 20px;">
        <br /><h2>Page Options</h2>
        <p style="width: 420px;">
            <label for="txtBreadcrumbTitle">Breadcrumb Title</label><br />
            <asp:TextBox runat="server" ID="txtBreadcrumbTitle" style="width: 430px;" />
        </p>
        <p style="width: 420px;">
            <label for="ddlMenu">Drop Down Menu</label><br />
            <asp:DropDownList runat="server" ID="ddlMenu" DataTextField="menu" DataValueField="id" style="width: 446px;" />
        </p>
        <p style="width: 420px;">
            <label for="txtMenuOrder">Order</label><br />
            <asp:TextBox runat="server" ID="txtMenuOrder" Text="1" style="width: 430px;" />
        </p>
        <p style="width: 420px;">
            <label for="">Parent Page</label><br />
            <asp:DropDownList runat="server" ID="ddlParentPage" DataTextField="ContentTitle" DataValueField="id" style="width: 446px;" />
        </p>
    </fieldset>
    
    <br style="clear: both;" />
    <asp:Button runat="server" ID="btnSave" Text="Save" CausesValidation="true" ValidationGroup="ValidateForm" CssClass="Green" />
    <asp:Button runat="server" ID="btnDelete" Text="Delete" CausesValidation="false" CssClass="Red" />
    <br /><br />

    <asp:RequiredFieldValidator ID="rfvPT" runat="server" ControlToValidate="txtPageTitle" ErrorMessage="You should create a title for this page." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vcePT" runat="server" TargetControlID="rfvPT" />

    <asp:RequiredFieldValidator ID="rfvPC" runat="server" ControlToValidate="txtPageContent" ErrorMessage="You should add some content to this page." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vcePC" runat="server" TargetControlID="rfvPC" />

    <asp:RequiredFieldValidator ID="rfvPL" runat="server" ControlToValidate="txtPermalink" ErrorMessage="You should create a permalink for this page." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vcePL" runat="server" TargetControlID="rfvPL" />
    <SM:PermalinkValidator ID="pvPermalink" runat="server" ControlToValidate="txtPermalink" PermalinkType="ContentPage" ErrorMessage="The permalink you have choosen is not unique." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vcePermalinkValidator" runat="server" TargetControlID="pvPermalink" />
        
    <asp:RequiredFieldValidator ID="rfvMenuTitle" runat="server" ControlToValidate="txtBreadcrumbTitle" ErrorMessage="You should create a breadcrumb title for this page." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vceMenuTitle" runat="server" TargetControlID="rfvMenuTitle" />

    <asp:RequiredFieldValidator ID="rfvMenu" runat="server" ControlToValidate="ddlMenu" ErrorMessage="You should select a menu for this page." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vceMenu" runat="server" TargetControlID="rfvMenu" />

    <asp:RequiredFieldValidator ID="rfvMO" runat="server" ControlToValidate="txtMenuOrder" ErrorMessage="You should select a position in the menu for this page." SetFocusOnError="true" Display="None" ValidationGroup="ValidateForm" />
    <ajax:ValidatorCalloutExtender ID="vceMO" runat="server" TargetControlID="rfvMO" />

</div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="FirstContentPlaceHolder" runat="server"></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server"></asp:Content>
