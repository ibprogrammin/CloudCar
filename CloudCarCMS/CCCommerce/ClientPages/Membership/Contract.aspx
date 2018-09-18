<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/CCMasterPages/Main.Master" CodeBehind="Contract.aspx.vb" Inherits="CloudCar.CCCommerce.Membership.Contract" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContentPlaceHolder" runat="server"></asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="SecondContentPlaceHolder" runat="server"></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="FirstContentPlaceHolder" runat="server"></asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ThirdContentPlaceHolder" runat="server">

<h2>Descriptions</h2>

<p>The FlexForward System consists of two levels of membership.  A Lightweight Program plan which allows a person to access the club for any class to a maximum of eight (8) classes per month and a Middleweight Program which allows a person to access the club a maximum of twelve (12) classes per month.  The Heavyweight Program allows unlimited usage of all regularly scheduled classes.</p>

<h2>Payment options</h2>

<p>People have three (3) payment options.  They can purchase classes on an annual basis for the Lightweight Program equivalent to a maximum value of 96 classes (12 months times 8 classes per month), preauthorization for a monthly purchase of 8 classes per month or a pay-as-you-go selection where they agree to pay for 3 months up front and then pay a quarterly fee to continue their program.  A person can choose the Middleweight Program equivalent to a maximum value of 144 classes (12 months times 12 classes per month), preauthorization for a monthly purchase of 12 classes per month or a pay-as-you-go selection where they agree to pay for the first 3 months up front and then pay a quarterly fee to continue the program.  The Heavyweight Program allows unlimited access to all classes.  Payment options include an annual payment up front, preauthorization for a monthly purchase or a pay-as-you-go selection where they agree to pay for the first 3 months up front and then pay quarterly to continue their membership.</p>

<h2>Class Usage Options</h2>

<p>People who choose a single annual purchase have complete flexibility to use their classes at any time for any regularly scheduled class they choose.  They may use up their classes as fast or slow as they wish.  In this case, the FlexForward benefit is activated to carry forward unused classes from year to year and will continue to do so as long as the membership is paid in full and remains in good standing.</p>

<p>Those choosing the monthly payment options can use up to eight (8) classes per month for the Lightweight program or up to twelve (12) classes per month for the Middleweight program.  Although there is no requirement to use any specific number of classes in any given week, they may not take classes in advance of paying for their upcoming month.  People who use up their monthly allotment of classes prior to the month’s end may purchase single extra passes or they can upgrade their monthly package by paying the difference in price between the two packages.  Any unused passes are carried forward.  Those on the Heavyweight program have no class usage constraints.</p>




<h2>Program Changes</h2>

<p>All plans come with a minimum three (3) month agreement.  Cancellations – Membership fees are not refundable although they can be transferred.  Preauthorization plans would be charged for any remaining balance equivalent to the original three months upon cancellation of the contract.  Pay-as-you-go plans would forfeit the initial down payment and no further expectation of payment would remain.  In all cases, no additional monetary penalties will be levied.  Upgrades – People have the option to upgrade to any plan at any time by paying the difference.  Downgrades – People can downgrade to a less expensive plan any time after the initial 3 month period has elapsed.  Medical Exemptions can be used to pause any plan for up to one month as long as a doctor’s note is supplied.  If periods of time longer than one month are required for medical leaves, supplemental notes will be required on a monthly basis.  Memberships that have been paused for medical reasons cannot be transferred.</p>

<h2>Exclusions and Limitations</h2>

<p>All membership programs must be paid in full within the first 5 business days at the beginning of the payment periods elected (annual, monthly preauthorization or pay-as-you-go) to remain in good standing.  Any default in preauthorized payments (NSF cheques, charged back credit card authorizations) or missed payments on the pay-as-you-go options will result in a breach of agreement without exception.  Any unused classes that have been carried forward to that point will be forfeited and continued membership would require a new agreement.</p>

<p>__________________________ reserves the sole discretion to eject and expel any member for disruptive or dangerous conduct within the club or for conduct inside or outside the club that would tarnish the club’s reputation including but not limited to criminal activity or bullying.  No written notice will be given nor will refunds be given.</p>

<p>Membership fees do not include annual insurance fees, grading fees, uniforms, sparring gear or any special activities or seminars inside or outside of the club.</p>

<p>I have read, fully understand and agree to the above programs.</p>

<asp:label runat="server" ID="lblStatus" />

<asp:CheckBox runat="server" ID="ckbAgree" Text="I have read, fully understand and agree to the above programs." />

<asp:Button runat="server" ID="btnSubmit" />


</asp:Content>