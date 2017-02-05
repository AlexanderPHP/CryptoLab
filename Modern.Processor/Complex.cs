using System;
using System.Text.RegularExpressions;

namespace Modern.Processor
{
    public class Complex : IHasArithmeticOperations
    {
        public double Re { get; set; }
        public double Im { get; set; }

        public Complex(double re, double im)
        {
            Re = re;
            Im = im;
        }

        public Complex(string complex)
        {
            var matches = Regex.Match(complex, @"([+|\-]?\d+\.?\d*)([+|\-]?\d+\.?\d*)");
            if (matches.Success)
            {
                Re = Convert.ToDouble(matches.Groups[1].ToString());
                Im = Convert.ToDouble(matches.Groups[2].ToString());
            }
            else
            {
                throw new ArgumentException("String value must be a complex.");
            }
        }

        public Complex Copy()
        {
            return new Complex(Re, Im);
        }

        public static Complex operator +(Complex a, Complex b)
        {
            return new Complex(a.Re + b.Re, a.Im + b.Im);
        }

        public static Complex operator -(Complex a, Complex b)
        {
            return new Complex(a.Re - b.Re, a.Im - b.Im);
        }

        public static Complex operator *(Complex a, Complex b)
        {
            return new Complex(a.Re * b.Re - a.Im * b.Im, a.Re * b.Im + a.Im * b.Re);
        }

        public static Complex operator /(Complex a, Complex b)
        {
            return new Complex((a.Re * b.Re + a.Im * b.Im) / (b.Re * b.Re + b.Im * b.Im)
                             , (b.Re * a.Im - a.Re * b.Im) / (b.Re * b.Re + b.Im * b.Im));
        }

        public static Complex operator !(Complex a)
        {
            return a - new Complex(0, 0);
        }

        public static bool operator ==(Complex a, Complex b)
        {
            return a.Re == b.Re && Math.Abs(a.Im - b.Im) < double.Epsilon;
        }

        public static bool operator !=(Complex a, Complex b)
        {
            return a.Re != b.Re && Math.Abs(a.Im - b.Im) > double.Epsilon;
        }

        public dynamic Square()
        {
            return this * this;
        }

        public double Pow(int n)
        {
            return Math.Pow(Mod(), n) * (Math.Cos(n * Radians()) + Math.Sin(n * Radians()));
        }

        public double Sqrt(double n, int k)
        {
            var a = (Radians() + 2 * Math.PI * k) / n;
            return Math.Pow(Mod(), 1 / n) * (Math.Cos(a) + Math.Sin(a));
        }

        public dynamic Reverse()
        {
            return new Complex(Re / (Re * Re + Im * Im), Im / (Re * Im + Re * Im));
        }

        public double Mod()
        {
            return Math.Sqrt(Re * Re + Im * Im);
        }

        public double Radians()
        {
            return Math.Atan2(Im, Re);
        }

        public double Degrees()
        {
            return Radians() * 180 / Math.PI;
        }

        public override string ToString()
        {
            return Im > 0 ? $"{Re}+{Im}i" : $"{Re}{Im}i";
        }

        protected bool Equals(Complex other)
        {
            return Re.Equals(other.Re) && Im.Equals(other.Im);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Complex)obj);
        }

        public override int GetHashCode()
        {
            return (Re.GetHashCode() * 397) ^ Im.GetHashCode();
        }

        public void ToDefault()
        {
            Re = 0;
            Im = 0;
        }

        public dynamic Add(dynamic b)
        {
            if (!(b is Complex))
            {
                throw new ArgumentException();
            }

            return this + b;
        }

        public dynamic Sub(dynamic b)
        {
            if (!(b is Complex))
            {
                throw new ArgumentException();
            }

            return this - b;
        }

        public dynamic Mul(dynamic b)
        {
            if (!(b is Complex))
            {
                throw new ArgumentException();
            }

            return this * b;
        }

        public dynamic Div(dynamic b)
        {
            if (!(b is Complex))
            {
                throw new ArgumentException();
            }

            return this / b;
        }
    }
}
