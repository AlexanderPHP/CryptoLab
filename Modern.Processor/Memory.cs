using System;

namespace Modern.Processor
{
    public class Memory<T>
    {
        private bool _state;
        private T _number;

        public Memory(T num)
        {
            _number = num;
            _state = true;
        }

        public void Write(T e)
        {
            _number = e;
            _state = true;
        }

        public T Read()
        {
            _state = true;
            return _number;
        }

        public void Add(dynamic e)
        {
            if (!(e is Frac) && !(e is Complex) && !(e is Pnumber))
            {
                throw new ArgumentException(nameof(e));
            }

            _number = _number + e;
        }

        public void Clear()
        {
            _state = false;
            _number = default(T);
        }

        public string GetStateString()
        {
            return _state ? "_On" : "_Off";
        }

        public T GetNumber()
        {
            return _number;
        }
    }
}
