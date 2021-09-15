namespace Codingame.Logger
{
    using System;
    using System.Text.Json;

    internal class Log
    {
        internal static void Write(object o)
        {
            Console.Error.WriteLine(JsonSerializer.Serialize(o));
        }
    }
}
