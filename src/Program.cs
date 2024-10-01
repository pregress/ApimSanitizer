using System.CommandLine;
using ApimSanitizer;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;


var fileOption = new Option<FileInfo?>(
    name: "--file",
    description: "The input open api definition file in yaml format.")
{
    IsRequired = true,
    Arity = ArgumentArity.ExactlyOne,
    
};
fileOption.AddAlias("-f");

var rootCommand = new RootCommand("Apim sanitizier, sanitize open api definitions to import them into Azure APIM");
rootCommand.AddOption(fileOption);

int exitCode = (int)CommandExitCodes.Success;

rootCommand.SetHandler((file) =>
{
    if (file == null)
    {
        exitCode = (int) CommandExitCodes.InvalidFile;
        return;
    }

    if (!file.Exists)
    {
        Console.WriteLine($"Error: File {file.FullName}  does not exist.");
        exitCode = (int)CommandExitCodes.FileNotExists;
        return;
    }

    if (!string.Equals(file.Extension, ".yml", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine($"Error: File: {file.FullName} must be a .yml.");
        exitCode = (int)CommandExitCodes.InvalidExtension;
        return;
    }

    var outputFilePath = Path.ChangeExtension(file.FullName, null) + "_apim.yml";

    try
    {
        var yamlContent = File.ReadAllText(file.FullName);
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var openApiDoc = deserializer.Deserialize<object>(yamlContent);

        RemoveLinksRecursive(openApiDoc);

        var modifiedYaml = serializer.Serialize(openApiDoc);

        File.WriteAllText(outputFilePath, modifiedYaml);

        Console.WriteLine($"Processed file saved as {outputFilePath}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error processing file: {ex.Message}");
        exitCode = (int) CommandExitCodes.Exception;
    }

}, fileOption);


await rootCommand.InvokeAsync(args);


return exitCode;


static void RemoveLinksRecursive(object? node)
{
    if (node is null)
        return;

    if (node is System.Collections.IDictionary dict)
    {
        if (dict.Contains("links"))
        {
            dict.Remove("links");
        }

        foreach (var key in dict.Keys)
        {
            RemoveLinksRecursive(dict[key]);
        }
    }
    else if (node is System.Collections.IEnumerable list)
    {
        foreach (var item in list)
        {
            RemoveLinksRecursive(item);
        }
    }
}