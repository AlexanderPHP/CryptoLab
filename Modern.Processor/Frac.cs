using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Modern.Processor
{
    public class Frac : IHasArithmeticOperations
    {
        private long _denominator;
        public long Numerator { get; private set; }

        public long Denominator
        {
            get { return _denominator; }
            private set
            {
                if(value == 0)
                {
                    throw new DivideByZeroException("Denominator must be non-zero.");
                }

                _denominator = value;
            }
        }

        public Frac(long numerator, long denominator = 1)
        {
            SetupFraction(numerator, denominator);
        }

        public Frac(string fraction)
        {
            var matches = Regex.Match(fraction, @"(\d+)\s*\/\s*(\d+)");
            if (matches.Success)
            {
                SetupFraction(Convert.ToInt64(matches.Groups[1].ToString()), Convert.ToInt64(matches.Groups[2].ToString()));
            }
            else
            {
                throw new ArgumentException("String value must be a fraction.");
            }
        }

        private void SetupFraction(long numerator, long denominator)
        {
            if (denominator < 0)
            {
                denominator *= -1;
                numerator *= -1;
            }
            Denominator = denominator;
            Numerator = numerator;
            Reduce();
        }

        public static Frac operator +(Frac a, Frac b)
        {
            var sum = new Frac(0);
            if (a.Denominator == b.Denominator)
            {
                sum.SetupFraction(a.Numerator + b.Numerator, a.Denominator);
                return sum;
            }

            var lcm = Lcm(a.Denominator, b.Denominator);
            sum.SetupFraction(a.Numerator * (lcm / a.Denominator) + b.Numerator * (lcm / b.Denominator), lcm);
            return sum;
        }

        public static Frac operator -(Frac a, Frac b)
        {
            return a + !b;
        }

        public static Frac operator *(Frac a, Frac b)
        {
            return new Frac(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
        }

        public static Frac operator /(Frac a, Frac b)
        {
            return new Frac(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
        }

        public static Frac operator !(Frac a)
        {
            return new Frac(-a.Numerator, a.Denominator);
        }

        public static implicit operator Frac(int a)
        {
            return new Frac(a);
        }

        public static bool operator ==(Frac a, Frac b)
        {
            Debug.Assert(a != null, "a != null");
            return a.Equals(b);
        }

        public static bool operator !=(Frac a, Frac b)
        {
            Debug.Assert(a != null, "a != null");
            return !a.Equals(b);
        }

        public static bool operator >(Frac a, Frac b)
        {
            var lcm = Lcm(a.Denominator, b.Denominator);
            return (a.Numerator * (lcm / a.Denominator) > b.Numerator * (lcm / b.Denominator));
        }

        public static bool operator <(Frac a, Frac b)
        {
            var lcm = Lcm(a.Denominator, b.Denominator);
            return (a.Numerator * (lcm / a.Denominator) < b.Numerator * (lcm / b.Denominator));
        }

        public dynamic Reverse()
        {
            return !this;
        }

        public dynamic Square()
        {
            return this * this;
        }

        public static dynamic Reverse(Frac a)
        {
            if (a.Numerator != 0)
            {
                return new Frac(a.Denominator, a.Numerator);
            }

            throw new ArgumentOutOfRangeException();
        }

        private void Reduce()
        {
            var gcd = Gcd(Numerator, Denominator);
            Numerator /= gcd;
            Denominator /= gcd;
        }

        private static long Gcd(long a, long b)
        {
            if (a < 0)
            {
                a *= -1;
            }

            while (b != 0)
            {
                a %= b;
                Swap(ref a, ref b);
            }
            return a;
        }

        private static long Lcm(long a, long b)
        {
            return a * b / Gcd(a, b);
        }

        private static void Swap(ref long lhs, ref long rhs)
        {
            var temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        public Frac Copy()
        {
            return this;
        }

        public override string ToString()
        {
            return Denominator != 1 ? $"{Numerator}/{Denominator}" : $"{Numerator}";
        }

        protected bool Equals(Frac other)
        {
            return Numerator == other.Numerator && Denominator == other.Denominator;
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

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Frac)obj);
        }
        /*
        public override int GetHashCode()
        {
            return (Numerator.GetHashCode() * 397) ^ Denominator.GetHashCode();
        }
        */
        public void ToDefault()
        {
            SetupFraction(0, 1);
        }

        public dynamic Add(dynamic b)
        {
            if(!(b is Frac))
            {
                throw new ArgumentException();
            }

            return this + b;
        }

        public dynamic Sub(dynamic b)
        {
            if (!(b is Frac))
            {
                throw new ArgumentException();
            }

            return this - b;
        }

        public dynamic Mul(dynamic b)
        {
            if (!(b is Frac))
            {
                throw new ArgumentException();
            }

            return this * b;
        }

        public dynamic Div(dynamic b)
        {
            if (!(b is Frac))
            {
                throw new ArgumentException();
            }

            return this / b;
        }
    }
}