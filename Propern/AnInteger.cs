using System;
using System.Collections.Generic;

namespace Propern
{
    public class AnInteger : IEnumerable<int>
    {
        static Random R = new Random();
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
}
