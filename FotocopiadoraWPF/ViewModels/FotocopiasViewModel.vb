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

        Public Property CancelarCommand As ICommand

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
            TieneCambios = False

        End Sub
        Private ReadOnly _fotocopiaOriginal As Fotocopia

        ' EDICIÓN
        Public Sub New(f As Fotocopia)
            Inicializar()

            Fotocopia = f

            _fotocopiaOriginal = New Fotocopia With {
            .Nombre = f.Nombre,
            .Fecha = f.Fecha,
            .Paginas = f.Paginas,
            .Anillados = f.Anillados,
            .Transferencia = f.Transferencia,
            .Efectivo = f.Efectivo,
            .Comentario = f.Comentario
        }

            EsEdicion = True
            TieneCambios = False
            PrecioPagina = Fotocopia.PrecioUnitario
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
            PagarConEfectivoCommand = New RelayCommand(AddressOf PagarConEfectivo)
            PagarConTransferenciaCommand = New RelayCommand(AddressOf PagarConTransferencia)
            CancelarCommand = New RelayCommand(AddressOf Cancelar)
            GuardarCommand = New RelayCommand(AddressOf Guardar, Function() TieneCambios)

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
                If Not EsEdicion Then
                    TieneCambios = True
                Else
                    EvaluarCambios()
                End If

            End Set
        End Property

        Public Property Comentario As String
            Get
                Return Fotocopia.Comentario
            End Get
            Set(value As String)
                Fotocopia.Comentario = value
                Avisar(NameOf(Comentario))
                If Not EsEdicion Then
                    TieneCambios = True
                Else
                    EvaluarCambios()
                End If

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
                Avisar(NameOf(PrecioPagina))

                RecalcularTodo()
                If Not EsEdicion Then
                    TieneCambios = True
                Else
                    EvaluarCambios()
                End If


            End Set
        End Property

        Public Property Anillados As Integer?
            Get
                Return If(Fotocopia.Anillados = 0, CType(Nothing, Integer?), Fotocopia.Anillados)
            End Get
            Set(value As Integer?)
                Fotocopia.Anillados = If(value, 0)
                RecalcularTodo()
                If Not EsEdicion Then
                    TieneCambios = True
                Else
                    EvaluarCambios()
                End If

            End Set
        End Property

        Public Property Fecha As Date
            Get
                Return Fotocopia.Fecha
            End Get
            Set(value As Date)
                Fotocopia.Fecha = value
                Avisar(NameOf(Fecha))
                If Not EsEdicion Then
                    TieneCambios = True
                Else
                    EvaluarCambios()
                End If

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

        'Private Sub Guardar()
        '    Try
        '        ' recalcular antes de guardar
        '        Fotocopia.PrecioTotal =
        '    (Fotocopia.Paginas * Fotocopia.PrecioUnitario) +
        '    (Fotocopia.Anillados * ObtenerPrecioAnillado())

        '        _fotocopiasRepo.ActualizarFotocopia(Fotocopia)
        '        TieneCambios = False

        '        MessageBox.Show("Cambios guardados correctamente")
        '        Cerrar(True)

        '    Catch ex As Exception
        '        MessageBox.Show("ERROR al guardar: " & ex.Message)
        '    End Try
        'End Sub

        Private Sub Guardar()
            Try
                ' Recalcular precios SIEMPRE
                Fotocopia.PrecioUnitario = PrecioPagina
                Fotocopia.PrecioTotal =
            (Fotocopia.Paginas * PrecioPagina) +
            (Fotocopia.Anillados * PrecioAnillado)

                If EsEdicion Then
                    ' ====== EDICIÓN ======
                    If Fotocopia.IdFotocopia <= 0 Then
                        Throw New Exception("IdFotocopia inválido. No se puede actualizar.")
                    End If

                    _fotocopiasRepo.ActualizarFotocopia(Fotocopia)
                    MessageBox.Show("Cambios guardados correctamente")

                    TieneCambios = False
                    Cerrar(True)

                Else
                    ' ====== ALTA ======
                    _fotocopiasRepo.GuardarFotocopia(Fotocopia)
                    MessageBox.Show("Fotocopia guardada correctamente")

                    LimpiarFormulario()

                End If

            Catch ex As Exception
                MessageBox.Show("ERROR al guardar: " & ex.Message)
            End Try
        End Sub



        Public Property GuardadoConExito As Boolean


        Private Function ObtenerPrecioAnillado() As Integer
            Return 0 ' si luego lo manejás dinámico, acá va
        End Function


        Private Sub Cancelar()
            Cerrar(False)
        End Sub

        Private Sub Cerrar(resultado As Boolean)
            For Each w As Window In Application.Current.Windows
                If w.IsActive Then
                    w.DialogResult = resultado
                    Exit For
                End If
            Next
        End Sub

        Private _tieneCambios As Boolean

        Public Property TieneCambios As Boolean
            Get
                Return _tieneCambios
            End Get
            Set(value As Boolean)
                If _tieneCambios <> value Then
                    _tieneCambios = value
                    Avisar(NameOf(TieneCambios))
                    CType(GuardarCommand, RelayCommand).RaiseCanExecuteChanged()
                End If
            End Set
        End Property

        Private Sub EvaluarCambios()
            TieneCambios =
        Fotocopia.Nombre <> _fotocopiaOriginal.Nombre OrElse
        Fotocopia.Fecha <> _fotocopiaOriginal.Fecha OrElse
        Fotocopia.Paginas <> _fotocopiaOriginal.Paginas OrElse
        Fotocopia.Anillados <> _fotocopiaOriginal.Anillados OrElse
        Fotocopia.Transferencia <> _fotocopiaOriginal.Transferencia OrElse
        Fotocopia.Efectivo <> _fotocopiaOriginal.Efectivo OrElse
        Fotocopia.Comentario <> _fotocopiaOriginal.Comentario

            Avisar(NameOf(NombreModificado))
            Avisar(NameOf(FechaModificada))
            Avisar(NameOf(PaginasModificadas))
            Avisar(NameOf(AnilladosModificados))
            Avisar(NameOf(ComentarioModificado))
            Avisar(NameOf(EfectivoModificado))
            Avisar(NameOf(TransferenciaModificada))
        End Sub


        Private Sub LimpiarFormulario()

            Fotocopia = New Fotocopia With {
        .Fecha = Date.Today
    }

            ' Reset de flags
            EsEdicion = False
            TieneCambios = False
            EsEmpleado = False

            ' Reset de precios
            PrecioPagina = PrecioNormal

            ' Notificar TODO lo que usa la vista
            Avisar(NameOf(Fotocopia))
            Avisar(NameOf(Nombre))
            Avisar(NameOf(Comentario))
            Avisar(NameOf(Paginas))
            Avisar(NameOf(Anillados))
            Avisar(NameOf(Fecha))
            Avisar(NameOf(Efectivo))
            Avisar(NameOf(Transferencia))
            Avisar(NameOf(TotalPaginas))
            Avisar(NameOf(TotalAnillados))
            Avisar(NameOf(Total))
            Avisar(NameOf(HelperPrecioPagina))

            Avisar(NameOf(NombreTieneError))
            Avisar(NameOf(NombreErrorText))

        End Sub
        ' ================== CAMPOS MODIFICADOS ==================

        Public ReadOnly Property NombreModificado As Boolean
            Get
                Return EsEdicion AndAlso Fotocopia.Nombre <> _fotocopiaOriginal.Nombre
            End Get
        End Property

        Public ReadOnly Property FechaModificada As Boolean
            Get
                Return EsEdicion AndAlso Fotocopia.Fecha <> _fotocopiaOriginal.Fecha
            End Get
        End Property

        Public ReadOnly Property PaginasModificadas As Boolean
            Get
                Return EsEdicion AndAlso Fotocopia.Paginas <> _fotocopiaOriginal.Paginas
            End Get
        End Property

        Public ReadOnly Property AnilladosModificados As Boolean
            Get
                Return EsEdicion AndAlso Fotocopia.Anillados <> _fotocopiaOriginal.Anillados
            End Get
        End Property

        Public ReadOnly Property ComentarioModificado As Boolean
            Get
                Return EsEdicion AndAlso Fotocopia.Comentario <> _fotocopiaOriginal.Comentario
            End Get
        End Property

        Public ReadOnly Property EfectivoModificado As Boolean
            Get
                Return EsEdicion AndAlso Fotocopia.Efectivo <> _fotocopiaOriginal.Efectivo
            End Get
        End Property

        Public ReadOnly Property TransferenciaModificada As Boolean
            Get
                Return EsEdicion AndAlso Fotocopia.Transferencia <> _fotocopiaOriginal.Transferencia
            End Get
        End Property
    End Class
End Namespace
