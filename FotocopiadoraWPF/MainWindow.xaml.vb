
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

            Select Case clickedItem.Tag?.ToString()

                Case "CajaAlta"
                    MainContent.Content = New MovimientosCajaView()

                Case "CajaListado"
                    MainContent.Content = New MovimientosCajaListadoView()

                Case "FotoAlta"
                    MainContent.Content = New FotocopiasView()

                Case "FotoListado"
                    MainContent.Content = New FotocopiasListadoView()

                Case "Balance"
                    MainContent.Content = New BalanceView()

                Case "Precios"
                    MainContent.Content = New PreciosView()

                Case "Estadisticas"
                    MainContent.Content = New EstadisticasView()
            End Select
        End Sub


    End Class

End Namespace
