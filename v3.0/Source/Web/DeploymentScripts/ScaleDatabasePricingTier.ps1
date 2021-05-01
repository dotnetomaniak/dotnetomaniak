[CmdletBinding()]
param (
	[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $ServicePrincipalApplicationID,
	[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $ServicePrincipalPass,
	[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $ServicePrincipalTenantID,
    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $ResourceGroupName,
    [Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $SQLServerName,
	[Parameter(Mandatory=$true)][ValidateNotNullOrEmpty()]
    [String]
    $SQLDBName,
    [Parameter(Mandatory=$true)][ValidateSet("Basic","Standard","Premium")] 
    [String] 
    $DBTier,
	[Parameter(Mandatory=$true)][ValidateSet("5","10","20","50","100","200","400","800")] 
    [String] 
    $DBCapacity,
	[Parameter(Mandatory=$true)][ValidateSet("100MB","500MB",
			"1GB","2GB","5GB",
			"10GB","20GB","30GB","40GB","50GB",
			"100GB","150GB","200GB","250GB","300GB","400GB","500GB","750GB"
			,"1TB")] 
    [String] 
    $DBMaxSize
)

az login --service-principal --username $ServicePrincipalApplicationID --password $ServicePrincipalPass --tenant $ServicePrincipalTenantID > $null
az sql db update -g $ResourceGroupName -s $SQLServerName -n $SQLDBName --tier $DBTier --capacity $DBCapacity --max-size $DBMaxSize > $null