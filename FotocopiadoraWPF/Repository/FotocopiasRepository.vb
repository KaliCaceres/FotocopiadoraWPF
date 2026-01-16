Imports System.Data
Imports Microsoft.Data.SqlClient

Public Class FotocopiasRepository

    '==================== INSERT ====================

    Public Sub GuardarFotocopia(f As Fotocopia, precioUnitario As Integer)

        Using cn As New SqlConnection(Configuracion.ConnectionString)
            cn.Open()

            Dim cmd As New SqlCommand("
                INSERT INTO fotocopias
                (nombre, fecha, paginas, anillados,
                 precio_unitario, precio_total, transferencia,
                 efectivo, comentario, id_estado)
                VALUES
                (@nombre, @fecha, @paginas, @anillados,
                 @precio_unitario, @precio_total, @transferencia,
                 @efectivo, @comentario, @id_estado)", cn)

            cmd.Parameters.Add("@nombre", SqlDbType.VarChar).
                Value = If(String.IsNullOrWhiteSpace(f.Nombre), "SIN NOMBRE", f.Nombre)

            cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = f.Fecha
            cmd.Parameters.Add("@paginas", SqlDbType.Int).Value = f.Paginas
            cmd.Parameters.Add("@anillados", SqlDbType.Int).Value = f.Anillados
            cmd.Parameters.Add("@precio_unitario", SqlDbType.Int).Value = precioUnitario
            cmd.Parameters.Add("@precio_total", SqlDbType.Int).Value = f.PrecioTotal
            cmd.Parameters.Add("@transferencia", SqlDbType.Int).Value = f.Transferencia
            cmd.Parameters.Add("@efectivo", SqlDbType.Int).Value = f.Efectivo
            cmd.Parameters.Add("@comentario", SqlDbType.VarChar).
                Value = If(f.Comentario, "")
            cmd.Parameters.Add("@id_estado", SqlDbType.Int).Value = 1

            cmd.ExecuteNonQuery()
        End Using

    End Sub

    '==================== UPDATE ====================

    Public Sub ActualizarFotocopia(f As Fotocopia)

        Using cn As New SqlConnection(Configuracion.ConnectionString)
            cn.Open()

            Dim cmd As New SqlCommand("
                UPDATE fotocopias
                SET nombre = @nombre,
                    fecha = @fecha,
                    paginas = @paginas,
                    anillados = @anillados,
                    precio_unitario = @precio_unitario,
                    precio_total = @precio_total,
                    transferencia = @transferencia,
                    efectivo = @efectivo,
                    comentario = @comentario
                WHERE id_fotocopia = @id", cn)

            cmd.Parameters.Add("@id", SqlDbType.Int).Value = f.IdFotocopia

            cmd.Parameters.Add("@nombre", SqlDbType.VarChar).
                Value = If(String.IsNullOrWhiteSpace(f.Nombre), "SIN NOMBRE", f.Nombre)

            cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = f.Fecha
            cmd.Parameters.Add("@paginas", SqlDbType.Int).Value = f.Paginas
            cmd.Parameters.Add("@anillados", SqlDbType.Int).Value = f.Anillados
            cmd.Parameters.Add("@precio_unitario", SqlDbType.Int).Value = f.PrecioUnitario
            cmd.Parameters.Add("@precio_total", SqlDbType.Int).Value = f.PrecioTotal
            cmd.Parameters.Add("@transferencia", SqlDbType.Int).Value = f.Transferencia
            cmd.Parameters.Add("@efectivo", SqlDbType.Int).Value = f.Efectivo
            cmd.Parameters.Add("@comentario", SqlDbType.VarChar).
                Value = If(f.Comentario, "")

            cmd.ExecuteNonQuery()
        End Using

    End Sub

    '==================== SELECT ====================

    Public Function ObtenerFotocopias() As List(Of Fotocopia)

        Dim lista As New List(Of Fotocopia)

        Using cn As New SqlConnection(Configuracion.ConnectionString)
            cn.Open()

            Dim cmd As New SqlCommand("
                SELECT  f.id_fotocopia,
                        f.nombre,
                        f.fecha,
                        f.paginas,
                        f.anillados,
                        f.precio_unitario,
                        f.precio_total,
                        f.transferencia,
                        f.efectivo,
                        f.comentario,
                        f.id_estado,
                        e.descripcion AS estado
                FROM fotocopias f
                INNER JOIN estados e ON f.id_estado = e.id_estado
                ORDER BY f.fecha DESC", cn)

            Using dr = cmd.ExecuteReader()
                While dr.Read()
                    lista.Add(New Fotocopia With {
                        .IdFotocopia = CInt(dr("id_fotocopia")),
                        .Nombre = dr("nombre").ToString(),
                        .Fecha = CDate(dr("fecha")),
                        .Paginas = CInt(dr("paginas")),
                        .Anillados = CInt(dr("anillados")),
                        .PrecioUnitario = CInt(dr("precio_unitario")),
                        .PrecioTotal = CInt(dr("precio_total")),
                        .Transferencia = CInt(dr("transferencia")),
                        .Efectivo = CInt(dr("efectivo")),
                        .Comentario = dr("comentario").ToString(),
                        .IdEstado = CInt(dr("id_estado")),
                        .Estado = dr("estado").ToString()
                    })
                End While
            End Using
        End Using

        Return lista
    End Function

End Class
