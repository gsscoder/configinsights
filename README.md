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
$ az login
$ az webapp config appsettings list -g YOUR_RES_GROUP -n YOUR_APP_SERVICE | configchk
```

![configchk output](/assets/configchk-out.png)
