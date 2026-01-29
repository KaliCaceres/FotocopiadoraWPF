Imports QuestPDF.Fluent
Imports QuestPDF.Infrastructure
Imports System.Diagnostics
Imports System.IO

Public Module BalancePdfGenerator

    Public Sub GenerarYMostrar(balance As Balance)

        QuestPDF.Settings.License = LicenseType.Community

        Dim carpeta = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Balances"
        )

        If Not Directory.Exists(carpeta) Then
            Directory.CreateDirectory(carpeta)
        End If

        Dim rutaPdf = Path.Combine(
            carpeta,
            $"Balance_{balance.Fecha:yyyyMMdd_HHmm}.pdf"
        )

        Dim documento = New BalancePdf(balance)
        documento.GeneratePdf(rutaPdf)

        Process.Start(New ProcessStartInfo(rutaPdf) With {
            .UseShellExecute = True
        })

    End Sub

End Module
