namespace RSCG_Decorator;

internal class ClassData
{
    public string Version = ThisAssembly.Info.Version;
    public string? nameSpace;
    public string? className;
    public MethodData[] methods = new MethodData[0];
    public PropertyData[] properties= new PropertyData[0];
    public string[] Interfaces= new string[0];
}
