using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Awaitables.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = UpdateDbWithRetry();

            if (result.IsSuccessful)
            {
                Console.WriteLine($"Wrote {result.Value} items to db");
            }
            else
            {
                Console.WriteLine(result.Exception);
            }

            Result<int> UpdateDbWithRetry()
            {
                return UpdateDb() switch
                {
                    { IsSuccessful: true } result => result,
                    { Exception: TimeoutException _ } => UpdateDbWithRetry(),
                    var result => result,
                };
            }

            async Result<int> UpdateDb()
            {
                var connectionString = "MyConnectionString";
                var integers = await Db.Query<int>(connectionString, "Select * from Integers");
                var squares = integers.Select(x => x ^ 2);
                return await Db.Write(connectionString, squares);
            }
        }
    }

    public static class Db
    {
        private static readonly Random _random = new Random();
        public static async Result<IEnumerable<TResult>> Query<TResult>(string connectionString, string query) => _random.Next() % 2 == 0
            ? throw new TimeoutException("could not connect")
            : Enumerable.Range(0, _random.Next() % 10).Cast<TResult>();

        public static async Result<int> Write<T>(string connectionString, IEnumerable<T> data) => data.Count() > 5
            ? throw new InvalidDataException("Too many items!!!")
            : data.Count();
    }

}
