using System.Collections.Generic;
using System;

namespace w9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var T = Console.ReadLine() ?? "";
            var P = Console.ReadLine() ?? "";
            foreach (var x in Solve(T, P))
            {
                Console.WriteLine(x);
            }
        }

        static IEnumerable<int> Solve(string t, string p)
        {
            var prefixSufix = Preprocess(p);
            var t_i = 0;
            var p_i = 0;
            while (t.Length > t_i)
            {
                if (p[p_i] == t[t_i])
                {
                    p_i++;
                    t_i++;
                }

                if (p_i == p.Length)
                {
                    yield return t_i - p_i;
                    p_i = prefixSufix[p_i - 1];
                }
                else if (t.Length > t_i && p[p_i] != t[t_i])
                {
                    if (p_i != 0) p_i = prefixSufix[p_i - 1];
                    else t_i++;
                }
            }
        }

        static int[] Preprocess(string p)
        {
            var longestSoFar = 0;
            var prefixSufix = new int[p.Length];
            for (int i = 1; i < p.Length; i++)
            {
                if (p[i] == p[longestSoFar]) prefixSufix[i] = ++longestSoFar;
                else if (longestSoFar != 0)
                {
                    longestSoFar = prefixSufix[longestSoFar - 1];
                    i--;
                }
                else prefixSufix[i] = longestSoFar;
            }
            return prefixSufix;
        }
    }
}