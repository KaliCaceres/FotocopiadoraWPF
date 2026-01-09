Imports FotocopiadoraWPF.ViewModels

Namespace FotocopiadoraWPF.Views

    Partial Public Class FotocopiasView
        Inherits UserControl
        Private Sub Efectivo_DoubleClick(sender As Object, e As MouseButtonEventArgs)
            Dim vm = CType(DataContext, FotocopiasViewModel)
            vm.PagarConEfectivo()
            e.Handled = True
        End Sub

        Private Sub Transferencia_DoubleClick(sender As Object, e As MouseButtonEventArgs)
            Dim vm = CType(DataContext, FotocopiasViewModel)
            vm.PagarConTransferencia()
            e.Handled = True
        End Sub

        Public Sub New()
            InitializeComponent()
            DataContext = New FotocopiasViewModel()
        End Sub


    End Class


End Namespace
