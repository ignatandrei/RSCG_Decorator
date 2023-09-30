namespace RSCG_DecoratorCommon;

public class MRUtils : IDisposable
{
    private readonly MethodRecognizer mr;
    private readonly IDecoratorMethodV1 decorator;

    public MRUtils(MethodRecognizer mr,IDecoratorMethodV1 decorator)
    {
        decorator.StartMethod(mr);
        this.mr = mr;
        this.decorator = decorator;
    }
    public void SendException(Exception ex)
    {
        decorator.ExceptionMethod(ex, mr);
    }
    public void Dispose()
    {
        decorator.EndMethod(mr);
    }
}
