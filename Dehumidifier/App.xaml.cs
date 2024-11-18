using System.Windows;
using WindowsFirewallHelper;

namespace Dehumidifier;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    private IFirewallRule rule;

    public App()
    {
        rule = FirewallManager.Instance.CreateApplicationRule(
            FirewallProfiles.Public | FirewallProfiles.Private | FirewallProfiles.Domain,
            "SteamBlock",
            FirewallAction.Block,
            "C:\\Program Files (x86)\\Steam\\Steam.exe"
        );
        rule.Direction = FirewallDirection.Outbound;

        NotifyIcon notifyIcon = new NotifyIcon();
        notifyIcon.Icon = new Icon("icon.ico");
        notifyIcon.Visible = true;
        notifyIcon.ContextMenuStrip = new ContextMenuStrip();
        notifyIcon.ContextMenuStrip.Items.Add("Exit", null, (sender, e) => { Shutdown(); });
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        FirewallManager.Instance.Rules.Add(rule);
    }

    private void Application_Shutdown(object sender, ExitEventArgs e)
    {
        FirewallManager.Instance.Rules.Remove(rule);
    }
}