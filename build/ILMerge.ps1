echo "Creating merged RunscopeFiddler.dll"
. C:\Chocolatey\bin\ILMerge.bat /out:RunscopeFiddler.dll /lib:..\RunscopeFiddlerExtension\bin\debug RunscopeFiddlerExtension.dll Microsoft.Owin.dll Microsoft.Owin.Host.HttpListener.dll Microsoft.Owin.Hosting.dll Owin.dll System.Net.Http.Formatting.dll System.Web.Http.dll Newtonsoft.Json.dll RunscopeOAuthDesktop.dll RunscopeWebPack.dll  System.Web.Http.Owin.dll /allowdup /v4
echo ""
echo "Done."
echo "Now you can create the chocolatey package using cpack"