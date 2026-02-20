Public Class MovimientoCaja
    Public Property IdMovimiento As Integer
    Public Property IdResumen As Integer
    Public Property Fecha As DateTime
    Public Property Tipo As String
    Public Property MetodoPago As String
    Public Property Monto As Integer
    Public Property Motivo As String

    'Public Property observacion As String
    Public Property Empleado As String

    Public Property IdMovimientoRelacionado As Integer?
End Class
