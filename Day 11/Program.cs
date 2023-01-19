using System.Collections.Immutable;
using Extensions;

List<(List<long> startingItems, (Func<long, long, long> @operator, string operand) operations, (long condition, int ifTrue, int ifFalse) test)> ParseInput()
{
    return File.ReadAllText("input.txt").Split(Environment.NewLine + Environment.NewLine)
        .Select(s => s.Split(Environment.NewLine))
        .Select(ls => (
            startingItems: ls[1].Split(": ")[1].Split(", ").Select(long.Parse).ToList(),
            operations: (@operator: ls[2].Split(": ")[1].Split(" ")[3] switch
                {
                    "*" => (Func<long, long, long>)((x, y) => x * y),
                    "+" => (Func<long, long, long>)((x, y) => x + y),
                    _ => throw new ArgumentOutOfRangeException()
                },
                operand: ls[2].Split(": ")[1].Split(" ")[4]),
            test: (condition: long.Parse(ls[3].Split(": ")[1].Split(" ")[2]),
                ifTrue: int.Parse(ls[4].Split(": ")[1].Split(" ")[3]),
                ifFalse: int.Parse(ls[5].Split(": ")[1].Split(" ")[3]))
        )).ToList();
}

var input = ParseInput();
var gigaModulo = input.Aggregate((long)1, (current, monkey) => current * monkey.test.condition);

var monkeyInspections = new long[input.Count];
for (var i = 0; i < 20; i++)
    IterateMonkeys();

Console.WriteLine("Solution 1: " + monkeyInspections.ToImmutableSortedSet().Take(^2..).Aggregate((x, y) => x * y));


input = ParseInput();
monkeyInspections = new long[input.Count];
for (var i = 0; i < 10000; i++)
    IterateMonkeys(true);

Console.WriteLine("Solution 2: " + monkeyInspections.ToImmutableSortedSet().Take(^2..).Aggregate((x, y) => x * y));

void IterateMonkeys(bool doGigaModulo = false)
{
    for (var index = 0; index < input.Count; index++)
    {
        var monkey = input[index];
        for (var i = monkey.startingItems.Count - 1; i >= 0; i--)
        {
            monkeyInspections[index]++;
            var item = monkey.startingItems[i];
            var newWorryLevel = doGigaModulo
                ? monkey.operations.@operator(item,
                    monkey.operations.operand == "old" ? item : long.Parse(monkey.operations.operand)) % gigaModulo
                : monkey.operations.@operator(item,
                    monkey.operations.operand == "old" ? item : long.Parse(monkey.operations.operand)) / 3;

            var monkeyTo = newWorryLevel % monkey.test.condition == 0 ? monkey.test.ifTrue : monkey.test.ifFalse;

            input[monkeyTo].startingItems.Add(newWorryLevel);
            monkey.startingItems.Remove(item);
        }
    }
}