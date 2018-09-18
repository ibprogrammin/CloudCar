<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="Menu.aspx.vb" Inherits="CloudCar.CCContentManagement.ClientForms.Menu1" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">

    <meta runat="server" id="PageKeywordsMeta" name="keywords" content="" />
    <meta runat="server" id="PageDescriptionMeta" name="description" content="" />
    
</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">

    <img runat="server" id="imgHeadingImage" src="" alt="" visible="false" />
    
    <h1><asp:Literal runat="server" ID="litHeading" /></h1>
    <span class="SansStack" style="font-size: 12px; color: #888;"><asp:Literal runat="server" ID="litBreadCrumb" /></span><br /><br />
    
    <div style="width: 650px;"><asp:Literal runat="server" ID="litContent" /></div>

    <hr class="MediumOrangeLine" style="float: left; clear: both;" /><br style="clear: both;" /><br />

    <div style="min-height: 450px; width: 100%;">
        <ajax:TabContainer runat="server" ID="tcMenu" TabStripPlacement="Top"></ajax:TabContainer>
    </div>

</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server">
    <asp:Literal runat="server" ID="litScripts" />
</asp:Content>