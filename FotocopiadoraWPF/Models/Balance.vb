Public Class Balance
    Public Property FechaInicio As Date
    Public Property FechaFin As Date

    Public Property ContadorEquipo1Inicio As Integer
    Public Property ContadorEquipo1Final As Integer

    Public Property ContadorEquipo2Inicio As Integer
    Public Property ContadorEquipo2Final As Integer

    Public Property EfectivoInicio As Decimal
    Public Property TotalEfectivo As Decimal

    Public Property TransferenciaInicio As Decimal
    Public Property TotalTransferencia As Decimal

    Public Property IdMes As Integer
    Public Property Anio As Integer

    Public Property Fecha As Date

    Public Property ContadorEquipo1Diferencia As Integer

    Public Property ContadorEquipo2Diferencia As Integer

    Public Property TotalContadores As Integer

    Public Property EfectivoDiferencia As Decimal

    Public Property TransferenciaDiferencia As Decimal

    Public Property TotalCaja As Decimal

    Public Property TotalPerdida As Integer

    Public Property TotalDeudor As Integer

    Public Property TotalPagadas As Integer

    Public Property Registradas As Integer

    Public Property NoRegistradas As Integer

    Public Property movimientosCajaEgreso As List(Of MovimientoCaja)
    Public Property movimientosCajaIngreso As List(Of MovimientoCaja)

    Public Property TotalEgresoTransferencia As Integer
    Public Property TotalEgresoEfectivo As Integer

    Public Property TotalIngresoTransferencia As Integer
    Public Property TotalIngresoEfectivo As Integer

    Public Property TotalPendientes As Integer
End Class
