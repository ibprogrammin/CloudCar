<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ProductControl.ascx.vb" Inherits="CloudCar.CCControls.ContentManagement.ProductControls.ProductControl" %>

<asp:Panel runat="server" ID="pnlProduct" class="CMSProductControl" style="float: left; outline: 1px solid #B3B3B3; border: 1px solid #FFF; text-align: center; position: relative;">
    <img id="imgProduct" runat="server" alt="" src="" width="155" height="130" style="margin: 3px;" />
    
    <div style="width: 100%; height: 30px; bottom: 0px; position: absolute; float: left; background-color: #A3BCCC; text-align: right; padding-top: 15px;">
        <asp:HyperLink ID="hlProductName" runat="server" Text="Product Name" CssClass="ProductControlLink" />
    </div>
</asp:Panel>