using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using NUnit.Framework;

namespace ComplexNumbers.Tests
{
    [TestFixture]
    public class ComplexNumberTests
    {
        [Test]
        public void Constructor_DefaultsImaginaryComponentToZero()
        {
            AssertComponents(new ComplexNumber(10.5), 10.5, 0);
        }

        [Test]
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

        [Test, Category("Equality")]
        public void Equality_AllEntryPointsAgreeForEqualValues()
        {
            var left = new ComplexNumber(2, 3);
            var right = new ComplexNumber(2, 3);
            Assert.That(left.Equals(right), Is.True);
            Assert.That(left.Equals((object)right), Is.True);
            Assert.That(left == right, Is.True);
            Assert.That(left != right, Is.False);
            var sameReference = left;
            Assert.That(left == sameReference, Is.True);
        }

        [Test, Category("Equality")]
        public void Equality_DistinguishesBothComponentsAndUsesExactValues()
        {
            var value = new ComplexNumber(2, 3);
            Assert.That(value == new ComplexNumber(2, 4), Is.False);
            Assert.That(value == new ComplexNumber(4, 3), Is.False);
            Assert.That(new ComplexNumber(0) == new ComplexNumber(double.Epsilon), Is.False);
            Assert.That(new ComplexNumber(0.1 + 0.2) == new ComplexNumber(0.3), Is.False);
            Assert.That(value.Equals("2+i3"), Is.False);
        }

        [Test, Category("Equality")]
        public void Equality_HandlesNullInBothPositions()
        {
            ComplexNumber missing = null;
            var value = new ComplexNumber(2, 3);
            Assert.That(missing == null, Is.True);
            Assert.That(value == missing, Is.False);
            Assert.That(missing == value, Is.False);
            Assert.That(value != missing, Is.True);
            Assert.That(missing != value, Is.True);
            Assert.That(value.Equals(missing), Is.False);
            Assert.That(value.Equals((object)null), Is.False);
        }

        [TestCase(double.PositiveInfinity, 3)]
        [TestCase(double.NegativeInfinity, 3)]
        [TestCase(double.NaN, 3)]
        [TestCase(3, double.NaN)]
        [TestCase(3, double.PositiveInfinity)]
        public void ObjectEquality_HandlesSpecialDoubleValues(double real, double imaginary)
        {
            var value = new ComplexNumber(real, imaginary);
            Assert.That(value.Equals((object)new ComplexNumber(real, imaginary)), Is.True);
        }

        [Test]
        public void EqualValues_HaveEqualHashCodes()
        {
            Assert.That(new ComplexNumber(2, 3).GetHashCode(),
                Is.EqualTo(new ComplexNumber(2, 3).GetHashCode()));
            Assert.That(new ComplexNumber(0.0, -0.0).GetHashCode(),
                Is.EqualTo(new ComplexNumber(-0.0, 0.0).GetHashCode()));
        }

        [Test, Category("Equality")]
        public void HashSet_FindsAnEqualValue()
        {
            var values = new HashSet<ComplexNumber> { new ComplexNumber(2, 3) };
            Assert.That(values.Contains(new ComplexNumber(2, 3)), Is.True);
        }

        [TestCase(null, 3, "2+i3")]
        [TestCase("", 3, "2+i3")]
        [TestCase("G", 3, "2+i3")]
        [TestCase("a", -3, "2-i3")]
        [TestCase("G", 0, "2")]
        [TestCase("p", -3, "(2,-3)")]
        public void Formatting_SupportsDocumentedFormats(string format, double imaginary, string expected)
        {
            Assert.That(new ComplexNumber(2, imaginary).ToString(format, CultureInfo.InvariantCulture),
                Is.EqualTo(expected));
        }

        [Test]
        public void Formatting_UsesSpecifiedCulture()
        {
            var value = new ComplexNumber(2.5, -3.5);
            Assert.That(value.ToString("A", CultureInfo.GetCultureInfo("ru-RU")), Is.EqualTo("2,5-i3,5"));
            Assert.That(value.ToString("A", CultureInfo.InvariantCulture), Is.EqualTo("2.5-i3.5"));
        }

        [Test]
        public void Formatting_DefaultsToCurrentCulture()
        {
            var previous = CultureInfo.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");
                var value = new ComplexNumber(2.5, 3.5);
                Assert.That(value.ToString(), Is.EqualTo("2,5+i3,5"));
                Assert.That(value.ToString("G", null), Is.EqualTo("2,5+i3,5"));
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = previous;
            }
        }

        [Test]
        public void Formatting_RejectsUnknownFormat()
        {
            Assert.Throws<FormatException>(() => new ComplexNumber(2).ToString("X", CultureInfo.InvariantCulture));
        }

        private static void AssertComponents(ComplexNumber value, double real, double imaginary)
        {
            Assert.That(value.Real, Is.EqualTo(real));
            Assert.That(value.Imaginary, Is.EqualTo(imaginary));
        }
    }
}
