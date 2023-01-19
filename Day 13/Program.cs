using System.Text;
using Extensions;

var input = File.ReadAllText("input.txt");

var sol1 = input.Split(Environment.NewLine + Environment.NewLine)
    .Select(s => s.Split(Environment.NewLine))
    .Select(tuple => Compare(tuple[0], tuple[1]))
    .SelectWhere((n, _) => n == -1, (_, ind) => ind + 1)
    .Sum();

Console.WriteLine($"Solution 1: {sol1}");

var sol2 = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    .ToList();
sol2.Add("[[2]]");
sol2.Add("[[6]]");
sol2.Sort(Compare);

Console.WriteLine($"Solution 2: {(sol2.IndexOf("[[2]]") + 1) * (sol2.IndexOf("[[6]]") + 1)}");

int Compare(string left, string right)
{
    var leftIsArr = left.StartsWith('[');
    var rightIsArr = right.StartsWith('[');
    switch (leftIsArr)
    {
        case true when rightIsArr:
        {
            var ind = 0;
            while (GetNextItem(left, ind) is { } curLeft)
            {
                var curRight = GetNextItem(right, ind);
                if (curRight is null) return 1;

                var comp = Compare(curLeft, curRight);
                if (comp != 0) return comp;
                ind++;
            }

            return GetNextItem(right, ind) is null ? 0 : -1;
        }
        case false when !rightIsArr:
            return int.Parse(left).CompareTo(int.Parse(right));
        default:
            return Compare(leftIsArr ? left : "[" + left + "]", rightIsArr ? right : "[" + right + "]");
    }
}

string? GetNextItem(string strArr, int index)
{
    var sbDepth = 0;
    var curIndex = 0;
    var acc = new StringBuilder();
    string? curVal = null;
    for (var i = 1; i < strArr.Length - 1 && curIndex <= index; i++)
    {
        switch (strArr[i])
        {
            case ',' when sbDepth == 0:
                curVal = acc.ToString();
                acc.Clear();
                curIndex++;
                break;
            case '[':
                acc.Append(strArr[i]);
                sbDepth++;
                break;
            case ']':
                acc.Append(strArr[i]);
                sbDepth--;
                break;
            default:
                acc.Append(strArr[i]);
                break;
        }
    }

    if (curIndex == index) curVal = acc.ToString();
    return curIndex >= index ? curVal == "" ? null : curVal : null;
}