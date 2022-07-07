Option Strict Off
Option Explicit On 

<System.Runtime.InteropServices.ProgId("Fon_NET.Fonetica")> Public Class Fonetica

    Private mvarNrOcorrencias As Short 'local copy

    Public ReadOnly Property NrOcorrencias() As Short
        Get
            NrOcorrencias = mvarNrOcorrencias
        End Get
    End Property

    '----------------------------------------------------------------------------------------'
    '                 C O D I G O    F O N E T I C O   
    '----------------------------------------------------------------------------------------'

    Public Function GeraCodigoFonetico(ByVal pstrCampoTexto) As String

        Dim strAux1 As String
        Dim strAux2 As String()
        Dim i As Integer

        i = 0
        strAux1 = Nothing
        strAux2 = Nothing

        strAux1 = GetFonetica(pstrCampoTexto)

        If strAux1 = Nothing Then
            strAux1 = ""
        End If

        strAux2 = strAux1.Split(",")
        strAux1 = ""

        For i = 0 To strAux2.Length - 1
            strAux1 = strAux1 + strAux2(i).PadRight(10) ' aspas simples + 8 posicoes + aspas simples ...
            strAux1 = strAux1 + ","
            strAux1 = Replace(strAux1, Chr(39), "")     ' retirando as aspas simples ...
        Next i

        Return strAux1

    End Function


    Public Function GetFonetica(ByVal StrTexto As String, Optional ByVal NrOcorrencias As Short = 0) As String
        Const RefString As String = "1234567890abcdefghijklmnopqrstuvxwyzáéíóúàçãõ "

        Dim CollObjPalavra As New CollObjPalavra
        Dim i, h As Integer
        Dim StrTemp, Palavra, iten, Primeira, Caracter As String

        StrTexto = Trim(LCase(StrTexto))

        For i = 1 To Len(StrTexto)
            Caracter = Mid(StrTexto, i, 1)
            If InStr(RefString, Caracter) <> 0 Then StrTemp = StrTemp & Caracter
        Next i
        StrTexto = StrTemp

        Palavra = ""
        iten = ""
        Primeira = ""
        i = 0
        h = 0

        For i = 1 To Len(StrTexto)
            iten = Mid(StrTexto, i, 1)
            If iten <> " " Then
                Palavra = Palavra & iten
            Else
                If CBool(Trim(CStr(Palavra <> ""))) Then
                    CollObjPalavra.Add(Palavra, "")
                End If
                Palavra = ""
            End If

            If i = Len(StrTexto) Then
                CollObjPalavra.Add(Palavra, "")
            End If
        Next

        '***********************************************************************
        'Retira as preposições trocar collObjPalavra por array utilizando split
        For i = CollObjPalavra.Count To 1 Step -1
            Select Case CollObjPalavra.Item(i).TextoDigitado
                Case "da", "das", "de", "do", "dos", "pelo", "pelos", "pela", "pelas", "com", "sem", "a", "as", "o", "os", "e", "ou", "c/", "s/", "p/"
                    CollObjPalavra.Remove((i))
            End Select
        Next

        'Verifica se exite palavras iguais dentro do mesmo texto
        For i = 1 To CollObjPalavra.Count
            For h = 1 To CollObjPalavra.Count
                If h <= CollObjPalavra.Count And i <= CollObjPalavra.Count And i <> h Then
                    If CollObjPalavra.Item(i).TextoDigitado = CollObjPalavra.Item(h).TextoDigitado Then
                        CollObjPalavra.Remove((h))
                        h = h - 1
                    End If
                End If
            Next
        Next

        If CollObjPalavra.Count = 0 Then Exit Function

        If NrOcorrencias = 0 Then NrOcorrencias = CollObjPalavra.Count

        For i = CollObjPalavra.Count To NrOcorrencias + 1 Step -1
            CollObjPalavra.Remove((i))
        Next

        ' fonetizacao
        For i = 1 To CollObjPalavra.Count
            If CollObjPalavra.Item(i).TextoDigitado <> "" Then
                Palavra = ""
                CollObjPalavra.Item(i).RaizFinal = ""

                Select Case Mid(Trim(CollObjPalavra.Item(i).TextoDigitado), 1, 1)
                    Case "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
                        CollObjPalavra.Item(i).RaizFinal = Mid(CollObjPalavra.Item(i).TextoDigitado, 1, 8)

                    Case Else 'as instruções a seguir referem-se a palavras que não iniciam c/ numero
                        Palavra = RetirarDuplicidateLetras(CollObjPalavra.Item(i).TextoDigitado) 'Retira letras repedidas Ex: erro = ero
                        Palavra = Final(Palavra) 'Ajusta o fim da palavra Ex: João = Joãn
                        Palavra = TratarQ(Palavra) 'Substitui o qu por k Ex: Queijo = keijo
                        Palavra = TratarCGS(Palavra) 'Ajusta o CGS Ex: caro = òaro
                        Palavra = TratarH(Palavra) 'Retira o h Ex: hotel = otel
                        Palavra = CodificarFonetica(Palavra) 'Transforma a string em um código fonético Ex: otel = C349
                        Palavra = ComprimirCodigo(Palavra) 'Comprime o código fonético Ex: C334499 = C349
                        CollObjPalavra.Item(i).RaizFinal = CalcularRaizFinal(Palavra) 'Ajusta o código fonético Ex: C349 = C3490000
                End Select
            End If
        Next

        For i = 1 To CollObjPalavra.Count
            GetFonetica = GetFonetica & "'" & CollObjPalavra.Item(i).RaizFinal & "'" & ","
        Next
        GetFonetica = Left(GetFonetica, Len(GetFonetica) - 1)
        mvarNrOcorrencias = CollObjPalavra.Count
    End Function

    Private Function Final(ByVal texto As Object) As String
        Dim Letra As Integer

        Letra = Len(Trim(texto))

        If Right(texto, 2) = "ao" Or Right(texto, 2) = "ão" Then
            Final = Mid(texto, 1, Letra - 1) & "n"
        Else
            If Right(texto, 1) = "m" Then
                Final = Mid(texto, 1, Letra - 1) & "n"
            Else
                If Right(texto, 1) = "x" Then
                    Final = Mid(texto, 1, Letra - 1) & "ks"
                Else
                    Final = texto
                End If
            End If
        End If
    End Function

    Private Function RetirarDuplicidateLetras(ByVal texto As Object) As String
        Dim i As Integer
        For i = 1 To Len(Trim(texto))
            If Mid(Trim(texto), i, 1) <> Mid(Trim(texto), i + 1, 1) Then
                RetirarDuplicidateLetras = RetirarDuplicidateLetras & Mid(Trim(texto), i, 1)
            End If
        Next
    End Function
    Private Function TratarQ(ByVal texto As String) As String
        texto = Replace(texto, "que", "ke")
        texto = Replace(texto, "qui", "ki")
        TratarQ = Replace(texto, "quo", "ko")
    End Function

    Private Function TratarCGS(ByVal CGS As Object) As String
        Dim seguinte, i, Letra As Object
        For i = 1 To Len(Trim(CGS))
            Letra = Mid(CGS, i, 1)
            seguinte = Mid(CGS, i + 1, 1)
            Select Case Letra
                Case "c", "ç"
                    Select Case seguinte
                        Case "e", "i", "é", "í"
                            TratarCGS = TratarCGS & "þ"
                        Case "h"
                            TratarCGS = TratarCGS & "ÿ"
                        Case Else
                            TratarCGS = TratarCGS & "ò"
                    End Select

                Case "g"
                    Select Case seguinte
                        Case "e", "i", "é", "í"
                            TratarCGS = TratarCGS & "ø"
                        Case Else
                            TratarCGS = TratarCGS & "ö"
                    End Select

                Case "s"
                    Select Case seguinte
                        Case "u", "ú"
                            TratarCGS = TratarCGS & "þ"
                        Case Else
                            TratarCGS = TratarCGS & "þ"
                    End Select

                Case Else
                    TratarCGS = TratarCGS & Letra
            End Select
        Next
    End Function
    Private Function TratarH(ByVal texto As String) As String
        TratarH = Replace(texto, "h", "")
    End Function
    Private Function CodificarFonetica(ByVal Palavra As Object) As String
        Dim i As Integer
        Dim Letra As String
        For i = 1 To Len(Trim(Palavra))
            Letra = Mid(Palavra, i, 1)
            Select Case Letra
                Case "b", "p"
                    CodificarFonetica = CodificarFonetica & 1
                Case "k", "q"
                    CodificarFonetica = CodificarFonetica & 2
                Case "d", "t"
                    CodificarFonetica = CodificarFonetica & 3
                Case "f", "v", "w"
                    CodificarFonetica = CodificarFonetica & 5
                Case "j"
                    CodificarFonetica = CodificarFonetica & 8
                Case "l"
                    CodificarFonetica = CodificarFonetica & 9
                Case "m"
                    CodificarFonetica = CodificarFonetica & "A"
                Case "n"
                    CodificarFonetica = CodificarFonetica & "B"
                Case "r"
                    CodificarFonetica = CodificarFonetica & "D"
                Case "s", "z"
                    CodificarFonetica = CodificarFonetica & "E"
                Case "x"
                    CodificarFonetica = CodificarFonetica & "F"
                Case "a", "á", "à", "ã"
                    CodificarFonetica = CodificarFonetica & 0
                Case "e", "é", "ê"
                    CodificarFonetica = CodificarFonetica & 4
                Case "i", "y", "í"
                    CodificarFonetica = CodificarFonetica & 7
                Case "o", "u", "ó", "ú", "õ"
                    CodificarFonetica = CodificarFonetica & "C"
                Case "þ"
                    CodificarFonetica = CodificarFonetica & "E"
                Case "ÿ"
                    CodificarFonetica = CodificarFonetica & "F"
                Case "ò"
                    CodificarFonetica = CodificarFonetica & 2
                Case "ø"
                    CodificarFonetica = CodificarFonetica & 8
                Case "ö"
                    CodificarFonetica = CodificarFonetica & 6
                Case Else
                    CodificarFonetica = CodificarFonetica & Letra
            End Select
        Next

    End Function
    Private Function ComprimirCodigo(ByVal pal_cod As String) As String
        Dim i As Integer
        For i = 1 To Len(Trim(pal_cod))
            If Mid(pal_cod, i, 1) <> Mid(pal_cod, i + 1, 1) Then
                ComprimirCodigo = ComprimirCodigo & Mid(pal_cod, i, 1)
            End If
        Next
    End Function

    Private Function CalcularRaizFinal(ByVal cod_comp As String) As String
        Dim i As Integer
        For i = 1 To 8
            If Mid(cod_comp, i, 1) <> "" Then
                CalcularRaizFinal = CalcularRaizFinal & Mid(cod_comp, i, 1)
            Else
                CalcularRaizFinal = CalcularRaizFinal & 0
            End If
        Next

    End Function


End Class