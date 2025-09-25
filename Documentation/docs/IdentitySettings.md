# Identity Settings
The Identity tab of the Settings dialog box allows you to set the NG9-1-1 identity of the PsapSimulator application and to configure the X.509 certificate that the application will use for SIPS, WSS and HTTPS.

## NG9-1-1 Identity
The application provides settings for Agency ID, Agent ID and Element ID. These identification settings are used primarily for NG9-1-1 event logging.

### Agency ID
The Agency ID identifies the agency to which a PSAP belongs to. Section 2.1.1 of NENA-STA-010.3f describes the Agency ID.

The default setting is: ng911test.net

### Agent ID
The Agent ID identifies an agent (call taker) within an agency. Section 2.1.2 of NENA-STA-010.3f describes the Agent ID.

The default setting is:  psapsimulator1@ng911test.net.

### Element ID
The Element ID is a logical name used to represent a physical implementation of a functional element. Section 2.1.3 of NENA-STA-010.3f describes the Element ID.

The default setting is: psapsimulator1.ng911test.net.

## X.509 Certificate
The application requires an X.509 certificate so that it can act as a server for SIP TLS and HTTPS requests. A certificate is also required if the application needs to authenticate itself as a client.

The application ships with a default self-signed X.509 certificate. The name of the certificate is PsapSimulator.pfx. This file contains a private key. A certificate file called PsapSimulator.cer is also distributed with the application. This file can be added to the trusted root if it is necessay to trust the PsapSimulator application.

If the Use Default Certificate checkbox is checked then the application will use the default certificate regardless of the Certificate File and Password settings in the Identity tab.

It is possible to use your own X.509 certificate. To do this, uncheck the Use Default Certificate checkbox and provide the certificate file path and password for your certificate.

