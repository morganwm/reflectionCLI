using System;
using ReflectionCli.Lib;

namespace ReflectionCli
{
    public class Exit : ICommand
    {
        public void Run(IParam param)
        {
            Console.WriteLine("Shutting down....");
            Program.ShutDown = true;
        }
    }
}