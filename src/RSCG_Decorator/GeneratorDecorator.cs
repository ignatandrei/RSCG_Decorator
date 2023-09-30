namespace RSCG_Decorator;

[Generator]
public class GeneratorDecorator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        
        //context.RegisterPostInitializationOutput(cnt =>
        //{
        //    cnt.AddSource("IDecoratorV1.cs", SourceGenerator.Helper.CopyCode.Copy.RSCG_DecoratorDecoratorRecognizersIDecoratorMethodV1);
        //    cnt.AddSource("MethodRecognizer", SourceGenerator.Helper.CopyCode.Copy.RSCG_DecoratorDecoratorRecognizersMethodRecognizer);
        //});
        var classesInterfaceV1 = context.SyntaxProvider.CreateSyntaxProvider(
                predicate: (s, _) => IsSyntaxTargetForGeneration(s),
                transform: (ctx, _) => GetSemanticTargetForGeneration(ctx))
            .Where(static m => m is not null)!
            ;

        var compilationAndData
            = context.CompilationProvider.Combine(classesInterfaceV1.Collect());

        context.RegisterSourceOutput(compilationAndData,
            (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    

    private void Execute(Compilation item1, ImmutableArray<Tuple<ClassDeclarationSyntax, INamedTypeSymbol>?> item2, SourceProductionContext spc)
    {
        if(item2.Length ==0 ) return;
        string[] IDecoratorMethodV1Names = new[]
        {
            "StartMethod",
            "ExceptionMethod",
            "EndMethod"
        };
        var classes = item2.Where(it => it != null && it.Item1!= null).Select(it=>it!).ToArray();
        foreach(var tpl in classes)
        {
            var cds = tpl.Item1;
            var data = new ClassData();

            var baseNamespace = cds.Parent as BaseNamespaceDeclarationSyntax;
            var name = baseNamespace?.Name.ToFullString();
            data.nameSpace = name;
            data.className = cds.Identifier.ValueText;
            
            var symbolClass = tpl.Item2;
            
            data.Interfaces=symbolClass.Interfaces
                
                .Select(it=>it.ToString())
                .Where(it=>!it.Contains(nameof(IDecoratorMethodV1)))
                .ToArray();

            var methods = symbolClass
                .GetMembers()
                .Where(it=>it.Kind == SymbolKind.Method)
                .Select(it=>it as IMethodSymbol)
                .Where(it=>it != null)
                .Select(it=>it!)
                .Where(it=>it.MethodKind == MethodKind.Ordinary)
                .Where(it=>it.DeclaredAccessibility == Accessibility.Public)
                .Where(it=>!IDecoratorMethodV1Names.Contains( it.Name))
                .ToArray()
                ;
            
            foreach(var method in methods)
            {
                var n = method.Name;
                var x = method.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                var m = new MethodData(method);
                Console.WriteLine(m.MethodDeclaration());
                Console.WriteLine(m.MethodCall());
                Console.WriteLine("---");


            }
            data.methods= methods.Select(it=>new MethodData(it)).ToArray();
            data.properties= symbolClass
                .GetMembers()
                .Where(it=>it.Kind==SymbolKind.Property)
                .Select(it=>it as IPropertySymbol)
                .Where(it=>it != null)
                .Select(it=>it!)
                .Select(it=> new PropertyData(it))
                .ToArray();

            var template = new Templates.MethodDecoratorV1(data);
            var result = template.Render();
            var fileName = $"{data.nameSpace}.{data.className}_{nameof(Templates.MethodDecoratorV1)}";
            spc.AddSource(fileName, result);

        }
    }

    private Tuple<ClassDeclarationSyntax,INamedTypeSymbol>? GetSemanticTargetForGeneration(GeneratorSyntaxContext ctx)
    {
        var cds = ctx.Node as ClassDeclarationSyntax;
        //var data = ctx.SemanticModel.GetSymbolInfo(ctx.Node).Symbol;
        if(cds == null ) return null;
        if (cds.Parent != null && (cds.Parent is not BaseNamespaceDeclarationSyntax)) return null;
        var data = ctx.SemanticModel.GetDeclaredSymbol(cds);
        if(data == null) return null;
        //var inter = data.AllInterfaces;
        //var x1 = data.GetMembers();
        //foreach(var s in x1)
        //{
        //    if(s is IPropertySymbol ps)
        //    {
        //        continue;
        //    }
        //    if(s is IMethodSymbol ms)
        //    {
        //        if(ms.pub)   
        //        continue;
        //    }
        //    var x = s.Name;
        //    var x2 = s.Name;
        //}
        return new Tuple<ClassDeclarationSyntax, INamedTypeSymbol>(cds,data);

    }

    private bool IsSyntaxTargetForGeneration(SyntaxNode s)
    {
        if(s is not ClassDeclarationSyntax cds) return false;
        if(cds.Parent != null && cds.Parent is not BaseNamespaceDeclarationSyntax) return false;
        if(cds.BaseList == null)return false;
        var types= cds.BaseList.Types.Select(t => t.ToFullString()).ToArray();
        if(types.Length == 0) return false;
        
        return types.Any(it=>it.Contains(nameof(IDecoratorMethodV1)));
    }
}
