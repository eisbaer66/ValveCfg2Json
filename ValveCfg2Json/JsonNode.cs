namespace ValveCfg2Json;

public class JsonNode
{
    public string          Name   { get; set; }
    public string          Value  { get; set; }
    public IList<JsonNode> Childs { get; set; }
}