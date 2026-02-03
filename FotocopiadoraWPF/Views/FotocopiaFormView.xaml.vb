Imports System.Windows.Controls
Imports System.Windows.Input

Imports FotocopiadoraWPF.ViewModels

Namespace Views
    Partial Public Class FotocopiaFormView
        Inherits UserControl
        Private Sub Efectivo_DoubleClick(sender As Object, e As MouseButtonEventArgs)
            Dim vm = CType(DataContext, FotocopiasViewModel)

            If Not vm.PuedeEditarPago Then
                e.Handled = True
                Return
            End If

            vm.Efectivo = vm.Total
            vm.Transferencia = 0
        End Sub

        Private Sub Transferencia_DoubleClick(sender As Object, e As MouseButtonEventArgs)
            Dim vm = CType(DataContext, FotocopiasViewModel)

            If Not vm.PuedeEditarPago Then
                e.Handled = True
                Return
            End If

            vm.Transferencia = vm.Total
            vm.Efectivo = 0
        End Sub


    End Class
End Namespace
