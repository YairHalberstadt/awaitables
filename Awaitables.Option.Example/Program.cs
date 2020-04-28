using System;
using System.Collections.Generic;

namespace Awaitables.Example
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var program = GetSelectedProgram();
            if (program.HasValue)
            {
                program.Value();
            }
            else
            {
                Console.WriteLine("Invalid argument");
            }
            async Option<Action> GetSelectedProgram()
            {
                var arg = await args.ElementAt(0);
                var asInt = await arg.TryParseInt();
                return await _programs.TryGetValue(asInt);
            }
        }

        public static Dictionary<int, Action> _programs = GeneratePrograms();

        private static Dictionary<int, Action> GeneratePrograms() => new Dictionary<int, Action>
        {
            { 1, () => Console.WriteLine("Play chess") },
            { 2, () => Console.WriteLine("Solve World Hunger") },
            { 3, () => Console.WriteLine("Win a noble prize") },
        };
    }

    public static class Extensions
    {
        public static Option<T> ElementAt<T>(this IReadOnlyList<T> list, int index)
            => list.Count > index ? Option.Some(list[index]) : Option.None<T>();
        public static Option<TValue> TryGetValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
            => dictionary.TryGetValue(key, out var value) ? Option.Some(value) : Option.None<TValue>();
        public static Option<int> TryParseInt(this string str)
            => int.TryParse(str, out var value) ? Option.Some(value) : Option.None<int>();
    }
}