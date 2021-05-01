[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $ResourceGroupName,
	[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $WebAppServiceName
)

$slots = Get-AzureRmWebAppSlot -ResourceGroupName $ResourceGroupName -Name $WebAppServiceName
@($slots).GetEnumerator() | Remove-AzureRmWebAppSlot -Force > $null