Imports FotocopiadoraWPF.ViewModels

Namespace FotocopiadoraWPF.Views

    Partial Public Class FotocopiasView
        Inherits UserControl

        Public Sub New()
            InitializeComponent()
            DataContext = New FotocopiasViewModel()
        End Sub
    End Class
End Namespace
