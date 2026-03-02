
Imports FotocopiadoraWPF.ViewModels
Imports FotocopiadoraWPF.Views
Imports Microsoft.Data.Sqlite
Imports Syncfusion.UI.Xaml.NavigationDrawer
Namespace FotocopiadoraWPF

    Partial Class MainWindow
        Inherits Window

        Private ReadOnly _balanceVM As New BalanceViewModel()



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
                    Dim view As New BalanceView()
                    view.DataContext = _balanceVM
                    MainContent.Content = view

                Case "Estadisticas"
                    Dim view As New EstadisticasView()
                    view.DataContext = _balanceVM
                    MainContent.Content = view

                Case "Precios"
                    MainContent.Content = New PreciosView()

                Case "LibroAlta"
                    MainContent.Content = New BibliotecaView()

                Case "LibroListado"
                    MainContent.Content = New BibliotecaView()
            End Select
        End Sub


    End Class

End Namespace
