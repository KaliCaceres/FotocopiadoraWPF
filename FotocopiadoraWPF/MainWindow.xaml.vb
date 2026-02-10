
Imports Syncfusion.UI.Xaml.NavigationDrawer
Imports FotocopiadoraWPF.Views
Imports Microsoft.Data.Sqlite
Namespace FotocopiadoraWPF

    Partial Class MainWindow
        Inherits Window



        Private Sub NavigationDrawer_ItemClicked(
            sender As Object,
            e As NavigationItemClickedEventArgs) _
            Handles navigationDrawer.ItemClicked

            Dim clickedItem As NavigationItem = TryCast(e.Item, NavigationItem)
            If clickedItem Is Nothing Then Exit Sub

            Select Case clickedItem.Header.ToString()
                Case "Fotocopias"
                    MainContent.Content = New FotocopiasView()

                Case "Balance"
                    MainContent.Content = New BalanceView()

                Case "Precios"
                    MainContent.Content = New PreciosView()

                Case "Listado"
                    MainContent.Content = New FotocopiasListadoView()

                Case "Movimientos"
                    MainContent.Content = New MovimientosCajaView()

                Case "Movimientos Listado"
                    MainContent.Content = New MovimientosCajaListadoView()
            End Select

        End Sub

    End Class

End Namespace
