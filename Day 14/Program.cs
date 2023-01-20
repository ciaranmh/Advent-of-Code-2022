using System.Numerics;
using Extensions;

var input = File.ReadLines("input.txt")
    .Select(line =>
        line.Split(" -> ")
            .Select(c =>
            {
                var cs = c.Split(',');
                return new Vector2(int.Parse(cs[0]), int.Parse(cs[1]));
            }).ToArray()
    );

var build = new Dictionary<Vector2, Block>();

var highestY = 0;
input.ForEach(line =>
{
    var start = line[0];
    foreach (var end in line[1..])
    {
        if (end.X > start.X)
        {
            var y = (int) start.Y;
            Enumerable.Range((int)start.X, (int)(end.X - start.X) + 1)
                .ForEach(x =>
                {
                    build[new Vector2(x, y)] = Block.Rock;
                    if (y > highestY) highestY = y;
                });
        }
        else if (start.X > end.X)
        {
            var y = (int) start.Y;
            Enumerable.Range((int)end.X, (int)(start.X - end.X) + 1)
                .ForEach(x =>
                {
                    build[new Vector2(x, y)] = Block.Rock; 
                    if (y > highestY) highestY = y;
                });
        }
        else if (end.Y > start.Y)
        {
            var x = (int) start.X;
            Enumerable.Range((int)start.Y, (int)(end.Y - start.Y) + 1)
                .ForEach(y =>
                {
                    build[new Vector2(x, y)] = Block.Rock; 
                    if (y > highestY) highestY = y;
                });
        }
        else if (start.Y > end.Y)
        {
            var x = (int) start.X;
            Enumerable.Range((int)end.Y, (int)(start.Y - end.Y) + 1)
                .ForEach(y =>
                {
                    build[new Vector2(x, y)] = Block.Rock; 
                    if (y > highestY) highestY = y;
                });
        }

        start = end;
    }
});
var build2 = new Dictionary<Vector2, Block>(build);

var sandNum = 0;
var sandPos = new Vector2(500, 0);
while (sandPos.Y < highestY)
{
    sandPos = new Vector2(500, 0);
    // sim sand
    while (sandPos.Y < highestY)
    {
        if (!build.ContainsKey(sandPos + Vector2.UnitY))
            sandPos += Vector2.UnitY;
        else if (!build.ContainsKey(sandPos + new Vector2(-1, 1)))
            sandPos += new Vector2(-1, 1);
        else if (!build.ContainsKey(sandPos + Vector2.One))
            sandPos += Vector2.One;
        else
        {
            build[sandPos] = Block.Sand;
            break;
        }
    }
    
    sandNum++;
}

Console.WriteLine($"Solution 1: {sandNum - 1}");

sandNum = 0;
while (!build2.ContainsKey(new Vector2(500,0)))
{
    sandPos = new Vector2(500, 0);
    // sim sand
    while (!build2.ContainsKey(new Vector2(500,0)))
    {
        if ((int)(sandPos.Y + 1) == highestY + 2)
        {
            build2[sandPos] = Block.Sand;
            break;
        }
        if (!build2.ContainsKey(sandPos + Vector2.UnitY))
            sandPos += Vector2.UnitY;
        else if (!build2.ContainsKey(sandPos + new Vector2(-1, 1)))
            sandPos += new Vector2(-1, 1);
        else if (!build2.ContainsKey(sandPos + Vector2.One))
            sandPos += Vector2.One;
        else
        {
            build2[sandPos] = Block.Sand;
            break;
        }
    }
    
    sandNum++;
}

Console.WriteLine($"Solution 2: {sandNum}");

file enum Block
{
    Sand,
    Rock
}