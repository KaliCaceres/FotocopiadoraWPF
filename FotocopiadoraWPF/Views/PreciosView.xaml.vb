Imports FotocopiadoraWPF.ViewModels
Imports Microsoft.Data.Sqlite

Namespace Views
    Partial Public Class PreciosView
        Inherits UserControl

        Public Sub New()
            InitializeComponent()
            Using cn As New SqliteConnection(Configuracion.ConnectionString)
                cn.Open()
            End Using
            Dim vm As New PreciosViewModel()
            DataContext = vm
            vm.Inicializar()
        End Sub
    End Class
End Namespace
