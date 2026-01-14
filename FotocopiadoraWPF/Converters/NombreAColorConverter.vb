Imports System.Globalization
Imports System.Windows.Data
Imports System.Windows.Media

Namespace Converters

    Public Class NombreAColorConverter
        Implements IValueConverter

        Private Shared ReadOnly Colores As Color() = {
            Color.FromRgb(&H5A, &H6C, &HFF), ' azul
            Color.FromRgb(&H10, &HB9, &H81), ' verde
            Color.FromRgb(&HF5, &H9E, &HB), ' amarillo
            Color.FromRgb(&HEF, &H44, &H44), ' rojo
            Color.FromRgb(&H8B, &H5C, &HFF), ' violeta
            Color.FromRgb(&HE, &H9F, &H6E)  ' teal
        }

        Public Function Convert(value As Object,
                                targetType As Type,
                                parameter As Object,
                                culture As CultureInfo) As Object _
            Implements IValueConverter.Convert

            If value Is Nothing Then
                Return New SolidColorBrush(Colores(0))
            End If

            Dim texto = value.ToString()
            Dim hash = Math.Abs(texto.GetHashCode())
            Dim index = hash Mod Colores.Length

            Return New SolidColorBrush(Colores(index))
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
