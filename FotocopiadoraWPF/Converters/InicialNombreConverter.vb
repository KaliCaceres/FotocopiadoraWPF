Imports System.Globalization
Imports System.Windows.Data
Imports System.Windows.Media


Namespace Converters

    Public Class InicialNombreConverter
        Implements IValueConverter

        Public Function Convert(value As Object,
                                targetType As Type,
                                parameter As Object,
                                culture As CultureInfo) As Object _
            Implements IValueConverter.Convert

            If value Is Nothing Then Return "?"

            Dim texto = value.ToString().Trim()

            If texto.Length = 0 Then Return "?"

            Return texto.Substring(0, 1).ToUpper()
        End Function

        Public Function ConvertBack(value As Object,
                                    targetType As Type,
                                    parameter As Object,
                                    culture As CultureInfo) As Object _
            Implements IValueConverter.ConvertBack

            Throw New NotImplementedException()
        End Function

    End Class

End Namespace
