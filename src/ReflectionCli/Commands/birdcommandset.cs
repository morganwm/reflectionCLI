using System;
using System.Collections.Generic;
using ReflectionCli.Lib;

namespace ReflectionCli.extended
{
    public class BirdCommandSet
    {
        public class Parrot : ICommand
        {
			private readonly IAssemblyService _assemblyService;
            private readonly IVariableService _variableService;

            public Parrot(IAssemblyService assemblyService, IVariableService variableService)
            {
                _assemblyService = assemblyService;
                _variableService = variableService;
            }

            public void Run(List<string> input) 
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