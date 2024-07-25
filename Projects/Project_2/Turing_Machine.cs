using System;
using System.Collections.Generic;
using System.Linq;
namespace TuringMachine
{
    class Program
    {
        static string TuringMachine(List<List<string>> Transitions, string input, string final)
        {
            var inp = input.Split('0');
            bool halted = false;
            var curr_state = "1";
            var TapeIdx = 20;
            List<string> Tape = new List<string>();
            for(int i = 0; i < 20; i++) Tape.Add("1");
            // foreach (var r in inp)
            // {
            //     Tape.Add("1"); Tape.Add("1");
            // }
            foreach (var r in inp)
                Tape.Add(r.ToString());
            for(int i = 0; i < 20; i++) Tape.Add("1");
            // foreach (var r in inp)
            // {
            //     Tape.Add("1"); Tape.Add("1");
            // }

            while (! halted)
            {
                halted = true;
                // var curr_char = Tape[TapeIdx].ToString();
                foreach (var t in Transitions)
                {
                        if (t[0] == curr_state && t[1] == Tape[TapeIdx].ToString())
                        {
                            halted = false;
                            curr_state = t[2];
                            Tape[TapeIdx] = t[3];
                            if(t[4] == "1") TapeIdx--;
                            else TapeIdx++;
                        }
                }
            }
            if (curr_state == final)
                return "Accepted";
            else return "Rejected";
        }
        static void Main(string[] args)
        {
            Dictionary<string, string> states = new Dictionary<string, string>();
            var Transisions_1 = Console.ReadLine().
                                Split(new string[] { "00" }, StringSplitOptions.None).
                                ToList();

            List<List<string>> Transisions = new List<List<string>>();

            foreach(var T in Transisions_1)
            {
                var x = T.Split('0').ToList();
                Transisions.Add(x);
                if(!states.ContainsKey(x[0])) states.Add(x[0], "");
                if(!states.ContainsKey(x[2])) states.Add(x[2], "");
            }

            int Num = int.Parse(Console.ReadLine());

            var Strings = new List<string>();

            for (int i = 0; i < Num; i++) Strings.Add(Console.ReadLine());

            var final = "";

            for (int i = 0; i < states.Count; i++) final += "1";

            foreach (var s in Strings)
            {
                var x = "";
                if (s == string.Empty) x = "1";
                else x = s;
                Console.WriteLine(TuringMachine(Transisions, x, final));
            }
        }
    }
}
