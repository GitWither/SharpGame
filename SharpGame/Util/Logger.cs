using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpGame.Util
{
    internal static class Logger
    {
        public static void Info(object message) => Log(ConsoleColor.White, SharedConstants.LoggerLevelInfo, message);
        public static void Warn(object message) => Log(ConsoleColor.Yellow, SharedConstants.LoggerLevelWarn, message);
        public static void Debug(object message) => Log(ConsoleColor.Gray, SharedConstants.LoggerLevelDebug, message);
        public static void Error(object message) => Log(ConsoleColor.Red, SharedConstants.LoggerLevelError, message);
        public static void Exception(Exception ex) => Log(ConsoleColor.Yellow, SharedConstants.LoggerLevelException, $"An unhandled exception has been thrown: {ex.Message}\n\n{ex.StackTrace}\n");
        private static void Log(ConsoleColor color, string level, object message)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"[{DateTime.Now}] [{level}]: {message}");
            Console.ResetColor();
        }
    }
}
