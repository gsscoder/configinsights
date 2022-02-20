# Configuration Insights

Tool and API to analyze configuration settings and try detect misconfiguration issues. This is an early development, incomplete and targeted only to Azure App Services.

## Install

**NOTE**: .NET 5.0 is required.

```powershell
# clone the repository
$ git clone https://github.com/gsscoder/configinsights

# change the working directory
$ cd configinsights

# build the executable
$ dotnet build -c release

# copy it to a directory of your convenience
$ cp -r .\artifacts\ConfigurationInsights.CLI\Release\net5.0 \XCOPY\configinsights

# put the directory in your path
$ $env:Path += ";\XCOPY\configinsights"

# test that the command can run
$ configchk --version
```

## Usage

```
$ az webapp config appsettings list -g mytestrg -n mytestfunc | configchk
Reading from standard input
JSON array parsed
Initiating settings analysis
[WRN]: "withdoublequote contains not allowed special characters or whitespace
[HINT] Aavoid special characters and whitespace in names except '_' and '-'
[WRN]: "withdoublequote is empty
Known name found
[ERR]: APPINSIGHTS_INSTRUMENTATIONKEY contains an invalid value
Known name found
APPLICATIONINSIGHTS_CONNECTION_STRING is OK
Possible name with semantic match found
[WRN]: AppTenantId may contain an invalid value
Known name found
AzureWebJobsStorage is OK
custom1 is OK
custom2 is OK
[WRN]: 'empty setting' contains not allowed special characters or whitespace
[HINT] Aavoid special characters and whitespace in names except '_' and '-'
[WRN]: 'empty setting' is empty
FUNCTIONS_EXTENSION_VERSION is OK
FUNCTIONS_WORKER_RUNTIME is OK
[WRN]: [very ' bad @ setting] contains not allowed special characters or whitespace
[HINT] Aavoid special characters and whitespace in names except '_' and '-'
[WRN]: [very ' bad @ setting] is empty
```
