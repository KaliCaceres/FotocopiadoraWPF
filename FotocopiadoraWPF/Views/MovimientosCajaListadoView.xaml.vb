Imports FotocopiadoraWPF.ViewModels

Namespace Views
    Public Class MovimientosCajaListadoView
        Public Sub New()
            InitializeComponent()
            DataContext = New MovimientosCajaViewModel(1)
            'MessageBox.Show(Me.DataContext?.GetType().ToString())

        End Sub
    End Class

End Namespace