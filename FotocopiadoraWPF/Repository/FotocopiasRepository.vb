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

            cmd.Parameters.AddWithValue("@nombre", Date.Now)
            cmd.Parameters.AddWithValue("@fecha",
            cmd.Parameters.AddWithValue("@paginas", If(vm.Paginas, 0))
            cmd.Parameters.AddWithValue("@anillados", If(vm.Anillados, 0))
            cmd.Parameters.AddWithValue("@precio_unitario",
            cmd.Parameters.AddWithValue("@precio_total", vm.Total)
            cmd.Parameters.AddWithValue("@transferencia", If(vm.Transferencia, 0))
            cmd.Parameters.AddWithValue("@efectivo", If(vm.Efectivo, 0))
            cmd.Parameters.AddWithValue("@comentario",
            cmd.Parameters.AddWithValue("@id_estado", 

            cmd.ExecuteNonQuery()
        End Using

    End Sub

End Class
