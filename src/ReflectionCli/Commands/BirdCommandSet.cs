using System;
using System.Collections.Generic;
using ReflectionCli.Lib;

namespace ReflectionCli.extended
{
    public class BirdCommandSet
    {
        public class Parrot : ICommand
        { 

            public void Run(List<string> input) 
            {
                input.ForEach(x => Console.WriteLine(x));
            }
        }

        public class Parakeet : ICommand
        {
            public void Run(List<string> input)
            {
                input.ForEach(x => Console.WriteLine($"  +{x}"));
            }

            public void Run(List<string> input, int number)
            {
                input.ForEach(x => Console.WriteLine($"     {number}: {x}"));
            }

            public void Run(List<int> inputints, int number)
            {
                inputints.ForEach(x => Console.WriteLine($"     {number}: {x}"));
            }
        }
    }
}