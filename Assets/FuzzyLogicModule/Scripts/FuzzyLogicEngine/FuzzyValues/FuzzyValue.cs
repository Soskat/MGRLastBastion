using FuzzyLogicEngine.Variables;
using System;
using UnityEngine;

namespace FuzzyLogicEngine.FuzzyValues
{
    [Serializable]
    public class FuzzyValue
    {
        [SerializeField]
        private VariableName linguisticVariable;
        [SerializeField]
        private VariableValue linguisticValue;
        [SerializeField]
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
