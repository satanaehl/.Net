using System;
using Xunit;

namespace ComplexNumbers.Tests
{
    public class ComplexNumberApproximationTests
    {
        [Fact]
        public void DefaultTolerances_AcceptRoundingWithoutChangingExactEquality()
        {
            var left = new ComplexNumber(0.1 + 0.2, 0.1 + 0.2);
            var right = new ComplexNumber(0.3, 0.3);
            var originalHash = left.GetHashCode();

            Assert.True(left.ApproximatelyEquals(right));
            Assert.True(right.ApproximatelyEquals(left));
            Assert.False(left.Equals(right));
            Assert.False(left == right);
            Assert.Equal(originalHash, left.GetHashCode());
            Assert.Equal(0.1 + 0.2, left.Real);
            Assert.Equal(0.3, right.Real);
        }

        [Theory]
        [InlineData(0, 0.125, 0.125, true)]
        [InlineData(0, 0.25, 0.125, false)]
        [InlineData(-0.0625, 0.0625, 0.125, true)]
        [InlineData(0, 5e-13, 1e-12, true)]
        [InlineData(0, 2e-12, 1e-12, false)]
        [InlineData(0, double.Epsilon, double.Epsilon, true)]
        [InlineData(0, double.Epsilon, 0, false)]
        public void AbsoluteTolerance_HandlesZeroAndInclusiveBoundary(
            double left, double right, double tolerance, bool expected)
        {
            AssertComparison(left, right, tolerance, 0, expected);
        }

        [Theory]
        [InlineData(7, 8, 0.125, true)]
        [InlineData(6, 8, 0.125, false)]
        [InlineData(1e200, 1e200 + 1e190, 1e-9, true)]
        [InlineData(1e200, 1.1e200, 1e-9, false)]
        [InlineData(double.MaxValue, -double.MaxValue, 1, false)]
        [InlineData(double.MaxValue, -double.MaxValue, 2, true)]
        [InlineData(double.MaxValue, -double.MaxValue, double.MaxValue, true)]
        [InlineData(double.Epsilon, 2 * double.Epsilon, 0.5, true)]
        [InlineData(double.Epsilon, 2 * double.Epsilon, 0.25, false)]
        public void RelativeTolerance_HandlesScaleAndOverflow(
            double left, double right, double tolerance, bool expected)
        {
            AssertComparison(left, right, 0, tolerance, expected);
        }

        [Fact]
        public void BothComponents_MustBeCloseIndependently()
        {
            var value = new ComplexNumber(1e200, 0);
            Assert.False(value.ApproximatelyEquals(new ComplexNumber(1e200, 1)));
            Assert.False(new ComplexNumber(0, 1e200).ApproximatelyEquals(new ComplexNumber(1, 1e200)));
        }

        [Theory]
        [InlineData(0, -0.0, true)]
        [InlineData(3, 3, true)]
        [InlineData(double.PositiveInfinity, double.PositiveInfinity, true)]
        [InlineData(double.NegativeInfinity, double.NegativeInfinity, true)]
        [InlineData(double.PositiveInfinity, double.NegativeInfinity, false)]
        [InlineData(double.PositiveInfinity, double.MaxValue, false)]
        [InlineData(double.NaN, double.NaN, false)]
        [InlineData(double.NaN, 0, false)]
        public void SpecialValues_HaveExplicitRules(double left, double right, bool expected)
        {
            AssertComparison(left, right, 0, 0, expected);
        }

        [Fact]
        public void NullAndNaNOnSameInstance_AreNotApproximatelyEqual()
        {
            Assert.False(new ComplexNumber(1).ApproximatelyEquals(null));
            var value = new ComplexNumber(double.NaN);
            Assert.False(value.ApproximatelyEquals(value));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(double.NaN)]
        [InlineData(double.PositiveInfinity)]
        [InlineData(double.NegativeInfinity)]
        public void InvalidTolerances_AreRejectedEvenForNull(double tolerance)
        {
            var value = new ComplexNumber(1);
            var absoluteError = Assert.Throws<ArgumentOutOfRangeException>(
                () => value.ApproximatelyEquals(null, absoluteTolerance: tolerance));
            var relativeError = Assert.Throws<ArgumentOutOfRangeException>(
                () => value.ApproximatelyEquals(value, relativeTolerance: tolerance));
            Assert.Equal("absoluteTolerance", absoluteError.ParamName);
            Assert.Equal("relativeTolerance", relativeError.ParamName);
        }

        private static void AssertComparison(double left, double right,
            double absoluteTolerance, double relativeTolerance, bool expected)
        {
            // Check each component and both operand orders to exercise symmetry.
            var realLeft = new ComplexNumber(left, 0);
            var realRight = new ComplexNumber(right, 0);
            var imaginaryLeft = new ComplexNumber(0, left);
            var imaginaryRight = new ComplexNumber(0, right);
            Assert.Equal(expected, realLeft.ApproximatelyEquals(realRight, absoluteTolerance, relativeTolerance));
            Assert.Equal(expected, realRight.ApproximatelyEquals(realLeft, absoluteTolerance, relativeTolerance));
            Assert.Equal(expected, imaginaryLeft.ApproximatelyEquals(imaginaryRight, absoluteTolerance, relativeTolerance));
            Assert.Equal(expected, imaginaryRight.ApproximatelyEquals(imaginaryLeft, absoluteTolerance, relativeTolerance));
        }
    }
}
