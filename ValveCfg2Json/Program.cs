using System.Text.Json;
using AutoMapper;
using ValveCfg2Json;
using ValveFormat.Superpower;

if (args.Length == 0)
{
    Console.WriteLine($"ERROR: no cfg files provided, aborting");
    return 1;
}

var mapper = new Mapper(new MapperConfiguration(e => e.CreateMap<Node, JsonNode>()));
mapper.ConfigurationProvider.AssertConfigurationIsValid();

foreach (var cfgFilename in args)
{
    try
    {
        await Convert(cfgFilename);
    }
    catch (Exception e)
    {
        Console.WriteLine($"unhandled exception processing '{cfgFilename}', skipping this file: {e}");
    }
}

return 0;

async Task Convert(string cfgFilename)
{
    if (!File.Exists(cfgFilename))
    {
        Console.WriteLine($"WARNING: could not find file '{cfgFilename}', skipping this file.");
        return;
    }

    //read cfg
    var cfg     = await File.ReadAllTextAsync(cfgFilename);
    var success = ValveParser.TryParse(cfg, out IList<Node> nodes, out var error, out var errorPosition);
    if (!success)
    {
        Console.WriteLine($"ERROR processing '{cfgFilename}', skipping this file: {error}");
        return;
    }

    //map to JsonNode (removing Parent-property)
    var jsonNodes = mapper.Map<IList<JsonNode>>(nodes);

    //write json
    var jsonTypeInfo = SourceGenerationContext.Default.IListJsonNode;
    var json         = JsonSerializer.Serialize(jsonNodes, jsonTypeInfo);
    var jsonFilename = cfgFilename + ".json";
    await File.WriteAllTextAsync(jsonFilename, json);

    Console.WriteLine($"converted '{cfgFilename}' to '{jsonFilename}'");
}