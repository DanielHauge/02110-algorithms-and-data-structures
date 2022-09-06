using System;
using System.Linq;
using System.Collections.Generic;

public class Week1
{
    public static Dictionary<string,int> mem;

    public static void Main(string[] args)
    {
        var N = int.Parse(System.Console.ReadLine());
        var prods = System.Console.ReadLine().Split(" ").Select(x => int.Parse(x)).ToArray();
        var stores = System.Console.ReadLine().Split(" ").Select(x => int.Parse(x)).ToArray();
        mem = new Dictionary<string, int>();
        var best = solveRec(prods, stores, 0, N-1);
        var bestProfit = stores[best.Item2]-prods[best.Item1]-(best.Item2-best.Item1)*100;
        bestProfit = bestProfit > 0 ? bestProfit : 0;
        System.Console.WriteLine(bestProfit);
    }

    public static (int, int, int) solveRec(int[] producers, int[]stores, int min, int max){
        
        if (max-min>0){
            var mid = (min+max)/2;

            var solution = solvesub(mid, mid, producers, stores, min, max);
            var solveEarlier = solveRec(producers, stores, min, mid);
            var solveLater = solveRec(producers, stores, mid+1, max);

            var bestSubResult = solveEarlier.Item3 > solveLater.Item3 ? solveEarlier : solveLater;
            var bestresult = bestSubResult.Item3 > solution.Item3 ? bestSubResult : solution;
            return bestresult;
        } else{
            var singleRes = solvesub(min,max, producers, stores, min, max);
            return singleRes;
        }
    }

    public static (int, int, int) solvesub(int startJ, int startI, int[] producers, int[]stores, int min, int max){
        
        int bestPurchase = producers[startI], bestSale = stores[startJ], bestJ=startJ, bestI = startI;
        
        for (int j = startJ; j < max; j++)
        {
            if (stores[j]-(j-startJ)*100 > bestSale){
                bestSale = stores[j]-(j-startJ)*100;
                bestJ = j;
            };
        }

        for (int i = startI; i > min; i--)
        {
            if (producers[i]+(startI-i)*100 < bestPurchase){
                bestPurchase = producers[i]+(startI-i)*100;
                bestI = i;
            };
        }

        var profit = bestSale-bestPurchase-(bestJ-bestI)*100;
        return (bestI,bestJ, profit);
    }
}