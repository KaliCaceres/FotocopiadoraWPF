Imports Microsoft.Data.Sqlite

Public Class MovimientosCajaRepository

    Public Sub Insertar(m As MovimientoCaja)
        Using cn As New SqliteConnection(Configuracion.ConnectionString)
            cn.Open()

            Dim sql As String =
            "INSERT INTO movimientos_caja
            (id_resumen, fecha, tipo, metodo_pago, monto, motivo, observacion, empleado)
            VALUES
            (@id_resumen, @fecha, @tipo, @metodo, @monto, @motivo, @obs, @empleado)"

            Using cmd As New SqliteCommand(sql, cn)
                cmd.Parameters.AddWithValue("@id_resumen", m.IdResumen)
                cmd.Parameters.AddWithValue("@fecha", m.Fecha.ToString("yyyy-MM-dd HH:mm:ss"))
                cmd.Parameters.AddWithValue("@tipo", m.Tipo)
                cmd.Parameters.AddWithValue("@metodo", m.MetodoPago)
                cmd.Parameters.AddWithValue("@monto", m.Monto)
                cmd.Parameters.AddWithValue("@motivo", m.Motivo)
                cmd.Parameters.AddWithValue("@obs", If(m.Observacion, ""))
                cmd.Parameters.AddWithValue("@empleado", m.Empleado)

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
                            .Observacion = rd.GetString(7),
                            .Empleado = rd.GetString(8)
                        })
                    End While
                End Using
            End Using
        End Using

        Return lista
    End Function
End Class
