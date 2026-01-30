Imports QuestPDF.Fluent
Imports QuestPDF.Helpers
Imports QuestPDF.Infrastructure

Public Class BalancePdf
    Implements IDocument

    Private ReadOnly _balance As Balance

    Public Sub New(balance As Balance)
        _balance = balance
    End Sub

    Public Function GetMetadata() As DocumentMetadata _
        Implements IDocument.GetMetadata
        Return DocumentMetadata.Default
    End Function

    Public Function GetSettings() As DocumentSettings _
        Implements IDocument.GetSettings
        Return DocumentSettings.Default
    End Function

    Public Sub Compose(container As IDocumentContainer) _
        Implements IDocument.Compose

        container.Page(Sub(page)
                           page.Size(PageSizes.A4)
                           page.Margin(30)

                           page.Content().Column(Sub(col)
                                                     col.Spacing(10)

                                                     col.Item().Text("Balance Diario").FontSize(20).Bold()

                                                     col.Item().Text($"Fecha: {_balance.Fecha:dd/MM/yyyy HH:mm}")
                                                     col.Item().Text($"Equipo 1: {_balance.ContadorEquipo1}")
                                                     col.Item().Text($"Equipo 2: {_balance.ContadorEquipo2}")
                                                     col.Item().Text($"Efectivo: ${_balance.Efectivo}")
                                                     col.Item().Text($"Transferencia: ${_balance.Transferencia}")
                                                     col.Item().Text($"Periodo: {_balance.IdMes}/{_balance.Anio}")

                                                 End Sub)
                       End Sub)
    End Sub
End Class
