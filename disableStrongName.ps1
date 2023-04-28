#This script is FOR TEST ONLY. It bypasses StrongName verification
#for SQL assemblies, since they are delay signed the installer
#will fail on non-signed builds unless we add the exceptions.


#List of SQL public keys
$publicKeys = @(
        "*,b03f5f7f11d50a3a")
 
$publicKeys | ForEach-Object {
    New-Item -Path "HKLM:\SOFTWARE\Microsoft\StrongName\Verification\$_"             -Force | Out-Null
    New-Item -Path "HKLM:\SOFTWARE\Wow6432Node\Microsoft\StrongName\Verification\$_" -Force | Out-Null
}