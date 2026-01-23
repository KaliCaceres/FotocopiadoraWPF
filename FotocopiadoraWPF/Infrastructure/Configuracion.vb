'Public Class Configuracion
'    Public Shared ReadOnly ConnectionString As String =
'        "Data Source=SOPORTE-SISTEMA\SQLEXPRESS;" &
'        "Initial Catalog=fotocopiadora;" &
'        "Integrated Security=True;" &
'        "Encrypt=False;" &
'        "Connect Timeout=30;"
'End Class

Imports Microsoft.Data.Sqlite

Public Class Configuracion
    Public Shared ReadOnly ConnectionString As String =
        "Data Source=" &
        IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fotocopiadora.db") &
        ";"
End Class

