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
    private IFirewallRule? _rule;
    private string _ruleName = "Dehumidifier";

    private async void Application_Startup(object sender, StartupEventArgs e)
    {
        var processes = Process.GetProcessesByName("Dehumidifier").ToList();
        if (processes.Count > 1)
        {
            Process.GetCurrentProcess().Kill();
        }

        NotifyIcon notifyIcon = new NotifyIcon();
        notifyIcon.Icon = new Icon("icon.ico");
        notifyIcon.Text = $"{_ruleName} v{Assembly.GetExecutingAssembly().GetName().Version}";
        notifyIcon.Visible = true;
        notifyIcon.ContextMenuStrip = new ContextMenuStrip();
        notifyIcon.ContextMenuStrip.Items.Add("Exit", null, (sender, e) => { Shutdown(); });

        string? path = await GetExecutablePathAsync();
        if (path is null) { Shutdown(); return; }

        _rule = FirewallManager.Instance.CreateApplicationRule(
            FirewallProfiles.Public | FirewallProfiles.Private | FirewallProfiles.Domain,
            _ruleName,
            FirewallAction.Block,
            path
        );
        _rule.Direction = FirewallDirection.Outbound;

        FirewallManager.Instance.Rules.Add(_rule);
    }

    private void Application_Shutdown(object sender, ExitEventArgs e)
    {
        foreach (var rule in FirewallManager.Instance.Rules)
        {
            if (rule.Name == _ruleName)
            {
                FirewallManager.Instance.Rules.Remove(rule);
            }
        }
    }

    private Task<string?> GetExecutablePathAsync()
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
        return Task.FromResult(result);
    }
}