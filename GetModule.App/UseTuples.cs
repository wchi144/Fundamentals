using System;

namespace GetModule.App
{
    public class UseTuples
    {
        private void printSomeTuple(string name, int age)
        {
            var subjectNameAndAge = (Name: name, Age: age);
            Console.WriteLine($"{subjectNameAndAge.Name} is {subjectNameAndAge.Age} years old.");
        }
    }
}
