echo "Creating merged RunscopeFiddler.dll"
. C:\Chocolatey\bin\ILMerge.bat /out:RunscopeFiddler.dll /lib:..\RunscopeFiddlerExtension\bin\debug RunscopeFiddlerExtension.dll System.Net.Http.Formatting.dll System.Web.Http.dll Newtonsoft.Json.dll RunscopeOAuthDesktop.dll RunscopeWebPack.dll /allowdup /v4
echo ""
echo "Done."
echo "Now you can create the chocolatey package using cpack"