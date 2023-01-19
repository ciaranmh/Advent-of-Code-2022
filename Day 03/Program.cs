using Extensions;

var input = File.ReadLines("input.txt").ToArray();

var alpha = ("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower() + "ABCDEFGHIJKLMNOPQRSTUVWXYZ").ToCharArray();

var sol1 = input
    .Select(s => (s[..(s.Length/2)],s[(s.Length/2)..]))
    .Sum(ss => Array.IndexOf(alpha, ss.Item1.Intersect(ss.Item2).First()) + 1);

var sol2 = input
    .Chunk(3)
    .Sum(group => Array.IndexOf(alpha,group.Intersect().First()) + 1);

Console.WriteLine("Solution 1: " + sol1);

Console.WriteLine("Solution 2: " + sol2);