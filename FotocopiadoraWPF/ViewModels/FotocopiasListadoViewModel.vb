Imports System.Windows
Imports System.Windows.Data
Imports System.Windows.Input

Imports System.Collections.ObjectModel
Imports System.ComponentModel

Imports FotocopiadoraWPF.Views
Imports FotocopiadoraWPF.Converters

Namespace ViewModels

    Public Class FotocopiasListadoViewModel
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Private ReadOnly _repo As New FotocopiasRepository()

        Public Property Fotocopias As ObservableCollection(Of Fotocopia)
        Public ReadOnly Property EditarCommand As ICommand
        Public ReadOnly Property EliminarCommand As ICommand
        Public ReadOnly Property FotocopiasView As ICollectionView

        Public Sub New()

            Fotocopias = New ObservableCollection(Of Fotocopia)(
                _repo.ObtenerFotocopias()
            )

            CargarDatos()

            Dim view = CollectionViewSource.GetDefaultView(Fotocopias)
            view.GroupDescriptions.Add(
            New PropertyGroupDescription("Fecha", New FechaSoloDiaConverter())
        )

            FotocopiasView = view

            EditarCommand = New RelayCommand(Of Fotocopia)(
                AddressOf EditarFotocopia)

            EliminarCommand = New RelayCommand(Of Fotocopia)(
                AddressOf EliminarFotocopia)
        End Sub

        Private Sub CargarDatos()
            Fotocopias.Add(New Fotocopia With {.Nombre = "Juan", .Fecha = #01/15/2026#})
            Fotocopias.Add(New Fotocopia With {.Nombre = "Ana", .Fecha = #01/15/2026#})
            Fotocopias.Add(New Fotocopia With {.Nombre = "Pedro", .Fecha = #01/14/2026#})
        End Sub
        Private Sub EditarFotocopia(f As Fotocopia)
            If f Is Nothing Then Return

            ' Clonamos para no modificar hasta guardar
            Dim copia As New Fotocopia With {
        .IdFotocopia = f.IdFotocopia,
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

            Dim vm = New EditarFotocopiaViewModel(copia)
            Dim win As New EditarFotocopiaWindow()

            win.DataContext = vm          ' 🔥 CLAVE
            win.Owner = Application.Current.MainWindow



            If win.ShowDialog() = True Then
                ' Persistís cambios (luego haremos UPDATE)
                f.Nombre = copia.Nombre
                f.Fecha = copia.Fecha
                f.Paginas = copia.Paginas
                f.Anillados = copia.Anillados
                f.Transferencia = copia.Transferencia
                f.Efectivo = copia.Efectivo
                f.Comentario = copia.Comentario
                f.PrecioTotal = copia.PrecioTotal
            End If
        End Sub

        Private Sub EliminarFotocopia(f As Fotocopia)
            If f Is Nothing Then Return

            If MessageBox.Show("¿Eliminar esta fotocopia?",
                               "Confirmar",
                               MessageBoxButton.YesNo,
                               MessageBoxImage.Warning) = MessageBoxResult.Yes Then
                Fotocopias.Remove(f)
            End If
        End Sub

    End Class
End Namespace