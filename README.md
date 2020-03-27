TaskbarCovid19 / CoronaBand
-------------------------

A Windows taskbar deskband that shows COVID-19 stats.

![snap01](https://raw.githubusercontent.com/vhanla/TaskbarCovid19/master/.gitassets/snap01.png)
Shows chart popup info on mouse hover.

API used Coronavirus tracking API (https://github.com/ExpDev07/coronavirus-tracker-api) 

![snap02](https://raw.githubusercontent.com/vhanla/TaskbarCovid19/master/.gitassets/snap02.png)

This is a demonstration (proof of concept) of writing a deskband streamed for educational purposes at Twtich.

## Packages used (NUGET):

- LiveCharts
- Newtonsoft.Json
- RestSharp
- SimpleInjector

## How to install/uninstall

After compiling for your platform (x86/x64) register the deskband (dll) from elevated privileges command line.

Installation command:

`%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\regasm.exe /nologo /codebase "%~dp0CoronaBand.dll"`

Uninstall command:

`%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\regasm.exe /unregister /nologo /codebase "%~dp0CoronaBand.dll"`

Restart Explorer.exe after it.
