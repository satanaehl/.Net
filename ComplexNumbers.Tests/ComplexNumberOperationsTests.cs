using System;
using System.Numerics;
using Xunit;

namespace ComplexNumbers.Tests
{
    public class ComplexNumberOperationsTests
    {
        [Theory]
        [InlineData(2, 3, 4, -1, 11, 10)]
        [InlineData(0, 1, 0, 1, -1, 0)]
        [InlineData(2, 3, 0, 0, 0, 0)]
        [InlineData(2, 3, 1, 0, 2, 3)]
        [InlineData(-2, 0, 4, 0, -8, 0)]
        public void Multiplication_MatchesExpectedValuesAndStandardComplex(
            double real, double imaginary, double otherReal, double otherImaginary,
            double expectedReal, double expectedImaginary)
        {
            var left = new ComplexNumber(real, imaginary);
            var right = new ComplexNumber(otherReal, otherImaginary);

            var result = left * right;
            var reference = new Complex(real, imaginary) * new Complex(otherReal, otherImaginary);

            Assert.Equal(expectedReal, result.Real);
            Assert.Equal(expectedImaginary, result.Imaginary);
            Assert.Equal(reference.Real, result.Real);
            Assert.Equal(reference.Imaginary, result.Imaginary);
            Assert.Equal(real, left.Real);
            Assert.Equal(imaginary, left.Imaginary);
            Assert.Equal(otherReal, right.Real);
            Assert.Equal(otherImaginary, right.Imaginary);
            Assert.NotSame(left, result);
            Assert.NotSame(right, result);
        }

        [Theory]
        [InlineData(2, 3, 4)]
        [InlineData(2, -3, -2)]
        [InlineData(2, 3, 0)]
        public void ScalarMultiplication_WorksInBothOrdersWithoutChangingOperand(
            double real, double imaginary, double scalar)
        {
            var value = new ComplexNumber(real, imaginary);
            var reference = new Complex(real, imaginary) * scalar;

            var rightProduct = value * scalar;
            var leftProduct = scalar * value;

            Assert.Equal(reference.Real, rightProduct.Real);
            Assert.Equal(reference.Imaginary, rightProduct.Imaginary);
            Assert.Equal(reference.Real, leftProduct.Real);
            Assert.Equal(reference.Imaginary, leftProduct.Imaginary);
            Assert.Equal(real, value.Real);
            Assert.Equal(imaginary, value.Imaginary);
        }

        [Fact]
        public void Multiplication_RejectsNullOperands()
        {
            var value = new ComplexNumber(2, 3);
            ComplexNumber missing = null;

            Assert.Throws<ArgumentNullException>(() => value * missing);
            Assert.Throws<ArgumentNullException>(() => missing * value);
            Assert.Throws<ArgumentNullException>(() => missing * 2.0);
            Assert.Throws<ArgumentNullException>(() => 2.0 * missing);
        }

        [Theory]
        [InlineData(double.PositiveInfinity, 3, 2, double.PositiveInfinity, 6)]
        [InlineData(2, double.NaN, 2, 4, double.NaN)]
        public void ScalarMultiplication_ScalesSpecialComponentsIndependently(
            double real, double imaginary, double scalar, double expectedReal, double expectedImaginary)
        {
            var value = new ComplexNumber(real, imaginary);
            var rightProduct = value * scalar;
            var leftProduct = scalar * value;

            Assert.Equal(expectedReal, rightProduct.Real);
            Assert.Equal(expectedImaginary, rightProduct.Imaginary);
            Assert.Equal(expectedReal, leftProduct.Real);
            Assert.Equal(expectedImaginary, leftProduct.Imaginary);
        }

        [Theory]
        [InlineData(2, 3)]
        [InlineData(-2, -3)]
        [InlineData(0, 0)]
        [InlineData(2, double.PositiveInfinity)]
        [InlineData(double.NaN, 3)]
        public void Conjugate_MatchesStandardComplexAndLeavesOriginalUnchanged(double real, double imaginary)
        {
            var value = new ComplexNumber(real, imaginary);
            var reference = Complex.Conjugate(new Complex(real, imaginary));

            var conjugate = value.Conjugate();

            Assert.Equal(reference.Real, conjugate.Real);
            Assert.Equal(reference.Imaginary, conjugate.Imaginary);
            Assert.Equal(real, value.Real);
            Assert.Equal(imaginary, value.Imaginary);
            Assert.NotSame(value, conjugate);
            Assert.True(value.Equals(conjugate.Conjugate()));
        }

        [Theory]
        [InlineData(3, 4, 5)]
        [InlineData(-3, -4, 5)]
        [InlineData(0, 0, 0)]
        [InlineData(-5, 0, 5)]
        [InlineData(0, -5, 5)]
        [InlineData(3e200, 4e200, 5e200)]
        [InlineData(3e-200, 4e-200, 5e-200)]
        [InlineData(double.Epsilon, 0, double.Epsilon)]
        public void Magnitude_HandlesOrdinaryAndExtremeFiniteValues(double real, double imaginary, double expected)
        {
            var value = new ComplexNumber(real, imaginary);
            var magnitude = value.Magnitude;

            AssertClose(expected, magnitude);
            AssertClose(new Complex(real, imaginary).Magnitude, magnitude);
            Assert.Equal(real, value.Real);
            Assert.Equal(imaginary, value.Imaginary);
        }

        [Theory]
        [InlineData(double.PositiveInfinity, 0)]
        [InlineData(0, double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity, double.NegativeInfinity)]
        [InlineData(double.PositiveInfinity, double.NaN)]
        [InlineData(double.NaN, double.NegativeInfinity)]
        [InlineData(double.NaN, 1)]
        [InlineData(1, double.NaN)]
        [InlineData(double.NaN, double.NaN)]
        [InlineData(double.MaxValue, double.MaxValue)]
        public void Magnitude_HandlesNonFiniteResultsLikeStandardComplex(double real, double imaginary)
        {
            Assert.Equal(new Complex(real, imaginary).Magnitude, new ComplexNumber(real, imaginary).Magnitude);
        }

        private static void AssertClose(double expected, double actual)
        {
            Assert.True(double.IsFinite(actual));
            if (expected == 0)
                Assert.Equal(0.0, actual);
            else
                Assert.InRange(actual / expected, 1 - 1e-14, 1 + 1e-14);
        }
    }
}
