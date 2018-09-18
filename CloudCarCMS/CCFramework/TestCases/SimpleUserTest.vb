Imports System
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports CloudCar.CCFramework.Model

Namespace CCFramework.TestCases

    <TestClass()> _
    Public Class SimpleUserTest

#Region "Additional test attributes"
        '
        'You can use the following additional attributes as you write your tests:
        '
        'Use ClassInitialize to run code before running the first test in the class
        '<ClassInitialize()>  _
        'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
        'End Sub
        '
        'Use ClassCleanup to run code after all tests in a class have run
        '<ClassCleanup()>  _
        'Public Shared Sub MyClassCleanup()
        'End Sub
        '
        'Use TestInitialize to run code before running each test
        '<TestInitialize()>  _
        'Public Sub MyTestInitialize()
        'End Sub
        '
        'Use TestCleanup to run code after each test has run
        '<TestCleanup()>  _
        'Public Sub MyTestCleanup()
        'End Sub
        '
#End Region

        <TestMethod()> _
        Public Sub SimpleUserEqualsTest()
            Dim SimpleUserA As IEquatable(Of SimpleUser) = _
                    New SimpleUser() With {.ID = 1, .FirstName = "Daniel", .MiddleName = "", _
                    .LastName = "Sevitti", .Email = "daniel@seriousmonkey.ca", _
                    .PhoneNumber = "905.390.0635"}

            Dim SimpleUserB As SimpleUser = _
                    New SimpleUser() With {.ID = 1, .FirstName = "Daniel", .MiddleName = "", _
                    .LastName = "Sevitti", .Email = "daniel@seriousmonkey.ca", _
                    .PhoneNumber = "905.390.0635"}
            Dim expected As Boolean = True
            Dim actual As Boolean
            actual = SimpleUser.Equals(SimpleUserA, SimpleUserB)
            Assert.AreEqual(expected, actual)
        End Sub

        <TestMethod()> _
        Public Sub SimpleUserEqualsTest1()
            Dim SimpleUserA As IEquatable(Of SimpleUser) = _
                    New SimpleUser() With {.ID = 1, .FirstName = "Daniel", .MiddleName = "", _
                    .LastName = "Sevitti", .Email = "daniel@seriousmonkey.ca", _
                    .PhoneNumber = "905.390.0635"}

            Dim SimpleUserB As SimpleUser = _
                    New SimpleUser() With {.ID = 1, .FirstName = "Daniel", .MiddleName = "Carlo", _
                    .LastName = "Sevitti", .Email = "daniel@seriousmonkey.ca", _
                    .PhoneNumber = "905.390.0635"}
            Dim expected As Boolean = False
            Dim actual As Boolean
            actual = SimpleUser.Equals(SimpleUserA, SimpleUserB)
            Assert.AreEqual(expected, actual)
        End Sub

        <TestMethod()> _
        Public Sub SimpleUserEqualsTest2()
            Dim target As IEquatable(Of SimpleUser) = _
                    New SimpleUser() With {.ID = 1, .FirstName = "Daniel", .MiddleName = "", _
                    .LastName = "Sevitti", .Email = "daniel@seriousmonkey.ca", _
                    .PhoneNumber = "905.390.0635"}

            Dim other As SimpleUser = New SimpleUser() With {.ID = 1, .FirstName = "Daniel", .MiddleName = "", _
                    .LastName = "Sevitti", .Email = "daniel@seriousmonkey.ca", _
                    .PhoneNumber = "905.390.0635"}
            Dim expected As Boolean = True
            Dim actual As Boolean
            actual = target.Equals(other)
            Assert.AreEqual(expected, actual)
            'Assert.Inconclusive("Verify the correctness of this test method.")
        End Sub

        <TestMethod()> _
        Public Sub SimpleUserEqualsTes3()
            Dim target As IEquatable(Of SimpleUser) = _
                    New SimpleUser() With {.ID = 1, .FirstName = "Daniel", .MiddleName = "", _
                    .LastName = "Sevitti", .Email = "daniel@seriousmonkey.ca", _
                    .PhoneNumber = "905.390.0635"}

            Dim other As SimpleUser = New SimpleUser() With {.ID = 1, .FirstName = "Daniel", .MiddleName = "Carlo", _
                    .LastName = "Sevitti", .Email = "daniel@seriousmonkey.ca", _
                    .PhoneNumber = "905.390.0635"}
            Dim expected As Boolean = False
            Dim actual As Boolean
            actual = target.Equals(other)
            Assert.AreEqual(expected, actual)
        End Sub

        <TestMethod()> _
        Public Sub SimpleUserGetHashCodeTest()
            Dim target As SimpleUser = New SimpleUser() With {.ID = 1, .FirstName = "Daniel", .MiddleName = "", _
                    .LastName = "Sevitti", .Email = "daniel@seriousmonkey.ca", _
                    .PhoneNumber = "905.390.0635"}
            Dim expected As Integer = 159225656
            Dim actual As Integer
            actual = target.GetHashCode()
            Assert.AreEqual(expected, actual)
            'Assert.Inconclusive("Verify the correctness of this test method. " & target.GetHashCode)
        End Sub

    End Class

End Namespace