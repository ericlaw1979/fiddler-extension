$packageName = 'RunscopeFiddlerExtension' 

try { 
  
  $installDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)" 
  $fileToInstall = Join-Path $installDir "RunscopeFiddler.dll"
  $r = get-item "HKLM:\Software\Microsoft\Fiddler2"
  $destinationFolder = $r.GetValue("LMScriptPath")
  copy-item $fileToInstall $destinationFolder
  

  Write-ChocolateySuccess "$packageName"
} catch {
  Write-ChocolateyFailure "$packageName" "$($_.Exception.Message)"
  throw 
}
