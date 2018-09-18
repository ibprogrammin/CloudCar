Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports CloudCar.CCFramework.Commerce.PaymentProcessing

Namespace CCFramework.TestCases

    <TestClass()> _
    Public Class BraintreeClientTest

        <TestMethod()> _
        Public Sub BrainTreeClientSuccessTest()
            Dim CurrentBrainTreeClient As New BraintreeClient()
            CurrentBrainTreeClient.SetGateway("9rzxctg858q6zgg4", "3p388vkkxs2b8h9v", "fc1382d47231cedf6f30a962e5b04e03")
            CurrentBrainTreeClient.SetRequest(1251.25D, "5105105105105100", "05", "2015")

            Dim expected As String = "Success!"
            Dim actual As String = CurrentBrainTreeClient.ProcessRequest()

            Assert.AreEqual(expected, actual.Substring(0, 8))
        End Sub

        <TestMethod()> _
        Public Sub BrainTreeClientFailedTest()
            Dim CurrentBrainTreeClient As New BraintreeClient()
            CurrentBrainTreeClient.SetGateway("9rzxctg858q6zgg4", "3p388vkkxs2b8h9v", "fc1382d47231cedf6f30a962e5b04e03")
            CurrentBrainTreeClient.SetRequest(125.25D, "5105105105100", "05", "2012")

            Dim expected As String = "Message: Credit card number is invalid."
            Dim actual As String = CurrentBrainTreeClient.ProcessRequest()

            Assert.AreEqual(expected, actual.Substring(0, 39))
        End Sub

    End Class

End Namespace