Websocket client-server communication demo
==========================================

Simple solutions which demonstrates communication between server and client via websockets

3rd party library used:
- log4net 2.0.5
- Microsoft.Tpl.Dataflow 4.5.24
- Newtonsoft.Json 9.0.1
- vtortola.WebSocketListener 2.2.0.3

Notes
-----

vtortola.WebSocketListener is open source project licensed under MIT (https://github.com/vtortola/WebSocketListener)

Workflow
--------

- Compile solution <WSDemo.sln> (test env: Windows 8.1, .NET Framework 4.5, MS Visual Studio 2012)
- Configure 'WSDemo.Server.exe.config' and run websocket server <WSDemo.Server.exe> (console application)
- Configure 'WSDemo.Client.Win.exe.config' and run websocket client <WSDemo.Client.Win.exe> (windows forms application)
- Enter username in WS client
- Enter password in WS client
- Press 'Connect' in WS client
- Ensure user data file is generated and saved in websocket client' directory
- Press 'Disconnect' in WS client
- Press 'Delete key pair' in WS client
- Ensure user data file is deleted from websocket client' directory