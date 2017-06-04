using FuzzyLogicEngine.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogicEngine.FuzzyValues
{
    public class FuzzyValue
    {
        private VariableName linguisticVariable;
        private VariableValue linguisticValue;
        private float membershipValue;
        
        public VariableName LinguisticVariable { get { return linguisticVariable; } }
        public VariableValue LinguisticValue { get { return linguisticValue; } }
        public float MembershipValue { get { return membershipValue; } }


        public FuzzyValue(VariableName linguisticVariable, VariableValue linguisticValue, float membershipValue)
        {
            this.linguisticVariable = linguisticVariable;
            this.linguisticValue = linguisticValue;
            this.membershipValue = membershipValue;
        }

        public override string ToString()
        {
            return linguisticVariable + " - " + linguisticValue + " - " + membershipValue;
        }
    }
}
