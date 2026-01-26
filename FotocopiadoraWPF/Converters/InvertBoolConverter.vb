Imports System.Globalization
Imports System.Windows.Data

Namespace Converters

    Public Class InvertBoolConverter
        Implements IValueConverter

        Public Function Convert(
            value As Object,
            targetType As Type,
            parameter As Object,
            culture As CultureInfo) As Object _
            Implements IValueConverter.Convert

            If TypeOf value Is Boolean Then
                Return Not CBool(value)
            End If

            Return True
        End Function

        Public Function ConvertBack(
            value As Object,
            targetType As Type,
            parameter As Object,
            culture As CultureInfo) As Object _
            Implements IValueConverter.ConvertBack

            If TypeOf value Is Boolean Then
                Return Not CBool(value)
            End If

            Return False
        End Function

    End Class

End Namespace
