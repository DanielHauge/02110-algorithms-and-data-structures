using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace AlgoData
{
    internal class Program
    {
        class Zone : Vertex
        {
            public int i;
            public int O1; // Horizontal and Verticals -> O(1) by tracking zones into indexes during zones parsing.
            public int O2; // Furthest away (Euclidean) -> Convex hull -> For each point, check against points in convex hull.
            public int O3; // If there is atleast 2 zones between. (uuuhh how what ?)
            public int O4; // Always to end zone.
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

        class Vertex : Point
        {
            public HashSet<int> outgoing = new HashSet<int>();
            public HashSet<int> incomming = new HashSet<int>();
            
        }

        class Obstacle
        {

            public int index;
            public int from;
            public int to;
            public int capacity;
            public int flow;
        }

        

        static void Main(string[] args)
        {
            
            
            var n = int.Parse(Console.ReadLine());
            var zones = new Zone[n];
            var obstacles = new List<Obstacle>();
            var horizontalIndex = new Dictionary<int, HashSet<int>>();
            var verticalIndex = new Dictionary<int, HashSet<int>>();
            var cordIndex = new Dictionary<Tuple<int, int>, int>();

            for (int i = 0; i < n; i++)
            {
                var z = Console.ReadLine().Split(" ");
                // add to horizontal index
                var x = int.Parse(z[0]);
                if (horizontalIndex.TryGetValue(x, out var h))
                {
                    h.Add(i);
                } else {
                    horizontalIndex.Add(x, new HashSet<int>() { i });
                }
                // add to vertical index
                var y = int.Parse(z[1]);
                if (verticalIndex.TryGetValue(y, out var v))
                {
                    v.Add(i);
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
                };
            }
            // Obstacle (edge) construction
            var vertices = new List<Vertex>(zones);
            foreach (var zone in zones)
            {
                int? maxDist = null;
                int? maxDistI = null;
                var evalDist = zone.O2 > 0;
                if (zone.O1 > 0)
                {
                    // Find valid horizontal and vertical zones and add edges to them with capacity O1.
                    var hs = horizontalIndex.TryGetValue(zone.x, out var h) ? h : new HashSet<int>();
                    var vs = verticalIndex.TryGetValue(zone.y, out var v) ? v : new HashSet<int>();
                    vs.UnionWith(hs);
                    vs.ExceptWith(new int[] { zone.i });

                    if (vs.Count > 0)
                    {
                        vertices.Add(new Vertex());
                        obstacles.Add(new Obstacle()
                        {
                            capacity = zone.O1,
                            from = zone.i,
                            to = vertices.Count-1,
                            index = obstacles.Count,
                        });
                        foreach(var z in vs)
                        {
                            obstacles.Add(new Obstacle()
                            {
                                capacity = zone.O1,
                                from = vertices.Count-1,
                                to = z,
                                index = obstacles.Count,
                            });
                        }
                    }
                }
                if (zone.O3 > 0)
                {
                    // Find valid horizontal and vertical zones and add edges to them with capacity O2.
                    var validZones = new List<int>();

                    for (int i = 0; i < n; i++)
                    {
                        var otherZone = zones[i];
                        if (evalDist)
                        {
                            var dist = dist_squared(zones[i], zone);
                            if (!maxDist.HasValue || (maxDist.HasValue && dist > maxDist.Value))
                            {
                                maxDist = dist;
                                maxDistI = i;
                            }
                        }
                        var xDif = zone.x - otherZone.x;
                        var yDif = zone.y - otherZone.y;
                        var gcd = GCD(xDif, yDif);
                        if (Math.Abs(gcd) < 2) continue;
                        var xStep = xDif / gcd;
                        var yStep = yDif / gcd;
                        var pointsBetween = 0;
                        for (int j = 1; j < Math.Abs(gcd); j++)
                        {
                            if (cordIndex.ContainsKey(Tuple.Create(zone.x + xStep * j, zone.y + yStep * j)))
                            {
                                pointsBetween++;
                            }
                        }
                        if (pointsBetween > 1)
                        {
                            validZones.Add(otherZone.i);
                        }

                    }

                    if (validZones.Count > 0)
                    {
                        vertices.Add(new Vertex());
                        obstacles.Add(new Obstacle()
                        {
                            capacity = zone.O3,
                            from = zone.i,
                            to = vertices.Count-1,
                            index = obstacles.Count,
                        });

                        validZones.ForEach(z => obstacles.Add(new Obstacle()
                        {
                            capacity = zone.O3,
                            from = vertices.Count-1,
                            to = z,
                            index = obstacles.Count,
                        }));
                    }
                }
                if (evalDist)
                {
                    if (!maxDist.HasValue)
                    {
                        for (int i = 0; i < n; i++)
                        {
                            var dist = dist_squared(zones[i], zone);
                            if (!maxDist.HasValue || (maxDist.HasValue && dist > maxDist.Value))
                            {
                                maxDist = dist;
                                maxDistI = i;
                            }
                        }
                    }
                    obstacles.Add(new Obstacle()
                    {
                        capacity = zone.O2,
                        from = zone.i,
                        to = maxDistI.Value,
                        index = obstacles.Count,
                    });
                }
                if (zone.O4 > 0)
                {
                    obstacles.Add(new Obstacle()
                    {
                        capacity = zone.O4,
                        from = zone.i,
                        to = n-1,
                        index = obstacles.Count,
                    });
                }
            }

            vertices.Add(new Vertex());
            obstacles.Add(new Obstacle()
            {
                capacity = int.MaxValue,
                from = n-1,
                to = vertices.Count-1,
                index = obstacles.Count,
            });

            vertecies = vertices.ToArray();
            edges = new Obstacle[obstacles.Count];
            foreach (var obs in obstacles)
            {
                vertecies[obs.from].outgoing.Add(obs.index);
                vertecies[obs.to].incomming.Add(obs.index);
                edges[obs.index] = obs;
            }


            while (MaxFlow(0, new bool[vertecies.Length], vertecies.Length - 1))
            {
                // Do nothing
            }

            var totalFlow = vertecies[vertecies.Length - 1].incomming.Sum(x => obstacles[x].flow);
            Console.WriteLine(totalFlow);
        }

        static int dist_squared(Point a, Point b)
        {
            return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);
        }

        static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        // Solve max flow graph.
        static Vertex[] vertecies;
        static Obstacle[] edges;
        static bool MaxFlow(int v, bool[] visited, int finalIndex)
        {
            if (v == finalIndex) return true;
            visited[v] = true;
            foreach (var edgeI in vertecies[v].outgoing)
            {
                var edge = edges[edgeI];
                if (!visited[edge.to] && edge.flow < edge.capacity)
                {
                    if (MaxFlow(edge.to, visited, finalIndex))
                    {
                        edge.flow++;
                        return true;
                    }
                }
            }
            foreach (var edgeI in vertecies[v].incomming)
            {
                var edge = edges[edgeI];
                if (!visited[edge.from] && edge.flow > 0)
                {
                    if (MaxFlow(edge.from, visited, finalIndex))
                    {
                        edge.flow--;
                        return true;
                    }

                }
            }
            return false;
        }

        


    }

    

}