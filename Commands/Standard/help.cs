using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;


namespace reflectionCli {

    public class help : ICommand    {

        public help() {
            Console.WriteLine("Valid Commands:");
            Program.activeasm.ForEach(x => {
                Console.WriteLine(Environment.NewLine + "   - " + x.FullName);
                x.DefinedTypes.Where(z => (
                    //this has to be done this way as the ICommand interface is not object equivalent for runtime loaded assemblies
                    z.ImplementedInterfaces.Where(a => (a.Name == "ICommand"))
                                            .ToList()
                                            .Count != 0
                ))
                .ToList()
                .ForEach(y => Console.WriteLine("       - " + y.Name));
            });
        }
        public bool ExitVal()   {
            return false;
        }
    }
}