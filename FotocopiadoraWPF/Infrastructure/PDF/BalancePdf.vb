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

                                                     col.Item().Text("CENTRO UNIVERSITARIO").FontSize(20).Bold()
                                                     col.Item().Text("FOTOCOPIADORA").FontSize(16).Bold()
                                                     'col.Item().Text($"Inicio: {_balance.FechaInicio:dd/MM/yyyy HH:mm}")
                                                     'col.Item().Text($"Fin: {_balance.FechaFin:dd/MM/yyyy HH:mm}")
                                                     col.Item().Text($"Periodo: {_balance.IdMes}/{_balance.Anio}")

                                                     'Contadores
                                                     col.Item().Column(Sub(innerCol)
                                                                           innerCol.Item().Text("CONTADORES").FontSize(14).Bold()
                                                                           innerCol.Item().LineHorizontal(1)
                                                                       End Sub)

                                                     col.Item().Column(Sub(innerCol)

                                                                           innerCol.Item().Row(Sub(row)
                                                                                                   row.RelativeItem().AlignLeft().Text("    INICIO:").Bold()
                                                                                                   row.RelativeItem().AlignRight().Text(_balance.FechaInicio.ToString("dd/MM/yyyy HH:mm"))
                                                                                               End Sub)

                                                                           innerCol.Item().LineHorizontal(1)
                                                                       End Sub)

                                                     col.Item().Column(Sub(innerCol)
                                                                           innerCol.Item().Row(Sub(row)
                                                                                                   row.RelativeItem().AlignLeft().Text("       Toshiba Studio (307) Equipo 1:").Bold
                                                                                                   row.RelativeItem().AlignRight().Text(_balance.ContadorEquipo1Inicio.ToString)
                                                                                               End Sub)

                                                                           innerCol.Item().Row(Sub(row)
                                                                                                   row.RelativeItem().AlignLeft().Text("       Toshiba Studio (307) Equipo 2:").Bold
                                                                                                   row.RelativeItem().AlignRight().Text(_balance.ContadorEquipo2Inicio.ToString)
                                                                                               End Sub)
                                                                       End Sub)

                                                     col.Item().Column(Sub(innerCol)

                                                                           innerCol.Item().Row(Sub(row)
                                                                                                   row.RelativeItem().AlignLeft().Text("    FIN:").Bold()
                                                                                                   row.RelativeItem().AlignRight().Text(_balance.FechaFin.ToString("dd/MM/yyyy HH:mm"))
                                                                                               End Sub)
                                                                           innerCol.Item().LineHorizontal(1)
                                                                       End Sub)

                                                     col.Item().Column(Sub(innerCol)
                                                                           innerCol.Item().Row(Sub(row)
                                                                                                   row.RelativeItem().AlignLeft().Text("       Toshiba Studio (307) Equipo 1:").Bold
                                                                                                   row.RelativeItem().AlignRight().Text(_balance.ContadorEquipo1Final.ToString)
                                                                                               End Sub)

                                                                           innerCol.Item().Row(Sub(row)
                                                                                                   row.RelativeItem().AlignLeft().Text("       Toshiba Studio (307) Equipo 2:").Bold
                                                                                                   row.RelativeItem().AlignRight().Text(_balance.ContadorEquipo2Final.ToString)
                                                                                               End Sub)
                                                                       End Sub)

                                                     'Contadores
                                                     col.Item().Column(Sub(innerCol)
                                                                           innerCol.Item().Text("CAJA").FontSize(14).Bold()
                                                                           innerCol.Item().LineHorizontal(1)
                                                                       End Sub)

                                                     col.Item().Column(Sub(innerCol)

                                                                           innerCol.Item().Row(Sub(row)
                                                                                                   row.RelativeItem().AlignLeft().Text("    INICIO:").Bold()
                                                                                                   'row.RelativeItem().AlignRight().Text(_balance.FechaInicio.ToString("dd/MM/yyyy HH:mm"))
                                                                                               End Sub)

                                                                           innerCol.Item().LineHorizontal(1)
                                                                       End Sub)

                                                     col.Item().Column(Sub(innerCol)
                                                                           innerCol.Item().Row(Sub(row)
                                                                                                   row.RelativeItem().AlignLeft().Text("       Efectivo:").Bold
                                                                                                   row.RelativeItem().AlignRight().Text(_balance.EfectivoInicio.ToString)
                                                                                               End Sub)

                                                                           innerCol.Item().Row(Sub(row)
                                                                                                   row.RelativeItem().AlignLeft().Text("       Transferencia:").Bold
                                                                                                   row.RelativeItem().AlignRight().Text(_balance.TransferenciaInicio.ToString)
                                                                                               End Sub)
                                                                       End Sub)

                                                     col.Item().Column(Sub(innerCol)

                                                                           innerCol.Item().Row(Sub(row)
                                                                                                   row.RelativeItem().AlignLeft().Text("    FIN:").Bold()
                                                                                                   'row.RelativeItem().AlignRight().Text(_balance.FechaFin.ToString("dd/MM/yyyy HH:mm"))
                                                                                               End Sub)
                                                                           innerCol.Item().LineHorizontal(1)
                                                                       End Sub)

                                                     col.Item().Column(Sub(innerCol)
                                                                           innerCol.Item().Row(Sub(row)
                                                                                                   row.RelativeItem().AlignLeft().Text("       Efectivo:").Bold
                                                                                                   row.RelativeItem().AlignRight().Text(_balance.EfectivoFinal.ToString)
                                                                                               End Sub)

                                                                           innerCol.Item().Row(Sub(row)
                                                                                                   row.RelativeItem().AlignLeft().Text("       Transferencia:").Bold
                                                                                                   row.RelativeItem().AlignRight().Text(_balance.TransferenciaFinal.ToString)
                                                                                               End Sub)
                                                                       End Sub)

                                                     'Facultades
                                                     col.Item().Column(Sub(innerCol)
                                                                           innerCol.Item().Text("FACULTADES").FontSize(14).Bold()
                                                                           innerCol.Item().LineHorizontal(1)
                                                                       End Sub)

                                                     col.Item().Column(Sub(innerCol)

                                                                           innerCol.Item().Row(Sub(row)
                                                                                                   row.RelativeItem().AlignLeft().Text("   Centro Universitario:").Bold

                                                                                                   row.RelativeItem().AlignRight().Text(0.ToString())
                                                                                               End Sub)


                                                                           innerCol.Item().Row(Sub(row)
                                                                                                   row.RelativeItem().AlignLeft().Text("   IUNIR:").Bold

                                                                                                   row.RelativeItem().AlignRight().Text(0.ToString())

                                                                                               End Sub)

                                                                           innerCol.Item().Row(Sub(row)
                                                                                                   row.RelativeItem().AlignLeft().Text("   UCEL:").Bold

                                                                                                   row.RelativeItem().AlignRight().Text(0.ToString())
                                                                                               End Sub)

                                                                           innerCol.Item().Row(Sub(row)
                                                                                                   row.RelativeItem().AlignLeft().Text("   UGR:").Bold

                                                                                                   row.RelativeItem().AlignRight().Text(0.ToString())
                                                                                               End Sub)

                                                                           innerCol.Item().Row(Sub(row)
                                                                                                   row.RelativeItem().AlignLeft().Text("   UNR:").Bold

                                                                                                   row.RelativeItem().AlignRight().Text(0.ToString())
                                                                                               End Sub)
                                                                           innerCol.Item().Row(Sub(row)
                                                                                                   row.RelativeItem().AlignLeft().Text("   UNVM:").Bold

                                                                                                   row.RelativeItem().AlignRight().Text(0.ToString())
                                                                                               End Sub)

                                                                       End Sub)

                                                     col.Item().Column(Sub(innerCol)
                                                                           innerCol.Item().Text("FOTOCOPIAS POR PRECIO").FontSize(14).Bold()
                                                                           innerCol.Item().LineHorizontal(1)
                                                                       End Sub)

                                                     col.Item().Column(Sub(innerCol)

                                                                           innerCol.Item().Row(Sub(row)
                                                                                                   row.RelativeItem().AlignLeft().Text("   80:").Bold

                                                                                                   row.RelativeItem().AlignRight().Text(0.ToString())
                                                                                               End Sub)


                                                                           innerCol.Item().Row(Sub(row)
                                                                                                   row.RelativeItem().AlignLeft().Text("   70:").Bold

                                                                                                   row.RelativeItem().AlignRight().Text(0.ToString())

                                                                                               End Sub)

                                                                           innerCol.Item().Row(Sub(row)
                                                                                                   row.RelativeItem().AlignLeft().Text("   60:").Bold

                                                                                                   row.RelativeItem().AlignRight().Text(0.ToString())
                                                                                               End Sub)

                                                                           innerCol.Item().Row(Sub(row)
                                                                                                   row.RelativeItem().AlignLeft().Text("   50:").Bold

                                                                                                   row.RelativeItem().AlignRight().Text(0.ToString())
                                                                                               End Sub)

                                                                           innerCol.Item().Row(Sub(row)
                                                                                                   row.RelativeItem().AlignLeft().Text("   30:").Bold

                                                                                                   row.RelativeItem().AlignRight().Text(0.ToString())
                                                                                               End Sub)
                                                                       End Sub)

                                                     col.Item().Column(Sub(innerCol)
                                                                           innerCol.Item().Text("DEUDORES").FontSize(14).Bold()
                                                                           innerCol.Item().LineHorizontal(1)
                                                                       End Sub)

                                                     col.Item().Column(Sub(innerCol)

                                                                           'innerCol.Item().Row(Sub(row)
                                                                           '                        row.RelativeItem().AlignLeft().Text("   80:").Bold

                                                                           '                        row.RelativeItem().AlignRight().Text(0.ToString())
                                                                           '                    End Sub)


                                                                           'innerCol.Item().Row(Sub(row)
                                                                           '                        row.RelativeItem().AlignLeft().Text("   70:").Bold

                                                                           '                        row.RelativeItem().AlignRight().Text(0.ToString())

                                                                           '                    End Sub)

                                                                           'innerCol.Item().Row(Sub(row)
                                                                           '                        row.RelativeItem().AlignLeft().Text("   60:").Bold

                                                                           '                        row.RelativeItem().AlignRight().Text(0.ToString())
                                                                           '                    End Sub)

                                                                           'innerCol.Item().Row(Sub(row)
                                                                           '                        row.RelativeItem().AlignLeft().Text("   50:").Bold

                                                                           '                        row.RelativeItem().AlignRight().Text(0.ToString())
                                                                           '                    End Sub)

                                                                           'innerCol.Item().Row(Sub(row)
                                                                           '                        row.RelativeItem().AlignLeft().Text("   30:").Bold

                                                                           '                        row.RelativeItem().AlignRight().Text(0.ToString())
                                                                           '                    End Sub)
                                                                       End Sub)

                                                 End Sub)
                       End Sub)
    End Sub
End Class
