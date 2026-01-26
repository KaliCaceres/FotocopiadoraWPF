Imports System.ComponentModel
Imports System.Windows.Input

Namespace ViewModels

    Public Class BalanceViewModel
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Private Sub Avisar(p As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(p))
        End Sub

        Private ReadOnly _repo As New BalanceRepository()


        ' ==================== ESTADO UI ====================

        Private _inputsHabilitados As Boolean = False
        Public Property InputsHabilitados As Boolean
            Get
                Return _inputsHabilitados
            End Get
            Set(value As Boolean)
                If _inputsHabilitados = value Then Return
                _inputsHabilitados = value

                Avisar(NameOf(InputsHabilitados))
                Avisar(NameOf(EnModoEdicion))
                Avisar(NameOf(TextoBotonPrincipal))
                Avisar(NameOf(MostrarCancelar))
            End Set
        End Property

        ' Alias claro para el XAML
        Public ReadOnly Property EnModoEdicion As Boolean
            Get
                Return InputsHabilitados
            End Get
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

        ' ==================== COMMANDS ====================

        Public ReadOnly Property BotonPrincipalCommand As ICommand
        Public ReadOnly Property CancelarCommand As ICommand

        Public Sub New()
            _inputsHabilitados = False

            Avisar(NameOf(TextoBotonPrincipal))
            Avisar(NameOf(MostrarCancelar))
            Avisar(NameOf(InputsHabilitados))

            BotonPrincipalCommand =
        New RelayCommand(AddressOf EjecutarBotonPrincipal)

            CancelarCommand =
        New RelayCommand(AddressOf Cancelar, Function() True)


            CargarValoresIniciales()
        End Sub



        ' ==================== ACCIONES ====================

        Private Sub EjecutarBotonPrincipal()
            If InputsHabilitados Then
                Guardar()
                InputsHabilitados = False
            Else
                InputsHabilitados = True
            End If
        End Sub

        Private Sub Cancelar()
            InputsHabilitados = False
            RestaurarValores()
        End Sub

        Private _contadorEquipo1Final As Integer
        Public Property ContadorEquipo1Final As Integer
            Get
                Return _contadorEquipo1Final
            End Get
            Set(value As Integer)
                If _contadorEquipo1Final = value Then Return
                _contadorEquipo1Final = value
                Avisar(NameOf(ContadorEquipo1Final))
            End Set
        End Property


        Private _contadorEquipo2Final As Integer
        Public Property ContadorEquipo2Final As Integer
            Get
                Return _contadorEquipo2Final
            End Get
            Set(value As Integer)
                If _contadorEquipo2Final = value Then Return
                _contadorEquipo2Final = value
                Avisar(NameOf(ContadorEquipo2Final))
            End Set
        End Property

        Private _efectivoFinal As Integer
        Public Property EfectivoFinal As Integer
            Get
                Return _efectivoFinal
            End Get
            Set(value As Integer)
                If _efectivoFinal = value Then Return
                _efectivoFinal = value
                Avisar(NameOf(EfectivoFinal))
            End Set
        End Property


        Private _transferenciaFinal As Integer
        Public Property TransferenciaFinal As Integer
            Get
                Return _transferenciaFinal
            End Get
            Set(value As Integer)
                If _transferenciaFinal = value Then Return
                _transferenciaFinal = value
                Avisar(NameOf(TransferenciaFinal))
            End Set
        End Property


        Private Sub Guardar()

            ' 1️⃣ Guardar balance
            Dim nuevo As New Balance With {
        .ContadorEquipo1 = ContadorEquipo1Final,
        .ContadorEquipo2 = ContadorEquipo2Final,
        .Efectivo = EfectivoFinal,
        .Transferencia = TransferenciaFinal,
        .Fecha = Date.Now
    }

            _repo.GuardarBalance(nuevo)

            ' 2️⃣ Final → Inicio
            ContadorEquipo1Inicio = ContadorEquipo1Final
            ContadorEquipo2Inicio = ContadorEquipo2Final
            EfectivoInicio = EfectivoFinal
            TransferenciaInicio = TransferenciaFinal

            ' 3️⃣ Resetear Final
            ContadorEquipo1Final = 0
            ContadorEquipo2Final = 0
            EfectivoFinal = 0
            TransferenciaFinal = 0

            ' 4️⃣ Notificar cambios
            Avisar(NameOf(ContadorEquipo1Inicio))
            Avisar(NameOf(ContadorEquipo2Inicio))
            Avisar(NameOf(EfectivoInicio))
            Avisar(NameOf(TransferenciaInicio))

            Avisar(NameOf(ContadorEquipo1Final))
            Avisar(NameOf(ContadorEquipo2Final))
            Avisar(NameOf(EfectivoFinal))
            Avisar(NameOf(TransferenciaFinal))

            ' 5️⃣ Salir de edición
            InputsHabilitados = False
        End Sub



        Private Sub RestaurarValores()
            ' TODO: Restaurar valores originales
            ' Se va a usar cuando conectes DB
        End Sub

        Public Property ContadorEquipo1Inicio As Integer
        Public Property ContadorEquipo2Inicio As Integer
        Public Property EfectivoInicio As Integer
        Public Property TransferenciaInicio As Integer

        Private Sub CargarValoresIniciales()
            Dim ultimo = _repo.ObtenerUltimoBalance()

            If ultimo Is Nothing Then
                ContadorEquipo1Inicio = 0
                ContadorEquipo2Inicio = 0
                EfectivoInicio = 0
                TransferenciaInicio = 0
            Else
                ContadorEquipo1Inicio = ultimo.ContadorEquipo1
                ContadorEquipo2Inicio = ultimo.ContadorEquipo2
                EfectivoInicio = ultimo.Efectivo
                TransferenciaInicio = ultimo.Transferencia
            End If

            Avisar(NameOf(ContadorEquipo1Inicio))
            Avisar(NameOf(ContadorEquipo2Inicio))
            Avisar(NameOf(EfectivoInicio))
            Avisar(NameOf(TransferenciaInicio))
        End Sub

    End Class
End Namespace
