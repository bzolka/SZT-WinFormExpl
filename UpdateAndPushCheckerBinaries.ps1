Copy-Item ".\WinFormExpl-Test\WinFormExpl-Test\bin\Debug\WinFormExpl-Test.dll" -Destination "..\winforms-hf-uitest-binaries"
Push-Location -Path "..\winforms-hf-uitest-binaries"

try {
    git add -A
    git status
    $diff = git diff --cached | Out-String
    if ($diff -eq "")
    {
        Write-Host "Nothing to commit, exiting..."
        exit
    }
    
    $CommitMessage = Read-Host -Prompt 'Git commit message'
    git commit -m $CommitMessage
    git push
}
finally {
    Pop-Location  
}

