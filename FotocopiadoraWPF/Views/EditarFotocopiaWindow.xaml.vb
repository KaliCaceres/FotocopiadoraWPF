Imports System.ComponentModel
Imports FotocopiadoraWPF.ViewModels

Namespace Views
    Partial Public Class EditarFotocopiaWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub EditarFotocopiaWindow_DataContextChanged(sender As Object, e As DependencyPropertyChangedEventArgs) Handles Me.DataContextChanged
            Dim vm = TryCast(DataContext, FotocopiasViewModel)
            If vm Is Nothing Then Return

            AddHandler vm.PropertyChanged, AddressOf Vm_PropertyChanged
        End Sub

        Private Sub Vm_PropertyChanged(sender As Object, e As PropertyChangedEventArgs)
            If e.PropertyName = NameOf(FotocopiasViewModel.GuardadoConExito) Then
                Dim vm = CType(DataContext, FotocopiasViewModel)
                If vm.GuardadoConExito Then
                    Me.DialogResult = True
                    Me.Close()
                End If
            End If
        End Sub
    End Class
End Namespace
