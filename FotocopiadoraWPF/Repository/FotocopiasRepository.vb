Imports System.Data
Imports Microsoft.Data.Sqlite
Imports FotocopiadoraWPF.Services

Public Class FotocopiasRepository

    '==================== INSERT ====================

    Public Sub GuardarFotocopia(f As Fotocopia)

        If f.IdResumen <= 0 Then
            Throw New InvalidOperationException("No hay balance activo para guardar la fotocopia.")
        End If

        Using cn As New SqliteConnection(Configuracion.ConnectionString)
            cn.Open()

            Dim cmd As New SqliteCommand("
            INSERT INTO fotocopias
            (id_resumen, nombre, fecha, paginas, anillados,
             precio_unitario, precio_total,
             transferencia, efectivo, comentario, id_estado)
            VALUES
            (@id_resumen, @nombre, @fecha, @paginas, @anillados,
             @precio_unitario, @precio_total,
             @transferencia, @efectivo, @comentario, @estado)", cn)

            cmd.Parameters.AddWithValue("@id_resumen", f.IdResumen)
            cmd.Parameters.AddWithValue("@nombre",
                If(String.IsNullOrWhiteSpace(f.Nombre), "SIN NOMBRE", f.Nombre))

            cmd.Parameters.AddWithValue("@fecha", f.Fecha.ToString("yyyy-MM-dd HH:mm:ss"))
            cmd.Parameters.AddWithValue("@paginas", f.Paginas)
            cmd.Parameters.AddWithValue("@anillados", f.Anillados)
            cmd.Parameters.AddWithValue("@precio_unitario", f.PrecioUnitario)
            cmd.Parameters.AddWithValue("@precio_total", f.PrecioTotal)
            cmd.Parameters.AddWithValue("@transferencia", f.Transferencia)
            cmd.Parameters.AddWithValue("@efectivo", f.Efectivo)
            cmd.Parameters.AddWithValue("@comentario", If(f.Comentario, ""))
            cmd.Parameters.AddWithValue("@estado", f.IdEstado)

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
                comentario = @comentario,
                id_estado = @estado
            WHERE id_fotocopia = @id", cn)

            cmd.Parameters.AddWithValue("@id", f.IdFotocopia)
            cmd.Parameters.AddWithValue("@nombre", f.Nombre)
            cmd.Parameters.AddWithValue("@fecha", f.Fecha.ToString("yyyy-MM-dd HH:mm:ss"))
            cmd.Parameters.AddWithValue("@paginas", f.Paginas)
            cmd.Parameters.AddWithValue("@anillados", f.Anillados)
            cmd.Parameters.AddWithValue("@precio_unitario", f.PrecioUnitario)
            cmd.Parameters.AddWithValue("@precio_total", f.PrecioTotal)
            cmd.Parameters.AddWithValue("@transferencia", f.Transferencia)
            cmd.Parameters.AddWithValue("@efectivo", f.Efectivo)
            cmd.Parameters.AddWithValue("@comentario", If(f.Comentario, ""))
            cmd.Parameters.AddWithValue("@estado", f.IdEstado)

            Dim filas = cmd.ExecuteNonQuery()
            If filas = 0 Then
                Throw New Exception("No se actualizó ninguna fotocopia.")
            End If
        End Using
    End Sub

    '==================== SELECT ====================

    Public Function ObtenerFotocopiasPorBalance(idResumen As Integer) As List(Of Fotocopia)

        If idResumen <= 0 Then
            Throw New InvalidOperationException("Id de balance inválido.")
        End If

        Dim lista As New List(Of Fotocopia)

        Using cn As New SqliteConnection(Configuracion.ConnectionString)
            cn.Open()

            Dim cmd As New SqliteCommand("
            SELECT *
            FROM fotocopias
            WHERE id_resumen = @id_resumen
            ORDER BY fecha DESC", cn)

            cmd.Parameters.AddWithValue("@id_resumen", idResumen)

            Using dr = cmd.ExecuteReader()
                While dr.Read()
                    lista.Add(New Fotocopia With {
                        .IdFotocopia = dr.GetInt32(dr.GetOrdinal("id_fotocopia")),
                        .IdResumen = dr.GetInt32(dr.GetOrdinal("id_resumen")),
                        .Nombre = dr.GetString(dr.GetOrdinal("nombre")),
                        .Fecha = Date.Parse(dr.GetString(dr.GetOrdinal("fecha"))),
                        .Paginas = dr.GetInt32(dr.GetOrdinal("paginas")),
                        .Anillados = dr.GetInt32(dr.GetOrdinal("anillados")),
                        .PrecioUnitario = dr.GetInt32(dr.GetOrdinal("precio_unitario")),
                        .PrecioTotal = dr.GetInt32(dr.GetOrdinal("precio_total")),
                        .Transferencia = dr.GetInt32(dr.GetOrdinal("transferencia")),
                        .Efectivo = dr.GetInt32(dr.GetOrdinal("efectivo")),
                        .Comentario = dr.GetString(dr.GetOrdinal("comentario")),
                        .IdEstado = dr.GetInt32(dr.GetOrdinal("id_estado"))
                    })
                End While
            End Using
        End Using

        Return lista
    End Function

    '==================== UPDATE ESTADO ====================

    Public Sub ActualizarEstado(idFotocopia As Integer, idEstado As Integer)

        Using cn As New SqliteConnection(Configuracion.ConnectionString)
            cn.Open()

            Dim cmd = cn.CreateCommand()
            cmd.CommandText = "
            UPDATE fotocopias
            SET id_estado = @estado
            WHERE id_fotocopia = @id
            "

            cmd.Parameters.AddWithValue("@estado", idEstado)
            cmd.Parameters.AddWithValue("@id", idFotocopia)

            cmd.ExecuteNonQuery()
        End Using
    End Sub

End Class
