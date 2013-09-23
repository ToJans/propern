using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Propern
{

    public class Spec
    {
        public static GivenSpec<T> Given<T>(IEnumerable<T> Givens)
        {
            return new GivenSpec<T>(Givens);
        }

        public class GivenSpec<T>
        {
            public readonly IEnumerable<T> Givens;
            public GivenSpec(IEnumerable<T> Givens)
            {
                this.Givens = Givens;
            }

            public WhenSpec<T, R> When<R>(Func<T, R> Action)
            {
                return new WhenSpec<T, R>(Givens, Action);
            }
        }

        public class WhenSpec<T, R>
        {
            private IEnumerable<T> Givens;
            private Func<T, R> Action;

            public WhenSpec(IEnumerable<T> Givens, Func<T, R> Action)
            {
                this.Givens = Givens;
                this.Action = Action;
            }

            public ExpectSpec<T, R> Expect(Action<T, R> Assertion)
            {
                return new ExpectSpec<T, R>(Givens, Action, Assertion);
            }
        }

        public class ExpectSpec<T, R>
        {
            private IEnumerable<T> Givens;
            private Func<T, R> Action;
            private Action<T, R> Assertion;

            public ExpectSpec(IEnumerable<T> Givens, Func<T, R> Action, Action<T, R> Assertion)
            {
                this.Givens = Givens;
                this.Action = Action;
                this.Assertion = Assertion;
            }

            public void Verify(int Times = 100)
            {
                foreach (var context in Givens.Take(Times))
                {
                    Debug.WriteLine("Testing context:\n" + JsonConvert.SerializeObject(context));
                    var r = Action(context);
                    Debug.WriteLine("Output:\n" + JsonConvert.SerializeObject(r));
                    Assertion(context, r);
                    Debug.WriteLine("Asserted===============================");
                }
            }
        }
    }
}
