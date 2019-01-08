
function Add-Nuget-Sources {
    Write-Host 'Adding NuGet sources...' -ForegroundColor Green
    
    .\nuget.exe sources Add -Name "Sitefinity public repo" -Source http://nuget.sitefinity.com/nuget
    .\nuget.exe sources Add -Name "Iris NuGet" -Source \\telerik.com\distributions\OfficialReleases\Sitefinity\IrisNuGet
    .\nuget.exe sources Add -Name "Sitefinity private repo" -Source \\telerik.com\distributions\OfficialReleases\Sitefinity\nuget
    
    Write-Host 'Done' -ForegroundColor Green
}

function Restore-Packages {
    Write-Host 'Restoring packages...' -ForegroundColor Green
    $solutionFiles = Get-ChildItem -Path ../ -Filter *.sln -Recurse -File
    $solutionFiles | ForEach-Object {
        .\nuget.exe restore $_.FullName 
    }
    
    Write-Host 'Done' -ForegroundColor Green
}

function Upgrade-Packages {
    $packagesFilter = 'Telerik.Sitefinity'
    $version = "11.2.6900.0"

    $packageConfigFiles = Get-ChildItem -Path ../ -Filter packages.config -Recurse -File 
    $packageConfigFiles | ForEach-Object {
        $configFile = $_

        If (!$configFile.DirectoryName.Contains("AttributeRouting")) {
            continue
        }

        Write-Host 'Upgrading Sitefinity packages for' $configFile.Directory '...' -ForegroundColor Green
    
        [XML]$configFileContents = Get-Content $configFile.FullName
        $sitefinityPackageElements = Select-XML -Xml $configFileContents -XPath "//package[contains(@id,'Telerik.Sitefinity')]"

        $testArray = [System.Collections.ArrayList]@()

        foreach ($element in $sitefinityPackageElements) {
            $nodeAttributes = $element.Node.Attributes
            $packageName = $nodeAttributes['id'].Value
            $packageVersion = $nodeAttributes['version'].Value

            If ($testArray.Contains($packageName)) {
                continue
            }
        
            if ($packageName.Equals('Telerik.Sitefinity.All')) {
                If (!$packageVersion.Equals($versionShort)) {
                    .\nuget.exe update $configFile.FullName $packageName -Version $version -NonInteractive
                }

                break
            }

            If ($packageVersion.Equals($versionShort)) {
                continue
            }

            If ($packageName.Equals('Telerik.Sitefinity.Feather')) {
                If (!$testArray.Contains('Telerik.Sitefinity.Feather.Core')) {
                    $testArray.Add('Telerik.Sitefinity.Feather.Core');
                }

                If (!$testArray.Contains('Telerik.Sitefinity.Mvc')) {
                    $testArray.Add('Telerik.Sitefinity.Mvc');
                }
            }

            If ($packageName.Equals('Telerik.Sitefinity.Feather.Core')) {
                If (!$testArray.Contains('Telerik.Sitefinity.Mvc')) {
                    $testArray.Add('Telerik.Sitefinity.Mvc');
                }
            }
                                
            .\nuget.exe update $configFile.FullName $packageName -Version $version -NonInteractive

            Copy-Item .\OpenAccessNuGet.targets -Destination $configFile.DirectoryName -ErrorAction SilentlyContinue
        }
    
        Write-Host 'Done' -ForegroundColor Green
    }
}

function Build-Solutions {
    Write-Host 'Building solutions...' -ForegroundColor Green
    $solutionFiles = Get-ChildItem -Path ../ -Filter *.sln -Recurse -File
    $solutionFiles | ForEach-Object {
        $sln = $_
    
        $msBuildExe = 'C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe'
        & "$($msBuildExe)" -clp:Summary -v:q "$($sln.FullName)" /t:Build /m
    }

    Write-Host 'Done' -ForegroundColor Green
}

#Add-Nuget-Sources
#Restore-Packages
Upgrade-Packages
#Build-Solutions