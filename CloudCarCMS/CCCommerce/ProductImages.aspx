<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProductImages.aspx.vb" Inherits="CloudCar.CCCommerce.ProductImages" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <asp:Literal runat="server" ID="litMetaKeywords" />
    <asp:Literal runat="server" ID="litMetaDescription" />
</head>

<body>

<form id="form1" runat="server">


<div style="width: 640px; text-align: left;">
    <h2 style="text-align: left; width: 640px;"><asp:Literal runat="server" ID="litTitle" /></h2>
    <asp:Image runat="server" ID="imgMain" /><br />
    <asp:Label runat="server" ID="lblDescription" Font-Size="Small" Font-Italic="true" />
</div>

<hr style="width: 640px;" />

<asp:Repeater runat="server" id="rptImages">
    <HeaderTemplate>
        <div style="width: 640px; height: 120px; overflow: auto; text-align: left;">
        <table>
            <tr>
    </HeaderTemplate>
    <ItemTemplate>
        <td valign="middle" align="center" style="background-color: #000000; padding: 4px;">
            <asp:ImageButton runat="server" ID="btnImage" AlternateText='<%# Eval("Description") %>' ImageUrl='<%# String.Format("/images/db/{0}/80/image{1}.jpg", Eval("ImageID"), Eval("ImageID")) %>' OnCommand="btnImage_Command" CommandArgument='<%# Eval("ImageID") %>' /></td>
    </ItemTemplate>
    <FooterTemplate>
            </tr>
        </table>
        </div>
    </FooterTemplate>
</asp:Repeater>

</form>

</body>
</html>
