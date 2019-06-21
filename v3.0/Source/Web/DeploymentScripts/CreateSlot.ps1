[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $ResourceGroupName,
    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $ServicePlanName,
	[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $WebAppServiceName,
    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String] 
    $SlotName
)

New-AzureRmWebAppSlot -ResourceGroupName $ResourceGroupName -Name $WebAppServiceName -AppServicePlan $ServicePlanName -Slot $SlotName > $null