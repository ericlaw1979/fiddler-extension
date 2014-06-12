$packageName = 'RunscopeFiddlerExtension' 

try { 
  
   
  $r = get-item "HKLM:\Software\Microsoft\Fiddler2"
  $destinationFolder = $r.GetValue("LMScriptPath")
  $destinationFolder = $destinationFolder.Replace("""","") # Remove redundant surrouding quotes
  $targetFile = Join-Path $destinationFolder "RunscopeFiddler.dll"
  remove-item $targetFile
 
  Write-ChocolateySuccess "$packageName"
} catch {
  Write-ChocolateyFailure "$packageName" "$($_.Exception.Message)"
  throw 
}
