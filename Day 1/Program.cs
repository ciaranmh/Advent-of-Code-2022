using Extensions;

var input = File.ReadAllText("input.txt")
    .Split(Environment.NewLine + Environment.NewLine)
    .Select(s => s.Split(Environment.NewLine).Sum(int.Parse))
    .OrderDescending()
    .ToArray();

Console.WriteLine("Solution 1: " + input.First());

Console.WriteLine("Solution 2: " + input[..3].PrettyPrint());