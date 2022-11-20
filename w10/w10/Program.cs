using System;
namespace w10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var dynamicTable = new DynamicTable();
            var input = Console.ReadLine() ?? "";
            while (input != null && input != " ")
            {
                var split = input.Split(" ");
                switch (split[0])
                {
                    case "I":
                        dynamicTable.I(int.Parse(split[1]));
                        break;
                    case "D":
                        dynamicTable.D();
                        break;
                    case "P":
                        dynamicTable.P();
                        break;
                    case "S":
                        dynamicTable.S();
                        break;
                }
                input = Console.ReadLine();
            }
            
        }

        class DynamicTable
        {
            private int?[] ints;
            private int size;

            public DynamicTable()
            {
                ints = new int?[1];
            }

            public void P()
            {
                foreach(var i in ints)
                {
                    if (i != null) Console.Write($"{i} ");
                }
                Console.Write("\n");
            }
            public void S()
            {
                Console.WriteLine($"{size} {ints.Length}");
            }

            public void D()
            {
                size = size - 1;
                ints[size] = null;
                if (size == ints.Length / 4)
                {
                    Resize(ResizeOpt.Decrease);
                }
            }

            public void I(int X)
            {
                if (size == ints.Length)
                {
                    Resize(ResizeOpt.Increase);
                }
                ints[size] = X;
                size++;
            }


            enum ResizeOpt {
                Increase,
                Decrease,
            }

            private void Resize(ResizeOpt opt)
            {
                
                switch (opt)
                {
                    case ResizeOpt.Increase:
                        var increasedArray = new int?[ints.Length*2];
                        for (int i = 0; i < size; i++)
                        {
                            increasedArray[i] = ints[i];
                        }
                        ints = increasedArray;
                        break;
                    case ResizeOpt.Decrease:
                        if (ints.Length == 1) return;
                        var decreasedArray = new int?[ints.Length / 2];
                        for (int i = 0; i < size; i++)
                        {
                            decreasedArray[i] = ints[i];
                        }
                        ints = decreasedArray;
                        break;
                }
            }


        }

    }
}