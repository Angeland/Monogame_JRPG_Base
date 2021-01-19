using System;
using System.IO;

namespace DreamsEnd.States.DebugHelp
{
    public class DebugConsole
    {
        private static StringWriter output = new StringWriter();
        public static void WriteLine(string value)
        {
            output.WriteLine(value);
        }
        public static void Write(string value)
        {
            output.Write(value);
        }
        public static string Read()
        {
            return output.ToString();
        }
        public static void Reset()
        {
            output = new StringWriter();
        }
    }
}
