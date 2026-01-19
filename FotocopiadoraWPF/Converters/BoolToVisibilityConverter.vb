Imports System.Globalization
Imports System.Windows
Imports System.Windows.Data

Namespace Converters


    Public Class BoolToVisibilityConverter
        Implements IValueConverter

        Public Function Convert(value As Object,
                                targetType As Type,
                                parameter As Object,
                                culture As CultureInfo) As Object _
            Implements IValueConverter.Convert

            If value Is Nothing Then Return Visibility.Collapsed
            Return If(CBool(value), Visibility.Visible, Visibility.Collapsed)
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
