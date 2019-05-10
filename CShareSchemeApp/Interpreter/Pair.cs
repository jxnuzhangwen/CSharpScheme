using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter.Scheme
{
    public class Pair : SchemeUtils
    {
        /** The first element of the pair. **/
        public Object first;

        /** The other element of the pair. **/
        public Object rest;

        /** Build a pair from two components. **/
        public Pair(Object first, Object rest)
        {
            this.first = first; this.rest = rest;
        }

        /** Two pairs are equal if their first and rest fields are equal. **/
        public Boolean equals(Object x)
        {
            if (x == this) return true;
            else if (!(x is Pair)) return false;
            else
            {
                Pair that = (Pair)x;
                return equal(this.first, that.first)
                  && equal(this.rest, that.rest);
            }
        }

        /** Return a String representation of the pair. **/
        public String toString() { return stringify(this, true); }

        /** Build up a String representation of the Pair in a StringBuilder. **/
        public void stringifyPair(Boolean quoted, StringBuilder buf)
        {
            String special = null;
            if ((rest is Pair) && Pair.rest(rest) == null)
                special = (first == "quote") ? "'" : (first == "quasiquote") ? "`"
              : (first == "unquote") ? "," : (first == "unquote-splicing") ? ",@"
              : null;

            if (special != null)
            {
                buf.Append(special); stringify(second(this), quoted, buf);
            }
            else
            {
                buf.Append('(');
                stringify(first, quoted, buf);
                Object tail = rest;
                while (tail is Pair)
                {
                    buf.Append(' ');
                    stringify(((Pair)tail).first, quoted, buf);
                    tail = ((Pair)tail).rest;
                }
                if (tail != null)
                {
                    buf.Append(" . ");
                    stringify(tail, quoted, buf);
                }
                buf.Append(')');
            }
        }
    }
}
