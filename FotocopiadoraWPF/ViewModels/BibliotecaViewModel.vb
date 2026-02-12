Imports System.Collections.ObjectModel
Imports System.ComponentModel

Public Class BibliotecaViewModel
    Implements INotifyPropertyChanged

    Private ReadOnly _repo As New LibrosRepository()

    Public Event PropertyChanged As PropertyChangedEventHandler _
        Implements INotifyPropertyChanged.PropertyChanged

    Private Sub Avisar(p As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(p))
    End Sub

    Public Property Libros As ObservableCollection(Of Libro)
    Public Property LibrosView As ICollectionView

    Public Property NuevoLibro As New Libro()

    Public Property GuardarCommand As ICommand

    Private _busqueda As String
    Public Property Busqueda As String
        Get
            Return _busqueda
        End Get
        Set(value As String)
            _busqueda = value
            Avisar(NameOf(Busqueda))
            LibrosView.Refresh()
        End Set
    End Property

    Public Sub New()

        Libros = New ObservableCollection(Of Libro)(_repo.ObtenerLibros())

        LibrosView = CollectionViewSource.GetDefaultView(Libros)
        LibrosView.Filter = AddressOf Filtrar

        GuardarCommand = New RelayCommand(AddressOf GuardarLibro)

    End Sub

    Private Function Filtrar(obj As Object) As Boolean

        If String.IsNullOrWhiteSpace(Busqueda) Then Return True

        Dim l = CType(obj, Libro)
        Return l.Titulo.ToLower().Contains(Busqueda.ToLower())

    End Function

    Private Sub GuardarLibro()

        If String.IsNullOrWhiteSpace(NuevoLibro.Titulo) Then Return

        _repo.Guardar(NuevoLibro)

        Libros.Add(New Libro With {
            .Titulo = NuevoLibro.Titulo
        })

        NuevoLibro = New Libro()
        Avisar(NameOf(NuevoLibro))

    End Sub

End Class
