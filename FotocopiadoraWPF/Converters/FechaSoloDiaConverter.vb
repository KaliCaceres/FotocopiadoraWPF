Imports System.Globalization
Imports System.Windows.Data

Public Class FechaSoloDiaConverter
    Implements IValueConverter

    Public Function Convert(value As Object, targetType As Type,
                            parameter As Object, culture As CultureInfo) As Object _
                            Implements IValueConverter.Convert

        If TypeOf value Is DateTime Then
            Return CType(value, DateTime).Date
        End If

        Return value
    End Function

    Public Function ConvertBack(value As Object, targetType As Type,
                                parameter As Object, culture As CultureInfo) As Object _
                                Implements IValueConverter.ConvertBack
        Throw New NotImplementedException()
    End Function
End Class
