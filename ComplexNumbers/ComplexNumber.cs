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
        /// Gets the magnitude, scaling finite components to avoid intermediate overflow or underflow.
        /// </summary>
        public double Magnitude
        {
            get
            {
                double real = Math.Abs(Real);
                double imaginary = Math.Abs(Imaginary);

                if (double.IsInfinity(real) || double.IsInfinity(imaginary))
                    return double.PositiveInfinity;

                double largest = Math.Max(real, imaginary);
                if (largest == 0)
                    return 0;

                double ratio = Math.Min(real, imaginary) / largest;
                return largest * Math.Sqrt(1 + ratio * ratio);
            }
        }

        /// <summary>
        /// Returns a new number with the imaginary component negated.
        /// </summary>
        public ComplexNumber Conjugate()
        {
            return new ComplexNumber(Real, -Imaginary);
        }

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
        /// Compares each component with an absolute or relative tolerance.
        /// NaN is never approximately equal; matching infinities are equal.
        /// </summary>
        /// <param name="other">The number to compare; null returns false.</param>
        /// <param name="absoluteTolerance">A finite, nonnegative tolerance near zero.</param>
        /// <param name="relativeTolerance">A finite, nonnegative tolerance relative to each component's scale.</param>
        public bool ApproximatelyEquals(ComplexNumber other,
            double absoluteTolerance = 1e-12, double relativeTolerance = 1e-12)
        {
            if (!double.IsFinite(absoluteTolerance) || absoluteTolerance < 0)
                throw new ArgumentOutOfRangeException(nameof(absoluteTolerance));
            if (!double.IsFinite(relativeTolerance) || relativeTolerance < 0)
                throw new ArgumentOutOfRangeException(nameof(relativeTolerance));

            return !ReferenceEquals(other, null)
                && ApproximatelyEqualComponent(Real, other.Real, absoluteTolerance, relativeTolerance)
                && ApproximatelyEqualComponent(Imaginary, other.Imaginary, absoluteTolerance, relativeTolerance);
        }

        private static bool ApproximatelyEqualComponent(double left, double right,
            double absoluteTolerance, double relativeTolerance)
        {
            if (double.IsNaN(left) || double.IsNaN(right))
                return false;
            if (left == right)
                return true;
            if (!double.IsFinite(left) || !double.IsFinite(right))
                return false;

            double difference = Math.Abs(left - right);
            if (difference <= absoluteTolerance)
                return true;

            double scale = Math.Max(Math.Abs(left), Math.Abs(right));
            // Normalize before subtracting only when the original difference overflows.
            double relativeDifference = double.IsInfinity(difference)
                ? Math.Abs(left / scale - right / scale)
                : difference / scale;
            return relativeDifference <= relativeTolerance;
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

        public static ComplexNumber operator *(ComplexNumber x, ComplexNumber y)
        {
            ArgumentNullException.ThrowIfNull(x);
            ArgumentNullException.ThrowIfNull(y);
            return new ComplexNumber(
                x.Real * y.Real - x.Imaginary * y.Imaginary,
                x.Real * y.Imaginary + x.Imaginary * y.Real);
        }

        public static ComplexNumber operator *(ComplexNumber x, double y)
        {
            ArgumentNullException.ThrowIfNull(x);
            return new ComplexNumber(x.Real * y, x.Imaginary * y);
        }

        public static ComplexNumber operator *(double x, ComplexNumber y)
        {
            return y * x;
        }

        public static bool operator !=(ComplexNumber x, ComplexNumber y)
        {
            return !(x == y);
        }

        /// <summary>
        /// Divides using Smith's formula with scaling for large finite operands.
        /// A zero divisor returns NaN for both components.
        /// </summary>
        public static ComplexNumber operator /(ComplexNumber x, ComplexNumber y)
        {
            ArgumentNullException.ThrowIfNull(x);
            ArgumentNullException.ThrowIfNull(y);

            double a = x.Real;
            double b = x.Imaginary;
            double c = y.Real;
            double d = y.Imaginary;

            if (c == 0 && d == 0)
                return new ComplexNumber(double.NaN, double.NaN);

            bool finite = double.IsFinite(a) && double.IsFinite(b)
                && double.IsFinite(c) && double.IsFinite(d);

            if (finite)
            {
                // Pure real/imaginary divisors need no intermediate products.
                if (d == 0)
                    return new ComplexNumber(a / c, b / c);
                if (c == 0)
                    return new ComplexNumber(b / d, -a / d);

                // The denominator below can be up to twice its largest component.
                if (Math.Max(Math.Abs(c), Math.Abs(d)) > double.MaxValue / 2)
                {
                    a /= 2;
                    b /= 2;
                    c /= 2;
                    d /= 2;
                }

                // Bring subnormal divisors into the normal range when scaling is safe.
                const double minNormal = 2.2250738585072014e-308;
                if (Math.Max(Math.Abs(c), Math.Abs(d)) < 4 * minNormal
                    && Math.Max(Math.Abs(a), Math.Abs(b)) <= Math.ScaleB(double.MaxValue, -54))
                {
                    a = Math.ScaleB(a, 54);
                    b = Math.ScaleB(b, 54);
                    c = Math.ScaleB(c, 54);
                    d = Math.ScaleB(d, 54);
                }
            }

            if (Math.Abs(c) > Math.Abs(d))
            {
                double ratio = d / c;
                double denominator = c + d * ratio;
                return new ComplexNumber(
                    DivideSum(a, b, ratio, denominator, finite),
                    DivideSum(b, -a, ratio, denominator, finite));
            }
            else
            {
                double ratio = c / d;
                double denominator = d + c * ratio;
                return new ComplexNumber(
                    DivideSum(b, a, ratio, denominator, finite),
                    DivideSum(-a, b, ratio, denominator, finite));
            }
        }

        public static ComplexNumber operator /(ComplexNumber x, double y)
        {
            return x / new ComplexNumber(y);
        }

        public static ComplexNumber operator /(double x, ComplexNumber y)
        {
            return new ComplexNumber(x) / y;
        }

        private static double DivideSum(double first, double second, double ratio, double denominator, bool finite)
        {
            const double minNormal = 2.2250738585072014e-308;
            if (finite && Math.Max(Math.Abs(first), Math.Abs(second)) < 4 * minNormal)
            {
                first = Math.ScaleB(first, 54);
                second = Math.ScaleB(second, 54);
                denominator = Math.ScaleB(denominator, 54);
            }
            double sum = first + second * ratio;
            if (finite && double.IsInfinity(sum))
            {
                // Halve only the overflowing sum, preserving the other component.
                return ((first / 2 + (second / 2) * ratio) / denominator) * 2;
            }
            return sum / denominator;
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
