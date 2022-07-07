Param(
   [string]$user,
   [string]$pwd,
   [string]$service,
   [string]$serverName
)
try
{
if(!$user -Or !$pwd -Or !$service -Or !$serverName) {
 throw "One or more parameters missing"
}

Write-Host Securing password $pwd
$pwdSecure = ConvertTo-SecureString -String $pwd -AsPlainText -Force
Write-Host Creating credentials for $user
$Credential = New-Object -TypeName "System.Management.Automation.PSCredential" -ArgumentList $user, $pwdSecure

Write-Host Creating session on $serverName for user $user
$sess = New-PSSession -ComputerName $serverName -Credential $Credential

$sessionComputer = Invoke-Command -Session $sess -ScriptBlock { Get-WmiObject -Class Win32_Computersystem } 
Write-Host Session computer name $sessionComputer.Name

Write-Host Starting service $service
Invoke-Command -Session $sess -ScriptBlock { param($service) if($service){ get-service "$service" | start-service } } -ArgumentList($service)

Write-Host Exiting session
Remove-PSSession $sess
}
catch
{
	Write-Host Error trying to start service: $_.Exception.Message
}
