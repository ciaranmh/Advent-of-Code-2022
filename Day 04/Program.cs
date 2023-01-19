using Extensions;

var input = File.ReadLines("input.txt")
    .Select(s =>
    {
        var ss = s.Split(",").Select(sss => sss.Split("-")).ToArray();
        return
            ((min: ss[0][0].ToInt(), max: ss[0][1].ToInt()),
                (min: ss[1][0].ToInt(), max: ss[1][1].ToInt()));
    }).ToArray();

var sol1 = input.Count(c =>
    (c.Item1.min <= c.Item2.min && c.Item1.max >= c.Item2.max) ||
    (c.Item1.min >= c.Item2.min && c.Item1.max <= c.Item2.max));

var sol2 = input.Count(c => c.Item1.max >= c.Item2.min && c.Item2.max >= c.Item1.min);

Console.WriteLine("Solution 1: " + sol1);

Console.WriteLine("Solution 1: " + sol2);