using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;

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
            printAllAssemblies(list);

            
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
    }
}