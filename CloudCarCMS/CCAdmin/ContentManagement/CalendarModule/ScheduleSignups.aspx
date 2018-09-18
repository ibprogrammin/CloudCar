<%@ Page Title="" Language="vb" AutoEventWireup="false" CodeBehind="ScheduleSignups.aspx.vb" Inherits="CloudCar.CCAdmin.ContentManagement.CalendarModule.ScheduleSignups" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head id="Head1" runat="server">
    <style type="text/css">
        label { width: 125px;font-weight: bold;float: left; }
        label.CurrentSignup {  }
    </style>
    
    <title>Print Schedule</title>
</head>
<body>

<form id="form1" runat="server">

    <img src="/CCTemplates/Admin/Images/icons/order.icon.png" alt="Schedule Sign Ups" width="75" height="75" style="float:left; margin-right: 20px;" /><br />
    <h1 class="form-heading-style">Schedule Sign Ups</h1><hr />
    <br class="clear" />
    
    <fieldset>
        
        <h2 class="form-heading-style">Details <a href="javascript:window.print();" style="float: right; margin-right: 20px;">Print</a></h2><br /><br />
        
        <asp:HiddenField runat="server" ID="ScheduleIdHiddenField" />
        
        <label>Class</label>
        <asp:Label runat="server" ID="ProgramLabel" /> - <asp:Label runat="server" ID="DateLabel" /> <asp:Label runat="server" ID="TimeLabel" />
        <br style="clear: both;" /><br />
        
        <label>Duration (mins.)</label>
        <asp:Label runat="server" ID="DurationLabel" style="width: 670px;" />
        <br style="clear: both;" /><br />
        
        <label>Capacity</label>
        <asp:Label runat="server" ID="Capacity" style="width: 670px;" />
        <br style="clear: both;" /><br />
        
        <label class="CurrentSignup">Current Signups</label>
        <asp:Repeater runat="server" ID="SignUpRepeater">
            <HeaderTemplate>
                <table class="default-table" cellpadding="0" cellspacing="0" style="width: 650px;">
                    <thead class="default-table-header">
                        <tr>
                            <td>Name</td>
                            <td>Email</td>
                            <td>Phone</td>
                        </tr>
                    </thead>
           </HeaderTemplate> 
           <ItemTemplate>
                <tr>
                    <td><%#Eval("FirstName")%> <%#Eval("LastName")%></td>
                    <td><a href="mailto:<%#Eval("Email") %>"><%# Eval("Email") %></a></td>
                    <td><%#Eval("PhoneNumber")%></td>
                </tr>  
           </ItemTemplate>
           <FooterTemplate>
               </table>
           </FooterTemplate>
        </asp:Repeater>
        <br style="clear: both;" /><br />
        
    </fieldset>
    
    <br/><br />
    
    <br style="clear: both;" />

</form>
</body>
</html>