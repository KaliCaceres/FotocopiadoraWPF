Imports System.ComponentModel
Imports System.Windows.Input
Imports FotocopiadoraWPF.Services

Namespace ViewModels

    Public Class BalanceViewModel
        Implements INotifyPropertyChanged

        ' ==================== INotify ====================

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Private Sub Avisar(nombre As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(nombre))
        End Sub

        ' ==================== REPOSITORIOS ====================

        Private ReadOnly _repo As New BalanceRepository()
        Private ReadOnly _repoFotocopias As New FotocopiasRepository()
        Private ReadOnly _repoMovimientos As New MovimientosCajaRepository()

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

        ' ==================== VALORES INICIALES ====================

        Public Property ContadorEquipo1Inicio As Integer
        Public Property ContadorEquipo2Inicio As Integer
        Public Property EfectivoInicio As Decimal
        Public Property TransferenciaInicio As Decimal

        ' ==================== VALORES FINALES ====================

        Public Property ContadorEquipo1Final As Integer
        Public Property ContadorEquipo2Final As Integer
        Public Property EfectivoFinal As Decimal
        Public Property TransferenciaFinal As Decimal



        ' ==================== CONTADORES ====================

        Public ReadOnly Property ContadorEquipo1Diferencia As Integer
            Get
                Return ContadorEquipo1Final - ContadorEquipo1Inicio
            End Get
        End Property

        Public ReadOnly Property ContadorEquipo2Diferencia As Integer
            Get
                Return ContadorEquipo2Final - ContadorEquipo2Inicio
            End Get
        End Property

        Public ReadOnly Property TotalContadores As Integer
            Get
                Return ContadorEquipo1Diferencia + ContadorEquipo2Diferencia
            End Get
        End Property

        ' ==================== CAJA ====================

        '=========================================================================================

        'Suma la columna transferencias de las fotocopias con estado pagado (0) del balance actual
        Public ReadOnly Property TotalTransferencia As Decimal
            Get
                If BalanceActualService.BalanceActualId <= 0 Then Return 0

                Return _repoFotocopias.ObtenerTotales(
            BalanceActualService.BalanceActualId,
            0,
            "transferencia")
            End Get
        End Property

        'Suma la columna efectivo de las fotocopias con estado pagado (0) del balance actual
        Public ReadOnly Property TotalEfectivo As Decimal
            Get
                If BalanceActualService.BalanceActualId <= 0 Then Return 0

                Return _repoFotocopias.ObtenerTotales(
            BalanceActualService.BalanceActualId,
            0,
            "efectivo")
            End Get
        End Property

        'con TotalTransferencia y TotalEfectivo obtenemos el total cobrado en el mes en fotocpias

        '=========================================================================================


        '=========================================================================================
        '   MOVIMIENTOS DE CAJA: listas de movimientos, totales por tipo y método de pago para el balance actual
        '=========================================================================================

        'Lista de movimientos de caja de tipo "Egreso" del balance actual
        Public ReadOnly Property MovimientosCajaEgreso As List(Of MovimientoCaja)
            Get
                If BalanceActualService.BalanceActualId <= 0 Then
                    Return New List(Of MovimientoCaja)
                End If

                Return _repoMovimientos.ObtenerPorTipo(
                    BalanceActualService.BalanceActualId, "Egreso")
            End Get
        End Property

        'Lista de movimientos de caja de tipo "Ingreso" del balance actual
        Public ReadOnly Property MovimientosCajaIngreso As List(Of MovimientoCaja)
            Get
                If BalanceActualService.BalanceActualId <= 0 Then
                    Return New List(Of MovimientoCaja)
                End If

                Return _repoMovimientos.ObtenerPorTipo(
                    BalanceActualService.BalanceActualId, "Ingreso")
            End Get
        End Property

        'Calcula el total de ingresos por efectivo del balance actual
        Public ReadOnly Property TotalIngresoEfectivo As Decimal
            Get
                If BalanceActualService.BalanceActualId <= 0 Then Return 0
                Return _repoMovimientos.ObtenerTotales(
                    BalanceActualService.BalanceActualId, "Ingreso", "Efectivo")
            End Get
        End Property

        'Calcula el total de egresos por efectivo del balance actual
        Public ReadOnly Property TotalEgresoEfectivo As Decimal
            Get
                If BalanceActualService.BalanceActualId <= 0 Then Return 0
                Return _repoMovimientos.ObtenerTotales(
                    BalanceActualService.BalanceActualId, "Egreso", "Efectivo")
            End Get
        End Property

        'Calcula el total de ingresos por transferencia del balance actual
        Public ReadOnly Property TotalIngresoTransferencia As Decimal
            Get
                If BalanceActualService.BalanceActualId <= 0 Then Return 0
                Return _repoMovimientos.ObtenerTotales(
                    BalanceActualService.BalanceActualId, "Ingreso", "Transferencia")
            End Get
        End Property

        'Calcula el total de egresos por transferencia del balance actual
        Public ReadOnly Property TotalEgresoTransferencia As Decimal
            Get
                If BalanceActualService.BalanceActualId <= 0 Then Return 0
                Return _repoMovimientos.ObtenerTotales(
                    BalanceActualService.BalanceActualId, "Egreso", "Transferencia")
            End Get
        End Property
        '=========================================================================================


        '=========================================================================================
        '   CAJAS: subtotales por método de pago: es el dinero que tiene que haber al final del mes
        '=========================================================================================

        'TotalEfectivo: es lo que se facturó en efectivo por las fotocopias
        'EfectivoInicio: es el dinero en efectivo que había al inicio del mes (lo que quedó del mes anterior)
        'TotalEgresoEfectivo: es el dinero que salió en efectivo durante el mes (movimientos de caja)
        'TotalIngresoEfectivo: es el dinero que entró en efectivo durante el mes (movimientos de caja)
        Public ReadOnly Property EfectivoDiferencia As Decimal
            Get
                Return TotalEfectivo + EfectivoInicio - TotalEgresoEfectivo + TotalIngresoEfectivo
            End Get
        End Property

        'TotalTransferencia: es lo que se facturó por transferencia por las fotocopias
        'TransferenciaInicio: es el dinero por transferencia que había al inicio del mes (lo que quedó del mes anterior)
        'TotalEgresoTransferencia: es el dinero que salió por transferencia durante el mes (movimientos de caja)
        'TotalIngresoTransferencia: es el dinero que entró por transferencia durante el mes (movimientos de caja)
        Public ReadOnly Property TransferenciaDiferencia As Decimal
            Get
                Return TotalTransferencia + TransferenciaInicio - TotalEgresoTransferencia + TotalIngresoTransferencia
            End Get
        End Property

        'TotalCaja: es la suma de lo que se facturó en efectivo y por transferencia por las fotocopias
        Public ReadOnly Property TotalCaja As Decimal
            Get
                Return TransferenciaDiferencia + EfectivoDiferencia
            End Get
        End Property
        '=========================================================================================


        '=========================================================================================
        '   FOTOCOPIAS: Subtotales por tipo del balance actual
        '=========================================================================================

        Public ReadOnly Property TotalPagadas As Integer
            Get
                If BalanceActualService.BalanceActualId <= 0 Then Return 0
                Return _repoFotocopias.ObtenerTotalPaginasPorEstado(
                    BalanceActualService.BalanceActualId, 0)
            End Get
        End Property
        Public ReadOnly Property TotalDeudor As Integer
            Get
                If BalanceActualService.BalanceActualId <= 0 Then Return 0
                Return _repoFotocopias.ObtenerTotalPaginasPorEstado(
                    BalanceActualService.BalanceActualId, 1)
            End Get
        End Property
        Public ReadOnly Property TotalPerdida As Integer
            Get
                If BalanceActualService.BalanceActualId <= 0 Then Return 0
                Return _repoFotocopias.ObtenerTotalPaginasPorEstado(
                    BalanceActualService.BalanceActualId, 2)
            End Get
        End Property
        Public ReadOnly Property TotalPendientes As Integer
            Get
                If BalanceActualService.BalanceActualId <= 0 Then Return 0
                Return _repoFotocopias.ObtenerTotalPaginasPorEstado(
                    BalanceActualService.BalanceActualId, 4)
            End Get
        End Property

        'Registradas: es el total de paginas registradas en el sistema
        Public ReadOnly Property Registradas As Integer
            Get
                Return TotalPerdida + TotalDeudor + TotalPagadas + TotalPendientes
            End Get
        End Property

        'No registradas: es la diferencia entre el total real de paginas (TotalContadores) y las registradas en el sistema (Registradas)
        Public ReadOnly Property NoRegistradas As Integer
            Get
                Return TotalContadores - Registradas
            End Get
        End Property
        '=========================================================================================




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

            Dim entity As New BalanceEntity With {
                .ContadorEquipo1 = balanceActual.ContadorEquipo1Final,
                .ContadorEquipo2 = balanceActual.ContadorEquipo2Final,
                .Efectivo = balanceActual.EfectivoDiferencia,
                .Transferencia = balanceActual.TransferenciaDiferencia,
                .Fecha = balanceActual.FechaFin,
                .Anio = balanceActual.Anio,
                .IdMes = balanceActual.IdMes
            }

            Dim idResumen = _repo.GuardarBalance(entity)
            BalanceActualService.BalanceActualId = idResumen

            BalancePdfGenerator.GenerarYMostrar(balanceActual)

            ContadorEquipo1Inicio = ContadorEquipo1Final
            ContadorEquipo2Inicio = ContadorEquipo2Final

            EfectivoInicio = EfectivoFinal
            TransferenciaInicio = TransferenciaFinal

            ContadorEquipo1Final = 0
            ContadorEquipo2Final = 0
            EfectivoFinal = 0
            TransferenciaFinal = 0

            entity.IdResumen = idResumen
            _ultimoBalance = entity

            InputsHabilitados = False


            Avisar(NameOf(TotalEgresoEfectivo))
            Avisar(NameOf(TotalIngresoEfectivo))
            Avisar(NameOf(TotalEgresoTransferencia))
            Avisar(NameOf(TotalIngresoTransferencia))
            Avisar(NameOf(TotalCaja))
            Avisar(NameOf(EfectivoDiferencia))
            Avisar(NameOf(TransferenciaDiferencia))
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

        ' ==================== CONSTRUIR BALANCE ====================

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
                .TotalEfectivo = TotalEfectivo,
                .TransferenciaInicio = TransferenciaInicio,
                .TotalTransferencia = TotalTransferencia,
                .IdMes = MesSeleccionado,
                .Anio = Anio,
                .ContadorEquipo1Diferencia = ContadorEquipo1Diferencia,
                .ContadorEquipo2Diferencia = ContadorEquipo2Diferencia,
                .TotalContadores = TotalContadores,
                .EfectivoDiferencia = EfectivoDiferencia,
                .TransferenciaDiferencia = TransferenciaDiferencia,
                .TotalCaja = TotalCaja,
                .TotalPerdida = TotalPerdida,
                .TotalDeudor = TotalDeudor,
                .TotalPagadas = TotalPagadas,
                .Registradas = Registradas,
                .NoRegistradas = NoRegistradas,
                .movimientosCajaEgreso = MovimientosCajaEgreso,
                .movimientosCajaIngreso = MovimientosCajaIngreso,
                .TotalEgresoTransferencia = TotalEgresoTransferencia,
                .TotalEgresoEfectivo = TotalEgresoEfectivo,
                .TotalIngresoTransferencia = TotalIngresoTransferencia,
                .TotalIngresoEfectivo = TotalIngresoEfectivo,
                .TotalPendientes = TotalPendientes
            }

        End Function

    End Class

End Namespace