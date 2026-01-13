Imports System.Collections.ObjectModel
Imports System.ComponentModel
Namespace ViewModels
    Public Class FotocopiasListadoViewModel
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Private ReadOnly _repo As New FotocopiasRepository()

        Public Property Fotocopias As ObservableCollection(Of Fotocopia)

        Public Sub New()
            Fotocopias = New ObservableCollection(Of Fotocopia)(
                _repo.ObtenerFotocopias()
            )
        End Sub

    End Class
End Namespace