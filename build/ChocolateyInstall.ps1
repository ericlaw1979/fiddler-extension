$packageName = 'RunscopeFiddlerExtension' 

try { 
  
  $installDir = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)" 
  $fileToInstall = Join-Path $installDir "RunscopeFiddler.dll"
  $destinationFolder = Join-Path ${Env:ProgramFiles(x86)} "Fiddler2\Scripts" 
  copy-item $fileToInstall $destinationFolder
  

  Write-ChocolateySuccess "$packageName"
} catch {
  Write-ChocolateyFailure "$packageName" "$($_.Exception.Message)"
  throw 
}
