Namespace ViewModels
    Public Class EditarFotocopiaViewModel

        Public Property Fotocopia As Fotocopia
        Public ReadOnly Property GuardarCommand As ICommand
        Public ReadOnly Property CancelarCommand As ICommand

        Public Sub New(f As Fotocopia)
            Fotocopia = f
            GuardarCommand = New RelayCommand(AddressOf Guardar)
            CancelarCommand = New RelayCommand(AddressOf Cancelar)
        End Sub

        Private Sub Guardar()
            MessageBox.Show("Edición OK")
            Cerrar(True)
        End Sub

        Private Sub Cancelar()
            Cerrar(False)
        End Sub

        Private Sub Cerrar(resultado As Boolean)
            Dim win = Application.Current.Windows.
                  OfType(Of Window)().
                  FirstOrDefault(Function(w) w.DataContext Is Me)

            If win IsNot Nothing Then
                win.DialogResult = resultado
            End If
        End Sub

    End Class

End Namespace