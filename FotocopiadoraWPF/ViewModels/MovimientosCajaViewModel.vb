Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Windows.Data
Imports System.Windows.Input

Namespace ViewModels

    Public Class MovimientosCajaViewModel
        Implements INotifyPropertyChanged

        ' ===================== REPO =====================
        Private ReadOnly _repo As New MovimientosCajaRepository()
        Private ReadOnly _idResumen As Integer

        ' ===================== LISTADO =====================
        Public Property Movimientos As ObservableCollection(Of MovimientoCaja)
        Public Property MovimientosView As ICollectionView

        ' ===================== CONSTRUCTOR =====================
        Public Sub New(idResumen As Integer)
            _idResumen = idResumen

            Movimientos = New ObservableCollection(Of MovimientoCaja)(
                _repo.ObtenerPorResumen(idResumen)
            )

            PrepararVista()
            InicializarFormulario()

            GuardarCommand = New RelayCommand(AddressOf GuardarMovimiento)
        End Sub

        ' ===================== COMMAND =====================
        Public ReadOnly Property GuardarCommand As ICommand

        ' ===================== FORM =====================
        Private _motivo As String
        Public Property Motivo As String
            Get
                Return _motivo
            End Get
            Set(value As String)
                _motivo = value
                Avisar(NameOf(Motivo))
            End Set
        End Property

        Private _monto As Decimal
        Public Property Monto As Decimal
            Get
                Return _monto
            End Get
            Set(value As Decimal)
                _monto = value
                Avisar(NameOf(Monto))
            End Set
        End Property

        Private _tipoSeleccionado As String
        Public Property TipoSeleccionado As String
            Get
                Return _tipoSeleccionado
            End Get
            Set(value As String)
                _tipoSeleccionado = value
                Avisar(NameOf(TipoSeleccionado))
            End Set
        End Property

        Private _metodoSeleccionado As String
        Public Property MetodoSeleccionado As String
            Get
                Return _metodoSeleccionado
            End Get
            Set(value As String)
                _metodoSeleccionado = value
                Avisar(NameOf(MetodoSeleccionado))
            End Set
        End Property

        Private _empleadoSeleccionado As String
        Public Property EmpleadoSeleccionado As String
            Get
                Return _empleadoSeleccionado
            End Get
            Set(value As String)
                _empleadoSeleccionado = value
                Avisar(NameOf(EmpleadoSeleccionado))
            End Set
        End Property

        Private _observacion As String
        Public Property Observacion As String
            Get
                Return _observacion
            End Get
            Set(value As String)
                _observacion = value
                Avisar(NameOf(Observacion))
            End Set
        End Property

        ' ===================== COMBOS =====================
        Public ReadOnly Property Tipos As List(Of String) =
            New List(Of String) From {"Seleccionar", "Ingreso", "Egreso"}

        Public ReadOnly Property MetodosPago As List(Of String) =
            New List(Of String) From {"Seleccionar", "Efectivo", "Transferencia"}

        Public ReadOnly Property Empleados As List(Of String) =
            New List(Of String) From {"Seleccionar", "Juanchi", "Seba", "Paulina"}





        ' ===================== MÉTODOS =====================
        Private Sub PrepararVista()
            Dim view = CollectionViewSource.GetDefaultView(Movimientos)

            view.GroupDescriptions.Clear()
            view.GroupDescriptions.Add(
                New PropertyGroupDescription("Fecha", New FechaSoloDiaConverter())
            )

            MovimientosView = view
        End Sub

        Private Sub InicializarFormulario()
            TipoSeleccionado = Tipos.First()
            MetodoSeleccionado = MetodosPago.First()
            EmpleadoSeleccionado = Empleados.First()
            Motivo = ""
            Monto = 0
            Observacion = ""
        End Sub

        Private Sub GuardarMovimiento()
            Try
                If String.IsNullOrWhiteSpace(Motivo) OrElse Monto <= 0 Then
                    MessageBox.Show("Faltan datos obligatorios", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning)
                    Exit Sub
                End If

                Dim m As New MovimientoCaja With {
            .IdResumen = _idResumen,
            .Fecha = Fecha,
            .Motivo = Motivo,
            .Monto = Monto,
            .Tipo = TipoSeleccionado,
            .MetodoPago = MetodoSeleccionado,
            .Empleado = EmpleadoSeleccionado,
            .observacion = Observacion
        }

                _repo.Insertar(m)

                Movimientos.Insert(0, m)

                InicializarFormulario()

                MessageBox.Show("Movimiento guardado correctamente", "OK", MessageBoxButton.OK, MessageBoxImage.Information)

            Catch ex As Exception
                MessageBox.Show(
            ex.Message,
            "Error al guardar",
            MessageBoxButton.OK,
            MessageBoxImage.Error
        )
            End Try
        End Sub


        Private _fecha As DateTime = DateTime.Now
        Public Property Fecha As DateTime
            Get
                Return _fecha
            End Get
            Set(value As DateTime)
                _fecha = value
                Avisar(NameOf(Fecha))
            End Set
        End Property


        ' ===================== INotify =====================
        Private Sub Avisar(nombre As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(nombre))
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

    End Class

End Namespace
