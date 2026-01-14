Class FotocopiasListadoView
    Private Sub AbrirMenu(sender As Object, e As RoutedEventArgs)
        Dim btn = CType(sender, Button)
        btn.ContextMenu.IsOpen = True
    End Sub

End Class
