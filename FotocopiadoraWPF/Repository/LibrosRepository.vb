Imports Microsoft.Data.Sqlite

Public Class LibrosRepository

    Public Function ObtenerLibros() As List(Of Libro)

        Dim lista As New List(Of Libro)

        Using cn As New SqliteConnection(Configuracion.ConnectionString)
            cn.Open()

            Dim cmd As New SqliteCommand("SELECT * FROM libros ORDER BY titulo", cn)

            Using dr = cmd.ExecuteReader()
                While dr.Read()
                    lista.Add(New Libro With {
                        .IdLibro = CInt(dr("id_libro")),
                        .Titulo = dr("titulo").ToString()
                    })
                End While
            End Using
        End Using

        Return lista

    End Function

    Public Sub Guardar(libro As Libro)

        Using cn As New SqliteConnection(Configuracion.ConnectionString)
            cn.Open()

            Dim cmd As New SqliteCommand("
                INSERT INTO libros (titulo)
                VALUES (@titulo);
            ", cn)

            cmd.Parameters.AddWithValue("@titulo", libro.Titulo)
            cmd.ExecuteNonQuery()
        End Using

    End Sub

End Class
