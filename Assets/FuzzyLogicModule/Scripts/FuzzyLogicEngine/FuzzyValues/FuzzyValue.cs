using FuzzyLogicEngine.Variables;

namespace FuzzyLogicEngine.FuzzyValues
{
    public class FuzzyValue
    {
        private VariableName linguisticVariable;
        private VariableValue linguisticValue;
        private float membershipValue;
        
        public VariableName LinguisticVariable { get { return linguisticVariable; } }
        public VariableValue LinguisticValue { get { return linguisticValue; } }
        public float MembershipValue
        {
            get { return membershipValue; }
            set { membershipValue = value; }
        }


        // constructors:
        public FuzzyValue(VariableName linguisticVariable, VariableValue linguisticValue)
        {
            this.linguisticVariable = linguisticVariable;
            this.linguisticValue = linguisticValue;
            this.membershipValue = 0f;
        }

        public FuzzyValue(VariableName linguisticVariable, VariableValue linguisticValue, float membershipValue)
        {
            this.linguisticVariable = linguisticVariable;
            this.linguisticValue = linguisticValue;
            this.membershipValue = membershipValue;
        }


        // methods:
        public override string ToString()
        {
            return linguisticVariable + " - " + linguisticValue + " - " + membershipValue;
        }
    }
}
