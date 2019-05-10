using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Interpreter.Scheme
{
    public abstract class SchemeUtils
    {

        /** Convert a list of characters to a Scheme string, which is a char[]. **/
        public static char[] listToString(Object chars)
        {
            char[] str = new char[length(chars)];
            for (int i = 0; chars is Pair; i++)
            {
                str[i] = chr(first(chars));
                chars = rest(chars);
            }
            return str;
        }

        /** Coerces a Scheme object to a Scheme vector, which is a Object[]. **/
        public static Object[] vec(Object x)
        {
            if (x is Object[]) return (Object[])x;
            else return vec(error("expected a vector, got: " + x));
        }

        /** Coerces a Scheme object to a Scheme input port, which is an InputPort.
 * If the argument is null, returns interpreter.input. **/
        public static InputPort inPort(Object x, Scheme interp)
        {
            if (x == null) return interp.input;
            else if (x is InputPort) return (InputPort)x;
            else return inPort(error("expected an input port, got: " + x), interp);
        }

        /** listStar(args) is like Common Lisp (apply #'list* args) **/
        public static Object listStar(Object args)
        {
            if (rest(args) == null) return first(args);
            else return cons(first(args), listStar(rest(args)));
        }

        /** Coerces a Scheme object to a Scheme input port, which is a PrintWriter.
         * If the argument is null, returns System.out. **/
        public static TextWriter outPort(Object x, Scheme interp)
        {
            if (x == null) return interp.output;
            else if (x is TextWriter) return (TextWriter)x;
            else return outPort(error("expected an output port, got: " + x), interp);
        }

        /** Convert a vector to a List. **/
        public static Pair vectorToList(Object x)
        {
            if (x is Object[])
            {
                Object[] vec = (Object[])x;
                Pair result = null;
                for (int i = vec.Length - 1; i >= 0; i--)
                    result = cons(vec[i], result);
                return result;
            }
            else
            {
                error("expected a vector, got: " + x);
                return null;
            }
        }

        /** Same as Boolean.TRUE. **/
        public static Boolean TRUE = true;
        /** Same as Boolean.FALSE. **/
        public static Boolean FALSE = false;

        /** Convert Boolean to Boolean. **/
        public static Boolean truth(Boolean x) { return x ? TRUE : FALSE; }

        /** Convert Scheme object to Boolean.  Only #f is false, others are true. **/
        public static Boolean truth(Object x) { return (Boolean)x; }


        public static Double ZERO = 0.0;
        public static Double ONE = 1.0;

        /** Converts a Scheme object to a double, or calls error. **/
        public static double num(Object x)
        {
            if (x is Double) return (Double)x;
            else return num(error("expected a number, got: " + x));
        }

        /** Reverse the elements of a list. **/
        public static Object reverse(Object x)
        {
            Object result = null;
            while (x is Pair)
            {
                result = cons(first(x), result);
                x = rest(x);
            }
            return result;
        }

        /** Check if two objects are == or are equal numbers or characters. **/
        public static Boolean eqv(Object x, Object y)
        {
            return x == y
                || (x is Double && x == (y))
                || (x is Char && x == (y));
        }

        /** The length of a list, or zero for a non-list. **/
        public static int length(Object x)
        {
            int len = 0;
            while (x is Pair)
            {
                len++;
                x = ((Pair)x).rest;
            }
            return len;
        }

        /** cons(x, y) is the same as new Pair(x, y). **/
        public static Pair cons(Object a, Object b)
        {
            return new Pair(a, b);
        }

        /** Coerces a Scheme object to a Scheme string, which is a char[]. **/
        public static char[] str(Object x)
        {
            if (x is char[]) return (char[])x;
            else return str(error("expected a string, got: " + x));
        }

        /** Coerces a Scheme object to a Scheme symbol, which is a string. **/
        public static String sym(Object x)
        {
            if (x is String) return (String)x;
            else return sym(error("expected a symbol, got: " + x));
        }

        /** Converts a Scheme object to a char, or calls error. **/
        public static char chr(Object x)
        {
            if (x is Char) return (Char)x;
            else return chr(error("expected a char, got: " + x));
        }


        /** Converts a char to a Char. **/
        public static Char chr(Char ch)
        {
            return ch;
        }

        /** Convert a list of Objects to a Scheme vector, which is a Object[]. **/
        public static Object[] listToVector(Object objs)
        {
            Object[] vec = new Object[Length(objs)];
            for (int i = 0; objs is Pair; i++)
            {
                vec[i] = first(objs);
                objs = rest(objs);
            }
            return vec;
        }

        /** Like Common Lisp first; car of a Pair, or null for anything else. **/
        public static Object first(Object x)
        {
            return (x is Pair) ? ((Pair)x).first : null;
        }

        /** Like Common Lisp rest; car of a Pair, or null for anything else. **/
        public static Object rest(Object x)
        {
            return (x is Pair) ? ((Pair)x).rest : null;
        }

        /** The Length of a list, or zero for a non-list. **/
        public static int Length(Object x)
        {
            int len = 0;
            while (x is Pair)
            {
                len++;
                x = ((Pair)x).rest;
            }
            return len;
        }

        /** Like Common Lisp second. **/
        public static Object second(Object x)
        {
            return first(rest(x));
        }

        /** Like Common Lisp third. **/
        public static Object third(Object x)
        {
            return first(rest(rest(x)));
        }

        /** Write the object to a port.  If quoted is true, use "str" and #\c,
         * otherwise use str and c. **/
        public static Object write(Object x, TextWriter port, Boolean quoted)
        {
            port.Write(stringify(x, quoted));
            port.Flush();
            return x;
        }

        /** Check if two objects are equal. **/
        public static Boolean equal(Object x, Object y)
        {
            if (x == null || y == null)
            {
                return x == y;
            }
            else if (x is char[])
            {
                if (!(y is char[])) return false;
                char[] xc = (char[])x, yc = (char[])y;
                if (xc.Length != yc.Length) return false;
                for (int i = xc.Length - 1; i >= 0; i--)
                {
                    if (xc[i] != yc[i]) return false;
                }
                return true;
            }
            else if (x is Object[])
            {
                if (!(y is Object[])) return false;
                Object[] xo = (Object[])x, yo = (Object[])y;
                if (xo.Length != yo.Length) return false;
                for (int i = xo.Length - 1; i >= 0; i--)
                {
                    if (!equal(xo[i], yo[i])) return false;
                }
                return true;
            }
            else
            {
                return (x == y);
            }
        }

        /** Like Common Lisp (setf (first ... **/
        public static Object setFirst(Object x, Object y)
        {
            return (x is Pair) ? ((Pair)x).first = y
                : error("Attempt to set-car of a non-Pair:" + stringify(x));
        }

        /** Like Common Lisp (setf (rest ... **/
        public static Object setRest(Object x, Object y)
        {
            return (x is Pair) ? ((Pair)x).rest = y
                : error("Attempt to set-cdr of a non-Pair:" + stringify(x));
        }

        /** Convert x to a Java String giving its external representation. 
        * Strings and characters are quoted. **/
        public static String stringify(Object x) { return stringify(x, true); }

        /** Convert x to a Java String giving its external representation. 
        * Strings and characters are quoted iff <tt>quoted</tt> is true.. **/
        public static String stringify(Object x, Boolean quoted)
        {
            StringBuilder buf = new StringBuilder();
            stringify(x, quoted, buf);
            return buf.ToString();
        }

        public static void stringify(Object x, Boolean quoted, StringBuilder buf)
        {
            if (x == null)
                buf.Append("()");
            else if (x is Double)
            {
                double d = ((Double)x);
                if (Math.Round(d) == d) buf.Append((long)d); else buf.Append(d);
            }
            else if (x is Char)
            {
                if (quoted) buf.Append("#\\");
                buf.Append(x);
            }
            else if (x is Pair)
            {
                ((Pair)x).stringifyPair(quoted, buf);
            }
            else if (x is char[])
            {
                char[] chars = (char[])x;
                if (quoted) buf.Append('"');
                for (int i = 0; i < chars.Length; i++)
                {
                    if (quoted && chars[i] == '"') buf.Append('\\');
                    buf.Append(chars[i]);
                }
                if (quoted) buf.Append('"');
            }
            else if (x is Object[])
            {
                Object[] v = (Object[])x;
                buf.Append("#(");
                for (int i = 0; i < v.Length; i++)
                {
                    stringify(v[i], quoted, buf);
                    if (i != v.Length - 1) buf.Append(' ');
                }
                buf.Append(')');
            }
            else if ((Boolean)x == TRUE)
            {
                buf.Append("#t");
            }
            else if ((Boolean)x == FALSE)
            {
                buf.Append("#f");
            }
            else
            {
                buf.Append(x);
            }
        }


        //////////////// Error Routines ////////////////

        /** A continuable error. Prints an error message and then prompts for
        * a value to eval and return. **/
        public static Object error(String message)
        {
            System.Console.Error.WriteLine("**** ERROR: " + message);
            throw new SystemException(message);
        }

        public static Object warn(String message)
        {
            System.Console.Error.WriteLine("**** WARNING: " + message);
            return "<warn>";
        }

        /** Creates a one element list. **/
        public static Pair list(Object a)
        {
            return new Pair(a, null);
        }

        /** Creates a two element list. **/
        public static Pair list(Object a, Object b)
        {
            return new Pair(a, new Pair(b, null));
        }
    }
}
