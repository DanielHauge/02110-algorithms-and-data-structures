using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AlgoData
{
    internal class Program
    {
        class Zone : Point
        {
            public int i;
            public int O1; // Horizontal and Verticals -> O(1) by tracking zones into indexes during zones parsing.
            public int O2; // Furthest away (Euclidean) -> Convex hull -> For each point, check against points in convex hull.
            public int O3; // If there is atleast 2 zones between. (uuuhh how what ?)
            public int O4; // Always to end zone.
            public int[] outgoing;
        }

        class Point
        {
            public int x;
            public int y;
            public bool equals(Zone other)
            {
                return (x == other.x && y == other.y);
            }
        }

        static void Main(string[] args)
        {

            var n = int.Parse(Console.ReadLine());
            var zones = new Zone[n + n * 4];
            var horizontalIndex = new Dictionary<int, HashSet<int>>();
            var verticalIndex = new Dictionary<int, HashSet<int>>();
            var cordIndex = new Dictionary<Tuple<int, int>, int>();

            // Zone (vertex) construction
            for (int i = 0; i < n; i++)
            {
                var z = Console.ReadLine().Split(" ");
                // add to horizontal index
                var x = int.Parse(z[0]);
                if (horizontalIndex.TryGetValue(x, out var h))
                {
                    h.Add(i);
                    horizontalIndex[x] = h;
                }
                else
                {
                    horizontalIndex.Add(x, new HashSet<int>() { i });
                }
                // add to vertical index
                var y = int.Parse(z[1]);
                if (verticalIndex.TryGetValue(y, out var v))
                {
                    v.Add(i);
                    verticalIndex[y] = v;
                }
                else
                {
                    verticalIndex.Add(y, new HashSet<int>() { i });
                }
                // add to cord index
                cordIndex.Add(Tuple.Create(x, y), i);
                // Set zone.
                zones[i] = new Zone()
                {
                    i = i,
                    x = x,
                    y = y,
                    O1 = int.Parse(z[2]),
                    O2 = int.Parse(z[3]),
                    O3 = int.Parse(z[4]),
                    O4 = int.Parse(z[5]),
                    outgoing = new int[n + n * 4]
                };
            }
            for (int i = n; i < n + n * 4; i++) zones[i] = new Zone() { outgoing = new int[n + n * 4] };

            // Obstacle (edge) construction
            for (int i = 0; i < n; i++)
            {
                var zone = zones[i];
                zone.outgoing[n + i * 4 + 3] = zone.O4;
                zones[n + i * 4 + 3].outgoing[n - 1] = zone.O4;

                if (zone.O2 > 0)
                {
                    double maxDist = 0;
                    int? maxDistI = null;
                    for (int j = 0; j < n; j++)
                    {
                        if (j == i) continue;
                        var otherZone = zones[j];
                        var distance = dist_squared(zone, otherZone);
                        if (distance >  maxDist)
                        {
                            maxDist = distance;
                            maxDistI = j;
                        }
                    }
                    if (maxDistI.HasValue)
                    {
                        zone.outgoing[n + i * 4 + 1] = zone.O2;
                        zones[n + i * 4 + 1].outgoing[maxDistI.Value] = zone.O2;
                    }
                }

                if (zone.O1 > 0)
                {
                    zone.outgoing[n + i * 4] = zone.O1;
                    // Find valid horizontal and vertical zones and add edges to them with capacity O1.
                    var hs = horizontalIndex.TryGetValue(zone.x, out var h) ? h : new HashSet<int>();
                    foreach (var x in hs.Where(x => x != i)) zones[n + i * 4].outgoing[x] = zone.O1;

                    var vs = verticalIndex.TryGetValue(zone.y, out var v) ? v : new HashSet<int>();
                    foreach (var x in vs.Where(x => x != i)) zones[n + i * 4].outgoing[x] = zone.O1;
                }

                if (zone.O3 > 0)
                {
                    zone.outgoing[n + i * 4 + 2] = zone.O3;
                    // Find valid horizontal and vertical zones and add edges to them with capacity O2.
                    for (int j = 0; j < n; j++)
                    {
                        if (j == i) continue;
                        var otherZone = zones[j];
                        var xDif = zone.x - otherZone.x;
                        var yDif = zone.y - otherZone.y;
                        var gcd = GCD(Math.Max(xDif, yDif), Math.Min(xDif, yDif));
                        if (Math.Abs(gcd) < 2) continue;
                        var xStep = xDif / gcd;
                        var yStep = yDif / gcd;
                        var pointsBetween = 0;
                        for (int l = 1; l < Math.Abs(gcd); l++)
                        {
                            if (cordIndex.ContainsKey(Tuple.Create(zone.x + xStep * l, zone.y + yStep * l)))
                            {
                                pointsBetween++;
                            }
                        }

                        if (pointsBetween > 1)
                        {
                            zones[n + i * 4 + 2].outgoing[j] = zone.O3;
                        }
                    }
                }



            }



            var graph = construct_2d_graph(zones);


            //Console.WriteLine();
            //for (int i = 0; i < n; i++)
            //{
            //    for (int j = 0; j < n; j++)
            //    {
            //        Console.Write(graph[i, j]);
            //        Console.Write(" ");
            //    }
            //    Console.WriteLine("");

            //}


            var maxFlow = fulkerson_max_flow(graph, 0, n - 1);

            Console.WriteLine(maxFlow);
        }

        static int[,] construct_2d_graph(Zone[] zones)
        {
            var graph2d = new int[zones.Length, zones.Length];
            for (int i = 0; i < zones.Length; i++)
            {
                var vertex = zones[i];
                for (int j = 0; j < zones.Length; j++)
                {
                    graph2d[i, j] = vertex.outgoing[j];
                }
            }
            return graph2d;
        }

        static bool search_path_source_to_sink(int source_index, int sink_index, int[,] residual_graph, int[] path_when_true)
        {
            bool[] v = new bool[residual_graph.GetLength(0)];
            v[source_index] = true;
            path_when_true[source_index] = -1;
            Queue<int> q = new Queue<int>(new int[] { source_index });

            // Breath first search
            while (q.Count > 0)
            {
                var vertex_index = q.Dequeue();
                for (int i = 0; i < residual_graph.GetLength(0); i++)
                {
                    // If vertex already visisted or no more capacity to "flow" this way, then continue.
                    if (!v[i] && residual_graph[vertex_index, i] > 0)
                    {
                        if (i == sink_index) // If we have made it to the sink, then we are finished, and have concluded the path and should return true.
                        {
                            path_when_true[i] = vertex_index;
                            return true;
                        } // Otherwise mark as visited and enque next vertex to potential next visitation etc.
                        q.Enqueue(i);
                        v[i] = true;
                        path_when_true[i] = vertex_index;
                    }
                }
            }

            return false; // Could not reach end zone.
        }

        static int fulkerson_max_flow(int[,] residual_graph, int source_index, int sink_index)
        {
            var augmentedPath = new int[residual_graph.GetLength(0)];
            var flow = 0;
            int u, v;
            while (search_path_source_to_sink(source_index, sink_index, residual_graph, augmentedPath))
            {
                // Find the utmost largest flow on the augmented path.
                var flow_of_path = int.MaxValue;
                for (v = sink_index; v != source_index; v = augmentedPath[v])
                {
                    u = augmentedPath[v];
                    flow_of_path = Math.Min(flow_of_path, residual_graph[u, v]);
                }

                // Update the residual graph with the utmost largest flow.
                for (v = sink_index; v != source_index; v = augmentedPath[v])
                {
                    u = augmentedPath[v];
                    residual_graph[u, v] -= flow_of_path;
                    residual_graph[v, u] += flow_of_path;
                }

                // Increment the flow from this path to the total.
                flow = flow + flow_of_path;

                // -> Go again, now with updated residual graph etc.
            }

            return flow;
        }

        static double dist_squared(Point a, Point b)
        {
            return Math.Sqrt((Math.Pow(a.x - b.x, 2) + Math.Pow(a.y - b.y, 2)));
        }

        static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }


    }

}
