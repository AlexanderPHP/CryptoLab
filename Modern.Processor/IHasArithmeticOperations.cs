namespace Modern.Processor
{
    public interface IHasArithmeticOperations
    {
        dynamic Add(dynamic b);
        dynamic Sub(dynamic b);
        dynamic Mul(dynamic b);
        dynamic Div(dynamic b);
        dynamic Reverse();
        dynamic Square();
    }
}