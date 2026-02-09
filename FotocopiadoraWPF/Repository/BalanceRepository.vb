Imports Microsoft.Data.Sqlite

Public Class BalanceRepository

    Public Function GuardarBalance(b As BalanceEntity) As Integer
        Using cn As New SqliteConnection(Configuracion.ConnectionString)
            cn.Open()

            Dim cmd As New SqliteCommand("
        INSERT INTO resumenes
        (contador_equipo1, contador_equipo2, efectivo, transferencia, fecha, anio, id_mes)
        VALUES
        (@c1, @c2, @efectivo, @transferencia, @fecha, @anio, @id_mes);

        SELECT last_insert_rowid();
        ", cn)

            cmd.Parameters.AddWithValue("@c1", b.ContadorEquipo1)
            cmd.Parameters.AddWithValue("@c2", b.ContadorEquipo2)
            cmd.Parameters.AddWithValue("@efectivo", b.Efectivo)
            cmd.Parameters.AddWithValue("@transferencia", b.Transferencia)
            cmd.Parameters.AddWithValue("@fecha", Date.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            cmd.Parameters.AddWithValue("@anio", b.Anio)
            cmd.Parameters.AddWithValue("@id_mes", b.IdMes)

            Return Convert.ToInt32(cmd.ExecuteScalar())
        End Using
    End Function



    Public Function ObtenerUltimoBalance() As BalanceEntity
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
                    Return New BalanceEntity With {
                        .IdResumen = CInt(dr("id_resumen")),
                        .ContadorEquipo1 = CInt(dr("contador_equipo1")),
                        .ContadorEquipo2 = CInt(dr("contador_equipo2")),
                        .Efectivo = CInt(dr("efectivo")),
                        .Transferencia = CInt(dr("transferencia")),
                        .Fecha = Date.Parse(dr("fecha").ToString()),
                        .Anio = CInt(dr("anio")),
                        .IdMes = CInt(dr("id_mes"))
}
                End If
            End Using
        End Using

        Return Nothing
    End Function


End Class
