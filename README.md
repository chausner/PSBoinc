# PSBoinc
PowerShell module for managing BOINC clients on local and remote hosts

![PowerShell Gallery Version](https://img.shields.io/powershellgallery/v/PSBoinc)
[![license](https://img.shields.io/github/license/chausner/PSBoinc.svg)](https://github.com/chausner/PSBoinc/blob/master/LICENSE)

Installation
------------
Install via ```Install-Module PSBoinc -Scope CurrentUser```.

Usage
-----
RPC cmdlets operate in local or remote sessions. Local sessions are authenticated automatically:
```powershell
Enter-BoincSession
```
Remote sessions require the RPC password (can be looked up from gui_rpc_auth.cfg):
```powershell
Enter-BoincSession "192.168.0.11" "9096083b1c5a02d473a81784eb8e0862"
```
After a session has been opened, RPC cmdlets can be used to manage the client. Sessions are closed explicitly with ```Exit-BoincSession``` and implicitly when opening another session.

Help
----
Get a list of all cmdlets in PSBoinc:
```powershell
Get-Command -Module PSBoinc
```
Get help on command ```Add-BoincAccountManager```:
```powershell
Get-Help Add-BoincAccountManager -Detailed
```

Examples
--------
Do not receive new work from any project:
```powershell
Get-BoincProject | Set-BoincProject -NoMoreWork
```
Abort all tasks that are at least 3 days behind their deadline:
```powershell
Get-BoincTask | where { ([DateTimeOffset]::Now - $_.ReportDeadline).TotalDays -ge 3 } | Stop-BoincTask
```
Show a list of all current tasks from Rosetta@home and PrimeGrid, grouped by project and sorted by progress:
```powershell
Get-BoincTask -Project rosetta*,prime* | sort FractionDone -Descending | fl -GroupBy ProjectUrl -Property WorkunitName,FractionDone
```
Suspend tasks after their next checkpoint and shutdown the machine:
```powershell
while ($true)
{
    Get-BoincTask | where { ($_.CurrentCpuTime - $_.CheckpointCpuTime).TotalSeconds -le 30 } | Suspend-BoincTask
    if (-not (Get-BoincTask | where Suspended -NE $true))
    {
        break
    }
    Start-Sleep -Seconds 10
}
Stop-Computer
```
Attach multiple remote clients to a project:
```powershell
"pc-01","pc-02","pc-03" | foreach { 
    Enter-BoincSession $_ "9096083b1c5a02d473a81784eb8e0862"
    Add-BoincProject "http://boinc.bakerlab.org/rosetta" "johndoe@example.com" "P@ssW0rD!"
}
```
Show benchmark results for a list of remote clients:
```powershell
"pc-01","pc-02","pc-03" | foreach { 
    Enter-BoincSession $_
    $hostinfo = Get-BoincHostInformation
    [pscustomobject]@{Machine=$_;Whetstone=$hostinfo.FloatingPointOperations;Drhystone=$hostinfo.IntegerOperations}
}
```
