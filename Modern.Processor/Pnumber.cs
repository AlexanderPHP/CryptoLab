using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Modern.Processor
{
    public class Pnumber : IHasArithmeticOperations
    {
        private byte _b;
        private double A { get; set; }

        public byte B
        {
            get { return _b; }
            set
            {
                if(2 <= value && value <= 16)
                {
                    _b = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        public byte C { get; set; }

        public Pnumber(double a, byte b, byte c)
        {
            A = a;
            B = b;
            C = c;
        }

        public Pnumber(string a, byte b, byte c)
        {
            var matches = Regex.Match(a, @"^([+-])?([A-F0-9]+)(\.)?(?(3)([A-F0-9]+))$");
            if (!matches.Success)
            {
                throw new ArgumentException("String value must be a P-Number.");
            }

            var sign = false;
            if (matches.Groups[1].Success)
            {
                sign = matches.Groups[1].Value == "-";
            }

            var integer = matches.Groups[2].Value;
            if (integer.Any(ch => CharToInt(ch) > b))
            {
                throw new ArgumentException("Invalid base " + nameof(b));
            }

            var frac = "0";
            if (matches.Groups[3].Success)
            {
                frac = matches.Groups[4].Value;
                if (frac.Any(ch => CharToInt(ch) > b))
                {
                    throw new ArgumentException("Invalid base " + nameof(b));
                }
            }
            A = 0;
            B = b;
            C = c;
            for (var i = 0; i < integer.Length; i++)
            {
                A += CharToInt(integer[i]) * (int)Math.Pow(b, integer.Length - i - 1);
            }

            for (var i = 0; i < frac.Length; i++)
            {
                A += CharToInt(frac[i]) * Math.Pow(b, -(i + 1));
            }

            if (sign)
            {
                A = -A;
            }
        }

        public Pnumber Copy()
        {
            return new Pnumber(A, B, C);
        }

        public static Pnumber operator +(Pnumber a, Pnumber b)
        {
            if (a.B != b.B || a.C != b.C)
            {
                throw new ArgumentException();
            }

            return new Pnumber(a.A + b.A, a.B, a.C);
        }
        public static Pnumber operator -(Pnumber a, Pnumber b)
        {
            if (a.B != b.B || a.C != b.C)
            {
                throw new ArgumentException();
            }

            return new Pnumber(a.A - b.A, a.B, a.C);
        }

        public static Pnumber operator *(Pnumber a, Pnumber b)
        {
            if (a.B != b.B || a.C != b.C)
            {
                throw new ArgumentException();
            }

            return new Pnumber(a.A * b.A, a.B, a.C);
        }

        public static Pnumber operator /(Pnumber a, Pnumber b)
        {
            if (Math.Abs(b.A) < double.Epsilon)
            {
                throw new DivideByZeroException();
            }

            if (a.B != b.B || a.C != b.C)
            {
                throw new ArgumentException();
            }

            return new Pnumber(a.A - b.A, a.B, a.C);
        }

        public dynamic Reverse()
        {
            if (Math.Abs(A) < double.Epsilon)
            {
                throw new DivideByZeroException();
            }

            return new Pnumber(1 / A, B, C);
        }

        public dynamic Square()
        {
            return new Pnumber(A * A, B, C);
        }

        public override string ToString()
        {
            var sign = A < 0;
            var integer = (int)Math.Abs(A);
            var frac = Math.Abs(A) - integer;
            var build1 = new StringBuilder();
            while (Math.Abs(integer) > double.Epsilon)
            {
                build1.Insert(0, IntToChar(integer % B));
                integer /= B;
            }
            var build2 = new StringBuilder();
            for (var i = 0; i < C; i++)
            {
                var tmp = Math.Pow(B, -(i + 1));
                var n = 0;
                while (tmp * (n + 1) <= frac)
                {
                    n++;
                }
                frac -= tmp * n;
                build2.Append(IntToChar(n));
            }
            if (build1.ToString().Equals(" "))
            {
                build1.Append('0');
            }

            if (sign)
            {
                build1.Insert(0, '-');
            }

            return build1 + "." + build2;
        }

        public void SetExString(string c)
        {
            C = byte.Parse(c);
        }

        public void SetBaseString(string b)
        {
            B = byte.Parse(b);    
        }

        public int CharToInt(char c)
        {
            c = char.ToLower(c);
            if ('0' <= c && c <= '9')
            {
                return c - '0';
            }

            if ('a' <= c && c <= 'f')
            {
                return c - 'a' + 10;
            }

            throw new ArgumentOutOfRangeException(nameof(c));
        }

        public char IntToChar(int n)
        {
            if (0 <= n && n <= 9)
            {
                return (char)(n + '0');
            }

            if (10 <= n && n <= 15)
            {
                return (char)('A' + n - 10);
            }

            throw new ArgumentOutOfRangeException(nameof(n));
        }

        public void ToDefault()
        {
            A = 0;
            B = 2;
            C = 0;
        }

        public dynamic Add(dynamic b)
        {
            if (!(b is Pnumber))
            {
                throw new ArgumentException();
            }

            return this + b;
        }

        public dynamic Sub(dynamic b)
        {
            if (!(b is Pnumber))
            {
                throw new ArgumentException();
            }

            return this - b;
        }

        public dynamic Mul(dynamic b)
        {
            if (!(b is Pnumber))
            {
                throw new ArgumentException();
            }

            return this * b;
        }

        public dynamic Div(dynamic b)
        {
            if (!(b is Pnumber))
            {
                throw new ArgumentException();
            }

            return this / b;
        }
    }
}
