using System;
using System.Linq;
using System.Reflection;

namespace ReflectionCli
{
    public class List : ICommand
    {
        public List()
        {
            Console.WriteLine("Valid Commands:");

            Program.ActiveAsm.ToList().ForEach(t =>
            {
                Console.WriteLine($"{Environment.NewLine}   - {t.Value.FullName}: {t.Key}");

                t.Value.DefinedTypes.Where(u => (
                    // this has to be done this way as the ICommand interface is not object equivalent for runtime loaded assemblies
                    u.ImplementedInterfaces.Where(v => v.Name == "ICommand")
                        .ToList()
                        .Count != 0
                ))
                .ToList()
                .ForEach(v => Console.WriteLine($"       - {v.Name}"));
            });
        }

        public List(string name)
        {
            Console.WriteLine($"Valid Commands for {name}:");
            Program.ActiveAsm.ToList().ForEach(t =>
            {
                Console.WriteLine($"{Environment.NewLine}   - {t.Value.FullName}: {t.Key}");
                t.Value.DefinedTypes.Where(u => (
                    // this has to be done this way as the ICommand interface is not object equivalent for runtime loaded assemblies
                    u.ImplementedInterfaces.Where(v => v.Name == "ICommand")
                        .ToList()
                        .Count != 0
                ))
                .Where(u => u.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                .ToList()
                .ForEach(u =>
                {
                    u.AsType().GetConstructors().ToList().ForEach(v =>
                    {
                        Console.WriteLine($"{Environment.NewLine} +       - {name}");

                        v.GetParameters().ToList().ForEach(x =>
                        {
                            Console.WriteLine($"        - {x.Name} ({x.ParameterType.FullName})");
                        });
                    });
                });
            });
        }
        public bool ExitVal()
        {
            return false;
        }
    }
}