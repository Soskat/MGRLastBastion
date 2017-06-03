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

        // constructors:
        public LinguisticVariable(VariableName name)
        {
            this.name = name;
        }


        public List<FuzzyValue> Fuzzify(float x)
        {
            return null;
        }
    }
}
