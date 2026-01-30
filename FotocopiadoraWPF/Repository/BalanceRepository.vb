Imports Microsoft.Data.Sqlite

Public Class BalanceRepository

    Public Sub GuardarBalance(b As Balance)
        Using cn As New SqliteConnection(Configuracion.ConnectionString)
            cn.Open()

            Dim cmd As New SqliteCommand("
            INSERT INTO resumenes
            (contador_equipo1, contador_equipo2, efectivo, transferencia, fecha, anio, id_mes)
            VALUES
            (@c1, @c2, @efectivo, @transferencia, @fecha, @anio, @id_mes)
        ", cn)

            cmd.Parameters.AddWithValue("@c1", b.ContadorEquipo1)
            cmd.Parameters.AddWithValue("@c2", b.ContadorEquipo2)
            cmd.Parameters.AddWithValue("@efectivo", b.Efectivo)
            cmd.Parameters.AddWithValue("@transferencia", b.Transferencia)
            cmd.Parameters.AddWithValue("@fecha", Date.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            cmd.Parameters.AddWithValue("@anio", b.Anio)
            cmd.Parameters.AddWithValue("@id_mes", b.IdMes)
            cmd.ExecuteNonQuery()
        End Using
    End Sub


    Public Function ObtenerUltimoBalance() As Balance
        Using cn As New SqliteConnection(Configuracion.ConnectionString)
            cn.Open()

            Dim cmd As New SqliteCommand("
                SELECT *
                FROM resumenes
                ORDER BY fecha DESC
                LIMIT 1
            ", cn)

            Using dr = cmd.ExecuteReader()
                If dr.Read() Then
                    Return New Balance With {
                        .ContadorEquipo1 = If(IsDBNull(dr("contador_equipo1")), 0, CInt(dr("contador_equipo1"))),
                        .ContadorEquipo2 = If(IsDBNull(dr("contador_equipo2")), 0, CInt(dr("contador_equipo2"))),
                        .Efectivo = If(IsDBNull(dr("efectivo")), 0, CInt(dr("efectivo"))),
                        .Transferencia = If(IsDBNull(dr("transferencia")), 0, CInt(dr("transferencia")))
                    }
                End If
            End Using
        End Using

        Return Nothing
    End Function

End Class
