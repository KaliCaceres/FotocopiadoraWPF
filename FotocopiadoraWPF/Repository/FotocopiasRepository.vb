Imports System.Data
Imports Microsoft.Data.SqlClient
Imports FotocopiadoraWPF.ViewModels

Public Class FotocopiasRepository

    '==================== INSERT ====================

    Public Sub GuardarFotocopia(vm As FotocopiasViewModel)

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
                Value = If(String.IsNullOrWhiteSpace(vm.Nombre), "SIN NOMBRE", vm.Nombre)

            cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = vm.Fecha
            cmd.Parameters.Add("@paginas", SqlDbType.Int).Value = If(vm.Paginas, 0)
            cmd.Parameters.Add("@anillados", SqlDbType.Int).Value = If(vm.Anillados, 0)
            cmd.Parameters.Add("@precio_unitario", SqlDbType.Int).Value = vm.PrecioPagina
            cmd.Parameters.Add("@precio_total", SqlDbType.Int).Value = vm.Total
            cmd.Parameters.Add("@transferencia", SqlDbType.Int).Value = If(vm.Transferencia, 0)
            cmd.Parameters.Add("@efectivo", SqlDbType.Int).Value = If(vm.Efectivo, 0)
            cmd.Parameters.Add("@comentario", SqlDbType.VarChar).Value = If(vm.Comentario, "")
            cmd.Parameters.Add("@id_estado", SqlDbType.Int).Value = 1

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
