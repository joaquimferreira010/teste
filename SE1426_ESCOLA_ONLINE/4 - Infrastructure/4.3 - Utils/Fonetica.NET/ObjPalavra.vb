Option Strict Off
Option Explicit On
Friend Class ObjPalavra
	'local variable(s) to hold property value(s)
	Private mvarTextoDigitado As String 'local copy
	Private mvarRaizFinal As String 'local copy
	
	
	Public Property RaizFinal() As String
		Get
			'used when retrieving value of a property, on the right side of an assignment.
			'Syntax: Debug.Print X.RaizFinal
			RaizFinal = mvarRaizFinal
		End Get
		Set(ByVal Value As String)
			'used when assigning a value to the property, on the left side of an assignment.
			'Syntax: X.RaizFinal = 5
			mvarRaizFinal = Value
		End Set
	End Property
	
	
	
	
	
	Public Property TextoDigitado() As String
		Get
			'used when retrieving value of a property, on the right side of an assignment.
			'Syntax: Debug.Print X.TextoDigitado
			TextoDigitado = mvarTextoDigitado
		End Get
		Set(ByVal Value As String)
			'used when assigning a value to the property, on the left side of an assignment.
			'Syntax: X.TextoDigitado = 5
			mvarTextoDigitado = Value
		End Set
	End Property
End Class