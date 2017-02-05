using System;

namespace Modern.Processor
{
    public class Proc<T> where T : IHasArithmeticOperations
    {
        public T LopRes { get; private set; }
        public T Rop { get; private set; }
        public Oprtn Operation { get; private set; }

        public enum Oprtn
        {
            None,
            Add,
            Sub,
            Mul,
            Dvd
        }

        public enum Func
        {
            Rev,
            Sqr
        }

        public Proc(T lopRes, T rop)
        {
            ReSet(lopRes, rop);
        }

        public void ReSet(T lopRes, T rop)
        {
            LopRes = lopRes;
            Rop = rop;
            Operation = Oprtn.None;
        }

        public void OprtnClear()
        {
            Operation = Oprtn.None;
        }

        public void SetLopRes(T e)
        {
            Rop = e;
        }

        public void SetOprtn(Oprtn oprtn)
        {
            Operation = oprtn;
        }

        public void OprtnRun()
        {
            switch (Operation)
            {
                case Oprtn.None:
                    break;
                case Oprtn.Add:
                    LopRes.Add(Rop);
                    break;
                case Oprtn.Sub:
                    LopRes.Sub(Rop);
                    break;
                case Oprtn.Mul:
                    LopRes.Mul(Rop);
                    break;
                case Oprtn.Dvd:
                    LopRes.Div(Rop);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Operation), Operation, null);
            }
        }

        public void FuncRun(Func func)
        {
            switch (func)
            {
                case Func.Rev:
                    break;
                case Func.Sqr:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(func), func, null);
            }
        }
    }
}
