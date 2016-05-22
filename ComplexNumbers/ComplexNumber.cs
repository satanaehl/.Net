using System;
using System.Globalization;

namespace ComplexNumbers
{

    /// <summary>
    /// Represents a complex number.
    /// </summary>
    class ComplexNumber : IEquatable<ComplexNumber>, IFormattable
    {
        /// <summary>
        /// Initializes a new instance of the ComplexNumber class using the specified real and imaginary values.
        /// </summary>
        /// <param name="real">A real value.</param>
        /// <param name="imaginary">A imaginary value.</param>
        public ComplexNumber(double real, double imaginary = 0)
        {
            Real = real;
            Imaginary = imaginary;
        }

        //public ComplexNumber(double real) : this(real, 0) { }
        /// <summary>
        /// Gets the imaginary component of the current ComplexNumber object.
        /// </summary>
        public double Real { get; set; }
        /// <summary>
        /// Gets the real component of the current Complex object.
        /// </summary>
        public double Imaginary { get; set; }

        public bool Equals(ComplexNumber other)
        {
            if (other == null)
                return false;

            if (Math.Abs(Real - other.Real) < Double.Epsilon && Math.Abs(Imaginary - other.Imaginary) < Double.Epsilon)
                return true;
            return false;
        }

        /// <summary>
        /// Converts the value of the current complex number to its equivalent string representation in Cartesian form by using the specified format for its real and imaginary parts.
        /// </summary>
        /// <param name="format">"P" for a view as a point or position vector in a two-dimensional Cartesian coordinate system. "A" and "G" for Cartesian format.</param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
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

        public static ComplexNumber operator +(ComplexNumber x, ComplexNumber y)
        {
            ComplexNumber z = new ComplexNumber(x.Real + y.Real, x.Imaginary + y.Imaginary);
            return z;
        }

        public static ComplexNumber operator +(double x, ComplexNumber y)
        {
            ComplexNumber z = new ComplexNumber(x + y.Real, y.Imaginary);
            return z;
        }

        public static ComplexNumber operator +(ComplexNumber x, double y)
        {
            ComplexNumber z = new ComplexNumber(x.Real + y, x.Imaginary);
            return z;
        }

        public static ComplexNumber operator -(ComplexNumber x, ComplexNumber y)
        {
            ComplexNumber z = new ComplexNumber(x.Real - y.Real, x.Imaginary - y.Imaginary);
            return z;
        }

        public static ComplexNumber operator -(double x, ComplexNumber y)
        {
            ComplexNumber z = new ComplexNumber(x - y.Real, -y.Imaginary);
            return z;
        }

        public static ComplexNumber operator -(ComplexNumber x, double y)
        {
            ComplexNumber z = new ComplexNumber(x.Real - y, x.Imaginary);
            return z;
        }

        public static bool operator ==(ComplexNumber x, ComplexNumber y)
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

        /// <summary>
        /// Returns a value that indicates whether the current instance and a specified object have the same value.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            ComplexNumber number = obj as ComplexNumber;
            if ((object)number == null) return false;
            return (Math.Abs(Real - number.Real) < Double.Epsilon
                && Math.Abs(Imaginary - number.Imaginary) < Double.Epsilon);

        }

    }
}
