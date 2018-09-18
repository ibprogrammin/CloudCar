Imports System.Security.Cryptography
Imports CloudCar.CCFramework.Commerce
Imports CloudCar.CCFramework.Model
Imports CloudCar.CCFramework.Commerce.PaymentProcessing

Namespace CCFramework.Core

    Public Class CCFunctions

        Public Shared Function ValidateCardNumber(ByVal CCNumber As String) As Boolean
            If CCNumber.Trim = vbNullString Then
                Return False
            Else
                If Not CInt(CCNumber.Substring(0, 1)) = 4 Then
                    Return False
                ElseIf Not (CCNumber.Length = 13 Or CCNumber.Length = 16) Then
                    Return False
                End If

                Dim x As Integer
                Dim temp As String
                Dim total As Integer
                Dim cardNumber As String

                cardNumber = CCNumber.Replace(" ", "").Replace("-", "")

                For x = cardNumber.Length - 2 To 0 Step -2
                    temp = (Val(cardNumber.Substring(x, 1)) * 2).ToString
                    If temp.Length > 1 Then
                        total += CInt(Val(temp.Substring(0, 1)) + Val(temp.Substring(1, 1)))
                    Else
                        total += CInt(Val(temp))
                    End If
                Next

                For x = cardNumber.Length - 1 To 0 Step -2
                    total += CInt(Val(cardNumber.Substring(x, 1)))
                Next

                If total Mod 10 = 0 Then
                    Return True
                Else
                    Return False
                End If
            End If

        End Function

        Public Shared Function CleanCCNumber(ByVal strNumber As String) As String
            Return strNumber.Replace(" ", vbNullString).Replace("-", vbNullString)
        End Function

        Public Shared Function GetExpirationYears() As List(Of ListItem)
            Dim lYears As New List(Of ListItem)

            Dim x As Integer = 0

            For x = 0 To 10
                lYears.Add(New ListItem((Now.Year + x).ToString, (Now.Year + x).ToString))
            Next

            Return lYears
        End Function

        Public Shared Function GetExpirationMonths() As List(Of ListItem)
            Dim lMonths As New List(Of ListItem)

            Dim x As Integer

            For x = 1 To 12
                lMonths.Add(New ListItem(MonthName(x), x.ToString))
            Next

            Return lMonths
        End Function

        'Public Shared Function ProcessCard(ByVal id As Integer, ByVal FirstName As String, ByVal LastName As String, ByVal PCZIP As String, ByVal CCNumber As String, ByVal CCCVD As String, ByVal CCDate As Date, ByVal ChargeAmount As Decimal) As OptimalPayments.CCResponse

        '    Dim PaymentGateway As New OptimalPayments
        '    'Dim easyCrypt As New Commerce.EasyCryptography
        '    Dim CCResponse As OptimalPayments.CCResponse
        '    'Dim amount As Decimal

        '    'Dim Card As com.optimalpayments.test.webservices.CardTypeV1
        '    'Card = com.optimalpayments.test.webservices.CardTypeV1.VI

        '    'CCResponse = PaymentGateway.SimpleCharge(id, ChargeAmount, CCNumber, CCDate, Card, CCCVD, FirstName, LastName, PCZIP)

        '    If CCResponse.ErrorCode = 0 Then
        '        'litCreditStuff.Text = "Success!"
        '    ElseIf CCResponse.ErrorCode = 3006 Then
        '        'litCreditStuff.Text = "Expired!"
        '    ElseIf CCResponse.ErrorCode = 3002 Then
        '        'litCreditStuff.Text = "Invalid Card!"
        '    ElseIf CCResponse.ErrorCode = 3009 Then
        '        'litCreditStuff.Text = "Insufficient Funds!"
        '    Else
        '        'litCreditStuff.Text = "Error: " & CCResponse.ResponseTxt
        '    End If

        '    Return CCResponse

        'End Function

        Public Sub SaveAuthorizationCode(ByVal OrderID As Integer, ByVal AuthorizationCode As String, ByVal Response As String, ByVal ChargeAmount As Decimal)
            Dim ccPayment As CreditCardPaymentController = New CreditCardPaymentController()

            Dim payment As CreditCardPayment = ccPayment.GetOrderPayment(OrderID)
            ccPayment.SetPaymentAuthorized(payment.ID, AuthorizationCode, Response, ChargeAmount)
        End Sub

        Public Shared Function HideCardNumber(ByVal CardNumber As String) As String
            If CardNumber.Length > 4 Then
                Dim ReplaceCharacterLength As Integer = CardNumber.Length - 4
                Dim HiddenString As New StringBuilder

                HiddenString.Append("*"c, ReplaceCharacterLength)

                Return CardNumber.Replace(CardNumber.Substring(0, ReplaceCharacterLength), HiddenString.ToString)
            Else
                Return CardNumber
            End If
        End Function


    End Class

    Public Class EasyCryptography
        Private _IV As Byte() = {CType(16, Byte), CType(25, Byte), CType(109, Byte), CType(64, Byte), CType(52, Byte), CType(1, Byte), CType(78, Byte), CType(55, Byte), CType(74, Byte), CType(14, Byte), CType(32, Byte), CType(65, Byte), CType(5, Byte), CType(4, Byte), CType(99, Byte), CType(42, Byte)}

        Public Function GenerateEncryptionKey() As String
            Dim _RijnManaged As New System.Security.Cryptography.RijndaelManaged

            _RijnManaged.GenerateKey()

            Return ASCIIEncoding.ASCII.GetString(_RijnManaged.Key)
        End Function

        Public Function Encrypt(ByVal originalString As String, ByVal encryptionKey As String) As String
            Dim _RijndaelManaged As New RijndaelManaged

            _RijndaelManaged.IV = _IV
            _RijndaelManaged.Key = ASCIIEncoding.ASCII.GetBytes(encryptionKey)

            Dim encryptor As ICryptoTransform = _RijndaelManaged.CreateEncryptor

            Dim msEncrypt As New System.IO.MemoryStream()
            Dim csEncrypt As New CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)
            Dim toEncrypt() As Byte

            toEncrypt = ASCIIEncoding.ASCII.GetBytes(originalString)

            csEncrypt.Write(toEncrypt, 0, toEncrypt.Length)
            csEncrypt.FlushFinalBlock()

            Dim encrypted() As Byte

            encrypted = msEncrypt.ToArray

            Return Convert.ToBase64String(encrypted)
        End Function

        Public Function Decrypt(ByVal encryptedString As String, ByVal encryptionKey As String) As String

            Dim encryptedBytes As Byte() = System.Convert.FromBase64String(encryptedString)

            Dim _RijndaelManaged As New RijndaelManaged

            _RijndaelManaged.IV = _IV
            _RijndaelManaged.Key = ASCIIEncoding.ASCII.GetBytes(encryptionKey)

            Dim decryptor As ICryptoTransform = _RijndaelManaged.CreateDecryptor

            Dim msDecrypt As New System.IO.MemoryStream(encryptedBytes)
            Dim csDecrypt As New CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)

            Dim sReader As New System.IO.StreamReader(csDecrypt)
            Dim val As String = sReader.ReadToEnd

            Return val
        End Function

    End Class

End Namespace