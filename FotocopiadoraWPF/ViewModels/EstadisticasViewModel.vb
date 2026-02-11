Imports System.ComponentModel
Imports FotocopiadoraWPF.Services

Namespace ViewModels

    Public Class EstadisticasViewModel
        Implements INotifyPropertyChanged

        Private ReadOnly _repo As New FotocopiasRepository()

        Public Property TotalPaginas As Integer
        Public Property TotalAnillados As Integer
        Public Property TotalFacturado As Decimal
        Public Property TotalEfectivo As Decimal
        Public Property TotalTransferencia As Decimal
        Public Property CantidadOperaciones As Integer

        Public Sub New()

            If BalanceActualService.BalanceActualId <= 0 Then
                Exit Sub
            End If

            Dim resumen = _repo.ObtenerResumenFotocopias(
                BalanceActualService.BalanceActualId)

            TotalPaginas = resumen.TotalPaginas
            TotalAnillados = resumen.TotalAnillados
            TotalFacturado = resumen.TotalFacturado
            TotalEfectivo = resumen.TotalEfectivo
            TotalTransferencia = resumen.TotalTransferencia
            CantidadOperaciones = resumen.Cantidad

        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

    End Class

End Namespace
