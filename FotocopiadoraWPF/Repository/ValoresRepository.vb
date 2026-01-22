Imports Microsoft.Data.SqlClient
Imports Microsoft.Data.Sqlite
Public Class ValoresRepository

    Public Function ObtenerValores() As List(Of ValorConfiguracion)

        Dim lista As New List(Of ValorConfiguracion)

        Using cn As New SqliteConnection(Configuracion.ConnectionString)
            cn.Open()

            Dim cmd As New SqliteCommand("
            SELECT c.descripcion, v.valor
            FROM valores v
            INNER JOIN categorias c ON v.id_categoria = c.id_categoria
        ", cn)

            Using dr = cmd.ExecuteReader()
                While dr.Read()
                    lista.Add(New ValorConfiguracion With {
                    .Descripcion = dr.GetString(0),
                    .Valor = dr.GetDecimal(1)
                })
                End While
            End Using
        End Using

        Return lista
    End Function


    '==================== GUARDAR ====================

    Public Sub GuardarValores(valores As List(Of ValorConfiguracion))

        Using cn As New SqliteConnection(Configuracion.ConnectionString)
            cn.Open()

            Using tran = cn.BeginTransaction()

                Try
                    For Each v In valores
                        Dim cmd As New SqliteCommand("
                        UPDATE valores
                        SET valor = @valor
                        WHERE id_categoria = (
                            SELECT id_categoria
                            FROM categorias
                            WHERE descripcion = @descripcion
                        )
                    ", cn, tran)

                        cmd.Parameters.AddWithValue("@valor", v.Valor)
                        cmd.Parameters.AddWithValue("@descripcion", v.Descripcion)

                        cmd.ExecuteNonQuery()
                    Next

                    tran.Commit()

                Catch
                    tran.Rollback()
                    Throw
                End Try

            End Using
        End Using

    End Sub


End Class
