using System; // Noncompliant

/*
 
    The executable lines are marked with secondary locations.
     
*/

namespace Tests.Diagnostics
{
    using System.Linq;
    using System.Collections.Generic;

    public class Program
    {
        int f1 = 0;
        int f2;

        public void VariableDeclarations()
        {
            object o;
            var a = 5; // Secondary
            var b = 5; // Secondary
            int c, 
                d = 5, // Secondary
                e = 5; // Secondary

            var f = new object(); // Secondary
            var g = Tuple.Create(5); // Secondary
            var h = Tuple.Create(1, // Secondary
                2,

                3);

            Action action = () => // Secondary
            {
                int i = 5; // Secondary
            };
        }

        public void ForeachLoops(IEnumerable<object> collection)
        {
            foreach (var item in collection) // Secondary
            {
                foreach (var i in collection) // Secondary
                {
                }
            }

            foreach (var // Secondary
                item
                in
                collection)
            {
            }

            foreach (var item in collection) // Secondary
            {
                break; // Secondary
            }

            foreach (var item in collection) // Secondary
            {
                continue; // Secondary
            }

            foreach (var item in collection) // Secondary
            {
                return; // Secondary
            }
        }

        public void ForLoops()
        {
            for (var i = 0; i < 10; i++) // Secondary
            {
            }

            for (int i = 0, j = 10; i < 10 && j > 0; // Secondary
                i++, j--)
            {
            }

            for (var i // Secondary
                =
                0;
                i
                <
                10
                ;
                i++)
            {
                break; // Secondary
            }

            for (var i = 0; i < 10; i++) // Secondary
            {
                break; // Secondary
            }

            for (var i = 0; i < 10; i++) // Secondary
            {
                continue; // Secondary
            }

            for (var i = 0; i < 10; i++) // Secondary
            {
                return; // Secondary
            }
        }

        public void WhileLoops(IEnumerable<object> collection)
        {
            int i = 0, j = 0;
            while (i++ < 10) // Secondary
            {
                while (j++ < 10) // Secondary
                {
                }
            }

            while (f1 > f2) // Secondary
            {
            }

            while ( // Secondary
                f1
                >
                f2)
            {
            }

            while (true) // Secondary
            {
                break; // Secondary
            }

            while (true) // Secondary
            {
                continue; // Secondary
            }

            while (true) // Secondary
            {
                return; // Secondary
            }
        }

        public void DoWhileLoops(IEnumerable<object> collection)
        {
            do // Secondary
            {
                do // Secondary
                {
                } while (true);
            } while (true);

            do // Secondary
            {
            } while (f1 > f2);

            do // Secondary
            {
            } while (
            f1
            >
            f2);

            do // Secondary
            {
                break; // Secondary
            } while (true);

            do // Secondary
            {
                continue; // Secondary
            } while (true);

            do // Secondary
            {
                return; // Secondary
            } while (true);
        }

        public void IfStatements()
        {
            if (true) // Secondary
            {
                if (!false) // Secondary
                {
                }
            }

            if (f1 > f2 || // Secondary
                f1.GetHashCode() == f2) // should we report this too?
            {
            }
        }
    }
}