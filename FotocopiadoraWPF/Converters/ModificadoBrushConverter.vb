Imports System.Globalization
Imports System.Windows.Data
Imports System.Windows.Media

Namespace Converters

    Public Class ModificadoBorderConverter
        Implements IValueConverter

        Public Function Convert(
            value As Object,
            targetType As Type,
            parameter As Object,
            culture As CultureInfo
        ) As Object Implements IValueConverter.Convert

            If TypeOf value Is Boolean AndAlso CBool(value) Then
                ' 🟢 Verde si está modificado
                Return Brushes.Green
            End If

            ' ⚪ Borde por defecto
            Return Brushes.Gray
        End Function

        Public Function ConvertBack(
            value As Object,
            targetType As Type,
            parameter As Object,
            culture As CultureInfo
        ) As Object Implements IValueConverter.ConvertBack

            ' No necesitamos ConvertBack
            Return Binding.DoNothing
        End Function

    End Class

End Namespace
