#Copy-Item ".\WinFormExpl-Test\WinFormExpl-Test\bin\Debug\WinFormExpl-Test.dll" -Destination "..\winforms-hf-uitest-binaries"
# Update year below if requred
param (
    [string]$year = 2020,
    [string]$userName,  # GitHub username
    [switch]$all = $false,
    [switch]$a = $false, # Shortcut for all
    [switch]$clone = $false,
    [switch]$c = $false,  # Shortcut for clone    
    [switch]$build = $false,
    [switch]$b = $false # Shortcut for build

 )

Push-Location -Path "..\Work"

try {
    $clone = $clone -or $c -or $all -or $a
    $build = $build -or $b -or $all -or $a
    
    if (-not $clone -and -not  $build)
    {
        Write-Host "Nothing to do, please provide appropriate parameters"
    }
    
    if ($clone)
    {
         # Delete "hallg-mo" folder
        $studentFolder = "hallg-mo"
       
        if (Test-Path $studentFolder) {
            Remove-Item $studentFolder -Recurse -Force
        }
       
        if (Test-Path $studentFolder) {
            Write-Host "Folder " + $studentFolder + " could not be deleted before clone"  -ForegroundColor Red
            return
        }


        if ($userName -eq "")  {
            $userName = Read-Host -Prompt 'Enter GitHub user name of student'
            if ($userName -eq "")
            {
                Write-Host "GitHub user name can't be empty" -ForegroundColor Red
            }
        }
    
        $repoUrl = "https://github.com/bmeviauab00/hazi3-" + $year + "-" + $userName;
        git clone $repoUrl $studentFolder
    }
    
    if ($build)
    {
        dotnet build  ".\hallg-mo\Feladatok\WinFormExpl\ProjectFileForTest.csproj"
        if (-not $?) {
            Write-Host "dotnet build was unsuccessful" -ForegroundColor Red
        }
            
    }
}
finally {
    Pop-Location  
}
