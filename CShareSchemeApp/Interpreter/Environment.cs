using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interpreter.Scheme
{
    public class Environment : SchemeUtils
    {
        public Object vars;
        public Object vals;
        public Environment parent;

        /** Construct an empty environment: no bindings. **/
        public Environment() { }

        /** A constructor to extend an environment with var/val pairs. */
        public Environment(Object vars, Object vals, Environment parent)
        {
            this.vars = vars;
            this.vals = vals;
            this.parent = parent;
            if (!numberArgsOK(vars, vals))
                warn("wrong number of arguments: expected " + vars +
                    " got " + vals);
        }

        public Environment defPrim(String name, int id, int minArgs)
        {
            define(name, new Primitive(id, minArgs, minArgs));
            return this;
        }

        public Environment defPrim(String name, int id, int minArgs, int maxArgs)
        {
            define(name, new Primitive(id, minArgs, maxArgs));
            return this;
        }

        /** Add a new variable,value pair to this environment. */
        public Object define(Object var, Object val)
        {
            vars = cons(var, vars);
            vals = cons(val, vals);
            if (val is Procedure
                && ((Procedure)val).name.Equals("anonymous procedure"))
                ((Procedure)val).name = var.ToString();
            return var;
        }

        /** Find the value of a symbol, in this environment or a parent. */
        public Object lookup(String symbol)
        {
            Object varList = vars, valList = vals;
            // See if the symbol is bound locally
            while (varList != null)
            {
                if (string.Equals(first(varList), symbol))
                {
                    return first(valList);
                }
                else if (string.Equals(varList, symbol))
                {
                    return valList;
                }
                else
                {
                    varList = rest(varList);
                    valList = rest(valList);
                }
            }
            // If not, try to look for the parent
            if (parent != null) return parent.lookup(symbol);
            else return error("Unbound variable: " + symbol);
        }

        /** Set the value of an existing variable **/
        public Object set(Object var, Object val)
        {
            if (!(var is String))
                return error("Attempt to set a non-symbol: "
                        + stringify(var)); ;
            String symbol = (String)var;
            Object varList = vars, valList = vals;
            // See if the symbol is bound locally
            while (varList != null)
            {
                if (string.Equals(first(varList),symbol))
                {
                    return setFirst(valList, val);
                }
                else if (string.Equals(rest(varList), symbol))
                {
                    return setRest(valList, val);
                }
                else
                {
                    varList = rest(varList);
                    valList = rest(valList);
                }
            }
            // If not, try to look for the parent
            if (parent != null) return parent.set(symbol, val);
            else return error("Unbound variable: " + symbol);
        }

        /** See if there is an appropriate number of vals for these vars. **/
        Boolean numberArgsOK(Object vars, Object vals)
        {
            return ((vars == null && vals == null)
                || (vars is String)
                || (vars is Pair && vals is Pair
                    && numberArgsOK(((Pair)vars).rest, ((Pair)vals).rest)));
        }
    }
}
