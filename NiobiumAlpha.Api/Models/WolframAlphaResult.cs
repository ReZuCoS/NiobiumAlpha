using System.Text.Json.Serialization;

public class WolframAlphaResult
{
    [JsonPropertyName("queryresult")]
    public QueryResult? QueryResult { get; set; }
}

public class QueryResult
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("error")]
    public bool Error { get; set; }

    [JsonPropertyName("numpods")]
    public int Numpods { get; set; }

    [JsonPropertyName("datatypes")]
    public string? Datatypes { get; set; }

    [JsonPropertyName("timedout")]
    public string? Timedout { get; set; }

    [JsonPropertyName("timedoutpods")]
    public string? Timedoutpods { get; set; }

    [JsonPropertyName("timing")]
    public double Timing { get; set; }

    [JsonPropertyName("parsetiming")]
    public double Parsetiming { get; set; }

    [JsonPropertyName("parsetimedout")]
    public bool Parsetimedout { get; set; }

    [JsonPropertyName("recalculate")]
    public string? Recalculate { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("host")]
    public string? Host { get; set; }

    [JsonPropertyName("server")]
    public string? Server { get; set; }

    [JsonPropertyName("related")]
    public string? Related { get; set; }

    [JsonPropertyName("version")]
    public string? Version { get; set; }

    [JsonPropertyName("inputstring")]
    public string? Inputstring { get; set; }

    [JsonPropertyName("pods")]
    public Pod[]? Pods { get; set; }
}

public class Pod
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("scanner")]
    public string? Scanner { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("position")]
    public int Position { get; set; }

    [JsonPropertyName("error")]
    public bool Error { get; set; }

    [JsonPropertyName("numsubpods")]
    public int Numsubpods { get; set; }

    [JsonPropertyName("primary")]
    public bool Primary { get; set; }

    [JsonPropertyName("subpods")]
    public Subpod[]? Subpods { get; set; }

    [JsonPropertyName("expressiontypes")]
    public Expressiontypes? Expressiontypes { get; set; }

    [JsonPropertyName("states")]
    public State[]? States { get; set; }
}

public class Subpod
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("img")]
    public Img? Img { get; set; }

    [JsonPropertyName("plaintext")]
    public string? Plaintext { get; set; }
}

public class Img
{
    [JsonPropertyName("src")]
    public string? Src { get; set; }

    [JsonPropertyName("alt")]
    public string? Alt { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("themes")]
    public string? Themes { get; set; }

    [JsonPropertyName("colorinvertable")]
    public bool Colorinvertable { get; set; }

    [JsonPropertyName("contenttype")]
    public string? Contenttype { get; set; }
}

public class Expressiontypes
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class State
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("input")]
    public string? Input { get; set; }

    [JsonPropertyName("stepbystep")]
    public bool Stepbystep { get; set; }

    [JsonPropertyName("buttonstyle")]
    public string? Buttonstyle { get; set; }
}
