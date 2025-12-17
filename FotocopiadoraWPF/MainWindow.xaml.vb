Imports Syncfusion.UI.Xaml.NavigationDrawer

Namespace FotocopiadoraWPF

    Partial Class MainWindow
        Inherits Window

        Private Sub NavigationDrawer_ItemClicked(sender As Object, e As NavigationItemClickedEventArgs) _
            Handles navigationDrawer.ItemClicked

            Dim clickedItem As NavigationItem = TryCast(e.Item, NavigationItem)
            If clickedItem Is Nothing Then Exit Sub

            Select Case clickedItem.Header.ToString()
                Case "Fotocopias"
                    MainContent.Content = New FotocopiasView()

                Case "Balance"
                    MainContent.Content = New BalanceView()
            End Select

        End Sub

    End Class

End Namespace
