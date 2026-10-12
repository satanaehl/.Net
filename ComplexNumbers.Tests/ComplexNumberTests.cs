using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Xunit;

namespace ComplexNumbers.Tests
{
    public class ComplexNumberTests
    {
        [Fact]
        public void Constructor_DefaultsImaginaryComponentToZero()
        {
            AssertComponents(new ComplexNumber(10.5), 10.5, 0);
        }

        [Fact]
        public void Arithmetic_AddsAndSubtractsWithoutChangingOperands()
        {
            var left = new ComplexNumber(2, 3);
            var right = new ComplexNumber(4, -1);

            AssertComponents(left + right, 6, 2);
            AssertComponents(left - right, -2, 4);
            AssertComponents(left + 5, 7, 3);
            AssertComponents(5 + left, 7, 3);
            AssertComponents(left - 5, -3, 3);
            AssertComponents(5 - left, 3, -3);
            AssertComponents(left, 2, 3);
            AssertComponents(right, 4, -1);
        }

        [Fact, Trait("Category", "Equality")]
        public void Equality_AllEntryPointsAgreeForEqualValues()
        {
            var left = new ComplexNumber(2, 3);
            var right = new ComplexNumber(2, 3);
            Assert.True(left.Equals(right));
            Assert.True(left.Equals((object)right));
            Assert.True(left == right);
            Assert.False(left != right);
            var sameReference = left;
            Assert.True(left == sameReference);
        }

        [Fact, Trait("Category", "Equality")]
        public void Equality_DistinguishesBothComponentsAndUsesExactValues()
        {
            var value = new ComplexNumber(2, 3);
            Assert.False(value == new ComplexNumber(2, 4));
            Assert.False(value == new ComplexNumber(4, 3));
            Assert.False(new ComplexNumber(0) == new ComplexNumber(double.Epsilon));
            Assert.False(new ComplexNumber(0.1 + 0.2) == new ComplexNumber(0.3));
            Assert.False(value.Equals("2+i3"));
        }

        [Fact, Trait("Category", "Equality")]
        public void Equality_HandlesNullInBothPositions()
        {
            ComplexNumber missing = null;
            var value = new ComplexNumber(2, 3);
            Assert.True(missing == null);
            Assert.False(value == missing);
            Assert.False(missing == value);
            Assert.True(value != missing);
            Assert.True(missing != value);
            Assert.False(value.Equals(missing));
            Assert.False(value.Equals((object)null));
        }

        [Theory]
        [InlineData(double.PositiveInfinity, 3)]
        [InlineData(double.NegativeInfinity, 3)]
        [InlineData(double.NaN, 3)]
        [InlineData(3, double.NaN)]
        [InlineData(3, double.PositiveInfinity)]
        public void ObjectEquality_HandlesSpecialDoubleValues(double real, double imaginary)
        {
            var value = new ComplexNumber(real, imaginary);
            Assert.True(value.Equals((object)new ComplexNumber(real, imaginary)));
        }

        [Fact]
        public void EqualValues_HaveEqualHashCodes()
        {
            Assert.Equal(new ComplexNumber(2, 3).GetHashCode(), new ComplexNumber(2, 3).GetHashCode());
            Assert.Equal(new ComplexNumber(0.0, -0.0).GetHashCode(), new ComplexNumber(-0.0, 0.0).GetHashCode());
        }

        [Fact, Trait("Category", "Equality")]
        public void HashSet_FindsAnEqualValue()
        {
            var values = new HashSet<ComplexNumber> { new ComplexNumber(2, 3) };
            Assert.Contains(new ComplexNumber(2, 3), values);
        }

        [Theory]
        [InlineData(null, 3, "2+i3")]
        [InlineData("", 3, "2+i3")]
        [InlineData("G", 3, "2+i3")]
        [InlineData("a", -3, "2-i3")]
        [InlineData("G", 0, "2")]
        [InlineData("p", -3, "(2,-3)")]
        public void Formatting_SupportsDocumentedFormats(string format, double imaginary, string expected)
        {
            Assert.Equal(expected, new ComplexNumber(2, imaginary).ToString(format, CultureInfo.InvariantCulture));
        }

        [Fact]
        public void Formatting_UsesSpecifiedCulture()
        {
            var value = new ComplexNumber(2.5, -3.5);
            Assert.Equal("2,5-i3,5", value.ToString("A", CultureInfo.GetCultureInfo("ru-RU")));
            Assert.Equal("2.5-i3.5", value.ToString("A", CultureInfo.InvariantCulture));
        }

        [Fact]
        public void Formatting_DefaultsToCurrentCulture()
        {
            var previous = CultureInfo.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
                var value = new ComplexNumber(2.5, 3.5);
                Assert.Equal("2,5+i3,5", value.ToString());
                Assert.Equal("2,5+i3,5", value.ToString("G", null));
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = previous;
            }
        }

        [Fact]
        public void Formatting_RejectsUnknownFormat()
        {
            Assert.Throws<FormatException>(() => new ComplexNumber(2).ToString("X", CultureInfo.InvariantCulture));
        }

        private static void AssertComponents(ComplexNumber value, double real, double imaginary)
        {
            Assert.Equal(real, value.Real);
            Assert.Equal(imaginary, value.Imaginary);
        }
    }
}
