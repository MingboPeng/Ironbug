NEW_RELEASE_VERSION ?= 0.0.1


build-Grasshopper:
	dotnet build ./src/Ironbug.Grasshopper/Ironbug.Grasshopper.csproj /p:Configuration=Release /p:Platform=x64 /p:Version=$(NEW_RELEASE_VERSION) /restore
	mkdir -p installer/plugin-net8
	mv src/Ironbug.Grasshopper/bin/x64/Release/net8.0/* installer/plugin-net8/
	mkdir -p installer/plugin
	mv src/Ironbug.Grasshopper/bin/x64/Release/net48/* installer/plugin/
	mkdir -p installer/HVACTemplates
	cp -r doc/HVAC_GHTemplates/* installer/HVACTemplates

	ls -r installer

# run on macOS (Apple Silicon)
# restore is done separately: a build with "-f" + "/restore" would restore the referenced
# projects against the overridden framework and clobber their assets (NETSDK1005)
build-Grasshopper-mac:
	dotnet restore ./src/Ironbug.Grasshopper/Ironbug.Grasshopper.csproj
	dotnet build ./src/Ironbug.Grasshopper/Ironbug.Grasshopper.csproj -f net8.0 /p:Configuration=Release /p:Version=$(NEW_RELEASE_VERSION) --no-restore
	mkdir -p installer/plugin-mac
	mv src/Ironbug.Grasshopper/bin/Release/net8.0/* installer/plugin-mac/
	mkdir -p installer/HVACTemplates
	cp -r doc/HVAC_GHTemplates/* installer/HVACTemplates

	ls -r installer

build-console-win:
	dotnet build ./src/Ironbug.Console/Ironbug.Console.csproj /p:Configuration=Release /p:Platform=x64 /p:Version=$(NEW_RELEASE_VERSION) /restore
	ls ./src/Ironbug.Console/bin/x64/Release/
	7z a -tzip ironbug.console.win.zip ./src/Ironbug.Console/bin/x64/Release
	
	cp ./src/Ironbug.Console/bin/x64/Release/net8/* installer/plugin-net8
	cp ./src/Ironbug.Console/bin/x64/Release/net48/* installer/plugin/
	rm -r ./installer/plugin-net8/openstudio* ./installer/plugin-net8/OpenStudio*
	rm -r ./installer/plugin/openstudio* ./installer/plugin/OpenStudio*


build-console-linux:
	dotnet build ./src/Ironbug.Console/Ironbug.Console.csproj -a x64 /p:Configuration=Release /p:TargetFramework=net8 /p:Version=$(NEW_RELEASE_VERSION)
	ls ./src/Ironbug.Console/bin/Release/net8/linux-x64
	zip -r ironbug.console.linux.zip ./src/Ironbug.Console/bin/Release/net8/linux-x64

# run on macOS (Apple Silicon)
build-console-mac:
	dotnet build ./src/Ironbug.Console/Ironbug.Console.csproj -a arm64 /p:Configuration=Release /p:TargetFramework=net8 /p:Version=$(NEW_RELEASE_VERSION)
	ls ./src/Ironbug.Console/bin/Release/net8/osx-arm64
	zip -r ironbug.console.macos.zip ./src/Ironbug.Console/bin/Release/net8/osx-arm64

	cp ./src/Ironbug.Console/bin/Release/net8/osx-arm64/* installer/plugin-mac/
	rm -r ./installer/plugin-mac/openstudio* ./installer/plugin-mac/OpenStudio*

