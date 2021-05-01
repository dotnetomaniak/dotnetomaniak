[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $ResourceGroupName,
    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $ServicePlanName,
    [Parameter(Mandatory=$true)][ValidateSet("Free","Shared","Basic","Standard")] 
    [String] 
    $Tier
)

Set-AzureRmAppServicePlan -ResourceGroupName $ResourceGroupName -Name $ServicePlanName -Tier $Tier > $null