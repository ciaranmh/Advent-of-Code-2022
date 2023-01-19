using Extensions;

const int numStacks = 9, maxStackHeight = 8;

var input = File.ReadLines("input.txt").ToArray();

var stackStrings = input[..maxStackHeight];
var moveInfo = input[(maxStackHeight + 2)..].Select(s =>
{
    var split1 = s.Split(" from ");
    var fromSplit = split1[1].Split(" to ");
    return (n: split1[0][5..].ToInt(), from: fromSplit[0].ToInt() - 1, to: fromSplit[1].ToInt() - 1);
}).ToArray();

Stack<char>[] stacks;
RestStacks();

foreach (var move in moveInfo)
    for (var i = 0; i < move.n; i++)
        stacks[move.to].Push(stacks[move.from].Pop());

Console.WriteLine("Solution 1: " + string.Join("", stacks.Select(st => st.Peek())));

RestStacks();
foreach (var move in moveInfo)
{
    var stackMove = new char[move.n];
    for (var i = 0; i < move.n; i++)
        stackMove[i] = stacks[move.from].Pop();
    for (var i = move.n - 1; i >= 0; i--)
        stacks[move.to].Push(stackMove[i]);
}

Console.WriteLine("Solution 2: " + string.Join("", stacks.Select(st => st.Peek()).ToArray()));

void RestStacks()
{
    stacks = Enumerable.Range(0, numStacks).Select(_ => new Stack<char>()).ToArray();
    foreach (var crate in stackStrings.Reverse())
    {
        crate.Chunk(4)
            .Select(cs => new string(cs).TrimEnd())
            .ForEach((s, _) => !string.IsNullOrWhiteSpace(s), (s, i) => stacks[i].Push(s[1]));
    }
}