using System;
using System.Globalization;

namespace ComplexNumbers
{
    class ComplexNumber : IEquatable<ComplexNumber>, IFormattable
    {
        public ComplexNumber(double real, double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public ComplexNumber(double real) : this(real, 0) { }
        public double Real { get; set; }
        public double Imaginary { get; set; }

        public bool Equals(ComplexNumber other)
        {
            if (other == null)
                return false;

            if (Math.Abs(Real - other.Real) < Double.Epsilon && Math.Abs(Imaginary - other.Imaginary) < Double.Epsilon)
                return true;
            return false;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (String.IsNullOrEmpty(format)) format = "G";

            switch (format.ToUpperInvariant())
            {
                case "P":
                    return String.Format(formatProvider, "({0},{1})", Real, Imaginary);
                case "G":
                case "A":
                    if (Imaginary < 0)
                    {
                        return String.Format(formatProvider, "{0}-i{1}", Real, -Imaginary);
                    }
                    else if (Imaginary > 0)
                    {
                        return String.Format(formatProvider, "{0}+i{1}", Real, Imaginary);
                    }
                    else
                    {
                        return String.Format(formatProvider, "{0}", Real);
                    }

                default:
                    throw new FormatException(String.Format("The {0} format string is not supported.", format));
            }
        }

        static public ComplexNumber operator +(ComplexNumber x, ComplexNumber y)
        {
            ComplexNumber z = new ComplexNumber(x.Real + y.Real, x.Imaginary + y.Imaginary);
            return z;
        }

        static public ComplexNumber operator +(double x, ComplexNumber y)
        {
            ComplexNumber z = new ComplexNumber(x + y.Real, y.Imaginary);
            return z;
        }

        static public ComplexNumber operator +(ComplexNumber x, double y)
        {
            ComplexNumber z = new ComplexNumber(x.Real + y, x.Imaginary);
            return z;
        }

        static public ComplexNumber operator -(ComplexNumber x, ComplexNumber y)
        {
            ComplexNumber z = new ComplexNumber(x.Real - y.Real, x.Imaginary - y.Imaginary);
            return z;
        }

        static public ComplexNumber operator -(double x, ComplexNumber y)
        {
            ComplexNumber z = new ComplexNumber(x - y.Real, -y.Imaginary);
            return z;
        }

        static public ComplexNumber operator -(ComplexNumber x, double y)
        {
            ComplexNumber z = new ComplexNumber(x.Real - y, x.Imaginary);
            return z;
        }

        static public bool operator ==(ComplexNumber x, ComplexNumber y)
        {
            if (((object)x == null) && ((object)y == null))
            {
                return true;
            }
            if (((object)x == null) && !((object)y == null))
            {
                return false;
            }
            return x.Equals(y);
        }

        public static bool operator !=(ComplexNumber x, ComplexNumber y)
        {
            return !(x == y);
        }

        //public static explicit operator ComplexNumber(double num)
        //{
        //    return new ComplexNumber(num);
        //}

        //public static implicit operator double (ComplexNumber a)
        //{
        //    return a.Real;
        //}

        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        public override bool Equals(object obj)
        {
            ComplexNumber number = obj as ComplexNumber;
            if ((object)number == null) return false;
            return (Math.Abs(Real - number.Real) < Double.Epsilon
                && Math.Abs(Imaginary - number.Imaginary) < Double.Epsilon);

        }

    }
}
