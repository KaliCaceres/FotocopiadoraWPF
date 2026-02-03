Imports System.Data
Imports Microsoft.Data.Sqlite


Public Class FotocopiasRepository

    '==================== INSERT ====================

    Public Sub GuardarFotocopia(f As Fotocopia)

        Using cn As New SqliteConnection(Configuracion.ConnectionString)
            cn.Open()

            Dim cmd As New SqliteCommand("
            INSERT INTO fotocopias
            (nombre, fecha, paginas, anillados,
             precio_unitario, precio_total,
             transferencia, efectivo, comentario, id_estado)
            VALUES
            (@nombre, @fecha, @paginas, @anillados,
             @precio_unitario, @precio_total,
             @transferencia, @efectivo, @comentario, 1)", cn)

            cmd.Parameters.AddWithValue("@nombre",
            If(String.IsNullOrWhiteSpace(f.Nombre), "SIN NOMBRE", f.Nombre))

            cmd.Parameters.AddWithValue("@fecha", f.Fecha.ToString("yyyy-MM-dd"))
            cmd.Parameters.AddWithValue("@paginas", f.Paginas)
            cmd.Parameters.AddWithValue("@anillados", f.Anillados)
            cmd.Parameters.AddWithValue("@precio_unitario", f.PrecioUnitario)
            cmd.Parameters.AddWithValue("@precio_total", f.PrecioTotal)
            cmd.Parameters.AddWithValue("@transferencia", f.Transferencia)
            cmd.Parameters.AddWithValue("@efectivo", f.Efectivo)
            cmd.Parameters.AddWithValue("@comentario", If(f.Comentario, ""))

            cmd.ExecuteNonQuery()
        End Using
    End Sub


    '==================== UPDATE ====================

    Public Sub ActualizarFotocopia(f As Fotocopia)

        Using cn As New SqliteConnection(Configuracion.ConnectionString)
            cn.Open()

            Dim cmd As New SqliteCommand("
            UPDATE fotocopias SET
                nombre = @nombre,
                fecha = @fecha,
                paginas = @paginas,
                anillados = @anillados,
                precio_unitario = @precio_unitario,
                precio_total = @precio_total,
                transferencia = @transferencia,
                efectivo = @efectivo,
                comentario = @comentario
            WHERE id_fotocopia = @id", cn)

            cmd.Parameters.AddWithValue("@id", f.IdFotocopia)
            cmd.Parameters.AddWithValue("@nombre", f.Nombre)
            cmd.Parameters.AddWithValue("@fecha", f.Fecha.ToString("yyyy-MM-dd"))
            cmd.Parameters.AddWithValue("@paginas", f.Paginas)
            cmd.Parameters.AddWithValue("@anillados", f.Anillados)
            cmd.Parameters.AddWithValue("@precio_unitario", f.PrecioUnitario)
            cmd.Parameters.AddWithValue("@precio_total", f.PrecioTotal)
            cmd.Parameters.AddWithValue("@transferencia", f.Transferencia)
            cmd.Parameters.AddWithValue("@efectivo", f.Efectivo)
            cmd.Parameters.AddWithValue("@comentario", f.Comentario)

            Dim filas = cmd.ExecuteNonQuery()
            If filas = 0 Then
                Throw New Exception("No se actualizó ninguna fila.")
            End If
        End Using
    End Sub


    '==================== SELECT ====================

    Public Function ObtenerFotocopias() As List(Of Fotocopia)

        Dim lista As New List(Of Fotocopia)

        Using cn As New SqliteConnection(Configuracion.ConnectionString)
            cn.Open()

            Dim cmd As New SqliteCommand("
            SELECT *
            FROM fotocopias
            ORDER BY fecha DESC", cn)

            Using dr = cmd.ExecuteReader()
                While dr.Read()
                    lista.Add(New Fotocopia With {
                        .IdFotocopia = dr.GetInt32(0),
                        .Nombre = dr.GetString(1),
                        .Fecha = Date.Parse(dr.GetString(2)),
                        .Paginas = dr.GetInt32(3),
                        .Anillados = dr.GetInt32(4),
                        .PrecioUnitario = dr.GetInt32(5),
                        .PrecioTotal = dr.GetInt32(6),
                        .Transferencia = dr.GetInt32(7),
                        .Efectivo = dr.GetInt32(8),
                        .Comentario = dr.GetString(9),
                        .IdEstado = dr.GetInt32(10)
                    })
                End While
            End Using
        End Using

        Return lista
    End Function


End Class
