using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Modern.Polynomial
{
    internal class Polynomial
    {
        private List<Member> _members;

        public Polynomial(int coefficient = 0, int exponent = 0)
        {
            _members = new List<Member>
            {
                new Member(coefficient, exponent)
            };
            Simplify();
        }

        public int MaxExponent()
        {
            int max = int.MinValue;
            foreach (var member in _members)
                max = Math.Max(max, member.Exponent);
            return max;
        }

        public int MinExponent()
        {
            int min = int.MaxValue;
            foreach (var member in _members)
                min = Math.Min(min, member.Exponent);
            return min;
        }

        public int GetCoefficient(int exp)
        {
            foreach (var member in _members)
            {
                if (member.Exponent == exp)
                {
                    int i = member.Coefficient;
                    return i;
                }
            }
            return 0;
        }

        private void Simplify()
        {
            var newMembers = new List<Member>();
            for (var i = MinExponent(); i <= MaxExponent(); i++)
            {
                var sum = 0;
                foreach (var member in _members)
                {
                    if (member.Exponent == i) sum += member.Coefficient;
                }
                if (sum > 0)
                {
                    newMembers.Insert(0, new Member(sum, i));
                }
            }
            _members = newMembers;
        }

        public static Polynomial operator +(Polynomial lhs, Polynomial rhs)
        {
            foreach (var rhMember in rhs._members)
            {
                lhs._members.Add(rhMember);
            }
            lhs.Simplify();
            return lhs;
        }

        public static Polynomial operator -(Polynomial lhs, Polynomial rhs)
        {
            foreach (var rhMember in rhs._members)
            {
                lhs._members.Add(-rhMember);
            }
            lhs.Simplify();
            return lhs;
        }

        public static Polynomial operator -(Polynomial poly)
        {
            var t = new Polynomial();
            return t - poly;
        }

        public static Polynomial operator *(Polynomial lhs, Polynomial rhs)
        {
            var result = new Polynomial();
            foreach (var rhMember in rhs._members)
            {
                Polynomial term = new Polynomial();
                Polynomial polynomial = lhs._members.Select(lhMember => rhMember*lhMember).Aggregate(term, (current, t) => current + new Polynomial(t.Coefficient, t.Exponent));
                result = result + polynomial;
            }
            result.Simplify();
            return result;
        }

        public static bool operator ==(Polynomial lhs, Polynomial rhs)
        {
            Debug.Assert(lhs != null, "lhs != null");
            Debug.Assert(rhs != null, "rhs != null");
            if (lhs.MaxExponent() != rhs.MaxExponent())
                return false;
            if (lhs._members.Count != rhs._members.Count) 
                return false;

            return lhs._members.SequenceEqual(rhs._members);
        }

        public static bool operator !=(Polynomial lhs, Polynomial rhs)
        {
            return !(lhs == rhs);
        }

        public Polynomial Differentiate()
        {
            var result = new Polynomial();
            foreach (var member in _members)
            {
                result._members.Add(member.Differentiate());
            }
            result.Simplify();
            return result;
        }

        public double Calculate(int x)
        {
            double sum = 0;
            foreach (var member in _members)
                sum += member.Calculate(x);
            return sum;
        }

        public Member GetPolynomialByNumber(int x)
        {
            if(x > _members.Count)
                throw new ArgumentOutOfRangeException(nameof(x));
            return _members[x];
        }

        public override string ToString()
        {
            if(_members.Count == 0)
                return "0";
            var sb = new StringBuilder();
            foreach (var member in _members)
            {
                sb.Append(member);
            }
            return sb.ToString();
        }

        protected bool Equals(Polynomial other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Polynomial)obj);
        }

        public override int GetHashCode()
        {
            return _members?.GetHashCode() ?? 0;
        }
    }
}
