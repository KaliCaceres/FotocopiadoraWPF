Imports System.ComponentModel
Imports System.Windows.Input

Namespace ViewModels

    Public Class PreciosViewModel
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Private Sub Avisar(nombre As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(nombre))
        End Sub

        '==================== REPOSITORIO ====================

        Private ReadOnly _repo As New ValoresRepository()
        Private _valores As List(Of ValorConfiguracion)
        Private _valoresOriginales As List(Of ValorConfiguracion)

        '==================== CONSTRUCTOR ====================

        Public Sub New()

            BotonPrincipalCommand =
        New RelayCommand(AddressOf EjecutarBotonPrincipal,
                         AddressOf PuedeEjecutarBotonPrincipal)

            CancelarCommand = New RelayCommand(AddressOf Cancelar)

            Inicializar()
        End Sub


        Public Sub Inicializar()
            Try
                _valores = _repo.ObtenerValores()

                If _valores Is Nothing Then
                    _valores = New List(Of ValorConfiguracion)
                End If

                _valoresOriginales = _valores.
            Select(Function(v) New ValorConfiguracion With {
                .Descripcion = v.Descripcion,
                .Valor = v.Valor
            }).ToList()

                CargarValores()

            Catch ex As Exception
                MessageBox.Show(
            "Error cargando precios:" & vbCrLf & ex.Message,
            "Error",
            MessageBoxButton.OK,
            MessageBoxImage.Error)
            End Try
        End Sub



        '==================== ESTADO UI ====================

        Private _inputsHabilitados As Boolean = False
        Public Property InputsHabilitados As Boolean
            Get
                Return _inputsHabilitados
            End Get
            Set(value As Boolean)
                If _inputsHabilitados = value Then Return

                _inputsHabilitados = value
                Avisar(NameOf(InputsHabilitados))
                Avisar(NameOf(TextoBotonPrincipal))
                Avisar(NameOf(MostrarCancelar))

                ActualizarCanExecute()
            End Set
        End Property

        Public ReadOnly Property TextoBotonPrincipal As String
            Get
                Return If(InputsHabilitados, "Guardar", "Modificar")
            End Get
        End Property

        Public ReadOnly Property MostrarCancelar As Boolean
            Get
                Return InputsHabilitados
            End Get
        End Property

        '==================== CAMBIOS ====================

        Private Function ObtenerValorOriginal(descripcion As String) As Decimal
            Return _valoresOriginales.
            First(Function(v) v.Descripcion = descripcion).
            Valor
        End Function


        Public ReadOnly Property HayCambios As Boolean
            Get
                Return Anillado <> ObtenerValorOriginal("Anillado") _
                OrElse Empleado <> ObtenerValorOriginal("Empleado") _
                OrElse Precio1a100 <> ObtenerValorOriginal("1 - 100") _
                OrElse Precio101a400 <> ObtenerValorOriginal("101 - 400") _
                OrElse Precio401a700 <> ObtenerValorOriginal("401 - 700") _
                OrElse PrecioMas700 <> ObtenerValorOriginal("+700")
            End Get
        End Property



        '==================== PROPIEDADES INPUT ====================

        Private _anillado As Decimal
        Public Property Anillado As Decimal
            Get
                Return _anillado
            End Get
            Set(value As Decimal)
                If _anillado = value Then Return
                _anillado = value
                Avisar(NameOf(Anillado))
                Avisar(NameOf(HayCambios))
                ActualizarCanExecute()
            End Set
        End Property

        Private _empleado As Decimal
        Public Property Empleado As Decimal
            Get
                Return _empleado
            End Get
            Set(value As Decimal)
                If _empleado = value Then Return
                _empleado = value
                Avisar(NameOf(Empleado))
                Avisar(NameOf(HayCambios))
                ActualizarCanExecute()
            End Set
        End Property

        Private _precio1a100 As Decimal
        Public Property Precio1a100 As Decimal
            Get
                Return _precio1a100
            End Get
            Set(value As Decimal)
                If _precio1a100 = value Then Return
                _precio1a100 = value
                Avisar(NameOf(Precio1a100))
                Avisar(NameOf(HayCambios))
                ActualizarCanExecute()
            End Set
        End Property

        Private _precio101a400 As Decimal
        Public Property Precio101a400 As Decimal
            Get
                Return _precio101a400
            End Get
            Set(value As Decimal)
                If _precio101a400 = value Then Return
                _precio101a400 = value
                Avisar(NameOf(Precio101a400))
                Avisar(NameOf(HayCambios))
                ActualizarCanExecute()
            End Set
        End Property

        Private _precio401a700 As Decimal
        Public Property Precio401a700 As Decimal
            Get
                Return _precio401a700
            End Get
            Set(value As Decimal)
                If _precio401a700 = value Then Return
                _precio401a700 = value
                Avisar(NameOf(Precio401a700))
                Avisar(NameOf(HayCambios))
                ActualizarCanExecute()
            End Set
        End Property

        Private _precioMas700 As Decimal
        Public Property PrecioMas700 As Decimal
            Get
                Return _precioMas700
            End Get
            Set(value As Decimal)
                If _precioMas700 = value Then Return
                _precioMas700 = value
                Avisar(NameOf(PrecioMas700))
                Avisar(NameOf(HayCambios))
                ActualizarCanExecute()
            End Set
        End Property

        '==================== CARGA ====================

        Private Sub CargarValores()
            Anillado = ObtenerValor("Anillado")
            Empleado = ObtenerValor("Empleado")
            Precio1a100 = ObtenerValor("1 - 100")
            Precio101a400 = ObtenerValor("101 - 400")
            Precio401a700 = ObtenerValor("401 - 700")
            PrecioMas700 = ObtenerValor("+700")
        End Sub

        Private Function ObtenerValor(descripcion As String) As Decimal
            Return _valores.First(Function(v) v.Descripcion = descripcion).Valor
        End Function

        '==================== COMMANDS ====================

        Public ReadOnly Property BotonPrincipalCommand As ICommand
        Public ReadOnly Property CancelarCommand As ICommand

        Private Sub EjecutarBotonPrincipal()
            If InputsHabilitados Then
                Guardar()
                InputsHabilitados = False
            Else
                InputsHabilitados = True
            End If
        End Sub

        Private Function PuedeEjecutarBotonPrincipal() As Boolean
            If InputsHabilitados Then
                Return HayCambios
            End If
            Return True
        End Function

        Private Sub Cancelar()
            InputsHabilitados = False
            CargarValores()
            Avisar(NameOf(HayCambios))
            ActualizarCanExecute()
        End Sub

        Private Sub Guardar()
            ActualizarValor("Anillado", Anillado)
            ActualizarValor("Empleado", Empleado)
            ActualizarValor("1 - 100", Precio1a100)
            ActualizarValor("101 - 400", Precio101a400)
            ActualizarValor("401 - 700", Precio401a700)
            ActualizarValor("+700", PrecioMas700)

            _repo.GuardarValores(_valores)

            _valoresOriginales = _valores.
                Select(Function(v) New ValorConfiguracion With {
                    .Descripcion = v.Descripcion,
                    .Valor = v.Valor
                }).ToList()

            Avisar(NameOf(HayCambios))
            ActualizarCanExecute()
        End Sub

        Private Sub ActualizarValor(desc As String, valor As Decimal)
            _valores.First(Function(v) v.Descripcion = desc).Valor = valor
        End Sub

        Private Sub ActualizarCanExecute()
            DirectCast(BotonPrincipalCommand, RelayCommand).
                RaiseCanExecuteChanged()
        End Sub

    End Class

End Namespace