using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace Propern
{

    public class Spec
    {
        public static GivenSpec<T> Given<T>(IEnumerable<T> Givens)
        {
            return new GivenSpec<T>(Givens);
        }

        public static GivenSpec<T> Given<T,A,B>(IEnumerable<A> Given_a, IEnumerable<B> Given_b, Func<A,B,T> zip )
        {
            var ab = Given_a.Zip(Given_b, (a, b) => zip(a,b));
            return new GivenSpec<T>(ab);
        }

        public static GivenSpec<T> Given<T, A, B, C>(IEnumerable<A> Given_a, IEnumerable<B> Given_b, IEnumerable<C> Given_c, Func<A, B, C, T> zip)
        {
            var ab = Given_a.Zip(Given_b, (a, b) => new {a, b});
            var abc = ab.Zip(Given_c, (t, c) => zip(t.a, t.b, c));
            return new GivenSpec<T>(abc);
        }

        public static GivenSpec<T> Given<T, A, B, C, D>(IEnumerable<A> Given_a, IEnumerable<B> Given_b, IEnumerable<C> Given_c, IEnumerable<D> Given_d, Func<A, B, C, D, T> zip)
        {
            var ab = Given_a.Zip(Given_b, (a, b) => new { a, b });
            var abc = ab.Zip(Given_c, (t, c) => new { t.a, t.b, c });
            var abcd = abc.Zip(Given_d, (t, d) => zip(t.a, t.b, t.c, d));
            return new GivenSpec<T>(abcd);
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
