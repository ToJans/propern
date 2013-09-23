using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;


namespace Propern.Specs
{
    [TestClass]
    public class POC
    {
        public class AnInteger : IEnumerable<int>
        {
            Random R = new Random();
            int MinValue;
            int MaxValue;

            public AnInteger(int MinValue = int.MinValue, int MaxValue = int.MinValue)
            {
                this.MinValue = MinValue;
                this.MaxValue = MaxValue;
            }

            public IEnumerator<int> GetEnumerator()
            {
                while (true)
                {
                    yield return R.Next(MinValue, MaxValue);
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }



        [TestMethod]
        public void Test_a_sum()
        {
            Spec.Given(new AnInteger(MinValue: -10, MaxValue: 10)
                    .Zip(new AnInteger(MinValue: -10, MaxValue: 10), (a, b) => new { a, b }))
                .When(c => c.a - c.b)
                .Expect((c, r) => Assert.AreEqual(c.a, r + c.b))
                .Verify(100);
        }

        [TestMethod]
        public void Test_a_div()
        {
            Spec.Given(new AnInteger(MinValue: -10, MaxValue: 10)
                    .Zip(new AnInteger(MinValue: -10, MaxValue: 10).Where(b=> b!=0),(a,b) => new {a,b} ))
                .When(c => c.a / c.b)
                .Expect((c, r) => Assert.IsTrue(Math.Abs(r * c.b) <= Math.Abs(c.a)))
                .Verify(100);
        }

    }
}
