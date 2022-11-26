# Pathfinding
Unity简要演示寻路算法（A*、BFS）

- GitHub源项目：[Pathfinding - Matthew-J-Spencer](https://github.com/Matthew-J-Spencer/Pathfinding)

- 视频演示
  - YouTube演示：[Pathfinding - Matthew-J-Spencer](https://www.youtube.com/watch?v=i0x5fj4PqP4)
  - B站演示：[Pathfinding - Matthew-J-Spencer](https://www.bilibili.com/video/BV1v44y1h7Dt/?spm_id_from=333.788.recommend_more_video.-1&vd_source=c850cfae45ee56e9a0ab838c43fc9870)

# 寻路算法

## A*

核心代码：
```C#
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

        if (current == targetNode)
        {
            var currentPathTile = targetNode;
            var path = new List<NodeBase>();

            while (currentPathTile != startNode)
            {
                path.Add(currentPathTile);
                currentPathTile = currentPathTile.Connection;
            }
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
                }
            }
        }
    }

    return null;
}
```

## BFS

```C#
public static List<NodeBase> FindPathByBFS(NodeBase startNode, NodeBase targetNode)
{
    var toSearch = new List<NodeBase>() { startNode };
    HashSet<NodeBase> processed = new HashSet<NodeBase>() { startNode };

    while (toSearch.Any())
    {
        var current = toSearch[0];

        processed.Add(current);
        toSearch.Remove(current);

        if (current == targetNode)
        {
            var currentPathTile = targetNode;
            var path = new List<NodeBase>();
            while (currentPathTile != startNode)
            {
                path.Add(currentPathTile);
                currentPathTile = currentPathTile.Connection;
            }
            return path;
        }

        var neighbors = current.Neighbors.Where(node => node.Walkable && !processed.Contains(node));
        foreach (NodeBase neighbor in neighbors)
        {
            neighbor.SetConnection(current);

            toSearch.Add(neighbor);
            processed.Add(neighbor);
        }
    }

    return null;
}
```

# 参考文章

- [Introduction to the A* Algorithm - Red Blob Games](https://www.redblobgames.com/pathfinding/a-star/introduction.html)
