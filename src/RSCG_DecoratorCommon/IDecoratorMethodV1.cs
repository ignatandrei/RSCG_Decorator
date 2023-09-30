namespace RSCG_DecoratorCommon;

public interface IDecoratorMethodV1
{
    void StartMethod(MethodRecognizer recognizer);
    void ExceptionMethod(System.Exception ex, MethodRecognizer recognizer);
    void EndMethod(MethodRecognizer recognizer);

}
