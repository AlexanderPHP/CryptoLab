using System;
using System.Diagnostics;

namespace Modern.Polynomial
{
    internal class Member
    {

        public int Coefficient { get; set; }
        public int Exponent { get; set; }

        public Member(int coefficient = 0, int exponent = 0)
        {
            Coefficient = coefficient;
            Exponent = exponent;
        }

        public static bool operator ==(Member lhs, Member rhs)
        {
            Debug.Assert(rhs != null, "rhs != null");
            Debug.Assert(lhs != null, "lhs != null");
            return lhs.Coefficient == rhs.Coefficient && rhs.Exponent == lhs.Exponent;
        }

        public static bool operator !=(Member lhs, Member rhs)
        {
            return !(lhs == rhs);
        }

        public static Member operator -(Member meme)
        {
            return new Member(-meme.Coefficient, meme.Exponent);
        }

        public static Member operator *(Member lhs, Member rhs)
        {
            return new Member(lhs.Coefficient * rhs.Coefficient,
                              lhs.Exponent + rhs.Exponent);
        }

        public Member Differentiate()
        {
            return Exponent > 0 ? new Member(Coefficient * Exponent, Exponent - 1) : new Member();
        }

        public double Calculate(double x)
        {
            return Coefficient * Math.Pow(x, Exponent);
        }

        public override string ToString()
        {
            var coef = Coefficient < 0 ? Coefficient.ToString() : "+" + Coefficient;
            if(Exponent == 0)
                return $"{coef}";
            return $"{coef}*x^{Exponent}";
        }

        protected bool Equals(Member other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Member)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Coefficient * 397) ^ Exponent;
            }
        }

    }
}
