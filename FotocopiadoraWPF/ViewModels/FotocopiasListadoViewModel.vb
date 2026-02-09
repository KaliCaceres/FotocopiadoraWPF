Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Windows
Imports System.Windows.Data
Imports System.Windows.Input
Imports FotocopiadoraWPF.Converters
Imports FotocopiadoraWPF.Services
Imports FotocopiadoraWPF.Views

Namespace ViewModels

    Public Class FotocopiasListadoViewModel
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Private ReadOnly _repo As New FotocopiasRepository()

        Public Property Fotocopias As ObservableCollection(Of Fotocopia)
        Public ReadOnly Property EditarCommand As ICommand
        Public ReadOnly Property EliminarCommand As ICommand

        Public Property FotocopiasView As ICollectionView

        Public Sub New()

            Fotocopias = New ObservableCollection(Of Fotocopia)(
                _repo.ObtenerFotocopiasPorBalance(
                    BalanceActualService.BalanceActualId
                )
            )


            Dim view = CollectionViewSource.GetDefaultView(Fotocopias)

            ' 🔹 FILTRO
            view.Filter = AddressOf FiltrarPorEstado

            ' 🔹 GROUPING
            view.GroupDescriptions.Clear()
            view.GroupDescriptions.Add(
        New PropertyGroupDescription("Fecha", New FechaSoloDiaConverter())
    )

            ' 👉 PRIMERO asignás la vista
            FotocopiasView = view

            ' 👉 DESPUÉS seteás el filtro
            EstadoSeleccionado = EstadosFiltro.First() ' "Todos"

            EditarCommand = New RelayCommand(Of Fotocopia)(AddressOf EditarFotocopia)
            EliminarCommand = New RelayCommand(Of Fotocopia)(AddressOf EliminarFotocopia)
        End Sub


        Private Sub Avisar(nombre As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(nombre))
        End Sub

        Public Class EstadoItem
            Public Property Id As Integer?
            Public Property Nombre As String
        End Class
        Public Class PagoItem
            Public Property Id As String
            Public Property Nombre As String
        End Class

        Public ReadOnly Property MetodosPagoFiltro As List(Of PagoItem) =
        New List(Of PagoItem) From {
            New PagoItem With {.Id = "TODOS", .Nombre = "Todos"},
            New PagoItem With {.Id = "PAGADO", .Nombre = "Pagado"},
            New PagoItem With {.Id = "EFECTIVO", .Nombre = "Efectivo"},
            New PagoItem With {.Id = "TRANSFERENCIA", .Nombre = "Transferencia"}
        }

        Private _pagoSeleccionado As PagoItem

        Public Property PagoSeleccionado As PagoItem
            Get
                Return _pagoSeleccionado
            End Get
            Set(value As PagoItem)
                _pagoSeleccionado = value
                Avisar(NameOf(PagoSeleccionado))
                FotocopiasView.Refresh()
            End Set
        End Property

        Public ReadOnly Property EstadosFiltro As List(Of EstadoItem) =
        New List(Of EstadoItem) From {
        New EstadoItem With {.Id = Nothing, .Nombre = "Todos"},
        New EstadoItem With {.Id = 0, .Nombre = "Pagado"},
        New EstadoItem With {.Id = 1, .Nombre = "Deudor"},
        New EstadoItem With {.Id = 2, .Nombre = "Perdida"}
        }


        Private _estadoSeleccionado As EstadoItem

        Public Property EstadoSeleccionado As EstadoItem
            Get
                Return _estadoSeleccionado
            End Get
            Set(value As EstadoItem)
                _estadoSeleccionado = value
                Avisar(NameOf(EstadoSeleccionado))
                FotocopiasView.Refresh()
            End Set
        End Property


        Private Sub CargarDatos()
            Fotocopias.Add(New Fotocopia With {.Nombre = "Juan", .Fecha = #01/15/2026#})
            Fotocopias.Add(New Fotocopia With {.Nombre = "Ana", .Fecha = #01/15/2026#})
            Fotocopias.Add(New Fotocopia With {.Nombre = "Pedro", .Fecha = #01/14/2026#})
        End Sub

        '    Private Sub CargarDatosDesdeBD()
        '        For Each f In _repo.ObtenerFotocopias()
        '            Fotocopias.Add(f)
        '        Next
        '    End Sub

        '    Private Sub PrepararVista()
        '        Dim view = CollectionViewSource.GetDefaultView(Fotocopias)
        '        view.GroupDescriptions.Clear()
        '        view.GroupDescriptions.Add(
        '    New PropertyGroupDescription("Fecha", New FechaSoloDiaConverter())
        ')

        '        FotocopiasView = view
        '    End Sub

        Private Sub EditarFotocopia(f As Fotocopia)
            If f Is Nothing Then Return

            ' Clonamos para no modificar el original hasta guardar
            Dim copia As New Fotocopia With {
                .IdFotocopia = f.IdFotocopia,
                .IdResumen = f.IdResumen,   ' 👈 CLAVE
                .Nombre = f.Nombre,
                .Fecha = f.Fecha,
                .Paginas = f.Paginas,
                .Anillados = f.Anillados,
                .Transferencia = f.Transferencia,
                .Efectivo = f.Efectivo,
                .Comentario = f.Comentario,
                .PrecioUnitario = f.PrecioUnitario,
                .PrecioTotal = f.PrecioTotal
            }


            Dim vm As New FotocopiasViewModel(copia)
            Dim win As New EditarFotocopiaWindow()

            win.DataContext = vm
            win.Owner = Application.Current.MainWindow

            If win.ShowDialog() = True Then
                RefrescarListado()
            End If
        End Sub

        Private _estadoFiltro As Integer? = Nothing   ' Nothing = Todos

        Public Property EstadoFiltro As Integer?
            Get
                Return _estadoFiltro
            End Get
            Set(value As Integer?)
                If _estadoFiltro <> value Then
                    _estadoFiltro = value
                    Avisar(NameOf(EstadoFiltro))
                    FotocopiasView.Refresh()
                End If
            End Set
        End Property

        Private Function FiltrarPorEstado(obj As Object) As Boolean
            Dim f = CType(obj, Fotocopia)

            If f.IdEstado = 3 Then Return False

            If EstadoSeleccionado Is Nothing OrElse EstadoSeleccionado.Id Is Nothing Then
                Return True
            End If

            If f.IdEstado <> EstadoSeleccionado.Id.Value Then
                Return False
            End If

            If EstadoSeleccionado.Id.Value = 0 Then
                Return f.Efectivo > 0 OrElse f.Transferencia > 0
            End If

            Return True
        End Function





        Private Sub EliminarFotocopia(f As Fotocopia)
            If f Is Nothing Then Return

            If MessageBox.Show("¿Eliminar esta fotocopia?",
                       "Confirmar",
                       MessageBoxButton.YesNo,
                       MessageBoxImage.Warning) <> MessageBoxResult.Yes Then
                Return
            End If

            Try

                f.IdEstado = 3 ' Perdida

                _repo.ActualizarEstado(f.IdFotocopia, 3)

                FotocopiasView.Refresh()

            Catch ex As Exception
                MessageBox.Show("Error al anular la fotocopia: " & ex.Message)
            End Try
        End Sub

        Private Sub RefrescarListado()
            Fotocopias.Clear()

            For Each f In _repo.ObtenerFotocopiasPorBalance(
                BalanceActualService.BalanceActualId
            )
                Fotocopias.Add(f)
            Next

        End Sub


    End Class
End Namespace