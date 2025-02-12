using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Tiles;
using UnityEngine;

namespace Tarodev_Pathfinding._Scripts
{
    /// <summary>
    /// This algorithm is written for readability. Although it would be perfectly fine in 80% of games, please
    /// don't use this in an RTS without first applying some optimization mentioned in the video: https://youtu.be/i0x5fj4PqP4
    /// If you enjoyed the explanation, be sure to subscribe!
    ///
    /// Also, setting colors and text on each hex affects performance, so removing that will also improve it marginally.
    /// </summary>
    public static class Pathfinding
    {
        private static readonly Color PathColor = new Color(0.65f, 0.35f, 0.35f);
        private static readonly Color OpenColor = new Color(.4f, .6f, .4f);
        private static readonly Color ClosedColor = new Color(0.35f, 0.4f, 0.5f);

        public static List<NodeBase> FindPath(NodeBase startNode, NodeBase targetNode)
        {
            var toSearch = new List<NodeBase>() { startNode };
            var processed = new List<NodeBase>();

            while (toSearch.Any())
            {
                var current = toSearch[0];
                foreach (var t in toSearch)
                    if (t.F < current.F || t.F == current.F && t.H < current.H)
                        current = t;

                processed.Add(current);
                toSearch.Remove(current);

                current.SetColor(ClosedColor);

                if (current == targetNode)
                {
                    var currentPathTile = targetNode;
                    var path = new List<NodeBase>();
                    var count = 100;
                    while (currentPathTile != startNode)
                    {
                        path.Add(currentPathTile);
                        currentPathTile = currentPathTile.Connection;
                        count--;
                        if (count < 0) throw new Exception();
                        Debug.Log("sdfsdf");
                    }

                    foreach (var tile in path) tile.SetColor(PathColor);
                    startNode.SetColor(PathColor);
                    Debug.Log(path.Count);
                    return path;
                }

                foreach (var neighbor in current.Neighbors.Where(t => t.Walkable && !processed.Contains(t)))
                {
                    var inSearch = toSearch.Contains(neighbor);

                    var costToNeighbor = current.G + current.GetDistance(neighbor);

                    if (!inSearch || costToNeighbor < neighbor.G)
                    {
                        neighbor.SetG(costToNeighbor);
                        neighbor.SetConnection(current);

                        if (!inSearch)
                        {
                            neighbor.SetH(neighbor.GetDistance(targetNode));
                            toSearch.Add(neighbor);
                            neighbor.SetColor(OpenColor);
                        }
                    }
                }
            }

            return null;
        }

        #region BFS

        public static List<NodeBase> FindPathByBFS(NodeBase startNode, NodeBase targetNode)
        {
            var toSearch = new List<NodeBase>() { startNode };
            HashSet<NodeBase> processed = new HashSet<NodeBase>() { startNode };

            while (toSearch.Any())
            {
                var current = toSearch[0];

                processed.Add(current);
                toSearch.Remove(current);
                current.SetColor(ClosedColor);

                if (current == targetNode)
                {
                    var currentPathTile = targetNode;
                    var path = new List<NodeBase>();
                    var count = 100;
                    while (currentPathTile != startNode)
                    {
                        path.Add(currentPathTile);
                        currentPathTile = currentPathTile.Connection;
                        count--;
                        if (count < 0) throw new Exception();
                    }

                    foreach (var tile in path) tile.SetColor(PathColor);
                    startNode.SetColor(PathColor);
                    Debug.Log($"FindPathByBFS Done.  path.Count={path.Count}");
                    return path;
                }

                var neighbors = current.Neighbors.Where(node => node.Walkable && !processed.Contains(node));
                foreach (NodeBase neighbor in neighbors)
                {
                    neighbor.SetConnection(current);

                    toSearch.Add(neighbor);
                    processed.Add(neighbor);
                    neighbor.SetColor(OpenColor);
                }
            }

            return null;
        }

        #endregion

        #region Dijkstra

        public static List<NodeBase> FindPathByDijkstra(NodeBase startNode, NodeBase targetNode)
        {
            var toSearch = new List<NodeBase>() { startNode };
            HashSet<NodeBase> processed = new HashSet<NodeBase>() { startNode };

            while (toSearch.Any())
            {
                var current = toSearch[0];
                foreach (var t in toSearch)
                    if (t.G < current.G)
                        current = t;

                toSearch.Remove(current);
                current.SetColor(ClosedColor);

                if (current == targetNode)
                {
                    var currentPathTile = targetNode;
                    var path = new List<NodeBase>();
                    var count = 100;
                    while (currentPathTile != startNode)
                    {
                        path.Add(currentPathTile);
                        currentPathTile = currentPathTile.Connection;
                        count--;
                        if (count < 0) throw new Exception();
                    }

                    foreach (var tile in path) tile.SetColor(PathColor);
                    startNode.SetColor(PathColor);
                    Debug.Log($"FindPathByDijkstra Done.  path.Count={path.Count}");
                    return path;
                }

                var neighbors = current.Neighbors.Where(node => node.Walkable);
                foreach (NodeBase neighbor in neighbors)
                {
                    var costToNeighbor = current.G + current.GetDistance(neighbor);
                    if (!processed.Contains(neighbor) || costToNeighbor < neighbor.G)
                    {
                        neighbor.SetG(costToNeighbor);

                        toSearch.Add(neighbor);
                        processed.Add(neighbor);

                        neighbor.SetConnection(current);
                        neighbor.SetColor(OpenColor);
                    }
                }
            }

            return null;
        }

        #endregion

        #region Greedy Best First

        public static List<NodeBase> FindPathByGreedyBestFirst(NodeBase startNode, NodeBase targetNode)
        {
            var toSearch = new List<NodeBase>() { startNode };
            HashSet<NodeBase> processed = new HashSet<NodeBase>() { startNode };

            while (toSearch.Any())
            {
                var current = toSearch[0];
                foreach (var t in toSearch)
                    if (t.H < current.H)                    //The main difference with Dijkstra
                        current = t;

                toSearch.Remove(current);
                current.SetColor(ClosedColor);

                if (current == targetNode)
                {
                    var currentPathTile = targetNode;
                    var path = new List<NodeBase>();
                    var count = 100;
                    while (currentPathTile != startNode)
                    {
                        path.Add(currentPathTile);
                        currentPathTile = currentPathTile.Connection;
                        count--;
                        if (count < 0) throw new Exception();
                    }

                    foreach (var tile in path) tile.SetColor(PathColor);
                    startNode.SetColor(PathColor);
                    Debug.Log($"FindPathByDijkstra Done.  path.Count={path.Count}");
                    return path;
                }

                var neighbors = current.Neighbors.Where(node => node.Walkable);
                foreach (NodeBase neighbor in neighbors)
                {
                    if (!processed.Contains(neighbor))
                    {
                        var estimatedCostToTarget = neighbor.GetDistance(targetNode);    //The main difference with Dijkstra
                        neighbor.SetH(estimatedCostToTarget);

                        toSearch.Add(neighbor);
                        processed.Add(neighbor);

                        neighbor.SetConnection(current);
                        neighbor.SetColor(OpenColor);
                    }
                }
            }

            return null;
        }

        #endregion
    }
}