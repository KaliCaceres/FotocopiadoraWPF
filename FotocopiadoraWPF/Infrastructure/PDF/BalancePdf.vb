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

                                                     col.Item().Text("CENTRO UNIVERSITARIO").FontSize(20).Bold().AlignCenter()
                                                     col.Item().Text("FOTOCOPIADORA").FontSize(16).Bold().AlignCenter()
                                                     col.Item().Text($"Periodo: {_balance.IdMes}/{_balance.Anio}").AlignCenter()
                                                     col.Item().Column(Sub(innerCol)
                                                                           col.Item().Text($"Inicio: {_balance.FechaInicio:dd/MM/yyyy HH:mm}")
                                                                           col.Item().Text($"Fin: {_balance.FechaFin:dd/MM/yyyy HH:mm}")
                                                                       End Sub)

                                                     '--------------------------------------------------------------------------------------------------------------------------------------------
                                                     col.Item().Column(Sub(innerCol)
                                                                           innerCol.Item().Text("CONTADORES").FontSize(13).Bold()
                                                                           innerCol.Item().LineHorizontal(1)
                                                                       End Sub)

                                                     col.Item().PaddingLeft(20).Column(Sub(innerCol)

                                                                                           innerCol.Item().Row(Sub(row)
                                                                                                                   row.RelativeItem().AlignLeft().Text("Toshiba Studio (307) Equipo 1:").Bold().FontSize(12)
                                                                                                               End Sub)

                                                                                           innerCol.Item().LineHorizontal(1)
                                                                                       End Sub)

                                                     col.Item().PaddingLeft(40).Column(Sub(innerCol)
                                                                                           innerCol.Item().Row(Sub(row)
                                                                                                                   row.RelativeItem().AlignLeft().Text("Inicio:").Bold().FontSize(10)
                                                                                                                   row.RelativeItem().AlignRight().Text(_balance.ContadorEquipo1Inicio.ToString).FontSize(10)
                                                                                                               End Sub)

                                                                                           innerCol.Item().Row(Sub(row)
                                                                                                                   row.RelativeItem().AlignLeft().Text("Fin:").Bold().FontSize(10)
                                                                                                                   row.RelativeItem().AlignRight().Text(_balance.ContadorEquipo1Final.ToString).FontSize(10)
                                                                                                               End Sub)
                                                                                           innerCol.Item().Row(Sub(row)
                                                                                                                   row.RelativeItem().AlignLeft().Text("Subtotal:").ExtraBold().FontSize(10)
                                                                                                                   row.RelativeItem().AlignRight().Text(_balance.ContadorEquipo1Diferencia.ToString).Bold().FontSize(10)
                                                                                                               End Sub)
                                                                                       End Sub)

                                                     col.Item().PaddingLeft(20).Column(Sub(innerCol)

                                                                                           innerCol.Item().Row(Sub(row)
                                                                                                                   row.RelativeItem().AlignLeft().Text("Toshiba Studio (307) Equipo 2:").Bold().FontSize(12)
                                                                                                               End Sub)
                                                                                           innerCol.Item().LineHorizontal(1)
                                                                                       End Sub)

                                                     col.Item().PaddingLeft(40).Column(Sub(innerCol)
                                                                                           innerCol.Item().Row(Sub(row)
                                                                                                                   row.RelativeItem().AlignLeft().Text("Inicio:").Bold().FontSize(10)
                                                                                                                   row.RelativeItem().AlignRight().Text(_balance.ContadorEquipo2Inicio.ToString).FontSize(10)
                                                                                                               End Sub)

                                                                                           innerCol.Item().Row(Sub(row)
                                                                                                                   row.RelativeItem().AlignLeft().Text("Fin:").Bold().FontSize(10)
                                                                                                                   row.RelativeItem().AlignRight().Text(_balance.ContadorEquipo2Final.ToString).FontSize(10)
                                                                                                               End Sub)
                                                                                           innerCol.Item().Row(Sub(row)
                                                                                                                   row.RelativeItem().AlignLeft().Text("Subtotal:").ExtraBold().FontSize(10)
                                                                                                                   row.RelativeItem().AlignRight().Text(_balance.ContadorEquipo2Diferencia.ToString).Bold().FontSize(10)
                                                                                                               End Sub)
                                                                                       End Sub)

                                                     col.Item().PaddingLeft(20).Column(Sub(innerCol)

                                                                                           innerCol.Item().Row(Sub(row)
                                                                                                                   row.RelativeItem().AlignLeft().Text("Total:").Bold().FontSize(12)
                                                                                                                   row.RelativeItem().AlignRight().Text(_balance.TotalContadores.ToString).Bold().FontSize(12)
                                                                                                               End Sub)
                                                                                           innerCol.Item().LineHorizontal(1)
                                                                                       End Sub)
                                                     '--------------------------------------------------------------------------------------------------------------------------------------------
                                                     col.Item().Column(Sub(innerCol)
                                                                           innerCol.Item().Text("FOTOCOPIAS").FontSize(13).Bold()
                                                                           innerCol.Item().LineHorizontal(1)
                                                                       End Sub)

                                                     col.Item().PaddingLeft(20).Column(Sub(innerCol)
                                                                                           innerCol.Item().Row(Sub(row)
                                                                                                                   row.RelativeItem().AlignLeft().Text("Contadores:").FontSize(10).Bold()
                                                                                                                   row.RelativeItem().AlignRight().Text(_balance.TotalContadores.ToString).FontSize(10)
                                                                                                               End Sub)
                                                                                           innerCol.Item().Column(Sub(row)
                                                                                                                      row.Item().Text("Registradas").FontSize(10).Bold()
                                                                                                                      row.Item().LineHorizontal(1)
                                                                                                                  End Sub)
                                                                                           innerCol.Item().PaddingLeft(30).Row(Sub(row)
                                                                                                                                   row.RelativeItem().AlignLeft().Text("Pagadas:").FontSize(10)
                                                                                                                                   row.RelativeItem().AlignRight().Text(_balance.TotalPagadas.ToString).FontSize(10)
                                                                                                                               End Sub)
                                                                                           innerCol.Item().PaddingLeft(30).Row(Sub(row)
                                                                                                                                   row.RelativeItem().AlignLeft().Text("Deudores:").FontSize(10)
                                                                                                                                   row.RelativeItem().AlignRight().Text(_balance.TotalDeudor.ToString).FontSize(10)
                                                                                                                               End Sub)
                                                                                           innerCol.Item().PaddingLeft(30).Row(Sub(row)
                                                                                                                                   row.RelativeItem().AlignLeft().Text("Perdidas:").FontSize(10)
                                                                                                                                   row.RelativeItem().AlignRight().Text(_balance.TotalPerdida.ToString).FontSize(10)
                                                                                                                               End Sub)
                                                                                           innerCol.Item().PaddingLeft(30).Row(Sub(row)
                                                                                                                                   row.RelativeItem().AlignLeft().Text("Subtotal:").FontSize(10).Bold()
                                                                                                                                   row.RelativeItem().AlignRight().Text(_balance.Registradas.ToString).FontSize(10).Bold()
                                                                                                                               End Sub)

                                                                                           innerCol.Item().Row(Sub(row)
                                                                                                                   row.RelativeItem().AlignLeft().Text("No registradas:").FontSize(10).Bold()
                                                                                                                   row.RelativeItem().AlignRight().Text(_balance.NoRegistradas.ToString).FontSize(10).Bold()
                                                                                                               End Sub)
                                                                                       End Sub)



                                                     '--------------------------------------------------------------------------------------------------------------------------------------------

                                                     col.Item().Column(Sub(innerCol)
                                                                           innerCol.Item().Text("MOVIMIENTOS DE CAJA").FontSize(13).Bold()
                                                                           innerCol.Item().LineHorizontal(1)
                                                                       End Sub)

                                                     col.Item().PaddingLeft(20).Column(Sub(innerCol)

                                                                                           innerCol.Item().Row(Sub(row)
                                                                                                                   row.RelativeItem().AlignLeft().Text("Egreso:").Bold().FontSize(12)
                                                                                                               End Sub)

                                                                                           innerCol.Item().LineHorizontal(1)
                                                                                       End Sub)

                                                     col.Item().PaddingLeft(40).Column(Sub(innerCol)

                                                                                           For Each m In _balance.movimientosCajaEgreso

                                                                                               innerCol.Item().Row(Sub(row)

                                                                                                                       row.RelativeItem(2).Text(m.Fecha.ToString("dd/MM HH:mm")).FontSize(10)

                                                                                                                       row.RelativeItem(3).Text(m.Motivo).FontSize(10)

                                                                                                                       row.RelativeItem(2).AlignRight().Text(m.MetodoPago).FontSize(10)

                                                                                                                       row.RelativeItem(2).AlignRight().Text(m.Monto.ToString("N0")).FontSize(10)

                                                                                                                   End Sub)
                                                                                           Next

                                                                                       End Sub)

                                                     col.Item().PaddingLeft(20).Column(Sub(innerCol)

                                                                                           innerCol.Item().Row(Sub(row)
                                                                                                                   row.RelativeItem().AlignLeft().Text("Ingreso:").Bold().FontSize(12)
                                                                                                               End Sub)

                                                                                           innerCol.Item().LineHorizontal(1)
                                                                                       End Sub)

                                                     col.Item().PaddingLeft(40).Column(Sub(innerCol)

                                                                                           For Each m In _balance.movimientosCajaIngreso

                                                                                               innerCol.Item().Row(Sub(row)

                                                                                                                       row.RelativeItem(2).Text(m.Fecha.ToString("dd/MM HH:mm")).FontSize(10)

                                                                                                                       row.RelativeItem(3).Text(m.Motivo).FontSize(10)

                                                                                                                       row.RelativeItem(2).AlignRight().Text(m.MetodoPago).FontSize(10)

                                                                                                                       row.RelativeItem(2).AlignRight().Text(m.Monto.ToString("N0")).FontSize(10)

                                                                                                                   End Sub)

                                                                                           Next

                                                                                       End Sub)

                                                     '--------------------------------------------------------------------------------------------------------------------------------------------

                                                     col.Item().Column(Sub(innerCol)
                                                                           innerCol.Item().Text("CAJA").FontSize(13).Bold()
                                                                           innerCol.Item().LineHorizontal(1)
                                                                       End Sub)

                                                     col.Item().PaddingLeft(20).Column(Sub(innerCol)

                                                                                           innerCol.Item().Row(Sub(row)
                                                                                                                   row.RelativeItem().AlignLeft().Text("Efectivo:").Bold().FontSize(12)
                                                                                                               End Sub)

                                                                                           innerCol.Item().LineHorizontal(1)
                                                                                       End Sub)

                                                     col.Item().PaddingLeft(40).Column(Sub(innerCol)
                                                                                           innerCol.Item().Row(Sub(row)
                                                                                                                   row.RelativeItem().AlignLeft().Text("Inicio:").FontSize(10)
                                                                                                                   row.RelativeItem().AlignRight().Text(_balance.EfectivoInicio.ToString).FontSize(10)
                                                                                                               End Sub)
                                                                                           innerCol.Item().PaddingLeft(60).Row(Sub(row)
                                                                                                                                   row.RelativeItem().AlignLeft().Text("Ingresos:").FontSize(10)
                                                                                                                                   row.RelativeItem().AlignRight().Text("+" + _balance.TotalIngresoEfectivo.ToString).FontSize(10)
                                                                                                                               End Sub)

                                                                                           innerCol.Item().PaddingLeft(60).Row(Sub(row)
                                                                                                                                   row.RelativeItem().AlignLeft().Text("Egresos:").FontSize(10)
                                                                                                                                   row.RelativeItem().AlignRight().Text("-" + _balance.TotalEgresoEfectivo.ToString).FontSize(10)
                                                                                                                               End Sub)

                                                                                           innerCol.Item().Row(Sub(row)
                                                                                                                   row.RelativeItem().AlignLeft().Text("Fin:").FontSize(10)
                                                                                                                   row.RelativeItem().AlignRight().Text(_balance.EfectivoFinal.ToString).FontSize(10)
                                                                                                               End Sub)
                                                                                           innerCol.Item().Row(Sub(row)
                                                                                                                   row.RelativeItem().AlignLeft().Text("Subtotal:").ExtraBold().FontSize(10)
                                                                                                                   row.RelativeItem().AlignRight().Text(_balance.EfectivoDiferencia.ToString).ExtraBold().FontSize(10)
                                                                                                               End Sub)
                                                                                       End Sub)

                                                     col.Item().PaddingLeft(20).Column(Sub(innerCol)

                                                                                           innerCol.Item().Row(Sub(row)
                                                                                                                   row.RelativeItem().AlignLeft().Text("Transferencia:").Bold().FontSize(12)
                                                                                                               End Sub)
                                                                                           innerCol.Item().LineHorizontal(1)
                                                                                       End Sub)

                                                     col.Item().PaddingLeft(40).Column(Sub(innerCol)
                                                                                           innerCol.Item().Row(Sub(row)
                                                                                                                   row.RelativeItem().AlignLeft().Text("Inicio:").FontSize(10)
                                                                                                                   row.RelativeItem().AlignRight().Text(_balance.TransferenciaInicio.ToString).FontSize(10)
                                                                                                               End Sub)
                                                                                           innerCol.Item().PaddingLeft(60).Row(Sub(row)
                                                                                                                                   row.RelativeItem().AlignLeft().Text("Ingresos:").FontSize(10)
                                                                                                                                   row.RelativeItem().AlignRight().Text("+" + _balance.TotalIngresoTransferencia.ToString).FontSize(10)
                                                                                                                               End Sub)

                                                                                           innerCol.Item().PaddingLeft(60).Row(Sub(row)
                                                                                                                                   row.RelativeItem().AlignLeft().Text("Egresos:").FontSize(10)
                                                                                                                                   row.RelativeItem().AlignRight().Text("-" + _balance.TotalEgresoTransferencia.ToString).FontSize(10)
                                                                                                                               End Sub)

                                                                                           innerCol.Item().Row(Sub(row)
                                                                                                                   row.RelativeItem().AlignLeft().Text("Fin:").FontSize(10)
                                                                                                                   row.RelativeItem().AlignRight().Text(_balance.TransferenciaFinal.ToString).FontSize(10)
                                                                                                               End Sub)
                                                                                           innerCol.Item().Row(Sub(row)
                                                                                                                   row.RelativeItem().AlignLeft().Text("Subtotal:").ExtraBold().FontSize(10)
                                                                                                                   row.RelativeItem().AlignRight().Text(_balance.TransferenciaDiferencia.ToString).ExtraBold().FontSize(10)
                                                                                                               End Sub)
                                                                                       End Sub)
                                                     col.Item().PaddingLeft(20).Column(Sub(innerCol)

                                                                                           innerCol.Item().Row(Sub(row)
                                                                                                                   row.RelativeItem().AlignLeft().Text("Total:").Bold().FontSize(12)
                                                                                                                   row.RelativeItem().AlignRight().Text(_balance.TotalCaja.ToString).Bold().FontSize(10)
                                                                                                               End Sub)
                                                                                           innerCol.Item().LineHorizontal(1)
                                                                                       End Sub)
                                                     'Facultades
                                                     'col.Item().Column(Sub(innerCol)
                                                     '                      innerCol.Item().Text("FACULTADES").FontSize(10).Bold()
                                                     '                      innerCol.Item().LineHorizontal(1)
                                                     '                  End Sub)

                                                     'col.Item().Column(Sub(innerCol)

                                                     '                      innerCol.Item().Row(Sub(row)
                                                     '                                              row.RelativeItem().AlignLeft().Text("   Centro Universitario:").Bold

                                                     '                                              row.RelativeItem().AlignRight().Text(0.ToString())
                                                     '                                          End Sub)


                                                     '                      innerCol.Item().Row(Sub(row)
                                                     '                                              row.RelativeItem().AlignLeft().Text("   IUNIR:").Bold

                                                     '                                              row.RelativeItem().AlignRight().Text(0.ToString())

                                                     '                                          End Sub)

                                                     '                      innerCol.Item().Row(Sub(row)
                                                     '                                              row.RelativeItem().AlignLeft().Text("   UCEL:").Bold

                                                     '                                              row.RelativeItem().AlignRight().Text(0.ToString())
                                                     '                                          End Sub)

                                                     '                      innerCol.Item().Row(Sub(row)
                                                     '                                              row.RelativeItem().AlignLeft().Text("   UGR:").Bold

                                                     '                                              row.RelativeItem().AlignRight().Text(0.ToString())
                                                     '                                          End Sub)

                                                     '                      innerCol.Item().Row(Sub(row)
                                                     '                                              row.RelativeItem().AlignLeft().Text("   UNR:").Bold

                                                     '                                              row.RelativeItem().AlignRight().Text(0.ToString())
                                                     '                                          End Sub)
                                                     '                      innerCol.Item().Row(Sub(row)
                                                     '                                              row.RelativeItem().AlignLeft().Text("   UNVM:").Bold

                                                     '                                              row.RelativeItem().AlignRight().Text(0.ToString())
                                                     '                                          End Sub)

                                                     '                  End Sub)

                                                     'col.Item().Column(Sub(innerCol)
                                                     '                      innerCol.Item().Text("FOTOCOPIAS POR PRECIO").FontSize(10).Bold()
                                                     '                      innerCol.Item().LineHorizontal(1)
                                                     '                  End Sub)

                                                     'col.Item().Column(Sub(innerCol)

                                                     '                      innerCol.Item().Row(Sub(row)
                                                     '                                              row.RelativeItem().AlignLeft().Text("   100:").Bold

                                                     '                                              row.RelativeItem().AlignRight().Text(0.ToString())
                                                     '                                          End Sub)


                                                     '                      innerCol.Item().Row(Sub(row)
                                                     '                                              row.RelativeItem().AlignLeft().Text("   70:").Bold

                                                     '                                              row.RelativeItem().AlignRight().Text(0.ToString())

                                                     '                                          End Sub)

                                                     '                      innerCol.Item().Row(Sub(row)
                                                     '                                              row.RelativeItem().AlignLeft().Text("   60:").Bold

                                                     '                                              row.RelativeItem().AlignRight().Text(0.ToString())
                                                     '                                          End Sub)

                                                     '                      innerCol.Item().Row(Sub(row)
                                                     '                                              row.RelativeItem().AlignLeft().Text("   50:").Bold

                                                     '                                              row.RelativeItem().AlignRight().Text(0.ToString())
                                                     '                                          End Sub)

                                                     '                      innerCol.Item().Row(Sub(row)
                                                     '                                              row.RelativeItem().AlignLeft().Text("   30:").Bold

                                                     '                                              row.RelativeItem().AlignRight().Text(0.ToString())
                                                     '                                          End Sub)
                                                     '                  End Sub)

                                                     'col.Item().Column(Sub(innerCol)
                                                     '                      innerCol.Item().Text("DEUDORES").FontSize(10).Bold()
                                                     '                      innerCol.Item().LineHorizontal(1)
                                                     '                  End Sub)

                                                     'col.Item().Column(Sub(innerCol)

                                                     'innerCol.Item().Row(Sub(row)
                                                     '                        row.RelativeItem().AlignLeft().Text("   100:").Bold

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
                                                     'End Sub)

                                                 End Sub)
                       End Sub)
    End Sub
End Class
