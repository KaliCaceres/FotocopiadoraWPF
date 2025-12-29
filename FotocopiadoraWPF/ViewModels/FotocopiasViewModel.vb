Imports System.ComponentModel

Namespace ViewModels

    Public Class FotocopiasViewModel

        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Private Sub Avisar(nombre As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(nombre))
        End Sub

        Private _paginas As Integer?
        Private _anillados As Integer?

        Private Const PrecioPagina As Integer = 800
        Private Const PrecioAnillado As Integer = 2000

        Public ReadOnly Property TotalPaginas As Integer
            Get
                Return If(Paginas, 0) * PrecioPagina
            End Get
        End Property

        Public ReadOnly Property TotalAnillados As Integer
            Get
                Return If(Anillados, 0) * PrecioAnillado
            End Get
        End Property

        Public ReadOnly Property Total As Integer
            Get
                Return TotalPaginas + TotalAnillados
            End Get
        End Property

        Public Property Paginas As Integer?
            Get
                Return _paginas
            End Get
            Set(value As Integer?)
                _paginas = value
                Avisar(NameOf(Paginas))
                Avisar(NameOf(TotalPaginas))
                Avisar(NameOf(Total))
            End Set
        End Property

        Public Property Anillados As Integer?
            Get
                Return _anillados
            End Get
            Set(value As Integer?)
                _anillados = value
                Avisar(NameOf(Anillados))
                Avisar(NameOf(TotalAnillados))
                Avisar(NameOf(Total))
            End Set
        End Property

    End Class

End Namespace
