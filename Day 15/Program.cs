using System.Collections;
using System.Numerics;
using Extensions;

var input = File.ReadLines("input.txt")
    .Select(l => l[10..].Split(": closest beacon is at "))
    .Select(c =>
    {
        var s = c[0][2..].Split(", y=");
        var b = c[1][2..].Split(", y=");
        return (sensor: new Vector2(int.Parse(s[0]), int.Parse(s[1]))
            , beacon: new Vector2(int.Parse(b[0]), int.Parse(b[1])));
    }).ToArray();

const int rowToCheck = 2000000;
var beaconCantBe = new HashSet<int>();
foreach (var (sensor, beacon) in input)
{
    var mRange = MDist(sensor, beacon);

    for (var x = 0; x < mRange; x++)
    {
        if (MDist(sensor, new Vector2(sensor.X + x, rowToCheck)) > mRange) break;

        beaconCantBe.Add((int)sensor.X + x);
    }

    for (var x = 0; x < mRange; x++)
    {
        if (MDist(sensor, new Vector2(sensor.X - x, rowToCheck)) > mRange) break;

        beaconCantBe.Add((int)sensor.X - x);
    }
}

foreach (var (_, beacon) in input)
    if ((int)beacon.Y == rowToCheck)
        beaconCantBe.Remove((int)beacon.X);

Console.WriteLine($"Solution 1: {beaconCantBe.Count}");

long ans = 0;
foreach (var (sens, bea) in input)
{
    // generate all sens range points + 1
    var mRange = MDist(sens, bea);

    // top -> right
    var edges = GetPoints(sens with { Y = sens.Y - mRange - 1 }, sens with { X = sens.X + mRange + 1 })
        .Concat(GetPoints(sens with { Y = sens.Y + mRange + 1 }, sens with { X = sens.X + mRange + 1 }))
        .Concat(GetPoints(sens with { X = sens.X - mRange - 1 }, sens with { Y = sens.Y + mRange + 1 }))
        .Concat(GetPoints(sens with { X = sens.X - mRange - 1 }, sens with { Y = sens.Y - mRange - 1 }));

    foreach (var point in edges)
    {
        var covered = false;
        foreach (var (sensor, beacon) in input)
        {
            if (MDist(sensor, point) > MDist(sensor, beacon)) continue;
            covered = true;
            break;
        }

        if (!covered)
        {
            Console.WriteLine($"X: {point.X}, Y: {point.Y}");
            ans = (long)point.X * 4000000 + (long)point.Y;
            break;
        }
    }

    if (ans != 0) break;
}

Console.WriteLine($"Solution 2: {ans}");

IEnumerable<Vector2> GetPoints(Vector2 p1, Vector2 p2)
{
    var m = (p1.Y - p2.Y) / (p1.X - p2.X);
    for (var i = 0; i <= MathF.Abs(p1.X - p2.X); i++)
    {
        if (p1.X + i is < 0 or > 4000000 || p1.Y + m * i is < 0 or > 4000000) continue;
        yield return new Vector2(p1.X + i, p1.Y + m * i);
    }
}

int MDist(Vector2 p1, Vector2 p2)
    => (int)(MathF.Abs(p1.X - p2.X) + MathF.Abs(p1.Y - p2.Y));