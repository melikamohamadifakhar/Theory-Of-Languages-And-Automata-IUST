using System;
using System.Collections.Generic;
using System.Linq;
namespace _1
{
    class Program
    {
        static int L;
        static bool Accepted(string[][] transactions, string[] finals, string currentState,
                            string theString, int idx)
            {
                if(idx == L - 1)
                {
                    if (finals.Contains(currentState))
                        return true;
                    return false;
                }
                for (int i = 0; i < transactions.Length; i++)
                {
                    if(transactions[i][0] == currentState)
                        {
                            int index = idx;
                            if(transactions[i][1] == theString.ElementAt(idx).ToString())
                            {
                                currentState = transactions[i][2];
                                index++;
                                if(Accepted(transactions, finals, currentState,
                                    theString, index)) { return true; }
                            }
                            else if(transactions[i][1] == "$")
                            {
                                currentState = transactions[i][2];
                                if(Accepted(transactions, finals, currentState,
                                    theString, index)) { return true; }
                            }
                        }
                }
                return false;
            }
        static void Main(string[] args)
        {

            string input_states = Console.ReadLine();
            string currentState = input_states.Substring(1, input_states.Length - 2).Split(',').ToArray()[0];
            string characters = Console.ReadLine();
            string input_finals = Console.ReadLine();
            string[] finals = input_finals.Substring(1, input_finals.Length - 2).Split(',').ToArray();

            long num = Int64.Parse(Console.ReadLine());
            string[][] transactions = new string[num][];
            for(int i = 0; i < num; i++){
                string input_transaction = Console.ReadLine();
                var transaction = input_transaction.Split(',');

                transactions[i] = transaction;
            }
            string TheString = Console.ReadLine();
            L = TheString.Length;
            if(Accepted(transactions, finals, currentState, TheString, 0))
            {
                System.Console.WriteLine("Accepted");
                return;
            }
            Console.WriteLine("Rejected");
        }
    }
}
