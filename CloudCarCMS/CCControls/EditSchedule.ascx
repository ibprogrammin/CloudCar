<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="EditSchedule.ascx.vb"
    Inherits="SMECommerceTemplate.EditSchedule" %>
<asp:HiddenField runat="server" ID="hfProgramID" Value="0" />
<asp:HiddenField runat="server" ID="hfDay" Value="0" />
<asp:HiddenField runat="server" ID="hfScheduleID" Value="0" />
<asp:Label runat="server" ID="dummy" />
<ajax:ModalPopupExtender runat="server" ID="mpeEditSchedule" TargetControlID="dummy"
    PopupControlID="panEditSchedule" BackgroundCssClass="modalBackground" CancelControlID="ibClose" />
<asp:Panel runat="server" ID="panEditSchedule" Style="display: none; padding: 5px;"
    BackColor="Black" Width="425px" BorderColor="White" BorderStyle="Solid" BorderWidth="1px">
    <asp:ImageButton runat="server" ID="ibClose" Style="float: right;" ImageUrl="~/images/Kyoto Studio/delete_16.png" />
    <p class="Attention">
        Edit Schedule
    </p>
    <table>
        <tr>
            <td class="FormFieldName">
                Time Slot
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtHours" CssClass="FormField" />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align:center;">
                <asp:Button runat="server" ID="btnSave" CssClass="FormButton" Text="Save Schedule" />
                <asp:Button runat="server" ID="btnDelete" CssClass="FormButton" Text="Delete Schedule" />
            </td>
        </tr>
    </table>
</asp:Panel>
