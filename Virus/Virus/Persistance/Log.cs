using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Virus.Persistance
{
    public class Log
    {
        private static Queue<string> normalQueue = new Queue<string>();
        private static Queue<string> errorQueue = new Queue<string>();
        public static void WriteLineToLog(string line)
        {
            normalQueue.Enqueue(line);
            lock (normalQueue)
            {
                File.AppendAllText(@"..\..\log.txt", normalQueue.Dequeue() + Environment.NewLine);
            }
        }
        public static void WriteErrorGameLog(int BoardSize, string line)
        {
            errorQueue.Enqueue(line);
            lock (errorQueue)
            {
                File.AppendAllText(@"..\..\logForGame"+BoardSize+".txt", errorQueue.Dequeue() + Environment.NewLine);
            }
        }
    }
}
