using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using GetModule.App;

namespace GetModules
{
    public class Program
    {
        static void Main(string[] args)
        {
            var root = Assembly.GetExecutingAssembly().Location;
            root = Path.GetDirectoryName(root);

            if (!Directory.Exists(root))
            {
                throw new FileNotFoundException($"folder {root} does not exisit");
            }

            var assemblies = GetAllAssemblies(root);
            Console.WriteLine("Assemblies:");
            Util.PrintList(assemblies);

            var modules = GetModules(assemblies, typeof(IModule), typeof(NinjectModule));
            Console.WriteLine("Modules:");
            Util.PrintList(modules);
        }

        static IEnumerable<Assembly> GetAllAssemblies(string path)
        {
            var assemblies = new List<Assembly>();

            foreach (var dll in Directory.GetFiles(path, "*.dll"))
            {
                assemblies.Add(Assembly.LoadFile(dll));
            }

            return assemblies;
        }

        static private IEnumerable<Type> GetModules(IEnumerable<Assembly> assemblies, params Type[] types)
        {
            var moduleInstances = new List<Type>();

            foreach (var assembly in assemblies)
            {
                moduleInstances.Concat(
                    assembly.GetTypes().Where(t => types.Contains(t.BaseType))
                    .Select(t => Activator.CreateInstance(t))
                );
            }

            return moduleInstances;
        }

        private interface IModule { };
        private class NinjectModule { };
    }
}