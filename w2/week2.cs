using System;
using System.Linq;
using System.Collections.Generic;

public class Week2
{

    public static long?[,] mem;
    public static long[,] D;
    public static long[,] R;

    public static void Main(string[] args)
    {
        var N = long.Parse(System.Console.ReadLine());
        
        if (N > 1)
        {
            R = new long[N, N - 1];
            D = new long[N - 1, N];
            mem = new long?[N, N];
            for (long i = 0; i < N; i++)
            {
                var nums = System.Console.ReadLine().Split(" ").Select(x => long.Parse(x)).ToArray();
                for (long j = 0; j < nums.Length; j++)
                {
                    R[i, j] = nums[j];
                }
            }

            for (long i = 0; i < N - 1; i++)
            {
                var nums = System.Console.ReadLine().Split(" ").Select(x => long.Parse(x)).ToArray();
                for (long j = 0; j < nums.Length; j++)
                {
                    D[i, j] = nums[j];
                }
            }
            var bestProfit = OPT_W(N - 1, N - 1);
            System.Console.WriteLine(bestProfit);
        } else{
            System.Console.WriteLine(long.Parse(System.Console.ReadLine()) + long.Parse(System.Console.ReadLine()));
        } 
    }

    public static long OPT_W(long i, long j)
    {
        var solved = mem[i, j];
        if (solved.HasValue) return solved.Value;
        var opt_d = i > 0 ? OPT_W(i - 1, j) + D[i - 1, j] : 0;
        var opt_r = j > 0 ? OPT_W(i, j - 1) + R[i, j - 1] : 0;
        var best = opt_d > opt_r ? opt_d : opt_r;
        mem[i, j] = best;
        return best;
    }

}