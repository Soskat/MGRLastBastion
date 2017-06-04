using FuzzyLogicEngine.FuzzyValues;
using FuzzyLogicEngine.MembershipFunctions;
using FuzzyLogicEngine.Variables;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogicEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            LinguisticVariable HRvariable = new LinguisticVariable(VariableName.HR);
            HRvariable.AddFunction(new TrapezoidMembershipFunction(HRvariable.Name, VariableValue.Low, 10f, 20f, 30f, 40f));
            HRvariable.AddFunction(new TriangleMembershipFunction(HRvariable.Name, VariableValue.Medium, 30f, 40f, 50f));
            HRvariable.AddFunction(new LinearMembershipFunction(HRvariable.Name, VariableValue.High, 40f, 50f));

            float[] inputValuesArray = { 15f, 20f, 35f, 40f, 49f, 51f };
            foreach(var inputValue in inputValuesArray)
            {
                Console.WriteLine("\n\n>> Test for input value = " + inputValue);
                List<FuzzyValue> output = HRvariable.Fuzzify(inputValue);
                foreach(var fuzzyNo in output)
                {
                    Console.WriteLine(fuzzyNo);
                }
            }

            Console.ReadKey();
        }
    }
}
