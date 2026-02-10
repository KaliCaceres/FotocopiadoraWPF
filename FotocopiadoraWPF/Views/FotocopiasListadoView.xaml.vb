
Imports FotocopiadoraWPF.Services
Imports FotocopiadoraWPF.ViewModels

Namespace Views
    Partial Public Class FotocopiasListadoView

        Inherits UserControl

        Public Sub New()
            InitializeComponent()

            If BalanceActualService.BalanceActualId <= 0 Then
                MessageBox.Show("No hay un balance activo.", "Atención",
                            MessageBoxButton.OK, MessageBoxImage.Warning)
                Return
            End If

            DataContext = New FotocopiasListadoViewModel()
        End Sub
        Private Sub AbrirMenu(sender As Object, e As RoutedEventArgs)
            Dim btn = CType(sender, Button)
            btn.ContextMenu.PlacementTarget = btn
            btn.ContextMenu.IsOpen = True
        End Sub

    End Class

End Namespace