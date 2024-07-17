using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//K + L - M * N + (O ^ P) * W / U / V * T + Q
namespace PDA_Calculator
{
    class Program
    {
        static Dictionary<string, int> Priority = new Dictionary<string, int>();

        static void fill_dictionary()
        {
            Priority.Add("^", 3);
            Priority.Add("*", 2);
            Priority.Add("/", 2);
            Priority.Add("+", 1);
            Priority.Add("-", 1);

            Priority.Add("sin", 4);
            Priority.Add("cos", 4);
            Priority.Add("tan", 4);
            Priority.Add("abs", 4);
            Priority.Add("exp", 4);
            Priority.Add("ln", 4);
            Priority.Add("sqrt", 4);
        }
        static double CalculateBinary(double op1, double op2, string operat)
        {
            try{
            if (operat == "*") return op1 * op2;
            else if (operat == "/") 
            return op1 / op2;
            else if (operat == "+") return op1 + op2;
            else if (operat == "-") return op1 - op2;
            else return Math.Pow(op1, op2);
            }
            catch
            {
                throw new Exception();
            }
        }
        private static double CalculateUnary(double operand, string operatorName)
        {
            try{
                if (operatorName == "sin") return Math.Sin(operand);
                else if (operatorName == "cos") return Math.Cos(operand);
                else if (operatorName == "tan") return Math.Tan(operand);
                else if (operatorName == "abs") return Math.Abs(operand);
                else if (operatorName == "ln") return Math.Log10(operand);
                else if (operatorName == "sqrt") return Math.Sqrt(operand);
                else return Math.Exp(operand);
            }
            catch {
                throw new Exception();
            }
        }
        static double Evaluate(List<string> Exp)
        {
            Stack<string> Origin = new Stack<string>();
            Stack<double> tmp = new Stack<double>();
            foreach (var E in Exp)
                Origin.Push(E);
            while ( Origin.Count > 0)
            {
                double idk;
                while (double.TryParse(Origin.Peek(), out idk))
                {
                    tmp.Push(idk);
                    Origin.Pop();
                }
                var op = Origin.Peek();
                if ( Priority.ContainsKey(op) && Priority[op] == 4)
                    tmp.Push(CalculateUnary(tmp.Pop(), Origin.Pop()));
                else if (Priority.ContainsKey(op))
                {
                    tmp.Push(CalculateBinary(tmp.Pop(), tmp.Pop(), Origin.Pop()));
                }
                else {throw new Exception();}
            }
            return tmp.Pop();
        }


        static List<string> Prefix(string phrase)
        {
            List<string> P = new List<string>();
            for (int i = 0; i < phrase.Length; i++)
            {
                if (phrase[i] == ' ') continue;
                else if ((phrase[i] >= '0' && phrase[i] <= '9') || phrase[i] == '.' || phrase[i] == '-')
                {
                    StringBuilder sbuf = new StringBuilder();
                    while (i < phrase.Length &&
                            (phrase[i] >= '0' &&
                            phrase[i] <= '9' ||
                            phrase[i]  == '.' ||
                            phrase[i]  == '-') )
                    {
                        sbuf.Append(phrase[i++]);
                    }
                    i--;
                    P.Add(sbuf.ToString());
                }
                else if ((phrase[i] >= 'a' && phrase[i] <= 'z'))
                {
                    StringBuilder sbuf = new StringBuilder();
                    while (i < phrase.Length &&
                            (phrase[i] >= 'a' &&
                            phrase[i] <= 'z'))
                    {
                        sbuf.Append(phrase[i++]);
                    }
                    i--;
                    P.Add(sbuf.ToString());
                }
                else P.Add(phrase[i].ToString());
            }
            P.Reverse();
            List<string> Exp = new List<string>();
            Stack<string> Ops = new Stack<string>();

            for (int i = 0; i < P.Count; i++)
            {
                double idk;
                if (double.TryParse(P[i], out idk))
                {
                    Exp.Add(P[i]);
                }
                else if (Priority.ContainsKey(P[i]))
                {
                    if (Ops.Count == 0) {Ops.Push(P[i]); continue; }
                    var top = Ops.Peek();
                    // if (Priority.ContainsKey(top))
                    // {
                        while (Priority.ContainsKey(top) && (Priority[top] > Priority[P[i]]) && Ops.Count > 0)
                        {
                            Exp.Add(Ops.Pop());
                            if (Ops.Count > 0) top = Ops.Peek();

                        }
                    // }
                    Ops.Push(P[i]);
                }
                else if (P[i] == "(")
                {
                    while (Ops.Peek() != ")" && (Ops.Count > 0))
                    {
                        Exp.Add(Ops.Pop());
                    }
                    if (Ops.Count == 0)
                    {Exp.Add("INVALID"); return Exp;}
                    Ops.Pop();
                }
                else if (P[i] == ")")
                {
                    Ops.Push(")");
                }
                else
                {
                    Exp.Add(P[i]);
                }
            }
            while (Ops.Count > 0)
                Exp.Add(Ops.Pop());
            Exp.Reverse();
            return Exp;
        }
        static long Parentheses_check(string str)
        {
            
            Stack stack = new Stack();
            char[] strs = str.ToCharArray();
            char open = '(';
            char close = ')';
            int idx = 0; int wrongidx = 0;
            while(idx < strs.Length){
                char current = strs[idx];
                if(open != current && close != current)
                    {idx++; continue;}
                if(open == current){
                    wrongidx = idx;
                    stack.Push(current);
                    idx++;
                }
                else if(close ==current){
                    if(stack.Count == 0){return idx+1;}
                    object last = stack.Pop();
                    if(current == ')' && last.ToString() == "("){ idx++; wrongidx--; continue; }
                    return idx+1 ;
                }
            }
                if(stack.Count == 0){wrongidx = -2;}
                return wrongidx+1;
        }
        static void Main(string[] args)
        {
            try{
            fill_dictionary();
            string phrase = Console.ReadLine();
            if (Parentheses_check(phrase) != -1)
            {
                System.Console.WriteLine("INVALID");
                return;
            }
            var prefix = Prefix(phrase);
            if (prefix[prefix.Count - 1] == "INVALID") 
                throw new Exception();
            var eval = Evaluate(prefix);
            if (double.IsInfinity(eval) || double.IsNaN(eval))
                throw new Exception();
            double ans = Math.Truncate(100 * eval) / 100;
            Console.WriteLine(string.Format("{0:0.00}", ans));
            }
            catch{
                System.Console.WriteLine("INVALID");
            }
        }
    }
}

