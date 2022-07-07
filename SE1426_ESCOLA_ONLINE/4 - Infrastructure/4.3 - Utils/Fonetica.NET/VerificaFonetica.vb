Imports System
Imports Fonetica

Public Enum Tipo
    Nome_Completo = 0
    Primeiro_Nome
    Segundo_Nome
    Penultimo_Nome
    Ultimo_Nome
    Primeiro_e_Segundo_Nome
    Primeiro_e_Penultimo_Nome
    Primeiro_e_Ultimo_Nome
    Segundo_e_Ultimo_Nome
    Ultimo_e_Penultimo_Nome
End Enum

Public Class VerificaFonetica
    Function Valida(ByVal lstrNome1 As String, ByVal lstrNome2 As String, ByVal lstrPesquisa As Tipo) As Boolean
        Dim objFonetica As New Fonetica
        Dim lstrRetornoFonetica1, lstrRetornoFonetica2 As String
        Valida = True

        ' Limpa os espaços em branco excedentes
        While lstrNome1.IndexOf("  ") > -1
            lstrNome1 = lstrNome1.Replace("  ", " ").Trim
        End While
        While lstrNome2.IndexOf("  ") > -1
            lstrNome2 = lstrNome2.Replace("  ", " ").Trim
        End While

        Select Case lstrPesquisa
            Case Is = Tipo.Nome_Completo

                lstrRetornoFonetica1 = objFonetica.GetFonetica(lstrNome1, 0)
                lstrRetornoFonetica2 = objFonetica.GetFonetica(lstrNome2, 0)
                If lstrRetornoFonetica1 <> lstrRetornoFonetica2 Then
                    Valida = False
                End If

            Case Is = Tipo.Penultimo_Nome

                Dim PenultimoNome1, PenultimoNome2 As String
                Penultimo_Nome(lstrNome1, PenultimoNome1)
                Penultimo_Nome(lstrNome2, PenultimoNome2)

                lstrRetornoFonetica1 = objFonetica.GetFonetica(PenultimoNome1, 0)
                lstrRetornoFonetica2 = objFonetica.GetFonetica(PenultimoNome2, 0)
                If lstrRetornoFonetica1 <> lstrRetornoFonetica2 Then
                    Valida = False
                End If

            Case Is = Tipo.Primeiro_e_Segundo_Nome

                Dim PrimeiroNome1, SegundoNome1 As String
                Dim PrimeiroNome2, SegundoNome2 As String

                Primeiro_e_Segundo_Nome(lstrNome1, PrimeiroNome1, SegundoNome1)
                Primeiro_e_Segundo_Nome(lstrNome2, PrimeiroNome2, SegundoNome2)

                lstrRetornoFonetica1 = objFonetica.GetFonetica(PrimeiroNome1 & " " & SegundoNome1, 0)
                lstrRetornoFonetica2 = objFonetica.GetFonetica(PrimeiroNome2 & " " & SegundoNome2, 0)
                If lstrRetornoFonetica1 <> lstrRetornoFonetica2 Then
                    Valida = False
                End If

            Case Is = Tipo.Primeiro_e_Penultimo_Nome

                Dim PrimeiroNome1, PenultimoNome1 As String
                Dim PrimeiroNome2, PenultimoNome2 As String

                Primeiro_e_Penultimo_Nome(lstrNome1, PrimeiroNome1, PenultimoNome1)
                Primeiro_e_Penultimo_Nome(lstrNome2, PrimeiroNome2, PenultimoNome2)

                lstrRetornoFonetica1 = objFonetica.GetFonetica(PrimeiroNome1 & " " & PenultimoNome1, 0)
                lstrRetornoFonetica2 = objFonetica.GetFonetica(PrimeiroNome2 & " " & PenultimoNome2, 0)
                If lstrRetornoFonetica1 <> lstrRetornoFonetica2 Then
                    Valida = False
                End If

            Case Is = Tipo.Primeiro_e_Ultimo_Nome

                Dim PrimeiroNome1, UltimoNome1 As String
                Dim PrimeiroNome2, UltimoNome2 As String

                Primeiro_e_Ultimo(lstrNome1, PrimeiroNome1, UltimoNome1)
                Primeiro_e_Ultimo(lstrNome2, PrimeiroNome2, UltimoNome2)

                lstrRetornoFonetica1 = objFonetica.GetFonetica(PrimeiroNome1 & " " & UltimoNome1, 0)
                lstrRetornoFonetica2 = objFonetica.GetFonetica(PrimeiroNome2 & " " & UltimoNome2, 0)
                If lstrRetornoFonetica1 <> lstrRetornoFonetica2 Then
                    Valida = False
                End If

            Case Is = Tipo.Primeiro_Nome

                Dim PrimeiroNome1 As String
                Dim PrimeiroNome2 As String

                Primeiro_Nome(lstrNome1, PrimeiroNome1)
                Primeiro_Nome(lstrNome2, PrimeiroNome2)

                lstrRetornoFonetica1 = objFonetica.GetFonetica(PrimeiroNome1, 0)
                lstrRetornoFonetica2 = objFonetica.GetFonetica(PrimeiroNome2, 0)
                If lstrRetornoFonetica1 <> lstrRetornoFonetica2 Then
                    Valida = False
                End If

            Case Is = Tipo.Segundo_e_Ultimo_Nome

                Dim SegundoNome1, UltimoNome1 As String
                Dim SegundoNome2, UltimoNome2 As String

                Segundo_e_Ultimo(lstrNome1, SegundoNome1, UltimoNome1)
                Segundo_e_Ultimo(lstrNome2, SegundoNome2, UltimoNome2)

                lstrRetornoFonetica1 = objFonetica.GetFonetica(SegundoNome1 & " " & UltimoNome1, 0)
                lstrRetornoFonetica2 = objFonetica.GetFonetica(SegundoNome2 & " " & UltimoNome2, 0)
                If lstrRetornoFonetica1 <> lstrRetornoFonetica2 Then
                    Valida = False
                End If

            Case Is = Tipo.Segundo_Nome

                Dim SegundoNome1 As String
                Dim SegundoNome2 As String

                Segundo_Nome(lstrNome1, SegundoNome1)
                Segundo_Nome(lstrNome2, SegundoNome2)

                lstrRetornoFonetica1 = objFonetica.GetFonetica(SegundoNome1, 0)
                lstrRetornoFonetica2 = objFonetica.GetFonetica(SegundoNome2, 0)
                If lstrRetornoFonetica1 <> lstrRetornoFonetica2 Then
                    Valida = False
                End If

            Case Is = Tipo.Ultimo_e_Penultimo_Nome

                Dim PenultimoNome1, UltimoNome1 As String
                Dim PenultimoNome2, UltimoNome2 As String

                Ultimo_e_Penultimo_Nome(lstrNome1, PenultimoNome1, UltimoNome1)
                Ultimo_e_Penultimo_Nome(lstrNome2, PenultimoNome2, UltimoNome2)

                lstrRetornoFonetica1 = objFonetica.GetFonetica(PenultimoNome1 & " " & UltimoNome1, 0)
                lstrRetornoFonetica2 = objFonetica.GetFonetica(PenultimoNome2 & " " & UltimoNome2, 0)
                If lstrRetornoFonetica1 <> lstrRetornoFonetica2 Then
                    Valida = False
                End If

            Case Is = Tipo.Ultimo_Nome

                Dim UltimoNome1 As String
                Dim UltimoNome2 As String

                Ultimo_Nome(lstrNome1, UltimoNome1)
                Ultimo_Nome(lstrNome2, UltimoNome2)

                lstrRetornoFonetica1 = objFonetica.GetFonetica(UltimoNome1, 0)
                lstrRetornoFonetica2 = objFonetica.GetFonetica(UltimoNome2, 0)
                If lstrRetornoFonetica1 <> lstrRetornoFonetica2 Then
                    Valida = False
                End If

        End Select
    End Function

    Sub Primeiro_e_Segundo_Nome(ByVal lstrNome As String, ByRef PrimeiroNome As String, ByRef SegundoNome As String)
        Dim lstrPalavra() As String
        lstrPalavra = Split(Trim(lstrNome), " ")
        PrimeiroNome = lstrPalavra(0)
        If UBound(lstrPalavra) > 0 Then
            SegundoNome = lstrPalavra(1)
        Else
            SegundoNome = ""
        End If
    End Sub

    Sub Primeiro_Nome(ByVal lstrNome As String, ByRef PrimeiroNome As String)
        Dim lstrPalavra() As String
        lstrPalavra = Split(Trim(lstrNome), " ")
        PrimeiroNome = lstrPalavra(0)
    End Sub

    Sub Penultimo_Nome(ByVal lstrNome As String, ByRef PenultimoNome As String)
        Dim lstrPalavra() As String
        lstrPalavra = Split(Trim(lstrNome), " ")
        If UBound(lstrPalavra) > 0 Then
            PenultimoNome = lstrPalavra(UBound(lstrPalavra) - 1)
        Else
            PenultimoNome = Trim(lstrNome)
        End If
    End Sub

    Sub Primeiro_e_Penultimo_Nome(ByVal lstrNome As String, ByRef PrimeiroNome As String, ByRef PenultimoNome As String)
        Dim lstrPalavra() As String
        lstrPalavra = Split(Trim(lstrNome), " ")
        PrimeiroNome = lstrPalavra(0)
        If UBound(lstrPalavra) > 0 Then
            PenultimoNome = lstrPalavra(UBound(lstrPalavra) - 1)
        Else
            PenultimoNome = ""
        End If
    End Sub

    Sub Segundo_e_Ultimo(ByVal lstrNome As String, ByRef SegundoNome As String, ByRef UltimoNome As String)
        Dim lstrPalavra() As String
        lstrPalavra = Split(Trim(lstrNome), " ")
        If UBound(lstrPalavra) > 0 Then
            SegundoNome = lstrPalavra(1)
        Else
            SegundoNome = ""
        End If
        UltimoNome = lstrPalavra(UBound(lstrPalavra))
    End Sub

    Sub Segundo_Nome(ByVal lstrNome As String, ByRef SegundoNome As String)
        Dim lstrPalavra() As String
        lstrPalavra = Split(Trim(lstrNome), " ")
        If UBound(lstrPalavra) > 0 Then
            SegundoNome = lstrPalavra(1)
        Else
            SegundoNome = lstrPalavra(0)
        End If
    End Sub

    Sub Ultimo_e_Penultimo_Nome(ByVal lstrNome As String, ByRef PenultimoNome As String, ByRef UltimoNome As String)
        Dim lstrPalavra() As String
        lstrPalavra = Split(Trim(lstrNome), " ")
        If UBound(lstrPalavra) > 0 Then
            PenultimoNome = lstrPalavra(UBound(lstrPalavra) - 1)
        Else
            PenultimoNome = ""
        End If
        UltimoNome = lstrPalavra(UBound(lstrPalavra))
    End Sub

    Sub Primeiro_e_Ultimo(ByVal lstrNome As String, ByRef PrimeiroNome As String, ByRef UltimoNome As String)
        Dim lstrPalavra() As String
        lstrPalavra = Split(Trim(lstrNome), " ")
        PrimeiroNome = lstrPalavra(0)
        UltimoNome = lstrPalavra(UBound(lstrPalavra))
    End Sub

    Sub Ultimo_Nome(ByVal lstrNome As String, ByRef UltimoNome As String)
        Dim lstrPalavra() As String
        lstrPalavra = Split(Trim(lstrNome), " ")
        UltimoNome = lstrPalavra(UBound(lstrPalavra))
    End Sub
End Class
