Imports System.ComponentModel
Imports System.Windows.Input
Imports FotocopiadoraWPF.Services

Namespace ViewModels

    Public Class BalanceViewModel
        Implements INotifyPropertyChanged

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Private Sub Avisar(nombre As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(nombre))
        End Sub

        Private ReadOnly _repo As New BalanceRepository()
        Private _ultimoBalance As BalanceEntity

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
                Avisar(NameOf(TextoBotonPrincipal))
                Avisar(NameOf(MostrarCancelar))
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

        ' ==================== COMMANDS ====================

        Public ReadOnly Property BotonPrincipalCommand As ICommand
        Public ReadOnly Property CancelarCommand As ICommand

        Public Sub New()
            BotonPrincipalCommand = New RelayCommand(AddressOf EjecutarBotonPrincipal)
            CancelarCommand = New RelayCommand(AddressOf Cancelar)

            InputsHabilitados = False
            CargarValoresIniciales()
        End Sub

        ' ==================== ACCIONES ====================

        Private Sub EjecutarBotonPrincipal()
            If InputsHabilitados Then
                Guardar()
            Else
                InputsHabilitados = True
            End If
        End Sub

        Private Sub Cancelar()
            InputsHabilitados = False
            CargarValoresIniciales()
        End Sub

        ' ==================== VALORES FINALES ====================

        Public Property ContadorEquipo1Final As Integer
        Public Property ContadorEquipo2Final As Integer
        Public Property EfectivoFinal As Decimal
        Public Property TransferenciaFinal As Decimal

        ' ==================== VALORES INICIALES ====================

        Public Property ContadorEquipo1Inicio As Integer
        Public Property ContadorEquipo2Inicio As Integer
        Public Property EfectivoInicio As Decimal
        Public Property TransferenciaInicio As Decimal

        ' ==================== DETALLE ====================

        Private _anio As Integer = Date.Now.Year
        Public Property Anio As Integer
            Get
                Return _anio
            End Get
            Set(value As Integer)
                If _anio = value Then Return
                _anio = value
                Avisar(NameOf(Anio))
            End Set
        End Property

        Private _mesSeleccionado As Integer = Date.Now.Month
        Public Property MesSeleccionado As Integer
            Get
                Return _mesSeleccionado
            End Get
            Set(value As Integer)
                If _mesSeleccionado = value Then Return
                _mesSeleccionado = value
                Avisar(NameOf(MesSeleccionado))
            End Set
        End Property

        ' ==================== GUARDAR ====================

        Private Sub Guardar()

            Dim balanceActual = ConstruirBalanceActual()

            ' 🔹 Guardar en BD (entity)
            Dim entity As New BalanceEntity With {
                .ContadorEquipo1 = balanceActual.ContadorEquipo1Final,
                .ContadorEquipo2 = balanceActual.ContadorEquipo2Final,
                .Efectivo = balanceActual.EfectivoFinal,
                .Transferencia = balanceActual.TransferenciaFinal,
                .Fecha = balanceActual.FechaFin,
                .Anio = balanceActual.Anio,
                .IdMes = balanceActual.IdMes
            }

            Dim idResumen = _repo.GuardarBalance(entity)

            BalanceActualService.BalanceActualId = idResumen


            ' 🔹 PDF
            BalancePdfGenerator.GenerarYMostrar(balanceActual)

            ' 🔹 Final → Inicio
            ContadorEquipo1Inicio = ContadorEquipo1Final
            ContadorEquipo2Inicio = ContadorEquipo2Final
            EfectivoInicio = EfectivoFinal
            TransferenciaInicio = TransferenciaFinal

            ' 🔹 Reset finales
            ContadorEquipo1Final = 0
            ContadorEquipo2Final = 0
            EfectivoFinal = 0
            TransferenciaFinal = 0

            entity.IdResumen = idResumen
            _ultimoBalance = entity


            InputsHabilitados = False
        End Sub

        ' ==================== CARGA INICIAL ====================

        Private Sub CargarValoresIniciales()
            _ultimoBalance = _repo.ObtenerUltimoBalance()

            If _ultimoBalance IsNot Nothing Then
                BalanceActualService.BalanceActualId = _ultimoBalance.IdResumen
            End If


            If _ultimoBalance Is Nothing Then
                ContadorEquipo1Inicio = 0
                ContadorEquipo2Inicio = 0
                EfectivoInicio = 0
                TransferenciaInicio = 0
            Else
                ContadorEquipo1Inicio = _ultimoBalance.ContadorEquipo1
                ContadorEquipo2Inicio = _ultimoBalance.ContadorEquipo2
                EfectivoInicio = _ultimoBalance.Efectivo
                TransferenciaInicio = _ultimoBalance.Transferencia
            End If

            Avisar(NameOf(ContadorEquipo1Inicio))
            Avisar(NameOf(ContadorEquipo2Inicio))
            Avisar(NameOf(EfectivoInicio))
            Avisar(NameOf(TransferenciaInicio))
        End Sub

        ' ==================== BALANCE OPERATIVO ====================

        Private Function ConstruirBalanceActual() As Balance
            Dim fechaInicio As Date =
        If(_ultimoBalance Is Nothing,
           Date.Now,
           _ultimoBalance.Fecha)

            Return New Balance With {
        .FechaInicio = fechaInicio,
        .FechaFin = Date.Now,
        .ContadorEquipo1Inicio = ContadorEquipo1Inicio,
        .ContadorEquipo1Final = ContadorEquipo1Final,
        .ContadorEquipo2Inicio = ContadorEquipo2Inicio,
        .ContadorEquipo2Final = ContadorEquipo2Final,
        .EfectivoInicio = EfectivoInicio,
        .EfectivoFinal = EfectivoFinal,
        .TransferenciaInicio = TransferenciaInicio,
        .TransferenciaFinal = TransferenciaFinal,
        .IdMes = MesSeleccionado,
        .Anio = Anio
    }
        End Function


    End Class
End Namespace
