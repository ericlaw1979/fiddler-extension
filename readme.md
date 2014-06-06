#Runscope Fiddler Extension

This project creates a Fiddler extension that can be used to take requests captured by Fiddler, send them to Runscope and create a URL that can be used to share the request with other people.

The output of this project is a set of DLLs that are ILMerged into a RunscopeFiddler.dll that needs to be copied into the c:\program files(x86)\Fiddler2\Scripts folder.

Once the plugin is in the scripts folder, Fiddler will discover the DLL on startup and a new menu option "Share Request with Runscope" will appear on the session list context menu in Fiddler.

The build folder contains a nuspec file and some Powershell scripts for building a Chocolatey package that will deploy the plugin.