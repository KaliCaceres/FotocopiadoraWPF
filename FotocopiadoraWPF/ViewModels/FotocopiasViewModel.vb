Namespace ViewModels

    Public Class FotocopiasViewModel
        Implements ComponentModel.INotifyPropertyChanged

        Public Event PropertyChanged As ComponentModel.PropertyChangedEventHandler _
            Implements ComponentModel.INotifyPropertyChanged.PropertyChanged

        Protected Sub OnPropertyChanged(nombre As String)
            RaiseEvent PropertyChanged(Me, New ComponentModel.PropertyChangedEventArgs(nombre))
        End Sub

        Private _paginas As Integer?
        Public Property Paginas As Integer?
            Get
                Return _paginas
            End Get
            Set(value As Integer?)
                If _paginas <> value Then
                    _paginas = value
                    OnPropertyChanged(NameOf(Paginas))
                End If
            End Set
        End Property

    End Class

End Namespace
