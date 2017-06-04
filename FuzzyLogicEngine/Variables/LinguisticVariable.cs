using FuzzyLogicEngine.FuzzyValues;
using FuzzyLogicEngine.MembershipFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        // do fuzzify on given input crisp value
        public List<FuzzyValue> Fuzzify(float inputValue)
        {
            List<FuzzyValue> fuzzyNumbers = new List<FuzzyValue>();

            foreach(var func in functions)
            {
                fuzzyNumbers.Add(func.GetMembershipValue(inputValue));
            }

            return fuzzyNumbers;
        }
    }
}
