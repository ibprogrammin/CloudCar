<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Mobile.Master" CodeBehind="MobileThankYou.aspx.vb" Inherits="CloudCar.CCCommerce.Mobile.MobileThankYou" %>

<asp:Content ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="cphBeforeContent" runat="server"></asp:Content>

<asp:Content ContentPlaceHolderID="cphMainContent" runat="server">

    <div>
    
        <h1>Thank You</h1>
        <p>Your purchase has been sent to our warehouse and will be processed immediately. We appreciate your time today and once again <span style="color: #FF0023;">Thank You</span> for choosing to move with <span style="color: #FF0023;">AMJ Campbell</span>.</p>
        
        <ul>
            <li><a href="/Home/Privacy-Policy.html">Privacy Policy</a></li>
            <li><a href="/Home/Guarantee.html">Guarantee</a></li>
            <li><a href="/Home/Returns.html">Returns</a></li>
            <li><a href="/Home/Security.html">Security</a></li>
        </ul>

    </div>

</asp:Content>

<asp:Content ContentPlaceHolderID="cphAfterContent" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="cphScripts" runat="server"></asp:Content>