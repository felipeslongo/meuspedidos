using Android.App;
using MeusPedidos.Application;
using System.Globalization;

namespace MeusPedidos.AndroidApp
{
    [Application(
        AllowBackup = true,
        Icon = "@mipmap/ic_launcher",
        RoundIcon = "@mipmap/ic_launcher_round",
        SupportsRtl = true,
        Theme = "@style/AppTheme")]
    public class ApplicationX : Android.App.Application
    {
    }
}