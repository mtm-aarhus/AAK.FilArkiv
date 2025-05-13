### Build a new package version
Remember to update the project file with new version number. Then:

	dotnet pack -c Release

### Push package to GitHub

	 dotnet nuget push ./bin/Release/AAK.FilArkiv.{Version}.nupkg --source github --api-key "{Key}"