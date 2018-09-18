<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="EditImages.aspx.vb" Inherits="CloudCar.EditImages" %>

<asp:Content ID="Content3" ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">

<div class="DefaultContent">

    <table cellspacing="3" cellpadding="0" class="default-table" style="width: 920px;">
        <tr>
            <td style="text-align: center; vertical-align: middle;">
                <asp:FileUpload runat="server" ID="fuImage" Width="100%" CssClass="FileBox" />
            </td>
            <td style="width: 30%; text-align: center; vertical-align: middle;">
                <asp:LinkButton id="btnUpload" runat="server" CssClass="OrangeButton SerifStack" style="width: 97%; position: relative; top: 2px;"><span class="OrangeButton">Upload Image</span></asp:LinkButton>
            </td>
        </tr>
    </table>

    <asp:Label ID="lblStatus" runat="server" Text="" CssClass="StoreStatusMessage" Visible="false" />
    <br />

    <asp:GridView runat="server" ID="gvImages" AllowSorting="true" AutoGenerateColumns="False" DataSourceID="dsEN" DataKeyNames="PictureID" Width="920px" 
            GridLines="None" CellPadding="0" CellSpacing="0" CssClass="ShoppingCart">
        <RowStyle CssClass="SCRow" />
        <AlternatingRowStyle CssClass="SCAlternatingRow" />
        <HeaderStyle CssClass="TableHeader" />
        <Columns>
            <asp:TemplateField HeaderText="Image" SortExpression="EventAndNewsID" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="HeaderLeftCell">
                <ItemTemplate>
                    <asp:Image runat="server" ImageUrl='<%# "~/Handlers/GetPicture.ashx?PictureID=" & Eval("PictureID") & "&Size=80" %>' CssClass="TableImage" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Filename" SortExpression="PictureFileName" HeaderStyle-CssClass="HeaderCell">
                <ItemTemplate>
                    <asp:HyperLink runat="server" NavigateUrl='<%# "~/Handlers/GetPicture.ashx?PictureID=" & Eval("PictureID") %>' Text='<%# Eval("PictureFileName") %>' /><br />
                    <asp:TextBox runat="server" ReadOnly="true" Text='<%# "/images/db/" & Eval("PictureID") & "/full/" & Eval("PictureFileName") %>' CssClass="TextBox" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="PictureContentType" HeaderText="Type" SortExpression="PictureContentType" HeaderStyle-CssClass="HeaderCell" />
            <asp:BoundField DataField="PictureContentLength" HeaderText="Size" SortExpression="PictureContentLength" HeaderStyle-CssClass="HeaderCell" />
            <asp:TemplateField HeaderStyle-CssClass="HeaderCell">
                <ItemTemplate>
                    <asp:LinkButton id="lbDelete" runat="server" CssClass="RedButton SerifStack" CommandArgument='<%# Eval("PictureID") %>' CommandName="DeleteImage" Enabled='<%# HasRelationship(Eval("PictureID")) %>' style="width: 97%; text-decoration: none;"><span class="RedButton" style="padding-top: 8px; height: 35px;">Delete</span></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderStyle-CssClass="HeaderRightCell"><ItemTemplate></ItemTemplate></asp:TemplateField>
        </Columns>
    </asp:GridView>

</div>

<asp:SqlDataSource ID="dsEN" runat="server" ConnectionString="<%$ ConnectionStrings:MainConnectionString %>" SelectCommand="SELECT [PictureID], [PictureFileName], [PictureContentType], [PictureContentLength] FROM [Picture]" />

</asp:Content>