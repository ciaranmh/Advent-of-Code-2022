using System.Collections;
using System.Diagnostics.Contracts;
using System.Text;

namespace Extensions;

public static class CustomLinq
{
    #region IEnumerable extensions

    /// <summary>
    /// Selects items that meet a predicate
    /// </summary>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    [Pure]
    public static IEnumerable<TResult> SelectWhere<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, bool> predicate,
        Func<TSource, TResult> selector)
    {
        foreach (var item in source)
            if (predicate(item))
                yield return selector(item);
    }

    /// <summary>
    /// Selects items that meet a predicate
    /// </summary>
    /// <param name="source"></param>
    /// <param name="predicate">Predicate with index</param>
    /// <param name="selector">Selector with index</param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    [Pure]
    public static IEnumerable<TResult> SelectWhere<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, int, bool> predicate,
        Func<TSource, int, TResult> selector)
    {
        var sourceArr = source.ToArray();
        for (var index = 0; index < sourceArr.Length; index++)
        {
            var item = sourceArr[index];
            if (predicate(item, index))
                yield return selector(item, index);
        }
    }

    public static void ForEach<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, bool> predicate,
        Action<TSource> actor)
    {
        foreach (var item in source)
            if (predicate(item))
                actor(item);
    }

    public static void ForEach<TSource>(
        this IEnumerable<TSource> source,
        Action<TSource> actor)
    {
        foreach (var item in source)
            actor(item);
    }

    public static void ForEach<TSource>(
        this IEnumerable<TSource> source,
        Action<TSource, int> actor)
    {
        var sourceArr = source.ToArray();
        for (var index = 0; index < sourceArr.Length; index++)
        {
            var item = sourceArr[index];
            actor(item, index);
        }
    }

    public static void ForEach<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, int, bool> predicate,
        Action<TSource, int> actor)
    {
        var sourceArr = source.ToArray();
        for (var index = 0; index < sourceArr.Length; index++)
        {
            var item = sourceArr[index];
            if (predicate(item, index))
                actor(item, index);
        }
    }

    public static string PrettyPrint(this object? o)
    {
        return o switch
        {
            string s => s,
            IDictionary d => d.PrettyPrint(),
            IEnumerable l => l.PrettyPrint(),
            _ => o?.ToString()
        } ?? "null";
    }

    public static string PrettyPrint(this IEnumerable list)
    {
        var result = "[";
        result += string.Join(", ", list.Cast<object?>().Select(PrettyPrint));
        result += "]";
        return result;
    }

    public static string PrettyPrint(this IDictionary dictionary)
    {
        var result = "{";
        result += string.Join(", ", dictionary.Keys.Cast<object>()
            .Select(key => $"{{{key.PrettyPrint()} : {dictionary[key].PrettyPrint()}}}"));
        result += "}";
        return result;
    }

    [Pure]
    public static IEnumerable<TSource> Intersect<TSource>(this IEnumerable<IEnumerable<TSource>> source)
        => source.Aggregate((current, item) => current.Intersect(item));

    [Pure]
    public static IEnumerable<TResult> Intersect<TSource, TResult>(
        this IEnumerable<IEnumerable<TSource>> source,
        Func<TSource, TResult> selector)
    {
        var enumerable = source as IEnumerable<TSource>[] ?? source.ToArray();
        return enumerable.Aggregate(enumerable.First().Select(selector),
            (current, item) => current.Intersect(item.Select(selector)));
    }

    // turns a list of same length strings into 2d array
    public static TResult[,] ToMatrix<TSource, TResult>(this IEnumerable<IEnumerable<TSource>> source, int length,
        Func<IEnumerable<TSource>, TResult> selector)
    {
        var sourceArr = source.ToArray();
        var mat = new TResult[sourceArr.Length, sourceArr[0].Count() / length];

        for (var y = 0; y < sourceArr.Length; y++)
        {
            var temp = sourceArr[y].Chunk(length).Select(ls => selector(ls)).ToArray();
            for (var x = 0; x < temp.Length; x++)
                mat[y, x] = temp[x];
        }

        return mat;
    }

    #endregion

    #region string extensions

    /// <summary>
    /// Replaces char in a string at an index
    /// </summary>
    /// <param name="str"></param>
    /// <param name="index">The index of the char to replace</param>
    /// <param name="replace">The char replacing</param>
    /// <returns></returns>
    [Pure]
    public static string ReplaceAt(this string str, int index, char replace)
    {
        var temp = str.ToCharArray();
        temp[index] = replace;
        return new string(temp);
    }

    public static int ToInt(this string c)
    {
        return Convert.ToInt32(c);
    }

    #endregion

    #region char extensions

    public static int ToInt(this char c)
    {
        return Convert.ToInt32(c);
    }

    #endregion

    #region IList extensions

    /// <summary>
    /// Updates all the values in an IList
    /// </summary>
    /// <param name="source"></param>
    /// <param name="mapper"></param>
    /// <typeparam name="TSource"></typeparam>
    public static void UpdateAll<TSource>(this IList<TSource> source, Func<TSource, TSource> mapper)
    {
        for (var index = 0; index < source.Count; index++)
            source[index] = mapper(source[index]);
    }

    /// <summary>
    /// Updates all the values in an IList
    /// </summary>
    /// <param name="source"></param>
    /// <param name="mapper">mapper with index</param>
    /// <typeparam name="TSource"></typeparam>
    public static void UpdateAll<TSource>(this IList<TSource> source, Func<TSource, int, TSource> mapper)
    {
        for (var index = 0; index < source.Count; index++)
            source[index] = mapper(source[index], index);
    }

    #endregion

    #region T[] extensions

    /// <summary>
    /// Updates all the values in an Array
    /// </summary>
    /// <param name="source"></param>
    /// <param name="func"></param>
    /// <typeparam name="TSource"></typeparam>
    public static void ForEach<TSource>(this IList<TSource> source, Action<TSource> func)
    {
        foreach (var t in source)
            func(t);
    }

    /// <summary>
    /// Updates all the values in an Array
    /// </summary>
    /// <param name="source"></param>
    /// <param name="func">mapper with index</param>
    /// <typeparam name="TSource"></typeparam>
    public static void ForEach<TSource>(this IList<TSource> source, Action<TSource, int> func)
    {
        for (var index = 0; index < source.Count; index++)
            func(source[index], index);
    }

    #endregion

    #region T[,] extensions

    /// <summary>
    /// Gets a column of a 2D array
    /// </summary>
    /// <param name="source"></param>
    /// <param name="column"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [Pure]
    public static T[] GetColumn<T>(this T[,] source, int column)
    {
        var tempArr = new T[source.GetLength(0)];
        for (var row = 0; row < source.GetLength(0); row++)
            tempArr[row] = source[row, column];

        return tempArr;
    }

    /// <summary>
    /// Gets a row of a 2D array
    /// </summary>
    /// <param name="source"></param>
    /// <param name="row"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [Pure]
    public static T[] GetRow<T>(this T[,] source, int row)
    {
        var tempArr = new T[source.GetLength(1)];
        for (var column = 0; column < source.GetLength(1); column++)
            tempArr[column] = source[row, column];

        return tempArr;
    }

    /// <summary>
    /// Gets all columns of 2D array
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [Pure]
    public static T[][] GetColumns<T>(this T[,] source)
    {
        var tempArr = new T[source.GetLength(1)][];
        for (var col = 0; col < source.GetLength(1); col++)
            tempArr[col] = source.GetColumn(col);

        return tempArr;
    }

    /// <summary>
    /// Gets all rows of a 2D array
    /// </summary>
    /// <param name="source"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [Pure]
    public static T[][] GetRows<T>(this T[,] source)
    {
        var tempArr = new T[source.GetLength(0)][];
        for (var row = 0; row < source.GetLength(0); row++)
            tempArr[row] = source.GetRow(row);

        return tempArr;
    }

    [Pure]
    public static string ToMatrixString<T>(this T[,] matrix, string delimiter = "\t")
    {
        var s = new StringBuilder();
        for (var i = 0; i < matrix.GetLength(0); i++)
        {
            for (var j = 0; j < matrix.GetLength(1); j++)
                s.Append(matrix[i, j]).Append(delimiter);

            s.AppendLine();
        }

        return s.ToString();
    }

    #endregion
}