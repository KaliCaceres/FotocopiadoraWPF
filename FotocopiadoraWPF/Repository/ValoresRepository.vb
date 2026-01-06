Imports Microsoft.Data.SqlClient


Public Class ValoresRepository

    Public Function ObtenerValores() As List(Of ValorConfiguracion)

        Dim lista As New List(Of ValorConfiguracion)

        Using cn As New SqlConnection(Configuracion.ConnectionString)
            cn.Open()

            Dim cmd As New SqlCommand("
                SELECT c.descripcion, v.valor
                FROM valores v
                INNER JOIN categorias c ON v.id_categoria = c.id_categoria", cn)

            Using dr = cmd.ExecuteReader()
                While dr.Read()
                    lista.Add(New ValorConfiguracion With {
                        .Descripcion = dr("descripcion").ToString(),
                        .Valor = CDec(dr("valor"))
                    })
                End While
            End Using
        End Using

        Return lista
    End Function

End Class
