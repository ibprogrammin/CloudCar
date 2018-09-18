Namespace CCFramework.Commerce.PaymentProcessing

    Public Enum PaymentStatus
        Invalid = 0
        Approved = 1
        Declined = 2
        UserError = 3
        SystemError = 4
    End Enum

    Public Class PaymentStatusFunctions
        Public Shared Function GetPaymentStatusMessage(ByVal CurrentStatus As PaymentStatus) As String
            Select Case CurrentStatus
                Case PaymentStatus.Invalid
                    Return "Invalid"
                Case PaymentStatus.Approved
                    Return "Approved"
                Case PaymentStatus.Declined
                    Return "Declined"
                Case PaymentStatus.SystemError
                    Return "System Error"
                Case PaymentStatus.UserError
                    Return "User Error"
                Case Else
                    Return "Invalid"
            End Select
        End Function
    End Class

    

End Namespace