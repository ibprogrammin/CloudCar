<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="SignUpForm.aspx.vb" Inherits="CloudCar.CCContentManagement.ClientForms.SignUpForm" %>

<asp:Content ContentPlaceHolderID="HeadContentPlaceHolder" runat="server">
    
    <style type="text/css">
        label { width: 220px; float: left; }
        input[type=text] { float: left;margin-top: -5px; }
    </style>
    
</asp:Content>

<asp:Content ContentPlaceHolderID="FirstContentPlaceHolder" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="SecondContentPlaceHolder" runat="server">
    
    <br class="clear-right"/>
    
    <h1>Sign Up For A Class</h1>
    
    <p>This form is for non-members looking to try a workout at TFX only. If you are a member with us be sure to <a href="/Login.html" title="Login">Login Here</a>.
    If you are a member and not yet registered please go to the <a href="/Register.html">Registration Page</a> now and fill out the form. 
    You will be notified when your membership is approved and then you will be able to sign up for your workouts.
    </p>
    
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

    <label>Email Address</label>
    <asp:TextBox runat="server" ID="EmailTextBox" style="width: 240px;" />
    <br style="clear: left;" /><br />

    <h2>Finished?</h2>
    <p>When you are done filling out the form, click the submit button. 
    If you wish to start over you can use the reset button to clear the form.</p>

    <asp:Button runat="server" ID="btnSubmit" CssClass="Green" style="width: 225px; margin: 0px; margin-top: 15px;" Text="Submit Now" />
    <asp:LinkButton runat="server" ID="btnReset" style="width: 225px; margin-left: 20px; text-decoration: underline;" Text="Reset" />

    </div>

    <br style="clear: both;" />

</asp:Content>

<asp:Content ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContentPlaceHolder" runat="server"></asp:Content>