using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Propern.Specs
{
    [TestClass]
    public class Math_operations
    {
        [TestMethod]
        public void Subtract_2_numbers()
        {
            Spec.Given(new AnInteger(MinValue: -10, MaxValue: 10),
                       new AnInteger(MinValue: -10, MaxValue: 10),
                       (a, b) => new { subtractee = a, subtractor = b })
                .When(c => c.subtractee - c.subtractor)
                .Expect((c, result) => Assert.AreEqual(c.subtractee, result + c.subtractor))
                .Verify(100);
        }

        [TestMethod]
        public void Divide_2_numbers()
        {
            Spec.Given(
                    new AnInteger(MinValue: -10, MaxValue: 10),
                    new AnInteger(MinValue: -10, MaxValue: 10).Where(b => b != 0),
                    (a, b) => new { number_to_divide = a, divider = b })
                .When(c => c.number_to_divide / c.divider)
                .Expect((c, result) => Assert.IsTrue(Math.Abs(result * c.divider) <= Math.Abs(c.number_to_divide)))
                .Verify(100);
        }
    }
}
