using System;
using System.Globalization;

namespace ComplexNumbers
{

    /// <summary>
    /// Represents a complex number.
    /// </summary>
    internal class ComplexNumber : IEquatable<ComplexNumber>, IFormattable
    {
        /// <summary>
        /// Initializes a new instance of the ComplexNumber class using the specified real and imaginary values.
        /// </summary>
        /// <param name="real">A real value.</param>
        /// <param name="imaginary">An imaginary value.</param>
        public ComplexNumber(double real, double imaginary = 0)
        {
            Real = real;
            Imaginary = imaginary;
        }

        /// <summary>
        /// Gets or sets the real component of the current complex number.
        /// </summary>
        public double Real { get; set; }
        /// <summary>
        /// Gets or sets the imaginary component of the current complex number.
        /// </summary>
        public double Imaginary { get; set; }

        /// <summary>
        /// Compares both components exactly using Double.Equals, including NaN and infinities.
        /// </summary>
        public bool Equals(ComplexNumber other)
        {
            return !ReferenceEquals(other, null)
                && Real.Equals(other.Real)
                && Imaginary.Equals(other.Imaginary);
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
            return new ComplexNumber(x.Real + y.Real, x.Imaginary + y.Imaginary);
        }

        public static ComplexNumber operator +(double x, ComplexNumber y)
        {
            return y + x;
        }

        public static ComplexNumber operator +(ComplexNumber x, double y)
        {
            return new ComplexNumber(x.Real + y, x.Imaginary);
        }

        public static ComplexNumber operator -(ComplexNumber x, ComplexNumber y)
        {
            return new ComplexNumber(x.Real - y.Real, x.Imaginary - y.Imaginary);
        }

        public static ComplexNumber operator -(double x, ComplexNumber y)
        {
            return new ComplexNumber(x - y.Real, -y.Imaginary);
        }

        public static ComplexNumber operator -(ComplexNumber x, double y)
        {
            return new ComplexNumber(x.Real - y, x.Imaginary);
        }

        public static bool operator ==(ComplexNumber x, ComplexNumber y)
        {
            return ReferenceEquals(x, y)
                || (!ReferenceEquals(x, null) && x.Equals(y));
        }

        public static bool operator !=(ComplexNumber x, ComplexNumber y)
        {
            return !(x == y);
        }

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
            return Equals(obj as ComplexNumber);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Real.GetHashCode() * 397) ^ Imaginary.GetHashCode();
            }
        }
    }
}
