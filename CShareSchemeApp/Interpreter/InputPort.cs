using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Interpreter.Scheme
{
    public class InputPort : SchemeUtils
    {
        public static String EOF = "#!EOF";
        Boolean isPushedToken = false;
        Boolean isPushedChar = false;
        Object pushedToken = null;
        int pushedChar = -1;
        StringBuilder buff = new StringBuilder();
        TextReader inp;
        public InputPort(StringReader inp) { this.inp = inp; }
        public InputPort(TextReader inp) { this.inp = inp; }

        /** Is the argument the EOF object? **/
        //public static Boolean isEOF(Object x) { return x == EOF; }
        /** Is the argument the EOF object? **/
        public static Boolean isEOF(Object x) { return x == EOF; }


        /** Close the port.  Return TRUE if ok. **/
        public Object close()
        {
            try { this.inp.Close(); return TRUE; }
            catch (SystemException e) { return error("SystemException: " + e); }
        }

        /** Read and return a Scheme character or EOF. **/
        public Object readChar()
        {
            try
            {
                if (isPushedChar)
                {
                    isPushedChar = false;
                    if (pushedChar == -1) return EOF; else return chr((char)pushedChar);
                }
                else
                {
                    int ch = inp.Read();
                    if (ch == -1) return EOF; else return chr((char)ch);
                }
            }
            catch (SystemException e)
            {
                warn("On input, exception: " + e);
                return EOF;
            }
        }

        /** Peek at and return the next Scheme character (or EOF).
         * However, don't consume the character. **/
        public Object peekChar()
        {
            int p = peekCh();
            if (p == -1) return EOF; else return chr((char)p);
        }
        /** Peek at and return the next Scheme character as an int, -1 for EOF.
 * However, don't consume the character. **/
        public int peekCh()
        {
            try { return isPushedChar ? pushedChar : pushChar(inp.Read()); }
            catch (SystemException e)
            {
                warn("On input, exception: " + e);
                return -1;
            }
        }

        /** Push a character back to be re-used later. **/
        int pushChar(int ch)
        {
            isPushedChar = true;
            return pushedChar = ch;
        }

        /** Pop off the previously pushed character. **/
        int popChar()
        {
            isPushedChar = false;
            return pushedChar;
        }



        /** Read and return a Scheme expression, or EOF. **/
        public Object read()
        {
            try
            {
                Object token = nextToken();
                if (token == "(")
                    return readTail(false);
                else if (token == ")")
                { warn("Extra ) ignored."); return read(); }
                else if (token == ".")
                { warn("Extra . ignored."); return read(); }
                else if (token == "'")
                    return list("quote", read());
                else if (token == "`")
                    return list("quasiquote", read());
                else if (token == ",")
                    return list("unquote", read());
                else if (token == ",@")
                    return list("unquote-splicing", read());
                else
                    return token;
            }
            catch (SystemException e)
            {
                warn("On input, exception: " + e);
                return EOF;
            }
        }

        Object readTail(Boolean dotOK)
        {
            Object token = nextToken();
            if (token == EOF)
                return error("EOF during read.");
            else if (token == ")")
                return null;
            else if (token == ".")
            {
                Object result = read();
                token = nextToken();
                if (token != ")") warn("Where's the ')'? Got " +
                           token + " after .");
                return result;
            }
            else
            {
                isPushedToken = true;
                pushedToken = token;
                return cons(read(), readTail(true));
            }
        }

        Object nextToken()
        {
            int ch;

            // See if we should re-use a pushed char or token
            if (isPushedToken)
            {
                isPushedToken = false;
                return pushedToken;
            }
            else if (isPushedChar)
            {
                ch = popChar();
            }
            else
            {
                ch = inp.Read();
            }

            // Skip whitespace
            while (Char.IsWhiteSpace((char)ch)) ch = inp.Read();

            // See what kind of non-white character we got
            switch (ch)
            {
                case -1: return EOF;
                case '(': return "(";
                case ')': return ")";
                case '\'': return "'";
                case '`': return "`";
                case ',':
                    ch = inp.Read();
                    if (ch == '@') return ",@";
                    else { pushChar(ch); return ","; }
                case ';':
                    // Comment: skip to end of line and then read next token
                    while (ch != -1 && ch != '\n' && ch != '\r') ch = inp.Read();
                    return nextToken();
                case '"':
                    // Strings are represented as char[]
                    buff.Clear();
                    while ((ch = inp.Read()) != '"' && ch != -1)
                    {
                        buff.Append((char)((ch == '\\') ? inp.Read() : ch));
                    }
                    if (ch == -1) warn("EOF inside of a string.");
                    return buff.ToString().ToCharArray();
                case '#':
                    switch (ch = inp.Read())
                    {
                        case 't':
                        case 'T': return TRUE;
                        case 'f':
                        case 'F': return FALSE;
                        case '(':
                            pushChar('(');
                            return listToVector(read());
                        case '\\':
                            ch = inp.Read();
                            if (ch == 's' || ch == 'S' || ch == 'n' || ch == 'N')
                            {
                                pushChar(ch);
                                Object token = nextToken();
                                if (token == "space") return chr(' ');
                                else if (token == "newline") return chr('\n');
                                else
                                {
                                    isPushedToken = true;
                                    pushedToken = token;
                                    return chr((char)ch);
                                }
                            }
                            else
                            {
                                return chr((char)ch);
                            }
                        case 'e':
                        case 'i':
                        case 'd': return nextToken();
                        case 'b':
                        case 'o':
                        case 'x':
                            warn("#" + ((char)ch) + " not implemented, ignored.");
                            return nextToken();
                        default:
                            warn("#" + ((char)ch) + " not recognized, ignored.");
                            return nextToken();
                    }
                default:
                    buff.Clear();
                    int c = ch;
                    do
                    {
                        buff.Append((char)ch);
                        ch = inp.Read();
                    } while (!Char.IsWhiteSpace((char)ch) && ch != -1 &&
                         ch != '(' && ch != ')' && ch != '\'' && ch != ';'
                         && ch != '"' && ch != ',' && ch != '`');
                    pushChar(ch);
                    // Try potential numbers, but catch any format errors.
                    if (c == '.' || c == '+' || c == '-' || (c >= '0' && c <= '9'))
                    {
                        try { return Convert.ToDouble(buff.ToString()); }
                        catch (Exception e) { ; }
                    }
                    return buff.ToString().ToLower();
            }
        }

    }
}
