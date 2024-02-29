using System.Text.Json.Serialization;

namespace ValveCfg2Json;

[JsonSourceGenerationOptions(WriteIndented = true, GenerationMode = JsonSourceGenerationMode.Serialization)]
[JsonSerializable(typeof(IList<JsonNode>))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}