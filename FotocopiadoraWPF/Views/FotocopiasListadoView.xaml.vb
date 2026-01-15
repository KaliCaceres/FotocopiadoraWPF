Imports FotocopiadoraWPF.ViewModels

Namespace FotocopiadoraWPF.Views
    Partial Public Class FotocopiasListadoView


        Inherits UserControl
        Private Sub AbrirMenu(sender As Object, e As RoutedEventArgs)
            Dim btn = CType(sender, Button)
            btn.ContextMenu.PlacementTarget = btn
            btn.ContextMenu.IsOpen = True
        End Sub

        Public Sub New()
            InitializeComponent()
        End Sub

    End Class

End Namespace