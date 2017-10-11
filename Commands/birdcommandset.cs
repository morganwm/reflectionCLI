using System;
using System.Collections.Generic;

namespace ReflectionCli.extended
{
    public class BirdCommandSet
    {
        public class Parrot : ICommand
        {
            public Parrot(List<string> input)
            {
                input.ForEach(x => Console.WriteLine(x));
            }

            public bool ExitVal()
            {
                return false;
            }
        }

        public class Parakeet : ICommand
        {
            public Parakeet(List<string> input)
            {
                input.ForEach(x => Console.WriteLine($"  +{x}"));
            }

            public Parakeet(List<string> input, int number)
            {
                input.ForEach(x => Console.WriteLine($"     {number}: {x}"));
            }

            public Parakeet(List<int> inputints, int number)
            {
                inputints.ForEach(x => Console.WriteLine($"     {number}: {x}"));
            }

            public bool ExitVal()
            {
                return false;
            }
        }
    }
}