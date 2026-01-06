Imports System.ComponentModel

Public Class PreciosViewModel
    Implements INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged
    Private Sub Avisar(nombre As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(nombre))
    End Sub

    '==================== CAMPOS / REPOS ====================

    Private _valores As List(Of ValorConfiguracion)


    '==================== MÉTODOS PRIVADOS ====================

    Private Function ObtenerValor(descripcion As String) As Integer
        Return CInt(_valores.First(Function(v) v.Descripcion = descripcion).Valor)
    End Function


    Private _inputsHabilitados As Boolean = True

    Public Property InputsHabilitados As Boolean
        Get
            Return _inputsHabilitados
        End Get
        Set(value As Boolean)
            _inputsHabilitados = value
            Avisar(NameOf(InputsHabilitados))
        End Set
    End Property


    Public ReadOnly Property ToggleInputsCommand As ICommand

    Public Sub New()
        ToggleInputsCommand = New RelayCommand(AddressOf ToggleInputs)
    End Sub

    Private Sub ToggleInputs()
        InputsHabilitados = Not InputsHabilitados
    End Sub

End Class
