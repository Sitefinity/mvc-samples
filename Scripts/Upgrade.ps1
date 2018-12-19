
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

    $packageConfigFiles = Get-ChildItem -Path ../ -Filter packages.config -Recurse -File
    $packageConfigFiles | ForEach-Object {
        $configFile = $_
        $configFileContents = Get-Content $configFile.FullName
        #$configFileContentsNoParse = New-Object -TypeName System.Xml.XmlDocument
        #$configFileContentsNoParse.LoadXml($configFileContentsNoParse)

        Write-Host 'Upgrading Sitefinity packages for' $configFile.Directory '...' -ForegroundColor Green
    
        $sitefinityPackageElements = Select-XML -Xml $configFileContents -XPath "//package[contains(@id,'Telerik.Sitefinity')]"

        foreach ($element in $sitefinityPackageElements) {
            $nodeAttributes = $element.Node.Attributes
            $packageName = $nodeAttributes['id'].Value
            $packageVersion = $nodeAttributes['version'].Value
        
            if ($packageName.Equals('Telerik.Sitefinity.All')) {
                If (!$packageVersion.Equals($versionShort)) {
                    .\nuget.exe install $configFile.FullName $packageName -Version $version -NonInteractive
                }

                break
            }

            If ($packageVersion.Equals($versionShort)) {
                continue
            }
        
            .\nuget.exe install $configFile.FullName $packageName -Version $version -NonInteractive
        }
    
        Write-Host 'Done' -ForegroundColor Green
    }
}

function Build-Solutions {
    Write-Host 'Building solutions...' -ForegroundColor Green
    $solutionFiles | ForEach-Object {
        $sln = $_
    
        $msBuildExe = 'C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe'
        & "$($msBuildExe)" -clp:Summary -v:q "$($sln.FullName)" /t:Build /m
    }

    Write-Host 'Done' -ForegroundColor Green
}

#function Get-File-Content {
#    param([String]$path)
#
#    [System.Xml.XmlReader]$fileStream = [System.Xml.XmlReader]::Create(($path))
#    $byteArray = New-Object byte[] $fileStream.Length
#    $encoding = New-Object System.Text.UTF8Encoding $true
#    while ($fileStream.Read($byteArray, 0 , $byteArray.Length)) {
#        $result = $encoding.GetString($byteArray)
#    }
#    
#    $fileStream.Dispose()
#
#    return $fileStream.Read
#    #return $result
#}

$timer =  [System.Diagnostics.Stopwatch]::StartNew()
Restore-Packages

$restorePackagesElapsed = [System.Math]::Round($timer.Elapsed.TotalSeconds / 60, 2)
Write-Host 'Elapsed minutes: ', $restorePackagesElapsed

Upgrade-Packages

$updatePackagesElapsed = [System.Math]::Round($timer.Elapsed.TotalSeconds / 60, 2) - $restorePackagesElapsed
Write-Host 'Elapsed minutes: ', $updatePackagesElapsed

Build-Solutions

$buildElapsed = [System.Math]::Round($timer.Elapsed.TotalSeconds / 60, 2) - $restorePackagesElapsed - $updatePackagesElapsed
Write-Host 'Elapsed minutes: ', $buildElapsed