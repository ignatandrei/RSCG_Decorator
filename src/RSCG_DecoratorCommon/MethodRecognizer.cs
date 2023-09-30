namespace RSCG_DecoratorCommon;
public struct MethodRecognizer
{
    public MethodRecognizer(string nameClass, string nameMethod)
    {
        NameClass = nameClass;
        NameMethod = nameMethod;
    }
    public string? FileName { get; set; }
    public int Line { get; set; }
    public string NameClass { get; }
    public string NameMethod { get; }
    public Dictionary<string, object> ValueTypeParameters { get; set; } = new();
    public string UniqueId
    {
        get
        {
            return $"Method:{NameMethod} from Class:{NameClass} on File:{FileName} Line:{Line}";
        }
    }
    public string ValueTypeParametersString
    {
        get
        {
            if (ValueTypeParameters.Count == 0) return "";
            return string.Join(",",ValueTypeParameters
                .Select(x =>$"{x.Key} = {x.Value}")
                    .ToArray());
        }
    }
}

