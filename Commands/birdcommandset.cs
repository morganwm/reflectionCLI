using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace reflectionCli.extended {
    public class birdcommandset {

        public class Parrot : ICommand    {

            public Parrot(List<string> input) {
                input.ForEach(x => Console.WriteLine(x));
            }

            public bool ExitVal() {
                return false;
            }
        }

        public class Parakeet : ICommand    {

            public Parakeet(List<string> input) {
                input.ForEach(x => Console.WriteLine("  +" + x));
            }

            public bool ExitVal() {
                return false;
            }
        }
    }
}