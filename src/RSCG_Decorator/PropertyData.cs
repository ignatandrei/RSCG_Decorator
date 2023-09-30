namespace RSCG_Decorator;

internal class PropertyData
{
    private readonly IPropertySymbol propertySymbol;
    private string Name;
    private string Type;
    public bool CanCallSetMethod;
    public bool CanCallGetMethod;

    public PropertyData(IPropertySymbol propertySymbol)
    {
        this.propertySymbol = propertySymbol;
        Name = propertySymbol.Name;
        Type = propertySymbol.Type.ToDisplayString();
        var getAcces = propertySymbol.GetMethod?.DeclaredAccessibility;
        CanCallGetMethod = (getAcces == Accessibility.Public || getAcces == Accessibility.Internal);
        var setAcces = propertySymbol.GetMethod?.DeclaredAccessibility;
        CanCallSetMethod = (setAcces == Accessibility.Public || setAcces == Accessibility.Internal);
    }

    public string PropertyCode()
    {

        var get = CanCallGetMethod? $$"""
                get{
                return original.{{Name}};
                }
        """:"";
        var set = CanCallSetMethod?$$"""
            set{
                original.{{Name}}=value;
            }            
        
        """:"";
        return $$"""
            public {{Type}} {{Name}} {
                {{get}}
                {{set}}            
            } 
            """;

    }

}


