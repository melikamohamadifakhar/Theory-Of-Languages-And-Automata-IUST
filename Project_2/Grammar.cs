using System;
using System.Collections.Generic;
using System.Linq;

namespace TuringMachine
{
    class Program
    {
        static string CheckInput(SortedDictionary<string, List<string>> dict, string input)
        {
            foreach (string k in dict.Keys)
            {
                List<string> strings = new List<string>();
                strings.Add(k);
                for (int q = 0; q < 20000; q++)
                {
                    if (strings.Count > 0)
                    {
                        int idx = strings[0].IndexOf('<');
                            string variable = strings[0][idx].ToString() +
                                            strings[0][idx + 1].ToString() +
                                            strings[0][idx + 2].ToString();
                            string without_variable = strings[0];
                            without_variable = without_variable.Remove(idx, 3);
                            for (int x = 0; x < dict[variable].Count; x++)
                            {
                                string replaced = without_variable;
                                if (dict[variable][x] != "#")
                                    replaced = without_variable.Insert(idx, dict[variable][x]);
                                if (replaced == input) return "Accepted";
                                else if (replaced.IndexOf('<') != -1)
                                    strings.Add(replaced);
                            }
                            strings.RemoveAt(0);
                    }
                }
                
            }
            return "Rejected";
        }
        static void Main(string[] args)
        {
            int Num = int.Parse(Console.ReadLine());
            SortedDictionary<string, List<string>> Dictionary = new SortedDictionary<string, List<string>>();
            for (int i = 0; i < Num; i++)
            {
                List<string> input = Console.ReadLine().
                                Split(new string[] { "->" }, StringSplitOptions.None).
                                ToList();
                List<string> splitted = input[1].Split('|').ToList();
                for (int k = 0; k < splitted.Count; k++)
                    splitted[k] = splitted[k].Trim();
                input[0] = input[0].Trim();
                if (!Dictionary.ContainsKey(input[0]))
                {
                    Dictionary.Add(input[0], new List<string>());
                    for (int j = 0; j < splitted.Count; j++)
                    {
                        Dictionary[input[0]].Add(splitted[j]);
                    }
                }
                else
                {
                    for (int j = 0; j < splitted.Count; j++)
                    {
                        if (!Dictionary[input[0]].Contains(splitted[j]))
                            Dictionary[input[0]].Add(splitted[j]);
                    }
                }
            }
            string inp = Console.ReadLine();
            Console.WriteLine(CheckInput(Dictionary, inp));
        }
    }
}
