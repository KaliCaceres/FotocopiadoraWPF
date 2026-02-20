Imports System.ComponentModel
Imports System.Windows.Input

Namespace ViewModels

    Public Class EditarMovimientoViewModel
        Implements INotifyPropertyChanged

        ' Movimiento editable (CLON)
        Public Property Movimiento As MovimientoCaja

        ' ================= COMBOS =================
        Public ReadOnly Property Tipos As List(Of String) =
            New List(Of String) From {"Ingreso", "Egreso"}

        Public ReadOnly Property MetodosPago As List(Of String) =
            New List(Of String) From {"Efectivo", "Transferencia"}

        Public ReadOnly Property Empleados As List(Of String) =
            New List(Of String) From {"Juanchi", "Seba", "Paulina"}

        ' ================= COMMAND =================
        Public ReadOnly Property GuardarCommand As ICommand

        Public Sub New(original As MovimientoCaja)

            ' 🔥 CLONAMOS
            Movimiento = New MovimientoCaja With {
                .IdMovimiento = original.IdMovimiento,
                .IdResumen = original.IdResumen,
                .Fecha = original.Fecha,
                .Tipo = original.Tipo,
                .MetodoPago = original.MetodoPago,
                .Monto = original.Monto,
                .Motivo = original.Motivo,
                .observacion = original.observacion,
                .Empleado = original.Empleado
            }

            GuardarCommand = New RelayCommand(AddressOf Guardar)

        End Sub

        ' Se dispara desde la ventana
        Public Event SolicitarCerrar As Action(Of Boolean)

        Private Sub Guardar()

            If String.IsNullOrWhiteSpace(Movimiento.Motivo) _
               OrElse Movimiento.Monto <= 0 Then

                MessageBox.Show("Datos inválidos",
                                "Validación",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning)
                Exit Sub
            End If

            ' Notificamos a la ventana que cierre en TRUE
            RaiseEvent SolicitarCerrar(True)

        End Sub

        ' ================= INotify =================
        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

    End Class

End Namespace