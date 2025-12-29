Namespace FotocopiadoraWPF.Views

    Partial Public Class FotocopiasView
        Inherits UserControl

        Public Sub New()
            InitializeComponent()
            Me.DataContext = New ViewModels.FotocopiasViewModel()
        End Sub

    End Class

End Namespace
