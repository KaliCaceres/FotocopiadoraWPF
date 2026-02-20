Imports FotocopiadoraWPF.ViewModels

Public Class EditarMovimientoWindow

    Private _vm As EditarMovimientoViewModel

    Public Sub New(m As MovimientoCaja)
        InitializeComponent()

        _vm = New EditarMovimientoViewModel(m)

        AddHandler _vm.SolicitarCerrar, AddressOf CerrarVentana

        DataContext = _vm
    End Sub

    Private Sub CerrarVentana(resultado As Boolean)
        DialogResult = resultado
        Close()
    End Sub

    Public ReadOnly Property MovimientoEditado As MovimientoCaja
        Get
            Return _vm.Movimiento
        End Get
    End Property

End Class