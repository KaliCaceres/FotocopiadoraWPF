Imports System.ComponentModel
Imports System.Windows.Input
Imports System.Windows

Namespace ViewModels


    Public Class FotocopiasViewModel
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Private Sub Avisar(nombre As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(nombre))
        End Sub

        '==================== REPOS ====================

        Private ReadOnly _repo As New ValoresRepository()
        Private ReadOnly _fotocopiasRepo As New FotocopiasRepository()
        Private _valores As List(Of ValorConfiguracion)

        '==================== ESTADO ====================

        Public Property EsEdicion As Boolean

        '==================== COMANDOS ====================
        Public Property PagarConEfectivoCommand As ICommand
        Public Property PagarConTransferenciaCommand As ICommand

        Public Property CopiarTotalCommand As ICommand
        Public Property GuardarCommand As ICommand

        '==================== ENTIDAD ====================

        Public Property Fotocopia As Fotocopia

        '==================== CONSTRUCTORES ====================

        ' ALTA
        Public Sub New()
            Inicializar()

            Fotocopia = New Fotocopia With {
                .Fecha = Date.Today
            }

            EsEdicion = False
        End Sub

        ' EDICIÓN
        Public Sub New(f As Fotocopia)
            Inicializar()

            Fotocopia = f
            EsEdicion = True


            RecalcularTodo()
            Avisar(NameOf(Efectivo))
            Avisar(NameOf(Transferencia))
            Avisar(NameOf(Fecha))

        End Sub

        Private Sub PagarConEfectivo()
            Fotocopia.Efectivo = Total
            Fotocopia.Transferencia = 0
            Avisar(NameOf(Efectivo))
            Avisar(NameOf(Transferencia))
        End Sub

        Private Sub PagarConTransferencia()
            Fotocopia.Transferencia = Total
            Fotocopia.Efectivo = 0
            Avisar(NameOf(Transferencia))
            Avisar(NameOf(Efectivo))
        End Sub


        Private Sub Inicializar()

            CopiarTotalCommand = New RelayCommand(AddressOf CopiarTotal)
            GuardarCommand = New RelayCommand(AddressOf Guardar)
            PagarConEfectivoCommand = New RelayCommand(AddressOf PagarConEfectivo)
            PagarConTransferenciaCommand = New RelayCommand(AddressOf PagarConTransferencia)

            _valores = _repo.ObtenerValores()

            PrecioNormal = ObtenerValor("1 - 100")
            PrecioEmpleado = ObtenerValor("Empleado")
            PrecioAnillado = ObtenerValor("Anillado")

            PrecioPagina = PrecioNormal
        End Sub

        '==================== PROXY A Fotocopia ====================

        Public Property Nombre As String
            Get
                Return Fotocopia.Nombre
            End Get
            Set(value As String)
                Fotocopia.Nombre = value
                Avisar(NameOf(Nombre))
                Avisar(NameOf(NombreTieneError))
                Avisar(NameOf(NombreErrorText))
            End Set
        End Property

        Public Property Comentario As String
            Get
                Return Fotocopia.Comentario
            End Get
            Set(value As String)
                Fotocopia.Comentario = value
                Avisar(NameOf(Comentario))
            End Set
        End Property

        Public Property Paginas As Integer?
            Get
                Return If(Fotocopia.Paginas = 0, CType(Nothing, Integer?), Fotocopia.Paginas)
            End Get
            Set(value As Integer?)
                Fotocopia.Paginas = If(value, 0)

                PrecioPagina = If(Fotocopia.Paginas > 0,
                                  ObtenerPrecioPorCantidad(Fotocopia.Paginas),
                                  PrecioNormal)

                RecalcularTodo()
            End Set
        End Property

        Public Property Anillados As Integer?
            Get
                Return If(Fotocopia.Anillados = 0, CType(Nothing, Integer?), Fotocopia.Anillados)
            End Get
            Set(value As Integer?)
                Fotocopia.Anillados = If(value, 0)
                RecalcularTodo()
            End Set
        End Property

        Public Property Fecha As Date
            Get
                Return Fotocopia.Fecha
            End Get
            Set(value As Date)
                Fotocopia.Fecha = value
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
                PrecioPagina = ObtenerPrecioPorCantidad(Fotocopia.Paginas)
                RecalcularTodo()
            End Set

        End Property

        '==================== TOTALES ====================

        Public ReadOnly Property TotalPaginas As Integer
            Get
                Return Fotocopia.Paginas * PrecioPagina
            End Get
        End Property

        Public ReadOnly Property TotalAnillados As Integer
            Get
                Return Fotocopia.Anillados * PrecioAnillado
            End Get
        End Property

        Public ReadOnly Property Total As Integer
            Get
                Return TotalPaginas + TotalAnillados
            End Get
        End Property

        '==================== PAGOS ====================

        Public Property Efectivo As Integer?
            Get
                Return If(Fotocopia.Efectivo = 0, CType(Nothing, Integer?), Fotocopia.Efectivo)
            End Get
            Set(value As Integer?)
                Fotocopia.Efectivo = If(value, 0)
                Avisar(NameOf(Efectivo))
            End Set
        End Property

        Public Property Transferencia As Integer?
            Get
                Return If(Fotocopia.Transferencia = 0, CType(Nothing, Integer?), Fotocopia.Transferencia)
            End Get
            Set(value As Integer?)
                Fotocopia.Transferencia = If(value, 0)
                Avisar(NameOf(Transferencia))
            End Set
        End Property

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
                If Fotocopia.Paginas <= 0 Then Return ""
                If EsEmpleado Then Return $"${PrecioEmpleado}"
                Return $"${ObtenerPrecioPorCantidad(Fotocopia.Paginas)}"
            End Get
        End Property

        '==================== MÉTODOS ====================

        Private Sub RecalcularTodo()
            Avisar(NameOf(Paginas))
            Avisar(NameOf(Anillados))
            Avisar(NameOf(TotalPaginas))
            Avisar(NameOf(TotalAnillados))
            Avisar(NameOf(Total))
            Avisar(NameOf(HelperPrecioPagina))
        End Sub

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
            Fotocopia.Efectivo = Total
            Fotocopia.Transferencia = 0
            Avisar(NameOf(Efectivo))
            Avisar(NameOf(Transferencia))
        End Sub

        Private Sub Guardar()

            MostrarErrores = True
            If NombreTieneError Then Exit Sub

            Fotocopia.PrecioUnitario = PrecioPagina
            Fotocopia.PrecioTotal = Total

            If EsEdicion Then
                _fotocopiasRepo.ActualizarFotocopia(Fotocopia)
            Else
                _fotocopiasRepo.GuardarFotocopia(Fotocopia, PrecioPagina)
            End If

            MessageBox.Show("Guardado correctamente")
        End Sub

    End Class

End Namespace
