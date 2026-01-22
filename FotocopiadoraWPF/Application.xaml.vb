Imports System.Windows.Threading

Class Application

    ' Application-level events, such as Startup, Exit, and DispatcherUnhandledException
    ' can be handled in this file.
    Private Sub New()
        'Register Syncfusion<sup style="font-size:70%">&reg;</sup> License
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JGaF5cXGpCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWH1ccHRRR2JdVU1wWkVWYEs=")
    End Sub

    Protected Overrides Sub OnStartup(e As StartupEventArgs)
        MyBase.OnStartup(e)

        Infrastructure.DatabaseInitializer.Inicializar()
        Infrastructure.DatabaseInitializer.InsertarDatosIniciales()
    End Sub


    Private Sub App_DispatcherUnhandledException(
        sender As Object,
        e As DispatcherUnhandledExceptionEventArgs) _
        Handles Me.DispatcherUnhandledException

        MessageBox.Show(
            "ERROR NO CONTROLADO:" & vbCrLf & vbCrLf & e.Exception.Message,
            "Error",
            MessageBoxButton.OK,
            MessageBoxImage.Error)

        e.Handled = True
    End Sub


End Class
