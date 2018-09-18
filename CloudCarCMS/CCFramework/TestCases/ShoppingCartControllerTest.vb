Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Commerce.ShoppingCart
Imports System
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports CloudCar.CCFramework.Model

Namespace CCFramework.TestCases

    <TestClass()> _
    Public Class ShoppingCartControllerTest

        <TestMethod()> _
        Public Sub GetItemCountBySessionTest()
            Dim SessionId As String = "bea8d801-8982-4009-b1c3-868f4f707b88"
            Dim Expected As Integer = 5

            Dim Actual As Integer = ShoppingCartController.GetItemCount(SessionId)
            Assert.AreEqual(Expected, Actual, "The method did not return the expected value")

            SessionId = Nothing
            Actual = ShoppingCartController.GetItemCount(SessionId)
            Expected = 0
            Assert.AreEqual(Expected, Actual, "The method did not return the expected value")
        End Sub

        <TestMethod()> _
        Public Sub GetSubTotalBySessionTest()
            Dim SessionId As String
            Dim Expected As Decimal
            Dim Actual As Decimal

            SessionId = "bea8d801-8982-4009-b1c3-868f4f707b88"
            Expected = 2249.95D
            Actual = ShoppingCartController.GetSubTotal(SessionId, EPriceLevel.Regular)
            Assert.AreEqual(Expected, Actual, "The method did not return the expected value")

            SessionId = "796c7994-1807-482e-beae-8198e42feae5"
            Expected = 1169.97D
            Actual = ShoppingCartController.GetSubTotal(SessionId, EPriceLevel.Warehouse)
            Assert.AreEqual(Expected, Actual, "The method did not return the expected value")

            SessionId = Nothing
            Actual = ShoppingCartController.GetSubTotal(SessionId, EPriceLevel.Regular)
            Expected = 0
            Assert.AreEqual(Expected, Actual, "The method did not return the expected value")
        End Sub

    End Class

End Namespace