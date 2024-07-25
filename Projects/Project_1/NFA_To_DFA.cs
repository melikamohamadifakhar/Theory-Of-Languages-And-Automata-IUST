using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace _2
{
    class transition
    {
        public string src;
        public string label;
        public string dst;

        public transition(string src, string label, string dst)
        {
            this.src = src;
            this.label = label;
            this.dst = dst;
        }
    }
    class Program
    {
        static List<string> Make_Closure(List<string> states, List<transition> transitions)
        {
            List<string> closure = new List<string>();
            foreach (string state in states)
            {
                closure.Add(state);
            }
            Queue<string> queue = new Queue<string>();
            foreach (string state in states)
            {
                queue.Enqueue(state);
            }
            while (queue.Count != 0)
            {
                string dequeued = queue.Dequeue();
                for (int j = 0; j < transitions.Count; j++)
                {
                    if (transitions[j].src == dequeued && transitions[j].label == "$")
                    {
                        if(!closure.Contains(transitions[j].dst))
                        {
                            closure.Add(transitions[j].dst);
                            queue.Enqueue(transitions[j].dst);
                        }
                    }
                }
            }
            return closure;
        }
        static List<string> Change_State_WithLabel(List<string> states, string label, List<transition> transitions)
        {
            List<string> answer = new List<string>();
            foreach(var state in states)
            {
                foreach (var transition in transitions)
                {
                    if (transition.src == state && transition.label == label)
                    {
                        if (!answer.Contains(transition.dst))
                            answer.Add(transition.dst);
                    }
                }
            }
            return answer;
        }
        static long Solve(List<string> states, List<string> labels, List<transition> transitions)
        {
            bool add = false;
            var FinalDFAStates = new List<List<string>>();
            var list = new List<string>(); list.Add(states[0]);
            var closure = Make_Closure(list, transitions);
            closure.Sort();
            FinalDFAStates.Add(closure);
            for (var i = 0; i < FinalDFAStates.Count; i++)
            {
                for (var j = 0; j < labels.Count; j++)
                {
                    var ChangedStates = Change_State_WithLabel(FinalDFAStates[i], labels[j], transitions);
                    var SameState = Make_Closure(ChangedStates, transitions);
                    SameState.Sort();
                    if (!Containing(FinalDFAStates, SameState))
                    {
                        if(SameState.Count != 0)
                            FinalDFAStates.Add(SameState);
                        else
                            add = true;
                    }
                }
            }
            long result = FinalDFAStates.Count;
            if (add)
                result += 1;
            return result;
        }
        public static bool Containing(List<List<string>> big, List<string> small)
        {
            foreach (var b in big)
            {
                if (b.Count == small.Count)
                {
                    bool isEqual = Enumerable.SequenceEqual(b.OrderBy(e => e), small.OrderBy(e => e));
                    if (isEqual) return true;
                }
            }
            return false;
        }

        static void Main(string[] args)
        {
            string input_states = Console.ReadLine();
            string[] States = input_states.Substring(1, input_states.Length - 2).Split(',').ToArray();
            string input_characters = Console.ReadLine();
            string[] characters = input_characters.Substring(1, input_characters.Length - 2).Split(',').ToArray();
            string input_finals = Console.ReadLine();
            string[] finals = input_finals.Substring(1, input_finals.Length - 2).Split(',').ToArray();
            long transition_num = Int64.Parse(Console.ReadLine());
            List<transition> transitions = new List<transition>();
            for(int i = 0; i < transition_num; i++)
            {
                string[] t_in= Console.ReadLine().Split(',');
                var t = new transition(t_in[0], t_in[1], t_in[2]);
                transitions.Add(t);
            }
            System.Console.WriteLine(Solve(States.ToList(), characters.ToList(), transitions));
        }
    }
}
