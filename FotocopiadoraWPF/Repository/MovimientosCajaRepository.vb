Imports Microsoft.Data.Sqlite

Public Class MovimientosCajaRepository

    Public Sub Insertar(m As MovimientoCaja)
        Using cn As New SqliteConnection(Configuracion.ConnectionString)
            cn.Open()

            Dim sql As String =
            "INSERT INTO movimientos_caja
            (id_resumen, fecha, tipo, metodo_pago, monto, motivo, empleado)
            VALUES
            (@id_resumen, @fecha, @tipo, @metodo, @monto, @motivo, @empleado)"

            Using cmd As New SqliteCommand(sql, cn)
                cmd.Parameters.AddWithValue("@id_resumen", m.IdResumen)
                cmd.Parameters.AddWithValue("@fecha", m.Fecha.ToString("yyyy-MM-dd HH:mm:ss"))
                cmd.Parameters.AddWithValue("@tipo", m.Tipo)
                cmd.Parameters.AddWithValue("@metodo", m.MetodoPago)
                cmd.Parameters.AddWithValue("@monto", m.Monto)
                cmd.Parameters.AddWithValue("@motivo", m.Motivo)
                cmd.Parameters.AddWithValue("@empleado", m.Empleado)

                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub Actualizar(m As MovimientoCaja)
        Using cn As New SqliteConnection(Configuracion.ConnectionString)
            cn.Open()

            Dim sql As String =
        "UPDATE movimientos_caja SET
            fecha = @fecha,
            tipo = @tipo,
            metodo_pago = @metodo,
            monto = @monto,
            motivo = @motivo,
            empleado = @empleado
         WHERE id_movimiento = @id"

            Using cmd As New SqliteCommand(sql, cn)
                cmd.Parameters.AddWithValue("@id", m.IdMovimiento)
                cmd.Parameters.AddWithValue("@fecha", m.Fecha.ToString("yyyy-MM-dd HH:mm:ss"))
                cmd.Parameters.AddWithValue("@tipo", m.Tipo)
                cmd.Parameters.AddWithValue("@metodo", m.MetodoPago)
                cmd.Parameters.AddWithValue("@monto", m.Monto)
                cmd.Parameters.AddWithValue("@motivo", m.Motivo)
                cmd.Parameters.AddWithValue("@empleado", m.Empleado)

                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Sub Eliminar(idMovimiento As Integer)
        Using cn As New SqliteConnection(Configuracion.ConnectionString)
            cn.Open()

            Dim sql = "DELETE FROM movimientos_caja WHERE id_movimiento = @id"

            Using cmd As New SqliteCommand(sql, cn)
                cmd.Parameters.AddWithValue("@id", idMovimiento)
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Function ObtenerPorResumen(idResumen As Integer) As List(Of MovimientoCaja)
        Dim lista As New List(Of MovimientoCaja)

        Using cn As New SqliteConnection(Configuracion.ConnectionString)
            cn.Open()

            Dim sql As String =
            "SELECT * FROM movimientos_caja
             WHERE id_resumen = @id
             ORDER BY fecha DESC"

            Using cmd As New SqliteCommand(sql, cn)
                cmd.Parameters.AddWithValue("@id", idResumen)

                Using rd = cmd.ExecuteReader()
                    While rd.Read()
                        lista.Add(New MovimientoCaja With {
                            .IdMovimiento = rd.GetInt32(0),
                            .IdResumen = rd.GetInt32(1),
                            .Fecha = DateTime.Parse(rd.GetString(2)),
                            .Tipo = rd.GetString(3),
                            .MetodoPago = rd.GetString(4),
                            .Monto = rd.GetInt32(5),
                            .Motivo = rd.GetString(6),
                            .Empleado = rd.GetString(8)
                        })
                    End While
                End Using
            End Using
        End Using

        Return lista
    End Function

    Public Function ObtenerPorTipo(idResumen As Integer, tipo As String) As List(Of MovimientoCaja)
        Dim lista As New List(Of MovimientoCaja)

        Using cn As New SqliteConnection(Configuracion.ConnectionString)
            cn.Open()

            Dim sql As String =
            "SELECT * FROM movimientos_caja
             WHERE id_resumen = @id AND tipo = @tipo
             ORDER BY fecha DESC"

            Using cmd As New SqliteCommand(sql, cn)
                cmd.Parameters.AddWithValue("@id", idResumen)
                cmd.Parameters.AddWithValue("@tipo", tipo)

                Using rd = cmd.ExecuteReader()
                    While rd.Read()
                        lista.Add(New MovimientoCaja With {
                            .IdMovimiento = rd.GetInt32(0),
                            .IdResumen = rd.GetInt32(1),
                            .Fecha = DateTime.Parse(rd.GetString(2)),
                            .Tipo = rd.GetString(3),
                            .MetodoPago = rd.GetString(4),
                            .Monto = rd.GetInt32(5),
                            .Motivo = rd.GetString(6),
                            .Empleado = rd.GetString(8)
                        })
                    End While
                End Using
            End Using
        End Using

        Return lista
    End Function

    Public Function ObtenerTotales(idResumen As Integer, tipo As String, metodo As String) As Decimal

        Using cn As New SqliteConnection(Configuracion.ConnectionString)
            cn.Open()

            Dim cmd As New SqliteCommand("
            SELECT IFNULL(SUM(monto), 0)
            FROM movimientos_caja
            WHERE tipo = @tipo 
              AND metodo_pago = @metodo 
              AND id_resumen = @id", cn)

            cmd.Parameters.AddWithValue("@id", idResumen)
            cmd.Parameters.AddWithValue("@tipo", tipo)
            cmd.Parameters.AddWithValue("@metodo", metodo)

            Dim result = cmd.ExecuteScalar()

            If result Is Nothing OrElse result Is DBNull.Value Then
                Return 0
            End If

            Return Convert.ToDecimal(result)
        End Using

    End Function
End Class
