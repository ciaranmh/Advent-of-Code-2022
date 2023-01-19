var input = File.ReadLines("input.txt").ToArray();

var end = (0, 0);
var distance = GetDistance('S', 'E', h => h <= 1);
Console.WriteLine($"Solution 1: {distance[end]}");

distance = GetDistance('E', 'a', h => h >= -1);
Console.WriteLine($"Solution 2: {distance[end]}");

Dictionary<(int x, int y), int> GetDistance(char startFrom, char endAt, Func<int,bool> movable)
{
    var unvisited = new HashSet<(int, int)>();
    var distanceDict = new Dictionary<(int x, int y), int>();

    for (var y = 0; y < input.Length; y++)
    for (var x = 0; x < input[0].Length; x++)
    {
        if (input[y][x] == startFrom)
        {
            var start = (x, y);
            distanceDict[start] = 0;
        }
        else distanceDict[(x, y)] = int.MaxValue;

        unvisited.Add((x, y));
    }

    var endFound = false;
    while (!endFound)
    {
        var curNode = unvisited.MinBy(p => distanceDict[p]);
        Console.WriteLine($"From {curNode} : {distanceDict[curNode]}");
        foreach (var pos in GetUnvisitedNeighbours(curNode))
        {
            var newDist = distanceDict[curNode] + 1;
            if (newDist >= distanceDict[pos]) continue;
            Console.WriteLine($"\tSet {pos} : {newDist}");
            distanceDict[pos] = newDist;
            if (input[pos.y][pos.x] == endAt)
            {
                end = pos;
                endFound = true;
                break;
            }
        }

        unvisited.Remove(curNode);
    }

    return distanceDict;

    IEnumerable<(int x, int y)> GetUnvisitedNeighbours((int x, int y) pos)
    {
        var res = new List<(int x, int y)>();

        if (unvisited.Contains((pos.x - 1, pos.y)) && Reachable(pos, (pos.x - 1, pos.y)))
            res.Add((pos.x - 1, pos.y));
        if (unvisited.Contains((pos.x + 1, pos.y)) && Reachable(pos, (pos.x + 1, pos.y)))
            res.Add((pos.x + 1, pos.y));

        if (unvisited.Contains((pos.x, pos.y - 1)) && Reachable(pos, (pos.x, pos.y - 1)))
            res.Add((pos.x, pos.y - 1));
        if (unvisited.Contains((pos.x, pos.y + 1)) && Reachable(pos, (pos.x, pos.y + 1)))
            res.Add((pos.x, pos.y + 1));

        return res;
    }

    bool Reachable((int x, int y) from, (int x, int y) to)
        => movable(HeightDiff(from, to)) && to.x < input[0].Length && to.x >= 0 && to.y < input.Length && to.y >= 0;

    int HeightDiff((int x, int y) from, (int x, int y) to)
    {
        var fromHeight = input[from.y][from.x] switch
        {
            'S' => 'a',
            'E' => 'z',
            _ => input[from.y][from.x]
        };
        var toHeight = input[to.y][to.x] switch
        {
            'S' => 'a',
            'E' => 'z',
            _ => input[to.y][to.x]
        };
        return toHeight - fromHeight;
    }
}