using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter.Scheme
{
    public class Macro : Closure
    {
        /** Make a macro from a parameter list, body, and environment. **/
        public Macro(Object parms, Object body, Environment env) : base(parms, body, env){}

        /** Replace the old cons cell with the macro expansion, and return it. **/
        public Pair expand(Scheme interpreter, Pair oldPair, Object args)
        {
            Object expansion = apply(interpreter, args);
            if (expansion is Pair)
            {
                oldPair.first = ((Pair)expansion).first;
                oldPair.rest = ((Pair)expansion).rest;
            }
            else
            {
                oldPair.first = "begin";
                oldPair.rest = cons(expansion, null);
            }
            return oldPair;
        }

        /** Macro expand an expression **/
        public static Object macroExpand(Scheme interpreter, Object x)
        {
            if (!(x is Pair)) return x;
            Object fn = interpreter.eval(first(x), interpreter.globalEnvironment);
            if (!(fn is Macro)) return x;
            return ((Macro)fn).expand(interpreter, (Pair)x, rest(x));
        }
    }
}
