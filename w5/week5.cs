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
            var trees = Console.ReadLine()?.Split(" ")?.Select(x=>x) ?? Enumerable.Empty<string>();
            var enumerable = trees;
            if (trees.Any()) enumerable = trees.Skip(1);
            if (!enumerable.Any()) continue;
            try {
            foreach(var tree in enumerable.Select(x => int.Parse(x))){
                if (I(i,tree, N, M) >= Nodes.Length) continue;
                Nodes[I(i,tree, N, M)] = null;
            }
            }catch(Exception){}
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
                if (I(i,j, N,M) >= Nodes.Length) continue;
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
                if (I(j,i, N,M) >= Nodes.Length) continue;
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


        while (pathExistsDfs(0, new bool[Nodes.Length*Edges.Length])){
        }

        var totalFlow = Nodes[1].incomming?.Sum(x=> Edges[x].flow ) ?? 0;
        System.Console.WriteLine(totalFlow);
    }



    public static int I(int i, int j, int N, int M){
        var offSetNodes = (2+N+M);
        return (offSetNodes+i+j*N);
    }


    public static bool pathExistsDfs(int v, bool[] visited ){
        if (v == 1) return true;
        if (v >= visited.Length || v >= Nodes.Length) return false;
        visited[v] = true;
        foreach (var edgeI in Nodes[v].outgoing)
        {
            if (edgeI >= Edges.Length) continue;
            var edge = Edges[edgeI];
            if (edge.to >= visited.Length) continue;
            if (!visited[edge.to] && edge.flow < edge.capacity){
                if (pathExistsDfs(edge.to, visited)){
                    edge.flow++;
                    return true;
                }
            }
        }
        foreach(var edgeI in Nodes[v].incomming){
            if (edgeI >= Edges.Length) continue;
            var edge = Edges[edgeI];
            if (edge.from >= visited.Length) continue;
            if (!visited[edge.from] && edge.flow > 0){
                if (pathExistsDfs(edge.from, visited)){
                    edge.flow--;
                    return true;
                }
                
            }
        }
        return false;
    }


}