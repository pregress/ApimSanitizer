# ApimSanitizer
Tool that helps to sanitize open api defintion files to import them into Azure APIM.
This is to bypass the [limits](https://learn.microsoft.com/en-us/azure/api-management/api-management-api-import-restrictions#unsupported) of api management.

Currenlty only removes [links](https://swagger.io/docs/specification/v3_0/links/) from the definition.


# Install

## Global
```
dotnet tool install -g apimsanitizer
```

## In project
```
dotnet tool install dotnet-apimsanitizer --create-manifest-if-needed
```

# Usage

## CLI
```
apimsanitizer -f ./your/path/to/api-definition.yml
```

## PreBuild target in csproj
```(xml)
<Target Name="GenerateApimFile" BeforeTargets="Build">
    <Exec Command="dotnet apimsanitizer -f $(ProjectDir)/wwwroot/openapi.yml" />
</Target>
```

## Local testing

```
cd src
dotnet pack --configuration Release
dotnet tool install -g --add-source .\bin\Release apimsanitizer

apimsanitizer -f {path-to-openapi.yml}
```
