using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter.Scheme
{
    public class Continuation : Procedure
    {
        Exception cc = null;
        public Object value = null;

        public Continuation(Exception cc) { this.cc = cc; }

        public override Object apply(Scheme interpreter, Object args)
        {
            value = first(args);
            throw cc;
        }
    }
}
