# NG9-1-1 Event Logging Settings
This dialog box allows you to add, edit or delete NG9-1-1 Event Logging Servers.

The Enable Event Logging checkbox enables (if checked) event logging for the PsapSimulator application. If this checkbox is checked then the PsapSimulator application will send NG9-1-1 events to each enabled NG9-1-1 Event Logging Server.

Click on the Add button to add settings for a new event logging server. To edit an existing event logging server, double click on the event logging server listed in the list box, or select the server in the list and then click on the Edit button.

To delete an event logging server, select it from the list box and then click on the Delete button.

## <a name="EventLoggerSettings">NG9-1-1 Event Logger Settings</a>
This dialog box allows you to configure the settings for a new event logging server or to edit the settings of an existing event logging server.

### Name
Specifies the name of the event logging server. Each server must have a unique name.

### Enabled
If checked then the PsapSimulator application will send NG9-1-1 events to the event logging server.

### Logger URI
Specifies the HTTP or HTTPS URI for the event logging server. Only the scheme and the host portion of the URI should be specified here. For example: `https://192.168.1.76:11000`.
