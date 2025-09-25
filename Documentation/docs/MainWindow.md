# The Main Application Window
The main application window shows a list of calls and provides some basic information for each incoming call such who the call is from and call state.

This window also provides buttons for controlling calls and buttons for controlling the application.

## Call Controls

### Answer
Answers the call that has been ringing the longest. If there are no calls in the ringing state then this button picks up the call that has been in the Auto-Answered state the longest.

The state of the answered call changes to the On-Line state.

If there is another call in the On-Line state then that call is placed on hold.

This button does not automatically display the [Call Window](CallForm.md). If the call has only audio media then you can hear the caller and communicate with the caller via the microphone. If the call has video or text media, then you must display the Call Window in order to interact with the caller.

### Show
Displays the Call Window for currently selected call in the call list.

You can also show the Call Window for any call by double-clicking on the call in the call list display area.

### Hold
Places the selected call in the call list On-Hold.

### End Call
Ends the selected call in the call list.

### End All
Ends all calls currently being handled by the application regardless of call state.

## Miscellaneous Controls

### PSAP States
The PSAP States button displays a dialog box that allows the user to change the Element State, Service State and Queue State and then to notify any NG9-1-1 functional elements that are subscribed to these states.

After you change one or more state settings, click on the Notify button to force the application to send a NOTIFY request to each subscriber.

The PSAP states remain at their current settings until they are manually changed again.

The initial settings are:
- Element State: Normal
- Service State: Normal
- Security Posture: Green
- Queue State: Active

## Application Controls

### Start/Stop
Starts or stops the application's interfaces. If the application is not currently listening, it will not respond to call requests and all of its NG9-1-1 interfaces will be inactive.

**Note:** The first time that you click on the Start button after starting the application, it may take 4-5 seconds for the services managed by the application to actually start up.

### Settings
Displays the Settings dialog box so you can change the application's configuration settings.

### Close
Closes the application.




