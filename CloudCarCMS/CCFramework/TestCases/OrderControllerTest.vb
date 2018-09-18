Imports CloudCar.CCFramework.Commerce
Imports Microsoft.VisualStudio.TestTools.UnitTesting

Namespace CCFramework.TestCases

    <TestClass()> _
    Public Class OrderControllerTest

        <TestMethod()> _
        Public Sub GetItemCountTest()
            Dim OrderId As Integer = 0 ' TODO: Initialize to an appropriate value
            Dim Expected As Integer = 0 ' TODO: Initialize to an appropriate value

            Dim Actual As Integer = OrderController.GetItemCount(OrderId)
            Assert.AreEqual(Expected, Actual, "The method did not return the expected value")

            OrderId = 5
            Actual = OrderController.GetItemCount(OrderId)
            Expected = 12
            Assert.AreEqual(Expected, Actual, "The method did not return the expected value")

            OrderId = 9
            Actual = OrderController.GetItemCount(OrderId)
            Expected = 2
            Assert.AreEqual(Expected, Actual, "The method did not return the expected value")

            OrderId = Nothing
            Actual = OrderController.GetItemCount(OrderId)
            Expected = 0
            Assert.AreEqual(Expected, Actual, "The method did not return the expected value")
        End Sub

        <TestMethod()> _
        Public Sub OrderTotalsTest()
            Dim CurrentItemCount As Integer
            Dim CurrentSubtotal As Decimal
            Dim CurrentRate As Decimal
            Dim CurrentTax As Decimal
            Dim CurrentDiscount As Decimal

            Dim Expected As Decimal
            Dim Actual As Decimal

            Dim CurrentOrderTotal As OrderTotals

            CurrentItemCount = 3
            CurrentSubtotal = 450D
            CurrentRate = 15.99D
            CurrentTax = 0.13D
            CurrentDiscount = 2.5D

            CurrentOrderTotal = New OrderTotals(CurrentItemCount, CurrentSubtotal, CurrentRate, CurrentTax, CurrentDiscount)

            Actual = CurrentOrderTotal.Total
            Expected = 523.74D
            Assert.AreEqual(Expected, Actual, "The method did not return the expected value")

        End Sub

    End Class

End Namespace
