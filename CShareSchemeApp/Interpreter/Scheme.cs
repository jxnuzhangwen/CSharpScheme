using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Interpreter.Scheme
{
    public class Scheme: SchemeUtils
    {
        public InputPort input = new InputPort(Console.In);
        public TextWriter output = Console.Out;
        public Environment globalEnvironment = new Environment();

        //////////////// Main Loop

        /** Create a new Scheme interpreter, passing in the command line args
         * as files to load, and then enter a read eval write loop. **/
        //public static void Main(String[] files)
        //{
        //    new Scheme(files).readEvalWriteLoop();
        //}

        /** Create a Scheme interpreter and load an array of files into it.
        * Also load SchemePrimitives.CODE. **/ 
        public Scheme(String[] files)
        {
            Primitive.installPrimitives(globalEnvironment);
            try
            {
                load(new InputPort(new StringReader(SchemePrimitives.CODE)));
                /*
                for (int i = 0; i < (files == null ? 0 : files.length); i++)
                {
                    load(files[i]);
                }
                */
            }
            catch (Exception e) { ; }
        }

        public Scheme()
        {
            Primitive.installPrimitives(globalEnvironment);
        }

        //public Scheme(string s)
        //{
        //    Primitive.installPrimitives(globalEnvironment);
        //    load(new InputPort(new StringReader(s));
        //}

        public Object evalStatement(string statement){
            InputPort inp = new InputPort(new StringReader(statement));
            Object x = null;
            if (InputPort.isEOF(x = inp.read())) return TRUE;
            return eval(x);
        }

          /** Eval all the expressions coming from an InputPort. **/
          public Object load(InputPort ins) {
            Object x = null;
            for(;;) {
                if (InputPort.isEOF(x = ins.read())) return TRUE;
              eval(x); 
            }
          }

          /** Eval all the expressions in a file. Calls load(InputPort). **/
          public Object load(Object fileName)
          {
              String name = stringify(fileName, false);
              try { return load(new InputPort(new StreamReader(name))); }
              catch (SystemException e) { return error("can't load " + name); }
          }

        
          /** Prompt, read, eval, and write the result. 
           * Also sets up a catch for any SystemExceptions encountered. **/
          public void readEvalWriteLoop() {
            Object x;
            for(;;) {
              try {
	        output.Write("> "); output.Flush();
	        if (InputPort.isEOF(x = input.read())) return;
	        write(eval(x), output, true);
            output.WriteLine(); output.Flush();
              } catch (SystemException  e) { ; }
            }
          }

          //////////////// Evaluation

          /** Evaluate an object, x, in an environment. **/
          public Object eval(Object x, Environment env)
          {
              // The purpose of the while loop is to allow tail recursion.
              // The idea is that in a tail recursive position, we do "x = ..."
              // and loop, rather than doing "return eval(...)".
              while (true)
              {
                  if (x is String)
                  {         // VARIABLE
                      return env.lookup((String)x);
                  }
                  else if (!(x is Pair))
                  { // CONSTANT
                      return x;
                  }
                  else
                  {
                      Object fn = first(x);
                      Object args = rest(x);
                      if (string.Equals(fn, "quote"))
                      {             // QUOTE
                          return first(args);
                      }
                      else if (string.Equals(fn, "begin"))
                      {      // BEGIN
                          for (; rest(args) != null; args = rest(args))
                          {
                              eval(first(args), env);
                          }
                          x = first(args);
                      }
                      else if (string.Equals(fn, "define"))
                      {     // DEFINE
                          if (first(args) is Pair)
                              return env.define(first(first(args)),
                               eval(cons("lambda", cons(rest(first(args)), rest(args))), env));
                          else return env.define(first(args), eval(second(args), env));
                      }
                      else if (string.Equals(fn, "set!"))
                      {       // SET!
                          return env.set(first(args), eval(second(args), env));
                      }
                      else if (string.Equals(fn, "if"))
                      {         // IF
                          x = (truth(eval(first(args), env))) ? second(args) : third(args);
                      }
                      else if (string.Equals(fn, "cond"))
                      {       // COND
                          x = reduceCond(args, env);
                      }
                      else if (string.Equals(fn, "lambda"))
                      {     // LAMBDA
                          return new Closure(first(args), rest(args), env);
                      }
                      else if (string.Equals(fn, "macro"))
                      {      // MACRO
                          return new Macro(first(args), rest(args), env);
                      }
                      else
                      {                         // PROCEDURE CALL:
                          fn = eval(fn, env);
                          if (fn is Macro)
                          {          // (MACRO CALL)
                              x = ((Macro)fn).expand(this, (Pair)x, args);
                          }
                          else if (fn is Closure)
                          { // (CLOSURE CALL)
                              Closure f = (Closure)fn;
                              x = f.body;
                              env = new Environment(f.parms, evalList(args, env), f.env);
                          }
                          else
                          {                            // (OTHER PROCEDURE CALL)
                              return Procedure.proc(fn).apply(this, evalList(args, env));
                          }
                      }
                  }
              }
          }

          /** Eval in the global environment. **/
          public Object eval(Object x) { return eval(x, this.globalEnvironment); }

          /** Evaluate each of a list of expressions. **/
          Pair evalList(Object list, Environment env)
          {
              if (list == null)
                  return null;
              else if (!(list is Pair))
              {
                  error("Illegal arg list: " + list);
                  return null;
              }
              else
                  return cons(eval(first(list), env), evalList(rest(list), env));
          }

          /** Reduce a cond expression to some code which, when evaluated,
            * gives the value of the cond expression.  We do it that way to
            * maintain tail recursion. **/
          Object reduceCond(Object clauses, Environment env)
          {
              Object result = null;
              for (; ; )
              {
                  if (clauses == null) return FALSE;
                  Object clause = first(clauses); clauses = rest(clauses);
                  if (string.Equals(SchemeUtils.first(clause), "else")
                  || truth(result = eval(first(clause), env)))
                      if (rest(clause) == null) return list("quote", result);
                      else if (string.Equals(SchemeUtils.second(clause), "=>"))
                          return list(third(clause), list("quote", result));
                      else return cons("begin", rest(clause));
              }
          }
    }
}
