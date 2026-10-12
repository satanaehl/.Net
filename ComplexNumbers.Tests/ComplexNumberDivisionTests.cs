using System;
using System.Numerics;
using Xunit;

namespace ComplexNumbers.Tests
{
    public class ComplexNumberDivisionTests
    {
        [Theory]
        [InlineData(2, 3, 4, -1)]
        [InlineData(-2, 3, 1, 4)]
        [InlineData(0, 0, 2, 3)]
        [InlineData(2, 3, 1, 0)]
        [InlineData(2, 3, 0, 1)]
        [InlineData(1e200, 1e200, 1e200, 1e200)]
        [InlineData(1e-200, 1e-200, 1e-200, 1e-200)]
        public void Division_MatchesStandardComplexWithoutChangingOperands(
            double real, double imaginary, double divisorReal, double divisorImaginary)
        {
            var dividend = new ComplexNumber(real, imaginary);
            var divisor = new ComplexNumber(divisorReal, divisorImaginary);
            var expected = new Complex(real, imaginary) / new Complex(divisorReal, divisorImaginary);

            var result = dividend / divisor;

            AssertClose(expected.Real, result.Real);
            AssertClose(expected.Imaginary, result.Imaginary);
            Assert.Equal(real, dividend.Real);
            Assert.Equal(imaginary, dividend.Imaginary);
            Assert.Equal(divisorReal, divisor.Real);
            Assert.Equal(divisorImaginary, divisor.Imaginary);
            Assert.NotSame(dividend, result);
            Assert.NotSame(divisor, result);
        }

        [Theory]
        [InlineData(11, 10, 4, -1, 2, 3)]
        [InlineData(double.MaxValue, double.MaxValue, double.MaxValue, double.MaxValue, 1, 0)]
        [InlineData(double.MaxValue, double.MaxValue, 1, 1, double.MaxValue, 0)]
        [InlineData(double.Epsilon, double.Epsilon, double.Epsilon, double.Epsilon, 1, 0)]
        [InlineData(double.Epsilon, double.Epsilon, double.Epsilon, 2 * double.Epsilon, 0.6, -0.2)]
        [InlineData(double.MaxValue, double.Epsilon, double.Epsilon, 0, double.PositiveInfinity, 1)]
        [InlineData(double.MaxValue, double.Epsilon, 0, double.Epsilon, 1, double.NegativeInfinity)]
        public void Division_HandlesKnownResultsAndExtremeScales(
            double real, double imaginary, double divisorReal, double divisorImaginary,
            double expectedReal, double expectedImaginary)
        {
            var result = new ComplexNumber(real, imaginary) / new ComplexNumber(divisorReal, divisorImaginary);
            AssertClose(expectedReal, result.Real);
            AssertClose(expectedImaginary, result.Imaginary);
        }

        [Theory]
        [InlineData(2, 3, 2)]
        [InlineData(-2, 3, -4)]
        [InlineData(1e300, 1e-300, 1e-300)]
        public void DivisionByScalar_ScalesComponentsIndependently(double real, double imaginary, double scalar)
        {
            var result = new ComplexNumber(real, imaginary) / scalar;
            AssertClose(real / scalar, result.Real);
            AssertClose(imaginary / scalar, result.Imaginary);
        }

        [Theory]
        [InlineData(10, 2, 1)]
        [InlineData(-2, 0, 1)]
        public void ScalarDividedByComplex_MatchesStandardComplex(double scalar, double real, double imaginary)
        {
            var expected = scalar / new Complex(real, imaginary);
            var result = scalar / new ComplexNumber(real, imaginary);
            AssertClose(expected.Real, result.Real);
            AssertClose(expected.Imaginary, result.Imaginary);
        }

        [Fact]
        public void DivisionByZero_ReturnsNaNForBothComponents()
        {
            var value = new ComplexNumber(2, 3);
            AssertNaN(value / new ComplexNumber(0, -0.0));
            AssertNaN(value / 0.0);
            AssertNaN(2.0 / new ComplexNumber(0));
            AssertNaN(new ComplexNumber(0) / new ComplexNumber(0));
        }

        [Theory]
        [InlineData(double.NaN, 1, 2, 3)]
        [InlineData(2, 3, double.NaN, 1)]
        [InlineData(double.PositiveInfinity, 1, 2, 3)]
        [InlineData(1, double.NegativeInfinity, 2, 3)]
        [InlineData(2, 3, double.PositiveInfinity, 1)]
        [InlineData(2, 3, 1, double.NegativeInfinity)]
        [InlineData(2, 3, double.PositiveInfinity, double.PositiveInfinity)]
        public void Division_WithNonFiniteInputs_MatchesStandardComplex(
            double real, double imaginary, double divisorReal, double divisorImaginary)
        {
            var expected = new Complex(real, imaginary) / new Complex(divisorReal, divisorImaginary);
            var result = new ComplexNumber(real, imaginary) / new ComplexNumber(divisorReal, divisorImaginary);
            Assert.Equal(expected.Real, result.Real);
            Assert.Equal(expected.Imaginary, result.Imaginary);
        }

        [Fact]
        public void Division_RejectsNullOperands()
        {
            var value = new ComplexNumber(2, 3);
            ComplexNumber missing = null;
            Assert.Throws<ArgumentNullException>(() => value / missing);
            Assert.Throws<ArgumentNullException>(() => missing / value);
            Assert.Throws<ArgumentNullException>(() => missing / 2.0);
            Assert.Throws<ArgumentNullException>(() => 2.0 / missing);
        }

        private static void AssertNaN(ComplexNumber value)
        {
            Assert.True(double.IsNaN(value.Real));
            Assert.True(double.IsNaN(value.Imaginary));
        }

        private static void AssertClose(double expected, double actual)
        {
            if (!double.IsFinite(expected) || expected == 0)
                Assert.Equal(expected, actual);
            else
            {
                Assert.True(double.IsFinite(actual));
                Assert.InRange(actual / expected, 1 - 1e-14, 1 + 1e-14);
            }
        }
    }
}
