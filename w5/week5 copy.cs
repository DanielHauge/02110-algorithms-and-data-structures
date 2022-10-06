using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Week5
{

    private class Edge
    {
        public int index;
        public int from;
        public int to;
        public int capacity;
        public int flow;
    }

    private class Vertex
    {
        public HashSet<int> outgoing = new HashSet<int>();
        public HashSet<int> incomming = new HashSet<int>();
    }

    private static Edge[] Edges;
    private static Vertex[] Nodes;

    private const int MAXTABLESPERROW = 2;
    private const int MAXTABLESPERCOLUMN = 1;


    public static void Main(string[] args)
    {
        var nm = Console.ReadLine().Split(" ");
        var N = int.Parse(nm[0]);
        var M = int.Parse(nm[1]);
        Nodes = new Vertex[2+N+M+(N*M)]; // s and t nodes + Rows + Columns + Grid
        // Create nodes
        for (int i = 0; i < (2+N+M); i++) Nodes[i] = new Vertex();
        for (int i = 0; i < N; i++){
            for (int j = 0; j < M; j++) Nodes[I(i,j, N, M)] = new Vertex();
            foreach(var tree in Console.ReadLine().Split(" ").Skip(1).Select(x => int.Parse(x))){
                Nodes[I(i,tree, N, M)] = null;
            }
        } 

        

        ////// Create edges
        // Create row edges
        var edgeList = new List<Edge>();
        for (int i = 0; i < N; i++){
            // Base entry row
            var rowEdge = new Edge(){
                index = edgeList.Count,
                capacity = MAXTABLESPERROW,
                flow = 0,
                from = 0,
                to = 2+i,
            };
            Nodes[rowEdge.from].outgoing.Add(rowEdge.index);
            Nodes[rowEdge.to].incomming.Add(rowEdge.index);
            edgeList.Add(rowEdge);
            for (int j = 0; j < M; j++)
            {
                if (Nodes[I(i,j, N,M)] == null) continue;
                var edge = new Edge(){
                    index = edgeList.Count,
                    capacity = 1,
                    flow = 0,
                    from = 2+i,
                    to = I(i,j, N,M),
                };
                Nodes[edge.from].outgoing.Add(edge.index);
                Nodes[edge.to].incomming.Add(edge.index);
                edgeList.Add(edge);
            }
        } 


        // Create column edges
        for (int i = 0; i < M; i++)
        {
            // Base entry column edge
            var columnEdge = new Edge(){
                index = edgeList.Count,
                capacity = MAXTABLESPERCOLUMN,
                flow = 0,
                from = 2+N+i,
                to = 1,
            };
            Nodes[columnEdge.from].outgoing.Add(columnEdge.index);
            Nodes[columnEdge.to].incomming.Add(columnEdge.index);
            edgeList.Add(columnEdge);
            
            for (int j = 0; j < N; j++)
            {
                if (Nodes[I(j,i, N,M)] == null) continue;
                var edge = new Edge(){
                    index = edgeList.Count,
                    capacity = 1,
                    flow = 0,
                    from = I(j,i,N,M),
                    to = 2+N+i,
                };
                Nodes[edge.from].outgoing.Add(edge.index);
                Nodes[edge.to].incomming.Add(edge.index);
                edgeList.Add(edge);
            }
        }

        Edges = edgeList.ToArray();

        // print(Nodes, N, M);

        // foreach (var edge in Edges)
        // {
        //     System.Console.WriteLine($"{edge.index}: {edge.from} -> {edge.to} ({edge.capacity})");
        // }

        // System.Console.WriteLine($"{Nodes[0].outgoing.Select(x => x.ToString()).Aggregate((a,b) => $"{a},{b}")}");
        // foreach(var gg in Nodes[0].outgoing){
        //     var e = Edges[gg];
        //     System.Console.Write($"{e.from} -> {e.to} ({e.capacity}) | ");
        // }
        // System.Console.WriteLine();
        // System.Console.WriteLine($"{Nodes[I(0,1, N,M)].incomming.Select(x => x.ToString()).Aggregate((a,b) => $"{a},{b}")}");
        // foreach(var gg in Nodes[I(0,1, N,M)].incomming){
        //     var e = Edges[gg];
        //     System.Console.Write($"{formatNodeString(e.from)} -> {formatNodeString(e.to)} ({e.capacity}) | ");
        // }
        // System.Console.WriteLine();
        // foreach(var gg in Nodes[I(0,1, N,M)].outgoing){
        //     var e = Edges[gg];
        //     System.Console.Write($"{formatNodeString(e.from)} -> {formatNodeString(e.to)} ({e.capacity}) | ");
        // }
        // System.Console.WriteLine();
        // System.Console.WriteLine();



        

        while (pathExistsDfs(0, new bool[Nodes.Length])){
            System.Console.WriteLine();
        }

        var totalFlow = Nodes[1].incomming.Sum( x=> Edges[x].flow );
        System.Console.WriteLine(totalFlow);
        return;
    }



    private static void print(Vertex[] nodes, int N, int M){
        System.Console.WriteLine(N);
        System.Console.WriteLine(M);
        
        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < M; j++)
            {
                var node = Nodes[I(i,j, N, M)];
                System.Console.Write($"{i},{j} = ");
                if (node == null) System.Console.WriteLine("null");
                else System.Console.WriteLine($"yes");
            }
            System.Console.WriteLine();
        }
    }

    public static int I(int i, int j, int N, int M){
        var offSetNodes = (2+N+M);
        return (offSetNodes+i+j*N);
    }

    private static string formatNodeString(int x){
        if (x == 0) return "s";
        if (x == 1) return "t";
        if (x >= 2 && x < (2+4)) return $"r:{x-2}";
        if (x >= 6 && x < (6+8)) return $"c:{x-6}";
        var offset = 2+4+8;
        var xWithoutOffset = x - offset;
        var i = xWithoutOffset % 4;
        var j = (xWithoutOffset - i) / 4;
        return $"({i},{j})";
    }

    public static bool pathExistsDfs(int v, bool[] visited ){
        if (v == 1) return true;
        visited[v] = true;
        foreach (var edgeI in Nodes[v].outgoing)
        {
            var edge = Edges[edgeI];
            if (!visited[edge.to] && edge.flow < edge.capacity){
                if (pathExistsDfs(edge.to, visited)){
                    edge.flow++;
                    System.Console.Write($" ({formatNodeString(edge.to)}) ");
                    return true;
                }
            }
        }
        foreach(var edgeI in Nodes[v].incomming){
            var edge = Edges[edgeI];
            if (!visited[edge.from] && edge.flow > 0){
                if (pathExistsDfs(edge.from, visited)){
                    edge.flow--;
                    System.Console.Write($" (!!!{formatNodeString(edge.from)}) ");
                    return true;
                }
                
            }
        }
        return false;
    }


}