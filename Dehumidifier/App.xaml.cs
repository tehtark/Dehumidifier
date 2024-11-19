using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using WindowsFirewallHelper;

namespace Dehumidifier;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    private IFirewallRule _rule;

    public App()
    {
        string? path = GetExecutablePath();
        if (path is null) { Shutdown(); return; }

        _rule = FirewallManager.Instance.CreateApplicationRule(
            FirewallProfiles.Public | FirewallProfiles.Private | FirewallProfiles.Domain,
            "Dehumidifier",
            FirewallAction.Block,
            path
        );
        _rule.Direction = FirewallDirection.Outbound;

        NotifyIcon notifyIcon = new NotifyIcon();
        notifyIcon.Icon = new Icon("icon.ico");
        notifyIcon.Text = $"Dehumidifier v{Assembly.GetExecutingAssembly().GetName().Version}";
        notifyIcon.Visible = true;
        notifyIcon.ContextMenuStrip = new ContextMenuStrip();
        notifyIcon.ContextMenuStrip.Items.Add("Exit", null, (sender, e) => { Shutdown(); });
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        FirewallManager.Instance.Rules.Add(_rule);
    }

    private void Application_Shutdown(object sender, ExitEventArgs e)
    {
        if (FirewallManager.Instance.Rules.Contains(_rule))
        {
            FirewallManager.Instance.Rules.Remove(_rule);
        }
    }

    private string? GetExecutablePath()
    {
        string registryKey = @"SOFTWARE\WOW6432Node\Valve\Steam";
        string? result = null;
        using (RegistryKey? key = Registry.LocalMachine.OpenSubKey(registryKey))
        {
            if (key != null)
            {
                object? value = key.GetValue("InstallPath");

                if (value is not null)
                {
                    result = value.ToString() + @"\Steam.exe";
                }
            }
        }
        return result;
    }
}