using FuzzyLogicEngine.FuzzyValues;
using FuzzyLogicEngine.MembershipFunctions;
using System.Collections.Generic;

namespace FuzzyLogicEngine.Variables
{
    class LinguisticVariable
    {
        private VariableName name;
        private List<BaseMembershipFunction> functions;

        public VariableName Name { get { return name; } }

        // constructors:
        public LinguisticVariable(VariableName name)
        {
            this.name = name;
            functions = new List<BaseMembershipFunction>();
        }


        public void AddFunction(BaseMembershipFunction func)
        {
            functions.Add(func);
        }


        // do fuzzification on given input crisp value
        public List<FuzzyValue> Fuzzify(float inputValue)
        {
            List<FuzzyValue> fuzzyNumbers = new List<FuzzyValue>();

            foreach(var func in functions)
            {
                fuzzyNumbers.Add(func.GetMembershipValue(inputValue));
            }

            return fuzzyNumbers;
        }

        // do deffuziffication on given input fuzzy values using the weighted average method:
        public float Deffuzify(List<FuzzyValue> fuzzyValues)
        {
            float numerator = 0f;
            float denominator = 0f;

            foreach(FuzzyValue value in fuzzyValues)
            {
                numerator += value.MembershipValue * ((TrapezoidMembershipFunction)functions.Find(f => f.Value == value.LinguisticValue)).CenterOfHeight;
                denominator += value.MembershipValue;
            }

            return numerator / denominator;
        }
    }
}
