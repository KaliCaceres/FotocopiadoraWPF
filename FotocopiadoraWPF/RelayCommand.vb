Imports System.Windows.Input

Public Class RelayCommand
    Implements ICommand

    Private ReadOnly _execute As Action
    Private ReadOnly _canExecute As Func(Of Boolean)

    Public Sub New(execute As Action, Optional canExecute As Func(Of Boolean) = Nothing)
        _execute = execute
        _canExecute = canExecute
    End Sub

    Public Event CanExecuteChanged As EventHandler _
        Implements ICommand.CanExecuteChanged

    Public Function CanExecute(parameter As Object) As Boolean _
        Implements ICommand.CanExecute
        If _canExecute Is Nothing Then Return True
        Return _canExecute()
    End Function

    Public Sub Execute(parameter As Object) _
        Implements ICommand.Execute
        _execute()
    End Sub

    Public Sub RaiseCanExecuteChanged()
        RaiseEvent CanExecuteChanged(Me, EventArgs.Empty)
    End Sub
End Class
