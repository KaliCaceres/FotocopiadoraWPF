Imports System.Windows.Input

Public Class RelayCommand(Of T)
    Implements ICommand

    Private ReadOnly _execute As Action(Of T)
    Private ReadOnly _canExecute As Func(Of T, Boolean)

    Public Sub New(execute As Action(Of T),
                   Optional canExecute As Func(Of T, Boolean) = Nothing)

        _execute = execute
        _canExecute = canExecute
    End Sub

    Public Event CanExecuteChanged As EventHandler _
        Implements ICommand.CanExecuteChanged

    Public Function CanExecute(parameter As Object) As Boolean _
        Implements ICommand.CanExecute

        If _canExecute Is Nothing Then Return True

        If TypeOf parameter Is T Then
            Return _canExecute(CType(parameter, T))
        End If

        Return False
    End Function

    Public Sub Execute(parameter As Object) _
        Implements ICommand.Execute

        If TypeOf parameter Is T Then
            _execute(CType(parameter, T))
        End If
    End Sub

    Public Sub RaiseCanExecuteChanged()
        RaiseEvent CanExecuteChanged(Me, EventArgs.Empty)
    End Sub

End Class
