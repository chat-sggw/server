Przywrócenie paczek nuget. Wymagane przed zbudowaniem aplikacji  

``` shell
nuget restore src\ChatSggw.sln
```

Budowanie projektu i automatyczny deploy do paczki ZIP. 
Profil DevPackageProfile tworzy paczkę zip w  `dist\dev\ChatSggw.API.zip` 
Profil ProdPackageProfile tworzy paczkę zip w  `dist\prod\ChatSggw.API.zip` 

``` shell
msbuild /p:DeployOnBuild=true /p:PublishProfile=DevPackageProfile src\ChatSggw.API\ChatSggw.API.csproj
msbuild /p:DeployOnBuild=true /p:PublishProfile=ProdPackageProfile src\ChatSggw.API\ChatSggw.API.csproj
```

Odpalanie testów xUnit 

``` shell
src\packages\xunit.runner.console.2.3.1\tools\net452\xunit.console.exe src\ChatSggw.DomainTests\bin\Debug\ChatSggw.DomainTests.dll
```

Deploy aplikacji do IIS. Przed uruchomieniem pliku potrzebne są credenciate do serwera i plik `ChatSggw.API.SetParameters.xml`. Plik z parametrami tworzy się automatycznie po zbudowaniu projektu i jest w katalogu  dist\dev\ (dist\prod\), w takim pliku trzeba wyedytować różne ustawienia aplikacji (np. connection string do bazy danych lub application name z IISa). Podczas deploymentu trzeba zedytowany plik podstawić i ustawić do niego ścieżkę w parametrze `-setParamFile:"\path\to\ChatSggw.API.SetParameters.xml"`

``` shell
msdeploy.exe -verb:sync ^
-source:package='dist\dev\ChatSggw.API.zip' ^
-allowUntrusted:true ^
-dest:auto,computerName="https://example.com:8172/msdeploy.axd",userName="exampleuser",password="examplepassword",authtype="Basic",includeAcls="False" ^
-disableLink:AppPoolExtension ^
-disableLink:ContentExtension ^
-disableLink:CertificateExtension ^
-enableRule:DoNotDeleteRule ^
-setParamFile:"\path\to\ChatSggw.API.SetParameters.xml" ^
-enableRule:AppOffline
