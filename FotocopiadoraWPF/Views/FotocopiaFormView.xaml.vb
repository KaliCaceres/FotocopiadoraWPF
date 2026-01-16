Imports System.Windows.Controls
Imports System.Windows.Input
Imports FotocopiadoraWPF.ViewModels

Namespace FotocopiadoraWPF.Views
    Partial Public Class FotocopiaFormView
        Inherits UserControl

        Private Sub Efectivo_DoubleClick(sender As Object, e As MouseButtonEventArgs)
            Dim vm = TryCast(Me.DataContext, FotocopiasViewModel)
            If vm Is Nothing Then Return

            If vm.PagarConEfectivoCommand.CanExecute(Nothing) Then
                vm.PagarConEfectivoCommand.Execute(Nothing)
            End If
        End Sub

        Private Sub Transferencia_DoubleClick(sender As Object, e As MouseButtonEventArgs)
            Dim vm = TryCast(Me.DataContext, FotocopiasViewModel)
            If vm Is Nothing Then Return

            If vm.PagarConTransferenciaCommand.CanExecute(Nothing) Then
                vm.PagarConTransferenciaCommand.Execute(Nothing)
            End If
        End Sub

    End Class
End Namespace
