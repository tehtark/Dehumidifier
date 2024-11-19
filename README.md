# Dehumidifier
The ultimate tool for sharing your Steam library. It allows multiple users to enjoy the same single-player game without online conflicts. Simply start the program, and it blocks Steam's internet access with a firewall rule. Exit through the tray icon when you're finished, and internet access is restored.
## Usage
Dehumidifier helps you manage Steam internet access on your Windows PC. Here's how to use it effectively:

### Blocking Steam Internet Access:

- Launch Dehumidifier. It will run discreetly from your system tray.
- Dehumidifier automatically creates Windows Defender Firewall rules to block outgoing connections for Steam. This prevents Steam from accessing the internet.

### Restoring Internet Access: 

- To restore Steam's internet connection, right-click on the Dehumidifier tray icon and click "Exit."
- Important: Abruptly closing Dehumidifier may leave the firewall rules active, preventing Steam from connecting. Always exit Dehumidifier through the tray icon to ensure rules are removed.

### Manual Rule Removal:

- If you prefer not to use Dehumidifier again, you can manually remove the firewall rules it created.
- Open Windows Defender Firewall and navigate to the "Outbound Rules" section. Locate the rules named "Dehumidifier" and delete them.

### Troubleshooting:

- If you're unable to play a shared game even after launching Dehumidifier, try restarting your Steam client. This usually resolves any conflicts.
- Report any issues or feature requests on the GitHub repository.
### [Change Log](https://github.com/tehtark/Dehumidifier/blob/master/CHANGELOG.md)



