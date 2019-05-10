using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter.Scheme
{
    public abstract class Procedure : SchemeUtils
    {
        public String name = "anonymous procedure";

        public abstract Object apply(Scheme interpreter, Object args);

        /** Coerces a Scheme object to a procedure. **/
        public static Procedure proc(Object x)
        {
            if (x is Procedure) return (Procedure)x;
            else return proc(error("Not a procedure: " + stringify(x)));
        }
    }
}
