using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpGame.Util
{
    public static class Logger
    {
        [Conditional("DEBUG")]
        public static void Info(object message) => Log(ConsoleColor.White, SharedConstants.LoggerLevelInfo, message);
        [Conditional("DEBUG")]
        public static void Warn(object message) => Log(ConsoleColor.Yellow, SharedConstants.LoggerLevelWarn, message);
        [Conditional("DEBUG")]
        public static void Debug(object message) => Log(ConsoleColor.Gray, SharedConstants.LoggerLevelDebug, message);
        [Conditional("DEBUG")]
        public static void Error(object message) => Log(ConsoleColor.Red, SharedConstants.LoggerLevelError, message);
        [Conditional("DEBUG")]
        public static void Exception(Exception ex) => Log(ConsoleColor.DarkRed, SharedConstants.LoggerLevelException, $"An unhandled exception has been thrown: {ex.Message}\n\n{ex.StackTrace}\n");
        [Conditional("DEBUG")]
        private static void Log(ConsoleColor color, string level, object message)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"[{DateTime.Now}] [{level}] [{Thread.CurrentThread.Name}]: {message}");
            Console.ResetColor();
        }
    }
}
