using FuzzyLogicEngine.FuzzyValues;
using FuzzyLogicEngine.Variables;

namespace FuzzyLogicEngine.MembershipFunctions
{
    public abstract class BaseMembershipFunction
    {
        private VariableName name;
        private VariableValue value;

        public VariableName Name { get { return name; } }
        public VariableValue Value { get { return value; } }


        // function values areas:
        private float preValue;
        private float midValue;
        private float postValue;

        public float PreValue { get { return preValue; } }
        public float MidValue { get { return midValue; } }
        public float PostValue { get { return postValue; } }

        
        // constructors:
        public BaseMembershipFunction(VariableName name, VariableValue value)
        {
            this.name = name;
            this.value = value;
            preValue = postValue = 0f;
            midValue = 1f;
        }
        
        public BaseMembershipFunction(VariableName name, VariableValue value, float preValue, float midValue, float postValue)
        {
            this.name = name;
            this.value = value;
            this.preValue = preValue;
            this.midValue = midValue;
            this.postValue = postValue;
        }



        public abstract FuzzyValue GetMembershipValue(float inputValue);
    }
}
