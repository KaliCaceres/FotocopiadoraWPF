Imports System.Globalization
Imports System.Windows.Data
Imports System.Windows.Media

Namespace Converters
    Public Class ModificadoBorderConverter
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type,
                            parameter As Object, culture As CultureInfo) As Object _
        Implements IValueConverter.Convert

            Dim modificado = False
            If TypeOf value Is Boolean Then
                modificado = CBool(value)
            End If

            If modificado Then
                Return Brushes.Green
            End If

            Return Brushes.Gray
        End Function

        Public Function ConvertBack(...) As Object _
        Implements IValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function
    End Class

End Namespace