Imports System.ComponentModel
Imports System.Windows.Input

Namespace ViewModels

    Public Class FotocopiasViewModel
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Private Sub Avisar(nombre As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(nombre))
        End Sub


        Public ReadOnly Property CopiarTotalCommand As ICommand

        Public Sub New()
            CopiarTotalCommand = New RelayCommand(AddressOf CopiarTotal)
        End Sub

        Private Sub CopiarTotal()
            MiNumero = Total
        End Sub


        Private _paginas As Integer?
        Private _anillados As Integer?

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


        Private _efectivo As Integer?
        Private _transferencia As Integer?

        Public Property Efectivo As Integer?
            Get
                Return _efectivo
            End Get
            Set(value As Integer?)
                _efectivo = value
                Avisar(NameOf(Efectivo))
            End Set
        End Property

        Public Property Transferencia As Integer?
            Get
                Return _transferencia
            End Get
            Set(value As Integer?)
                _transferencia = value
                Avisar(NameOf(Transferencia))
            End Set
        End Property

        Public Sub PagarConEfectivo()
            Efectivo = Total
            Transferencia = 0
        End Sub

        Public Sub PagarConTransferencia()
            Transferencia = Total
            Efectivo = 0
        End Sub


        Private Const PrecioPagina As Integer = 80
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


        Private _miNumero As Integer?

        Public Property MiNumero As Integer?
            Get
                Return _miNumero
            End Get
            Set(value As Integer?)
                _miNumero = value
                Avisar(NameOf(MiNumero))
            End Set
        End Property

    End Class

End Namespace
