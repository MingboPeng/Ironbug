NEW_RELEASE_VERSION ?= 0.0.1


build-Grasshopper:
	dotnet build ./src/Ironbug.Grasshopper/Ironbug.Grasshopper.csproj /p:Configuration=Release /p:Platform=x64 /p:Version=$(NEW_RELEASE_VERSION) /restore
	mkdir -p installer/plugin
	mv src/Ironbug.Grasshopper/bin/x64/Release/net8-windows/* installer/plugin/
	mv src/Ironbug.Grasshopper/bin/x64/Release/net48 installer/plugin/
	mkdir -p installer/HVACTemplates
	cp doc/HVAC_GHTemplates/* installer/HVACTemplates -r

	ls installer -r

build-console-win:
	dotnet build ./src/Ironbug.Console/Ironbug.Console.csproj /p:Configuration=Release /p:Platform=x64 /p:Version=$(NEW_RELEASE_VERSION) /restore
	ls ./src/Ironbug.Console/bin/x64/Release/
	7z a -tzip ironbug.console.win.zip ./src/Ironbug.Console/bin/x64/Release
	
	cp ./src/Ironbug.Console/bin/x64/Release/net8/* installer/plugin
	cp ./src/Ironbug.Console/bin/x64/Release/net48/* installer/plugin/net48/
	rm ./installer/plugin/openstudio* -r
	rm ./installer/plugin/net48/openstudio* -r

	7z a -tzip installer/plugin/net48.zip ./installer/plugin/net48/*
	rm -r installer/plugin/net48

build-console-linux:
	dotnet build ./src/Ironbug.Console/Ironbug.Console.csproj -a x64 /p:Configuration=Release /p:TargetFramework=net8 /p:Version=$(NEW_RELEASE_VERSION)
	ls ./src/Ironbug.Console/bin/Release/net8/linux-x64
	zip -r ironbug.console.linux.zip ./src/Ironbug.Console/bin/Release/net8/linux-x64

