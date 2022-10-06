
using System;
using System.Linq;
using System.Text;
internal class Program
{
    private static int?[,] M;
    private static int[] C;
    private static void Main(string[] args)
    {
        var n = int.Parse(Console.ReadLine()) - 1; // n that accomodates zero index arrays.
        M = new int?[n+1, n+1];
        C = Console.ReadLine().Split(" ").Select(x => int.Parse(x)).ToArray();
        var maxPoints = n == 0 ? C[0] : Math.Max(C[0] + S(1, n), C[n] + S(0, n - 1));
        Console.WriteLine(maxPoints);
    }

    private static int S(int h, int t)
    {
        int val;
        if (M[h, t].HasValue) val = M[h, t].Value;
        if (h == t) val = 0;
        else if (t-h == 1) val = Math.Min(C[h], C[t]);
        else if (C[h] < C[t]) val = Math.Max(C[t - 1] + S(h, t - 2), C[h] + S(h + 1, t - 1));
        else if (C[h] > C[t]) val = Math.Max(C[t] + S(h + 1, t - 1), C[h + 1] + S(h + 2, t));
        else val = new int[]{ C[t - 1] + S(h, t - 2), C[h] + S(h + 1, t - 1), C[t] + S(h + 1, t - 1), C[h + 1] + S(h + 2, t)}.Max();
        M[h, t] = val;
        return val;
    }

}

