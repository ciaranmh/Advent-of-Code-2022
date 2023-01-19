using System.Numerics;

var input = File.ReadLines("input.txt").Select(s => s.Split(' ')).ToArray();

var timesVisited = new HashSet<Vector2> { Vector2.Zero };
var headPos = Vector2.Zero;
var tailPoss = new[] { Vector2.Zero };
var frames = new List<(string move, Vector2 head, List<Vector2> tails)>();

DoTails();
//PrintFrames();
Console.WriteLine("Solution 1: " + timesVisited.Count);

Console.WriteLine("\n\n=====================================================================\n");

headPos = Vector2.Zero;
timesVisited = new HashSet<Vector2> { Vector2.Zero };
tailPoss = new Vector2[9];
Array.Fill(tailPoss, Vector2.Zero);
frames = new List<(string move, Vector2 head, List<Vector2> tails)>();

DoTails();
//PrintFrames();
Console.WriteLine("Solution 2: " + timesVisited.Count);

void PrintFrames()
{
    var maxX = frames.Max(ls => Math.Max(ls.tails.MaxBy(v => v.X).X, ls.head.X)) + 1;
    var minX = frames.Min(ls => Math.Min(ls.tails.MinBy(v => v.X).X, ls.head.X));
    var maxY = frames.Max(ls => Math.Max(ls.tails.MaxBy(v => v.Y).Y, ls.head.Y)) + 1;
    var minY = frames.Min(ls => Math.Min(ls.tails.MinBy(v => v.Y).Y, ls.head.Y));
    foreach (var frame in frames)
    {
        Console.WriteLine(frame.move + ":");
        for (var y = maxY - 1; y >= minY; y--)
        {
            for (var x = minX; x < maxX; x++)
            {
                if (frame.head == new Vector2(x, y)) Console.Write("H");
                else if (frame.tails.Contains(new Vector2(x, y)))
                    Console.Write(frame.tails.Select((pos, i) => (vector2: pos, i))
                        .Where((tuple, _) => tuple.vector2 == new Vector2(x, y)).Min(vt => vt.i) + 1);
                else Console.Write("#");
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }
}

void DoTails()
{
    frames.Add(("start", headPos, new List<Vector2>(tailPoss)));
    foreach (var instruction in input)
    {
        var dir = instruction[0];
        var dist = int.Parse(instruction[1]);

        if (dir == "U")
            DoMove(Vector2.UnitY, dist);
        else if (dir == "D")
            DoMove(-Vector2.UnitY, dist);
        else if (dir == "L")
            DoMove(-Vector2.UnitX, dist);
        else if (dir == "R")
            DoMove(Vector2.UnitX, dist);

        frames.Add(($"{instruction[0]} {instruction[1]}", headPos, new List<Vector2>(tailPoss)));
    }
}

void DoMove(Vector2 move, int num)
{
    for (var i = 0; i < num; i++)
    {
        headPos += move;

        var prevTail = headPos;
        for (var index = 0; index < tailPoss.Length; index++)
        {
            tailPoss[index] += GetMove(prevTail, tailPoss[index]);
            if (index == tailPoss.Length - 1) timesVisited.Add(tailPoss[index]);
            prevTail = tailPoss[index];
        }
    }
}

Vector2 GetMove(Vector2 a, Vector2 b)
{
    var diff = Vector2.Subtract(a, b);
    return Vector2.Abs(diff) is { X: <= 1, Y: <= 1 } ? Vector2.Zero : Vector2.Clamp(diff, -Vector2.One, Vector2.One);
}