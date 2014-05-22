$packageName = 'RunscopeFiddlerExtension' 

try { 
  
   
  $destinationFolder = Join-Path ${Env:ProgramFiles(x86)} "Fiddler2\Scripts" 
  $targetFile = Join-Path $destinationFolder "RunscopeFiddler.dll"
  remove-item $targetFile
 
  Write-ChocolateySuccess "$packageName"
} catch {
  Write-ChocolateyFailure "$packageName" "$($_.Exception.Message)"
  throw 
}
