properties {
	$base_dir = resolve-path .
	$build_dir = "$base_dir\build"
	$source_dir = "$base_dir\src"
	$result_dir = "$build_dir\results"
	$global:config = "debug"
	$tag = $(git tag -l --points-at HEAD)
	$revision = @{ $true = "{0:00000}" -f [convert]::ToInt32("0" + $env:APPVEYOR_BUILD_NUMBER, 10); $false = "local" }[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];
	$suffix = @{ $true = ""; $false = "ci-$revision"}[$tag -ne $NULL -and $revision -ne "local"]
	$commitHash = $(git rev-parse --short HEAD)
	$buildSuffix = @{ $true = "$($suffix)-$($commitHash)"; $false = "$($branch)-$($commitHash)" }[$suffix -ne ""]
    $versionSuffix = @{ $true = "--version-suffix=$($suffix)"; $false = ""}[$suffix -ne ""]
}


task default -depends local
task local -depends compile, test
task ci -depends clean, release, local, pack, publish

task publish {
	exec { dotnet publish $source_dir\CaseManagement.CMMN.Host\CaseManagement.CMMN.Host.csproj -c $config -o $result_dir\services\CaseManagementApi }
	exec { dotnet publish $source_dir\CaseManagement.BPMN.Host\CaseManagement.BPMN.Host.csproj -c $config -o $result_dir\services\BpmnApi }
	exec { dotnet publish $source_dir\CCaseManagement.HumanTask.Host\CaseManagement.HumanTask.Host.csproj -c $config -o $result_dir\services\HumanTaskApi }
	exec { npm install $source_dir\CaseManagement.Website --prefix $source_dir\CaseManagement.Website }
	exec { npm run build-azure --prefix $source_dir\CaseManagement.Website }
	exec { dotnet publish $source_dir\CaseManagement.Website -c $config -o $result_dir\services\CaseManagementWebsite }
	exec { npm install $source_dir\CaseManagement.Tasklist.Website --prefix $source_dir\CaseManagement.Tasklist.Website }
	exec { npm run build-azure --prefix $source_dir\CaseManagement.Tasklist.Website }
	exec { dotnet publish $source_dir\CaseManagement.Tasklist.Website -c $config -o $result_dir\services\TaskListWebsite }
}

task clean {
	rd "$source_dir\artifacts" -recurse -force  -ErrorAction SilentlyContinue | out-null
	rd "$base_dir\build" -recurse -force  -ErrorAction SilentlyContinue | out-null
}

task release {
    $global:config = "release"
}

task compile -depends clean {
	echo "build: Tag is $tag"
	echo "build: Package version suffix is $suffix"
	echo "build: Build version suffix is $buildSuffix" 
	
	exec { dotnet --version }
	exec { dotnet --info }

	exec { msbuild -version }
	
	exec { dotnet restore .\CaseManagement.sln }
    exec { dotnet build .\CaseManagement.sln -c $config --version-suffix=$buildSuffix }
}
 
task pack -depends compile {
	exec { dotnet pack $source_dir\CaseManagement.BPMN\CaseManagement.BPMN.csproj -c $config --no-build $versionSuffix --output $result_dir }
	exec { dotnet pack $source_dir\CaseManagement.BPMN.AspNetCore\CaseManagement.BPMN.AspNetCore.csproj -c $config --no-build $versionSuffix --output $result_dir }
	exec { dotnet pack $source_dir\CaseManagement.BPMN.Common\CaseManagement.BPMN.Common.csproj -c $config --no-build $versionSuffix --output $result_dir }
	exec { dotnet pack $source_dir\CaseManagement.BPMN.Persistence.EF\CaseManagement.BPMN.Persistence.EF.csproj -c $config --no-build $versionSuffix --output $result_dir }
	exec { dotnet pack $source_dir\CaseManagement.BPMN.Persistence.EF\CaseManagement.BPMN.Persistence.EF.csproj -c $config --no-build $versionSuffix --output $result_dir }
	exec { dotnet pack $source_dir\CaseManagement.CMMN\CaseManagement.CMMN.csproj -c $config --no-build $versionSuffix --output $result_dir }
	exec { dotnet pack $source_dir\CaseManagement.CMMN.AspNetCore\CaseManagement.CMMN.AspNetCore.csproj -c $config --no-build $versionSuffix --output $result_dir }
	exec { dotnet pack $source_dir\CaseManagement.CMMN.Persistence.EF\CaseManagement.CMMN.Persistence.EF.csproj -c $config --no-build $versionSuffix --output $result_dir }
	exec { dotnet pack $source_dir\CaseManagement.Common\CaseManagement.Common.csproj -c $config --no-build $versionSuffix --output $result_dir }
	exec { dotnet pack $source_dir\CaseManagement.Common.SqlServer\CaseManagement.Common.SqlServer.csproj -c $config --no-build $versionSuffix --output $result_dir }
	exec { dotnet pack $source_dir\CaseManagement.HumanTask\CaseManagement.HumanTask.csproj -c $config --no-build $versionSuffix --output $result_dir }
	exec { dotnet pack $source_dir\CaseManagement.HumanTask.AspNetCore\CaseManagement.HumanTask.AspNetCore.csproj -c $config --no-build $versionSuffix --output $result_dir }
	exec { dotnet pack $source_dir\CaseManagement.HumanTask.Persistence.EF\CaseManagement.HumanTask.Persistence.EF.csproj -c $config --no-build $versionSuffix --output $result_dir }
}

task test {
    Push-Location -Path $base_dir\tests\CaseManagement.BPMN.Acceptance.Tests

    try {
        exec { & dotnet test -c $config -v n --no-build --no-restore }
    } finally {
        Pop-Location
    }
	
    Push-Location -Path $base_dir\tests\CaseManagement.BPMN.Tests

    try {
        exec { & dotnet test -c $config -v n --no-build --no-restore }
    } finally {
        Pop-Location
    }

    Push-Location -Path $base_dir\tests\CaseManagement.CMMN.Acceptance.Tests

    try {
        exec { & dotnet test -c $config -v n --no-build --no-restore }
    } finally {
        Pop-Location
    }
	
    Push-Location -Path $base_dir\tests\CaseManagement.CMMN.Tests

    try {
        exec { & dotnet test -c $config -v n --no-build --no-restore }
    } finally {
        Pop-Location
    }
	
    Push-Location -Path $base_dir\tests\CaseManagement.HumanTasks.Acceptance.Tests

    try {
        exec { & dotnet test -c $config -v n --no-build --no-restore }
    } finally {
        Pop-Location
    }
}