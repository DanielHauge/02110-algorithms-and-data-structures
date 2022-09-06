using System;
using System.Linq;

public class Week0
{
    public static void Main(string[] args)
    {
        var N = int.Parse(System.Console.ReadLine());
        var ints = System.Console.ReadLine().Split(" ");
        for (int i = N-1; i > -1; i--)
        {
            System.Console.Write(ints[i]+ " ");
        }
    }
}