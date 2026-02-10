Imports FotocopiadoraWPF.ViewModels

Namespace Views
    Public Class MovimientosCajaListadoView
        Public Sub New()
            InitializeComponent()
            DataContext = New MovimientosCajaViewModel(1)
        End Sub
    End Class

End Namespace