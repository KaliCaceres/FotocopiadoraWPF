Imports FotocopiadoraWPF.ViewModels

Namespace Views
    Partial Public Class MovimientosCajaView
        Public Sub New()
            InitializeComponent()
            DataContext = New MovimientosCajaViewModel(1)
        End Sub

    End Class
End Namespace
