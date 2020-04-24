using System;

namespace SourceEngine.Heatmap.Generator
{
	public class ConsoleMessageStyler
    {
        public void PrintWarningMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void PrintErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(message);
            Console.ResetColor();
        }


        public ConsoleMessageStyler() { }
	}
}
