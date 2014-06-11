$packageName = 'RunscopeFiddlerExtension' 

try { 
  
   
  $r = get-item "HKLM:\Software\Microsoft\Fiddler2"
  $destinationFolder = $r.GetValue("LMScriptPath")
  $targetFile = Join-Path $destinationFolder "RunscopeFiddler.dll"
  remove-item $targetFile
 
  Write-ChocolateySuccess "$packageName"
} catch {
  Write-ChocolateyFailure "$packageName" "$($_.Exception.Message)"
  throw 
}
