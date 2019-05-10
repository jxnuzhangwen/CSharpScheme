using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Interpreter.Scheme
{
    public class Primitive : Procedure
    {
        int minArgs;
        int maxArgs;
        int idNumber;

        public Primitive(int id, int minArgs, int maxArgs)
        {
            this.idNumber = id; this.minArgs = minArgs; this.maxArgs = maxArgs;
        }

        private  const int EQ = 0, LT = 1, GT = 2, GE = 3, LE = 4,
            ABS = 5, EOF_OBJECT = 6, EQQ = 7, EQUALQ = 8, FORCE = 9,
            CAR = 10, FLOOR = 11, CEILING = 12, CONS = 13,
            DIVIDE = 14, LENGTH = 15, LIST = 16, LISTQ = 17, APPLY = 18,
            MAX = 19, MIN = 20, MINUS = 21, NEWLINE = 22,
            NOT = 23, NULLQ = 24, NUMBERQ = 25, PAIRQ = 26, PLUS = 27,
            PROCEDUREQ = 28, READ = 29, CDR = 30, ROUND = 31, SECOND = 32,
            SYMBOLQ = 33, TIMES = 34, TRUNCATE = 35, WRITE = 36, APPEND = 37,
            BOOLEANQ = 38, SQRT = 39, EXPT = 40, REVERSE = 41, ASSOC = 42,
            ASSQ = 43, ASSV = 44, MEMBER = 45, MEMQ = 46, MEMV = 47, EQVQ = 48,
            LISTREF = 49, LISTTAIL = 50, STRINQ = 51, MAKESTRING = 52, STRING = 53,
            STRINGLENGTH = 54, STRINGREF = 55, STRINGSET = 56, SUBSTRING = 57,
            STRINGAPPEND = 58, STRINGTOLIST = 59, LISTTOSTRING = 60,
            SYMBOLTOSTRING = 61, STRINGTOSYMBOL = 62, EXP = 63, LOG = 64, SIN = 65,
            COS = 66, TAN = 67, ACOS = 68, ASIN = 69, ATAN = 70,
            NUMBERTOSTRING = 71, STRINGTONUMBER = 72, CHARQ = 73,
            CHARALPHABETICQ = 74, CHARNUMERICQ = 75, CHARWHITESPACEQ = 76,
            CHARUPPERCASEQ = 77, CHARLOWERCASEQ = 78, CHARTOINTEGER = 79,
            INTEGERTOCHAR = 80, CHARUPCASE = 81, CHARDOWNCASE = 82, STRINGQ = 83,
            VECTORQ = 84, MAKEVECTOR = 85, VECTOR = 86, VECTORLENGTH = 87,
            VECTORREF = 88, VECTORSET = 89, LISTTOVECTOR = 90, MAP = 91,
            FOREACH = 92, CALLCC = 93, VECTORTOLIST = 94, LOAD = 95, DISPLAY = 96,
            INPUTPORTQ = 98, CURRENTINPUTPORT = 99, OPENINPUTFILE = 100,
            CLOSEINPUTPORT = 101, OUTPUTPORTQ = 103, CURRENTOUTPUTPORT = 104,
            OPENOUTPUTFILE = 105, CLOSEOUTPUTPORT = 106, READCHAR = 107,
            PEEKCHAR = 108, EVAL = 109, QUOTIENT = 110, REMAINDER = 111,
            MODULO = 112, THIRD = 113, EOFOBJECTQ = 114, GCD = 115, LCM = 116,
            CXR = 117, ODDQ = 118, EVENQ = 119, ZEROQ = 120, POSITIVEQ = 121,
            NEGATIVEQ = 122,
            CHARCMP = 123 /* to 127 */, CHARCICMP = 128 /* to 132 */,
            STRINGCMP = 133 /* to 137 */, STRINGCICMP = 138 /* to 142 */,
            EXACTQ = 143, INEXACTQ = 144, INTEGERQ = 145,
            CALLWITHINPUTFILE = 146, CALLWITHOUTPUTFILE = 147
        ;

        //////////////// Extensions ////////////////

         const int NEW = -1, CLASS = -2, METHOD = -3, EXIT = -4,
            SETCAR = -5, SETCDR = -6, TIMECALL = -11, MACROEXPAND = -12,
            ERROR = -13, LISTSTAR = -14
        ;

        public static Environment installPrimitives(Environment env)
        {

            int n = int.MaxValue;

            env
             .defPrim("*", TIMES, 0, n)
             .defPrim("*", TIMES, 0, n)
             .defPrim("+", PLUS, 0, n)
             .defPrim("-", MINUS, 1, n)
             .defPrim("/", DIVIDE, 1, n)
             .defPrim("<", LT, 2, n)
             .defPrim("<=", LE, 2, n)
             .defPrim("=", EQ, 2, n)
             .defPrim(">", GT, 2, n)
             .defPrim(">=", GE, 2, n)
             .defPrim("abs", ABS, 1)
             .defPrim("acos", ACOS, 1)
             .defPrim("append", APPEND, 0, n)
             .defPrim("apply", APPLY, 2, n)
             .defPrim("asin", ASIN, 1)
             .defPrim("assoc", ASSOC, 2)
             .defPrim("assq", ASSQ, 2)
             .defPrim("assv", ASSV, 2)
             .defPrim("atan", ATAN, 1)
             .defPrim("Boolean?", BOOLEANQ, 1)
             .defPrim("caaaar", CXR, 1)
             .defPrim("caaadr", CXR, 1)
             .defPrim("caaar", CXR, 1)
             .defPrim("caadar", CXR, 1)
             .defPrim("caaddr", CXR, 1)
             .defPrim("caadr", CXR, 1)
             .defPrim("caar", CXR, 1)
             .defPrim("cadaar", CXR, 1)
             .defPrim("cadadr", CXR, 1)
             .defPrim("cadar", CXR, 1)
             .defPrim("caddar", CXR, 1)
             .defPrim("cadddr", CXR, 1)
             .defPrim("caddr", THIRD, 1)
             .defPrim("cadr", SECOND, 1)
             .defPrim("call-with-current-continuation", CALLCC, 1)
             .defPrim("call-with-input-file", CALLWITHINPUTFILE, 2)
             .defPrim("call-with-output-file", CALLWITHOUTPUTFILE, 2)
             .defPrim("car", CAR, 1)
             .defPrim("cdaaar", CXR, 1)
             .defPrim("cdaadr", CXR, 1)
             .defPrim("cdaar", CXR, 1)
             .defPrim("cdadar", CXR, 1)
             .defPrim("cdaddr", CXR, 1)
             .defPrim("cdadr", CXR, 1)
             .defPrim("cdar", CXR, 1)
             .defPrim("cddaar", CXR, 1)
             .defPrim("cddadr", CXR, 1)
             .defPrim("cddar", CXR, 1)
             .defPrim("cdddar", CXR, 1)
             .defPrim("cddddr", CXR, 1)
             .defPrim("cdddr", CXR, 1)
             .defPrim("cddr", CXR, 1)
             .defPrim("cdr", CDR, 1)
             .defPrim("char->integer", CHARTOINTEGER, 1)
             .defPrim("char-alphabetic?", CHARALPHABETICQ, 1)
             .defPrim("char-ci<=?", CHARCICMP + LE, 2)
             .defPrim("char-ci<?", CHARCICMP + LT, 2)
             .defPrim("char-ci=?", CHARCICMP + EQ, 2)
             .defPrim("char-ci>=?", CHARCICMP + GE, 2)
             .defPrim("char-ci>?", CHARCICMP + GT, 2)
             .defPrim("char-downcase", CHARDOWNCASE, 1)
             .defPrim("char-lower-case?", CHARLOWERCASEQ, 1)
             .defPrim("char-numeric?", CHARNUMERICQ, 1)
             .defPrim("char-upcase", CHARUPCASE, 1)
             .defPrim("char-upper-case?", CHARUPPERCASEQ, 1)
             .defPrim("char-whitespace?", CHARWHITESPACEQ, 1)
             .defPrim("char<=?", CHARCMP + LE, 2)
             .defPrim("char<?", CHARCMP + LT, 2)
             .defPrim("char=?", CHARCMP + EQ, 2)
             .defPrim("char>=?", CHARCMP + GE, 2)
             .defPrim("char>?", CHARCMP + GT, 2)
             .defPrim("char?", CHARQ, 1)
             .defPrim("close-input-port", CLOSEINPUTPORT, 1)
             .defPrim("close-output-port", CLOSEOUTPUTPORT, 1)
             .defPrim("complex?", NUMBERQ, 1)
             .defPrim("cons", CONS, 2)
             .defPrim("cos", COS, 1)
             .defPrim("current-input-port", CURRENTINPUTPORT, 0)
             .defPrim("current-output-port", CURRENTOUTPUTPORT, 0)
             .defPrim("display", DISPLAY, 1, 2)
             .defPrim("eof-object?", EOFOBJECTQ, 1)
             .defPrim("eq?", EQQ, 2)
             .defPrim("equal?", EQUALQ, 2)
             .defPrim("eqv?", EQVQ, 2)
             .defPrim("eval", EVAL, 1, 2)
             .defPrim("even?", EVENQ, 1)
             .defPrim("exact?", INTEGERQ, 1)
             .defPrim("exp", EXP, 1)
             .defPrim("expt", EXPT, 2)
             .defPrim("force", FORCE, 1)
             .defPrim("for-each", FOREACH, 1, n)
             .defPrim("gcd", GCD, 0, n)
             .defPrim("inexact?", INEXACTQ, 1)
             .defPrim("input-port?", INPUTPORTQ, 1)
             .defPrim("integer->char", INTEGERTOCHAR, 1)
             .defPrim("integer?", INTEGERQ, 1)
             .defPrim("lcm", LCM, 0, n)
             .defPrim("length", LENGTH, 1)
             .defPrim("list", LIST, 0, n)
             .defPrim("list->string", LISTTOSTRING, 1)
             .defPrim("list->vector", LISTTOVECTOR, 1)
             .defPrim("list-ref", LISTREF, 2)
             .defPrim("list-tail", LISTTAIL, 2)
             .defPrim("list?", LISTQ, 1)
             .defPrim("load", LOAD, 1)
             .defPrim("log", LOG, 1)
             .defPrim("macro-expand", MACROEXPAND, 1)
             .defPrim("make-string", MAKESTRING, 1, 2)
             .defPrim("make-vector", MAKEVECTOR, 1, 2)
             .defPrim("map", MAP, 1, n)
             .defPrim("max", MAX, 1, n)
             .defPrim("member", MEMBER, 2)
             .defPrim("memq", MEMQ, 2)
             .defPrim("memv", MEMV, 2)
             .defPrim("min", MIN, 1, n)
             .defPrim("modulo", MODULO, 2)
             .defPrim("negative?", NEGATIVEQ, 1)
             .defPrim("newline", NEWLINE, 0, 1)
             .defPrim("not", NOT, 1)
             .defPrim("null?", NULLQ, 1)
             .defPrim("number->string", NUMBERTOSTRING, 1, 2)
             .defPrim("number?", NUMBERQ, 1)
             .defPrim("odd?", ODDQ, 1)
             .defPrim("open-input-file", OPENINPUTFILE, 1)
             .defPrim("open-output-file", OPENOUTPUTFILE, 1)
             .defPrim("output-port?", OUTPUTPORTQ, 1)
             .defPrim("pair?", PAIRQ, 1)
             .defPrim("peek-char", PEEKCHAR, 0, 1)
             .defPrim("positive?", POSITIVEQ, 1)
             .defPrim("procedure?", PROCEDUREQ, 1)
             .defPrim("quotient", QUOTIENT, 2)
             .defPrim("rational?", INTEGERQ, 1)
             .defPrim("read", READ, 0, 1)
             .defPrim("read-char", READCHAR, 0, 1)
             .defPrim("real?", NUMBERQ, 1)
             .defPrim("remainder", REMAINDER, 2)
             .defPrim("reverse", REVERSE, 1)
             .defPrim("round", ROUND, 1)
             .defPrim("set-car!", SETCAR, 2)
             .defPrim("set-cdr!", SETCDR, 2)
             .defPrim("sin", SIN, 1)
             .defPrim("sqrt", SQRT, 1)
             .defPrim("string", STRING, 0, n)
             .defPrim("string->list", STRINGTOLIST, 1)
             .defPrim("string->number", STRINGTONUMBER, 1, 2)
             .defPrim("string->symbol", STRINGTOSYMBOL, 1)
             .defPrim("string-append", STRINGAPPEND, 0, n)
             .defPrim("string-ci<=?", STRINGCICMP + LE, 2)
             .defPrim("string-ci<?", STRINGCICMP + LT, 2)
             .defPrim("string-ci=?", STRINGCICMP + EQ, 2)
             .defPrim("string-ci>=?", STRINGCICMP + GE, 2)
             .defPrim("string-ci>?", STRINGCICMP + GT, 2)
             .defPrim("string-length", STRINGLENGTH, 1)
             .defPrim("string-ref", STRINGREF, 2)
             .defPrim("string-set!", STRINGSET, 3)
             .defPrim("string<=?", STRINGCMP + LE, 2)
             .defPrim("string<?", STRINGCMP + LT, 2)
             .defPrim("string=?", STRINGCMP + EQ, 2)
             .defPrim("string>=?", STRINGCMP + GE, 2)
             .defPrim("string>?", STRINGCMP + GT, 2)
             .defPrim("string?", STRINGQ, 1)
             .defPrim("substring", SUBSTRING, 3)
             .defPrim("symbol->string", SYMBOLTOSTRING, 1)
             .defPrim("symbol?", SYMBOLQ, 1)
             .defPrim("tan", TAN, 1)
             .defPrim("vector", VECTOR, 0, n)
             .defPrim("vector->list", VECTORTOLIST, 1)
             .defPrim("vector-length", VECTORLENGTH, 1)
             .defPrim("vector-ref", VECTORREF, 2)
             .defPrim("vector-set!", VECTORSET, 3)
             .defPrim("vector?", VECTORQ, 1)
             .defPrim("write", WRITE, 1, 2)
             .defPrim("write-char", DISPLAY, 1, 2)
             .defPrim("zero?", ZEROQ, 1)

             ///////////// Extensions ////////////////

             .defPrim("new", NEW, 1)
             .defPrim("class", CLASS, 1)
             .defPrim("exit", EXIT, 0, 1)
             .defPrim("error", ERROR, 0, n)
             .defPrim("_list*", LISTSTAR, 0, n)
               ;

            return env;

        }


        /** Return <0 if x is alphabetically first, >0 if y is first,
 * 0 if same.  Case insensitive iff ci is true.  Error if not strings. **/
        public static int stringCompare(Object x, Object y, Boolean ci)
        {
            if (x is char[] && y is char[])
            {
                char[] xc = (char[])x, yc = (char[])y;
                for (int i = 0; i < xc.Length; i++)
                {
                    int diff = (!ci) ? xc[i] - yc[i]
                      : Char.ToUpper(xc[i]) - Char.ToUpper(yc[i]);
                    if (diff != 0) return diff;
                }
                return xc.Length - yc.Length;
            }
            else
            {
                error("expected two strings, got: " + stringify(list(x, y)));
                return 0;
            }
        }

        public static char[] stringAppend(Object args)
        {
            StringBuilder result = new StringBuilder();
            for (; args is Pair; args = rest(args))
            {
                result.Append(stringify(first(args), false));
            }
            return result.ToString().ToCharArray();
        }

        static Boolean isList(Object x)
        {
            Object slow = x, fast = x;
            for (; ; )
            {
                if (fast == null) return true;
                if (slow == rest(fast) || !(fast is Pair)
                || !(slow is Pair)) return false;
                slow = rest(slow);
                fast = rest(fast);
                if (fast == null) return true;
                if (!(fast is Pair)) return false;
                fast = rest(fast);
            }
        }

        public static Object memberAssoc(Object obj, Object list, char m, char eq)
        {
            while (list is Pair)
            {
                Object target = (m == 'm') ? first(list) : first(first(list));
                Boolean found;
                switch (eq)
                {
                    case 'q': found = (target == obj); break;
                    case 'v': found = eqv(target, obj); break;
                    case ' ': found = equal(target, obj); break;
                    default: warn("Bad option to memberAssoc:" + eq); return FALSE;
                }
                if (found) return (m == 'm') ? list : first(list);
                list = rest(list);
            }
            return FALSE;
        }

        static Object append(Object args)
        {
            if (rest(args) == null) return first(args);
            else return append2(first(args), append(rest(args)));
        }

        static Object append2(Object x, Object y)
        {
            if (x is Pair) return cons(first(x), append2(rest(x), y));
            else return y;
        }

        static Object gcd(Object args)
        {
            long gcd = 0;
            while (args is Pair)
            {
                gcd = gcd2(Math.Abs((long)num(first(args))), gcd);
                args = rest(args);
            }
            return num(gcd);
        }

        static long gcd2(long a, long b)
        {
            if (b == 0) return a;
            else return gcd2(b, a % b);
        }

        static Object lcm(Object args)
        {
            long L = 1, g = 1;
            while (args is Pair)
            {
                long n = Math.Abs((long)num(first(args)));
                g = gcd2(n, L);
                L = (g == 0) ? g : (n / g) * L;
                args = rest(args);
            }
            return num(L);
        }

        public static Object numCompare(Object args, char op)
        {
            while (rest(args) is Pair)
            {
                double x = num(first(args)); args = rest(args);
                double y = num(first(args));
                switch (op)
                {
                    case '>': if (!(x > y)) return FALSE; break;
                    case '<': if (!(x < y)) return FALSE; break;
                    case '=': if (!(x == y)) return FALSE; break;
                    case 'L': if (!(x <= y)) return FALSE; break;
                    case 'G': if (!(x >= y)) return FALSE; break;
                    default: error("internal error: unrecognized op: " + op); break;
                }
            }
            return TRUE;
        }

        public static Object numCompute(Object args, char op, double result)
        {
            if (args == null)
            {
                switch (op)
                {
                    case '-': return num(0 - result);
                    case '/': return num(1 / result);
                    default: return num(result);
                }
            }
            else
            {
                while (args is Pair)
                {
                    double x = num(first(args)); args = rest(args);
                    switch (op)
                    {
                        case 'X': if (x > result) result = x; break;
                        case 'N': if (x < result) result = x; break;
                        case '+': result += x; break;
                        case '-': result -= x; break;
                        case '*': result *= x; break;
                        case '/': result /= x; break;
                        default: error("internal error: unrecognized op: " + op); break;
                    }
                }
                return num(result);
            }
        }

        /** Return the sign of the argument: +1, -1, or 0. **/
        static int sign(int x) { return (x > 0) ? +1 : (x < 0) ? -1 : 0; }

        /** Return <0 if x is alphabetically first, >0 if y is first,
         * 0 if same.  Case insensitive iff ci is true.  Error if not both chars. **/
        public static int charCompare(Object x, Object y, Boolean ci)
        {
            char xc = chr(x), yc = chr(y);
            if (ci) { xc = Char.ToLower(xc); yc = Char.ToLower(yc); }
            return xc - yc;
        }

        static Object numberToString(Object x, Object y)
        {
            int bases = (y is Double) ? (int)num(y) : 10;
            if (bases != 10 || num(x) == Math.Round(num(x)))
            {
                // An integer
                return ((long)num(x)).ToString().ToCharArray();
            }
            else
            {
                // A floating point number
                return x.ToString().ToCharArray();
            }
        }

        static TextWriter openOutputFile(Object filename)
        {
            try
            {
                return new StreamWriter(stringify(filename, false));
            }
            catch (FileNotFoundException e)
            {
                return (TextWriter)error("No such file: " + stringify(filename));
            }
            catch (SystemException e)
            {
                return (TextWriter)error("SystemException: " + e);
            }
        }

        static Object stringToNumber(Object x, Object y)
        {
            int bases = (y is Double) ? (int)num(y) : 10;
            try
            {
                return (bases == 10)
              ? Double.Parse(stringify(x, false))
              : num(long.Parse(stringify(x, false)));
            }
            catch (Exception e) { return FALSE; }
        }

        static InputPort openInputFile(Object filename)
        {
            try
            {
                return new InputPort(new StringReader(stringify(filename, false)));
            }
            catch (FileNotFoundException e)
            {
                return (InputPort)error("No such file: " + stringify(filename));
            }
            catch (SystemException e)
            {
                return (InputPort)error("SystemException: " + e);
            }
        }

        /** Map proc over a list of lists of args, in the given interpreter.
 * If result is non-null, accumulate the results of each call there
 * and return that at the end.  Otherwise, just return null. **/
        static Pair map(Procedure proc, Object args, Scheme interp, Pair result)
        {
            Pair accum = result;
            if (rest(args) == null)
            {
                args = first(args);
                while (args is Pair)
                {
                    Object x = proc.apply(interp, list(first(args)));
                    if (accum != null) accum = (Pair)(accum.rest = list(x));
                    args = rest(args);
                }
            }
            else
            {
                Procedure car = Procedure.proc(interp.eval("car")), cdr = Procedure.proc(interp.eval("cdr"));
                while (first(args) is Pair)
                {
                    Object x = proc.apply(interp, map(car, list(args), interp, list(null)));
                    if (accum != null) accum = (Pair)(accum.rest = list(x));
                    args = map(cdr, list(args), interp, list(null));
                }
            }
            return (Pair)rest(result);
        }

        static Boolean isExact(Object x)
        {
            if (!(x is Double)) return false;
            double d = num(x);
            return (d == Math.Round(d) && Math.Abs(d) < 102962884861573423.0);
        }

        /** Apply a primitive to a list of arguments. **/
        public override object apply(Scheme interp, object args)
        {
                  //First make sure there are the right number of arguments. 
      int nArgs = length(args);
      if (nArgs < minArgs) 
	return error("too few args, " + nArgs +
		     ", for " + this.name + ": " + args);
      else if (nArgs > maxArgs)
	return error("too many args, " + nArgs +
		     ", for " + this.name + ": " + args);

    Object x = first(args);
    Object y = second(args);

    switch (idNumber) {

      ////////////////  SECTION 6.1 BOOLEANS
    case NOT:       	return truth((Boolean)x == FALSE);
    case BOOLEANQ: return truth((Boolean)x == TRUE || (Boolean)x == FALSE);

      ////////////////  SECTION 6.2 EQUIVALENCE PREDICATES
    case EQVQ: 		return truth(eqv(x, y));
    case EQQ: 		return truth(x == y);
    case EQUALQ:  	return truth(equal(x,y));

      ////////////////  SECTION 6.3 LISTS AND PAIRS
    case PAIRQ:  	return truth(x is Pair);
    case LISTQ:         return truth(isList(x));
    case CXR:           for (int i = name.Length-2; i >= 1; i--) 
                          x = (name[i] == 'a') ? first(x) : rest(x);
                        return x;
    case CONS:  	return cons(x, y);
    case CAR:  	        return first(x);
    case CDR:  	        return rest(x);
    case SETCAR:        return setFirst(x, y);
    case SETCDR:        return setRest(x, y);
    case SECOND:  	return second(x);
    case THIRD:  	return third(x);
    case NULLQ:         return truth(x == null);
    case LIST:  	return args;
    case LENGTH:  	return num(length(x));
    case APPEND:        return (args == null) ? null : append(args);
    case REVERSE:       return reverse(x);
    case LISTTAIL: 	for (int k = (int)num(y); k>0; k--) x = rest(x);
                        return x;
    case LISTREF:  	for (int k = (int)num(y); k>0; k--) x = rest(x);
                        return first(x);
    case MEMQ:      	return memberAssoc(x, y, 'm', 'q');
    case MEMV:      	return memberAssoc(x, y, 'm', 'v');
    case MEMBER:    	return memberAssoc(x, y, 'm', ' ');
    case ASSQ:      	return memberAssoc(x, y, 'a', 'q');
    case ASSV:      	return memberAssoc(x, y, 'a', 'v');
    case ASSOC:     	return memberAssoc(x, y, 'a', ' ');

      ////////////////  SECTION 6.4 SYMBOLS
    case SYMBOLQ:  	return truth(x is String);
    case SYMBOLTOSTRING:return sym(x).ToCharArray();
    case STRINGTOSYMBOL:return new String(SchemeUtils.str(x));

      ////////////////  SECTION 6.5 NUMBERS
    case NUMBERQ:  	return truth(x is Double);
    case ODDQ:          return truth(Math.Abs(num(x)) % 2 != 0);
    case EVENQ:         return truth(Math.Abs(num(x)) % 2 == 0);
    case ZEROQ:         return truth(num(x) == 0);
    case POSITIVEQ:     return truth(num(x) > 0);
    case NEGATIVEQ:     return truth(num(x) < 0);
    case INTEGERQ:      return truth(isExact(x));
    case INEXACTQ:      return truth(!isExact(x));
    case LT:		return numCompare(args, '<');
    case GT:		return numCompare(args, '>');
    case EQ:		return numCompare(args, '=');
    case LE: 		return numCompare(args, 'L');
    case GE: 		return numCompare(args, 'G');
    case MAX: 		return numCompute(args, 'X', num(x));
    case MIN: 		return numCompute(args, 'N', num(x));
    case PLUS:		return numCompute(args, '+', 0.0);
    case MINUS:		return numCompute(rest(args), '-', num(x));
    case TIMES:		return numCompute(args, '*', 1.0);
    case DIVIDE:	return numCompute(rest(args), '/', num(x));
    case QUOTIENT:      double d = num(x)/num(y);
                        return num(d > 0 ? Math.Floor(d) : Math.Ceiling(d));
    case REMAINDER:     return num((long)num(x) % (long)num(y));
    case MODULO:        long xi = (long)num(x), yi = (long)num(y), m = xi % yi;
                        return num((xi*yi > 0 || m == 0) ? m : m + yi);
    case ABS: 		return num(Math.Abs(num(x)));
    case FLOOR: 	return num(Math.Floor(num(x)));
    case CEILING: 	return num(Math.Ceiling(num(x))); 
    case TRUNCATE: 	d = num(x);
      	                return num((d < 0.0) ? Math.Ceiling(d) : Math.Floor(d)); 
    case ROUND: 	return num(Math.Round(num(x)));
    case EXP:           return num(Math.Exp(num(x)));
    case LOG:           return num(Math.Log(num(x)));
    case SIN:           return num(Math.Sin(num(x)));
    case COS:           return num(Math.Cos(num(x)));
    case TAN:           return num(Math.Tan(num(x)));
    case ASIN:          return num(Math.Asin(num(x)));
    case ACOS:          return num(Math.Acos(num(x)));
    case ATAN:          return num(Math.Atan(num(x)));
    case SQRT:      	return num(Math.Sqrt(num(x)));
    case EXPT:      	return num(Math.Pow(num(x), num(y)));
    case NUMBERTOSTRING:return numberToString(x, y);
    case STRINGTONUMBER:return stringToNumber(x, y);
    case GCD:           return (args == null) ? ZERO : gcd(args);
    case LCM:           return (args == null) ? ONE  : lcm(args);
                        
      ////////////////  SECTION 6.6 CHARACTERS
    case CHARQ:           return truth(x is Char);
    case CHARALPHABETICQ: return truth(Char.IsLetter(chr(x)));
    case CHARNUMERICQ:    return truth(Char.IsDigit(chr(x)));
    case CHARWHITESPACEQ: return truth(Char.IsWhiteSpace(chr(x)));
    case CHARUPPERCASEQ:  return truth(Char.IsUpper(chr(x)));
    case CHARLOWERCASEQ:  return truth(Char.IsLower(chr(x)));
    case CHARTOINTEGER:   return (double)chr(x);
    case INTEGERTOCHAR:   return chr((char)(int)num(x));
    case CHARUPCASE:      return chr(Char.ToUpper(chr(x)));
    case CHARDOWNCASE:    return chr(Char.ToLower(chr(x)));
    case CHARCMP+EQ:      return truth(charCompare(x, y, false) == 0);
    case CHARCMP+LT:      return truth(charCompare(x, y, false) <  0);
    case CHARCMP+GT:      return truth(charCompare(x, y, false) >  0);
    case CHARCMP+GE:      return truth(charCompare(x, y, false) >= 0);
    case CHARCMP+LE:      return truth(charCompare(x, y, false) <= 0);
    case CHARCICMP+EQ:    return truth(charCompare(x, y, true)  == 0);
    case CHARCICMP+LT:    return truth(charCompare(x, y, true)  <  0);
    case CHARCICMP+GT:    return truth(charCompare(x, y, true)  >  0);
    case CHARCICMP+GE:    return truth(charCompare(x, y, true)  >= 0);
    case CHARCICMP+LE:    return truth(charCompare(x, y, true)  <= 0);

    case ERROR:         return error(stringify(args));

      ////////////////  SECTION 6.7 STRINGS
    case STRINGQ:   	return truth(x is char[]);
    case MAKESTRING:char[] str = new char[(int)num(x)];
      if (y != null) {
	char c = chr(y);
	for (int i = str.Length-1; i >= 0; i--) str[i] = c;
      }
      return str;
    case STRING:    	return listToString(args);
    case STRINGLENGTH: 	return num(SchemeUtils.str(x).Length);
    case STRINGREF: return chr(SchemeUtils.str(x)[(int)num(y)]);
    case STRINGSET: Object z = third(args); SchemeUtils.str(x)[(int)num(y)] = chr(z); 
                        return z;
    case SUBSTRING: 	int start = (int)num(y), end = (int)num(third(args));
                        return new String(SchemeUtils.str(x), start, end - start).ToCharArray();
    case STRINGAPPEND: 	return stringAppend(args);
    case STRINGTOLIST:  Pair result = null;
        char[] str2 = SchemeUtils.str(x);
			for (int i = str2.Length-1; i >= 0; i--)
			  result = cons(chr(str2[i]), result);
			return result;
    case LISTTOSTRING:  return listToString(x);
    case STRINGCMP+EQ:  return truth(stringCompare(x, y, false) == 0);
    case STRINGCMP+LT:  return truth(stringCompare(x, y, false) <  0);
    case STRINGCMP+GT:  return truth(stringCompare(x, y, false) >  0);
    case STRINGCMP+GE:  return truth(stringCompare(x, y, false) >= 0);
    case STRINGCMP+LE:  return truth(stringCompare(x, y, false) <= 0);
    case STRINGCICMP+EQ:return truth(stringCompare(x, y, true)  == 0);
    case STRINGCICMP+LT:return truth(stringCompare(x, y, true)  <  0);
    case STRINGCICMP+GT:return truth(stringCompare(x, y, true)  >  0);
    case STRINGCICMP+GE:return truth(stringCompare(x, y, true)  >= 0);
    case STRINGCICMP+LE:return truth(stringCompare(x, y, true)  <= 0);

      ////////////////  SECTION 6.8 VECTORS
    case VECTORQ:	return truth(x is Object[]);
    case MAKEVECTOR:    Object[] vec = new Object[(int)num(x)];
                        if (y != null) {
			  for (int i = 0; i < vec.Length; i++) vec[i] = y;
			}
			return vec;
    case VECTOR:        return listToVector(args);
    case VECTORLENGTH: return num(SchemeUtils.vec(x).Length);
    case VECTORREF: return SchemeUtils.vec(x)[(int)num(y)];
    case VECTORSET: return SchemeUtils.vec(x)[(int)num(y)] = third(args);
    case VECTORTOLIST:  return vectorToList(x);
    case LISTTOVECTOR:  return listToVector(x);

      ////////////////  SECTION 6.9 CONTROL FEATURES
    case EVAL:          return interp.eval(x);
    case FORCE:         return (!(x is Procedure)) ? x
              : Procedure.proc(x).apply(interp, null);
    case MACROEXPAND:   return Macro.macroExpand(interp, x);
    case PROCEDUREQ:	return truth(x is Procedure);
    case APPLY: return Procedure.proc(x).apply(interp, listStar(rest(args)));
    case MAP: return map(Procedure.proc(x), rest(args), interp, list(null));
    case FOREACH: return map(Procedure.proc(x), rest(args), interp, null);
    case CALLCC: Exception cc = new Exception();
        Continuation proc = new Continuation(cc);
        try { return Procedure.proc(x).apply(interp, list(proc)); }
        catch (Exception e)
        {
            if (e == cc) return proc.value; else throw e;
        }
      ////////////////  SECTION 6.10 INPUT AND OUPUT
    case EOFOBJECTQ:         return truth(string.Equals(x, InputPort.EOF));
    case INPUTPORTQ:         return truth(x is InputPort);
    case CURRENTINPUTPORT:   return interp.input;
    case OPENINPUTFILE:      return openInputFile(x);
    case CLOSEINPUTPORT:     return inPort(x, interp).close();
    case OUTPUTPORTQ: return truth(x is TextWriter);
    case CURRENTOUTPUTPORT:  return interp.output;
    case OPENOUTPUTFILE:     return openOutputFile(x);
    case CALLWITHOUTPUTFILE: TextWriter p = null;
                             try { p = openOutputFile(x);
                                   z = Procedure.proc(y).apply(interp, list(p));
                             } finally { if (p != null) p.Close(); }
                             return z;
    case CALLWITHINPUTFILE:  InputPort p2 = null;
                             try { p2 = openInputFile(x);
                                    z = Procedure.proc(y).apply(interp, list(p2));
                             }
                             finally { if (p2 != null) p2.close(); }
                             return z;
    case CLOSEOUTPUTPORT:    outPort(x, interp).Close(); return TRUE; 
    case READCHAR:      return inPort(x, interp).readChar();
    case PEEKCHAR:      return inPort(x, interp).peekChar();
    case LOAD:          return interp.load(x);
    case READ:  	return inPort(x, interp).read(); 
    case EOF_OBJECT:    return truth(InputPort.isEOF(x));
    case WRITE:  	return write(x, outPort(y, interp), true);
    case DISPLAY:       return write(x, outPort(y, interp), false);
    case NEWLINE:  	outPort(x, interp).WriteLine();
                        outPort(x, interp).Flush(); return TRUE;

      ////////////////  EXTENSIONS
    case CLASS:         try { return Type.GetType(stringify(x, false)); }
                        catch (Exception e) { return FALSE; }
    case NEW:           try { return Type.GetType(x.ToString()).Assembly.CreateInstance(x.ToString()); }
                        catch (Exception e) { return FALSE; }
    case EXIT: System.Environment.Exit((x == null) ? 0 : (int)num(x)); return 0;
    case LISTSTAR:      return listStar(args);
    default:            return error("internal error: unknown primitive: " 
				     + this + " applied to " + args);
    }
        }
    }
}
