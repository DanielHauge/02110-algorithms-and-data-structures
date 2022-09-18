using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

public class Week3
{
    private static int?[,] CostMatrix;
    private static string X, Y;
    private static StringBuilder xBuilder, yBuilder;
    const int GapPenalty = 1;

    public static void Main(string[] args)
    {
        
        X = Console.ReadLine();
        Y = Console.ReadLine();
        CostMatrix = new int?[X.Length+1, Y.Length+1];
        xBuilder = new StringBuilder();
        yBuilder = new StringBuilder();
        var leastCost = SA();
        // System.Console.WriteLine(leastCost);
        // PrintMatrix();
        buildAlignedStrings(X.Length, Y.Length);

    }

    public static int SA(){
        
        for (int i = 0; i < X.Length+1; i++)
        {
            CostMatrix[i,0] = i*GapPenalty;   
        }
        for (int j = 0; j < Y.Length+1; j++)
        {
            CostMatrix[0,j] = j*GapPenalty;
        }

        for (int i = 1; i < X.Length+1; i++)
        {
            for (int j = 1; j < Y.Length+1; j++)
            {
                var payDif = diff(i,j) + CostMatrix[i-1,j-1];
                var payXGap = GapPenalty + CostMatrix[i-1,j];
                var payYGap = GapPenalty + CostMatrix[i,j-1];
                var best = new int?[]{payDif,payXGap,payYGap}.Min();
                CostMatrix[i,j] = best;
            }
        }

        
        return CostMatrix[X.Length-1, Y.Length-1].Value;
    }

    public static int diff(int i, int j){
        return X[i-1] == Y[j-1] ? 0 : 1;
    }

    public static void PrintMatrix(){
        for (int j = 0; j < Y.Length+1; j++)
        {
            for (int i = 0; i < X.Length+1; i++)
            {
                System.Console.Write(CostMatrix[i,j] + "\t");
            }
            System.Console.WriteLine();
        }
    }

    public static void buildAlignedStrings(int i, int j){
        var xStack = new Stack<char>(X);
        var yStack = new Stack<char>(Y);

        while(i+j > 0){
            // System.Console.Write($"Testing {i}, {j}");

            var diff = (i > 0 && j > 0) ? CostMatrix[i-1,j-1] : null;
            var payX = i > 0 ? CostMatrix[i-1,j] : null;
            var payY = j > 0 ? CostMatrix[i,j-1] : null;
            bool payDiff = false;
            bool payXGap = false;
            if (diff.HasValue){
                payDiff = diff <= payX && diff <= payY;
                payXGap = payX <= payY;
            } else if (payX.HasValue && payY.HasValue){
                payXGap = payX <= payY;   
            } else if (payX.HasValue){
                payXGap = true;
            } else{
                payXGap = false;
            }

            // System.Console.WriteLine($"|  d {diff} and xg {payX}, yg {payY} with: {payDiff} | {payXGap}");

            if (payDiff){
                yBuilder.Insert(0,yStack.Pop());
                xBuilder.Insert(0,xStack.Pop());
                i--;
                j--;
            } else if (!payXGap){
                xBuilder.Insert(0,"-");
                yBuilder.Insert(0,yStack.Pop());
                j--;
            } else{
                yBuilder.Insert(0,"-");
                xBuilder.Insert(0,xStack.Pop());
                i--;
            }


        }

        System.Console.WriteLine(xBuilder.ToString());
        System.Console.WriteLine(yBuilder.ToString());


    }


}