Imports System.ComponentModel
Imports System.Windows.Input
Imports System.Windows.Data
Imports System.Windows

Namespace ViewModels

    Public Class FotocopiasViewModel
        Implements INotifyPropertyChanged



        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Private Sub Avisar(nombre As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(nombre))
        End Sub


        '==================== CAMPOS / REPOS ====================

        Private ReadOnly _repo As New ValoresRepository()
        Private ReadOnly _fotocopiasRepo As New FotocopiasRepository()
        Private _valores As List(Of ValorConfiguracion)

        '==================== CONSTRUCTOR ====================

        Public ReadOnly Property CopiarTotalCommand As ICommand
        Public ReadOnly Property GuardarCommand As ICommand

        Public Sub New()

            CopiarTotalCommand = New RelayCommand(AddressOf CopiarTotal)
            GuardarCommand = New RelayCommand(AddressOf Guardar)
            VerDetalleCommand = New RelayCommand(Of Fotocopia)(AddressOf VerDetalle)
            EditarCommand = New RelayCommand(Of Fotocopia)(AddressOf Editar)
            AnularCommand = New RelayCommand(Of Fotocopia)(AddressOf Anular)

            _valores = _repo.ObtenerValores()

            PrecioNormal = ObtenerValor("1 - 100")
            PrecioEmpleado = ObtenerValor("Empleado")
            PrecioAnillado = ObtenerValor("Anillado")

            PrecioPagina = PrecioNormal
            Fecha = Date.Today

        End Sub

        '==================== DATOS ====================

        Private _nombre As String
        Private _comentario As String
        Private _paginas As Integer?
        Private _anillados As Integer?
        Private _fecha As DateTime

        Public Property Nombre As String
            Get
                Return _nombre
            End Get
            Set(value As String)
                _nombre = value
                Avisar(NameOf(Nombre))
                Avisar(NameOf(NombreTieneError))
                Avisar(NameOf(NombreErrorText))
            End Set
        End Property

        Public Property Comentario As String
            Get
                Return _comentario
            End Get
            Set(value As String)
                _comentario = value
                Avisar(NameOf(Comentario))
            End Set
        End Property

        Public Property Paginas As Integer?
            Get
                Return _paginas
            End Get
            Set(value As Integer?)

                If value.HasValue AndAlso (value < 0 OrElse value > 5000) Then Exit Property

                _paginas = value

                If value.HasValue AndAlso value > 0 Then
                    PrecioPagina = ObtenerPrecioPorCantidad(value.Value)
                End If

                Avisar(NameOf(Paginas))
                Avisar(NameOf(PrecioPagina))
                Avisar(NameOf(TotalPaginas))
                Avisar(NameOf(Total))
                Avisar(NameOf(HelperPrecioPagina))
            End Set
        End Property

        Public Property Anillados As Integer?
            Get
                Return _anillados
            End Get
            Set(value As Integer?)
                _anillados = value
                Avisar(NameOf(Anillados))
                Avisar(NameOf(TotalAnillados))
                Avisar(NameOf(Total))
            End Set
        End Property

        Public Property Fecha As DateTime
            Get
                Return _fecha
            End Get
            Set(value As DateTime)
                _fecha = value
                Avisar(NameOf(Fecha))
            End Set
        End Property

        '==================== PRECIOS ====================

        Private _precioPagina As Integer
        Private _precioNormal As Integer
        Private _precioEmpleado As Integer
        Private _precioAnillado As Integer

        Public Property PrecioPagina As Integer
            Get
                Return _precioPagina
            End Get
            Set(value As Integer)
                _precioPagina = value
                Avisar(NameOf(PrecioPagina))
                Avisar(NameOf(TotalPaginas))
                Avisar(NameOf(Total))
            End Set
        End Property

        Public Property PrecioNormal As Integer
            Get
                Return _precioNormal
            End Get
            Private Set(value As Integer)
                _precioNormal = value
            End Set
        End Property

        Public Property PrecioEmpleado As Integer
            Get
                Return _precioEmpleado
            End Get
            Private Set(value As Integer)
                _precioEmpleado = value
            End Set
        End Property

        Public Property PrecioAnillado As Integer
            Get
                Return _precioAnillado
            End Get
            Private Set(value As Integer)
                _precioAnillado = value
            End Set
        End Property

        '==================== EMPLEADO ====================

        Private _esEmpleado As Boolean

        Public Property EsEmpleado As Boolean
            Get
                Return _esEmpleado
            End Get
            Set(value As Boolean)
                _esEmpleado = value
                Avisar(NameOf(EsEmpleado))
                Avisar(NameOf(HelperPrecioPagina))

                If Paginas.HasValue AndAlso Paginas > 0 Then
                    PrecioPagina = ObtenerPrecioPorCantidad(Paginas.Value)
                End If
            End Set
        End Property

        '==================== TOTALES ====================

        Public ReadOnly Property TotalPaginas As Integer
            Get
                Return If(Paginas, 0) * PrecioPagina
            End Get
        End Property

        Public ReadOnly Property TotalAnillados As Integer
            Get
                Return If(Anillados, 0) * PrecioAnillado
            End Get
        End Property

        Public ReadOnly Property Total As Integer
            Get
                Return TotalPaginas + TotalAnillados
            End Get
        End Property

        '==================== PAGOS ====================

        Private _efectivo As Integer?
        Private _transferencia As Integer?

        Public Property Efectivo As Integer?
            Get
                Return _efectivo
            End Get
            Set(value As Integer?)
                _efectivo = value
                Avisar(NameOf(Efectivo))
            End Set
        End Property

        Public Property Transferencia As Integer?
            Get
                Return _transferencia
            End Get
            Set(value As Integer?)
                _transferencia = value
                Avisar(NameOf(Transferencia))
            End Set
        End Property

        Public Sub PagarConEfectivo()
            Efectivo = Total
            Transferencia = 0
        End Sub

        Public Sub PagarConTransferencia()
            Transferencia = Total
            Efectivo = 0
        End Sub

        '==================== VALIDACIONES ====================

        Private _mostrarErrores As Boolean

        Public Property MostrarErrores As Boolean
            Get
                Return _mostrarErrores
            End Get
            Set(value As Boolean)
                _mostrarErrores = value
                Avisar(NameOf(NombreTieneError))
                Avisar(NameOf(NombreErrorText))
            End Set
        End Property

        Public ReadOnly Property NombreTieneError As Boolean
            Get
                If Not MostrarErrores Then Return False
                Return String.IsNullOrWhiteSpace(Nombre)
            End Get
        End Property

        Public ReadOnly Property NombreErrorText As String
            Get
                If NombreTieneError Then Return "Ingrese un nombre."
                Return String.Empty
            End Get
        End Property

        '==================== HELPERS ====================

        Public ReadOnly Property HelperPrecioPagina As String
            Get
                If Not Paginas.HasValue OrElse Paginas = 0 Then Return ""

                If EsEmpleado Then Return $"${PrecioEmpleado}"

                Return $"${ObtenerPrecioPorCantidad(Paginas.Value)}"
            End Get
        End Property

        '==================== MÉTODOS PRIVADOS ====================

        Private Function ObtenerValor(descripcion As String) As Integer
            Return CInt(_valores.First(Function(v) v.Descripcion = descripcion).Valor)
        End Function

        Private Function ObtenerPrecioPorCantidad(paginas As Integer) As Integer

            If EsEmpleado Then Return PrecioEmpleado

            If paginas <= 100 Then Return ObtenerValor("1 - 100")
            If paginas <= 400 Then Return ObtenerValor("101 - 400")
            If paginas <= 700 Then Return ObtenerValor("401 - 700")

            Return ObtenerValor("+700")

        End Function

        Private Sub CopiarTotal()
            MiNumero = Total
        End Sub

        Private Sub Guardar()

            MostrarErrores = True
            If NombreTieneError Then Exit Sub

            _fotocopiasRepo.GuardarFotocopia(Me)

            MessageBox.Show("Guardado correctamente")
            LimpiarFormulario()

        End Sub

        Private Sub LimpiarFormulario()

            Nombre = ""
            Comentario = ""

            Paginas = Nothing
            Anillados = Nothing

            PrecioPagina = PrecioNormal

            Efectivo = Nothing
            Transferencia = Nothing

            EsEmpleado = False
            Fecha = Date.Today

            Avisar(NameOf(Total))
            Avisar(NameOf(HelperPrecioPagina))

        End Sub

        '==================== OTROS ====================

        Private _miNumero As Integer?

        Public Property MiNumero As Integer?
            Get
                Return _miNumero
            End Get
            Set(value As Integer?)
                _miNumero = value
                Avisar(NameOf(MiNumero))
            End Set
        End Property

        Public Property VerDetalleCommand As ICommand
        Public Property EditarCommand As ICommand
        Public Property AnularCommand As ICommand

        Private Sub VerDetalle(f As Fotocopia)
            ' abrir modal o view detalle
        End Sub

        Private Sub Editar(f As Fotocopia)
            ' navegar a edición
        End Sub

        Private Sub Anular(f As Fotocopia)
            ' cambiar estado en BD
        End Sub


    End Class

End Namespace
