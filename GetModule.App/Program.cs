using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using System.Linq;
using Ninject;

namespace GetModules
{
    public class Program
    {
        static void Main(string[] args)
        {
            //printSomeTuple("linda", 26);
            var root = Assembly.GetExecutingAssembly().Location;
            root = Path.GetDirectoryName(root);

            if (!Directory.Exists(root))
            {
                throw new FileNotFoundException($"folder {root} does not exisit");
            }

            var list = GetAllAssemblies(root);
            //printAllAssemblies(list);
            getModules(list);
            getModules(list, IModule, NinjectModule);

            
        }

        static void printSomeTuple(string name, int age)
        {
            var subjectNameAndAge = (Name: name, Age: age);
            Console.WriteLine($"{subjectNameAndAge.Name} is {subjectNameAndAge.Age} years old.");
        }

        static IList<Assembly> GetAllAssemblies(string path)
        {
            var assemblies = new List<Assembly>();

            foreach (var dll in Directory.GetFiles(path, "*.dll"))
            {
                assemblies.Add(Assembly.LoadFile(dll));
            }

            return assemblies;
        }

        static void printAllAssemblies(IList<Assembly> assemblies)
        {

            foreach (var dll in assemblies)
            {
                Debug.WriteLine(dll);
            }
        }

        static private IEnumerable<Type> getModules(IEnumerable<Assembly> assemblies, params Type[] args)
        {
            var moduleInstances = new List<Type>();

            foreach (var assembly in assemblies)
            {
                // from those types get the type where interfact is IModule
                // create instance of those types
                moduleInstances.Concat(
                    assembly.GetTypes().Where(t => args.Contains(t.BaseType))
                    .Select(t => Activator.CreateInstance(t))
                );
            }

            return moduleInstances;
        }

        static private IEnumerable<Type> getModules(IEnumerable<Assembly> assemblies)
        {
            var moduleInstances = new List<Type>();

            foreach (var assembly in assemblies)
            {
                // from those types get the type where interfact is IModule
                // create instance of those types
                moduleInstances.Concat(
                    assembly.GetTypes().Where(t => t.BaseType == typeof(NinjectModule))
                    .Select(t => Activator.CreateInstance(t))
                );
            }

            return moduleInstances;
        }

        private interface IModule { };
        private class NinjectModule { };
    }
}