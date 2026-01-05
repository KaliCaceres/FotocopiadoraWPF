Imports System.Data
Imports FotocopiadoraWPF.ViewModels
Imports Microsoft.Data.SqlClient

Public Class FotocopiasRepository

    Private ReadOnly _cnString As String =
        "Data Source=SOPORTE-SISTEMA\SQLEXPRESS;Initial Catalog=fotocopiadora;Integrated Security=True; Encrypt=True;TrustServerCertificate=True"

    Public Sub GuardarFotocopia(vm As FotocopiasViewModel)

        Using cn As New SqlConnection(_cnString)
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

            cmd.Parameters.Add("@nombre", SqlDbType.VarChar).Value =
    If(String.IsNullOrWhiteSpace(vm.Nombre), "SIN NOMBRE", vm.Nombre)

            cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = vm.Fecha

            cmd.Parameters.Add("@paginas", SqlDbType.Int).Value = If(vm.Paginas, 0)
            cmd.Parameters.Add("@anillados", SqlDbType.Int).Value = If(vm.Anillados, 0)

            cmd.Parameters.Add("@precio_unitario", SqlDbType.Int).Value = vm.PrecioPagina
            cmd.Parameters.Add("@precio_total", SqlDbType.Int).Value = vm.Total

            cmd.Parameters.Add("@transferencia", SqlDbType.Int).Value = If(vm.Transferencia, 0)
            cmd.Parameters.Add("@efectivo", SqlDbType.Int).Value = If(vm.Efectivo, 0)

            cmd.Parameters.Add("@comentario", SqlDbType.VarChar).Value =
    If(vm.Comentario, "")

            cmd.Parameters.Add("@id_estado", SqlDbType.Int).Value = 1


            cmd.ExecuteNonQuery()
        End Using

    End Sub

End Class
