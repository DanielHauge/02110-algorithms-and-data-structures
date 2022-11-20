using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Week4
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


    public static void Main(string[] args)
    {

        var VNum = int.Parse(Console.ReadLine());
        Nodes = new Vertex[VNum];
        for (int i = 0; i < VNum; i++) Nodes[i] = new Vertex();
        var ENum = int.Parse(Console.ReadLine());
        Edges = new Edge[ENum];
        for (int i = 0; i < ENum; i++)
        {
            var edgeStrings = Console.ReadLine().Split(" ");
            var edge = new Edge() { index = i, from = int.Parse(edgeStrings[0]), to = int.Parse(edgeStrings[1]), capacity = int.Parse(edgeStrings[2]), flow = 0 };
            Edges[i] = edge;
            Nodes[edge.from].outgoing.Add(edge.index);
            Nodes[edge.to].incomming.Add(edge.index);
        }

        while (pathExistsDfs(0, new bool[VNum])){
            // Do nothing
        }

        var totalFlow = Nodes[1].incomming.Sum( x=> Edges[x].flow );
        System.Console.WriteLine(totalFlow);
        return;
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
                    return true;
                }
            }
        }
        foreach(var edgeI in Nodes[v].incomming){
            var edge = Edges[edgeI];
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