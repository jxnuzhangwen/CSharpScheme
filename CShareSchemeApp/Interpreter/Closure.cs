using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter.Scheme
{
    public class Closure : Procedure
    {
        public Object parms;
        public Object body;
        public Environment env;

        /** Make a closure from a parameter list, body, and environment. **/
        public Closure(Object parms, Object body, Environment env)
        {
            this.parms = parms;
            this.env = env;
            this.body = (body is Pair && rest(body) == null)
                ? first(body)
                : cons("begin", body);
        }
        /** Apply a closure to a list of arguments.  **/
        public override Object apply(Scheme interpreter, Object args)
        {
            return interpreter.eval(body, new Environment(parms, args, env));
        }
    }
}
