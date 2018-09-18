<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="EstimateForm.aspx.vb" Inherits="CloudCar.CCContentManagement.ClientForms.EstimateForm" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    
    <style type="text/css">
        label { width: 220px; float: left; }
        input[type=text] { float: left;margin-top: -5px; }
    </style>
    
</asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
    
    <br class="clear-right"/>
    

    <h1>Sports Conditioning Quote</h1>
    
    <asp:Label runat="server" ID="lblStatus" Visible="false" CssClass="status-message" />
    
    <div style="float: left; width: 580px; background-color: #777; padding: 20px;">
    
    <label>First name</label>
    <asp:TextBox runat="server" ID="FirstNameTextBox" style="width: 240px;" />
    <br style="clear: left;" /><br />
    
    <label>Last name</label>
    <asp:TextBox runat="server" ID="LastNameTextBox" style="width: 240px;" />
    <br style="clear: left;" /><br />
    
    <label>Phone</label>
    <asp:TextBox runat="server" ID="PhoneTextBox" style="width: 240px;" />
    <br style="clear: both;" /><br />
    
    <label>Cell</label>
    <asp:TextBox runat="server" ID="CellPhoneTextBox" style="width: 240px;" />
    <br style="clear: both;" /><br />

    <label>Email Address</label>
    <asp:TextBox runat="server" ID="EmailTextBox" style="width: 240px;" />
    <br style="clear: left;" /><br />
    
    <label>Team Name</label>
    <asp:TextBox runat="server" ID="TeamNameTextBox" style="width: 240px;" />
    <br style="clear: left;" /><br />
    
    <label>Sport</label>
    <asp:TextBox runat="server" ID="SportTextBox" style="width: 240px;" />
    <br style="clear: left;" /><br />
    
    <label>Age of Participants</label>
    <asp:TextBox runat="server" ID="AgeOfParticipantsTextBox" style="width: 240px;" />
    <br style="clear: left;" /><br />
    
    <label>Number of Participants</label>
    <asp:TextBox runat="server" ID="NumberOfParticipantsTextBox" style="width: 240px;"  />
    <br style="clear: both;" /><br />

    <h2>Finished?</h2>
    <p>When you are done filling out the form, click the submit button. 
    If you wish to start over you can use the reset button to clear the form.</p>

    <asp:Button runat="server" ID="btnSubmit" CssClass="Green" style="width: 225px; margin: 0px; margin-top: 15px;" Text="Submit Now" />
    <asp:LinkButton runat="server" ID="btnReset" style="width: 225px; margin-left: 20px; text-decoration: underline;" Text="Reset" />

    </div>

    <br style="clear: both;" />

</asp:Content>