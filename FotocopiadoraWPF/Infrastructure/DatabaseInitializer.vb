Imports Microsoft.Data.Sqlite

Namespace Infrastructure

    Public Class DatabaseInitializer

        Public Shared Sub Inicializar()

            Using cn As New SqliteConnection(Configuracion.ConnectionString)
                cn.Open()

                Dim sql As String = "
                PRAGMA foreign_keys = ON;

                CREATE TABLE IF NOT EXISTS estados (
                    id_estado INTEGER PRIMARY KEY AUTOINCREMENT,
                    descripcion TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS categorias (
                    id_categoria INTEGER PRIMARY KEY AUTOINCREMENT,
                    descripcion TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS valores (
                    id_valor INTEGER PRIMARY KEY AUTOINCREMENT,
                    id_categoria INTEGER NOT NULL,
                    valor REAL NOT NULL,
                    FOREIGN KEY (id_categoria) REFERENCES categorias(id_categoria)
                );

                CREATE TABLE IF NOT EXISTS fotocopias (
                    id_fotocopia INTEGER PRIMARY KEY AUTOINCREMENT,
                    nombre TEXT,
                    fecha TEXT,
                    paginas INTEGER,
                    anillados INTEGER,
                    precio_unitario INTEGER,
                    precio_total INTEGER,
                    transferencia INTEGER,
                    efectivo INTEGER,
                    comentario TEXT,
                    id_estado INTEGER,
                    FOREIGN KEY (id_estado) REFERENCES estados(id_estado)
                );

                CREATE TABLE IF NOT EXISTS meses (
                    id_mes INTEGER PRIMARY KEY AUTOINCREMENT,
                    descripcion TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS resumenes (
                    id_resumen INTEGER PRIMARY KEY AUTOINCREMENT,
                    contador_equipo1 INTEGER,
                    contador_equipo2 INTEGER,
                    id_mes INTEGER,
                    anio INTEGER,
                    transferencia INTEGER,
                    efectivo INTEGER,
                    fecha TEXT,
                    FOREIGN KEY (id_mes) REFERENCES meses(id_mes)
                );
                "

                Using cmd As New SqliteCommand(sql, cn)
                    cmd.ExecuteNonQuery()
                End Using

            End Using

        End Sub

        Public Shared Sub InsertarDatosIniciales()

            Using cn As New SqliteConnection(Configuracion.ConnectionString)
                cn.Open()

                Dim sql As String = "
        INSERT OR IGNORE INTO estados (id_estado, descripcion) VALUES (0, 'Pagado'), (1, 'Deudor'), (2, 'Perdida'), (3, 'Eliminada');

        INSERT OR IGNORE INTO categorias (id_categoria, descripcion) VALUES
        (1,'Anillado'),
        (2,'Empleado'),
        (3,'1 - 100'),
        (4,'101 - 400'),
        (5,'401 - 700'),
        (6,'+700');

        INSERT OR IGNORE INTO valores (id_categoria, valor) VALUES
        (1,2000),
        (2,30),
        (3,80),
        (4,70),
        (5,60),
        (6,50);

        INSERT OR IGNORE INTO meses (id_mes, descripcion) VALUES
        (1,'Enero'),(2,'Febrero'),(3,'Marzo'),(4,'Abril'),
        (5,'Mayo'),(6,'Junio'),(7,'Julio'),(8,'Agosto'),
        (9,'Septiembre'),(10,'Octubre'),(11,'Noviembre'),(12,'Diciembre');
        "

                Using cmd As New SqliteCommand(sql, cn)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

        End Sub


    End Class

End Namespace
