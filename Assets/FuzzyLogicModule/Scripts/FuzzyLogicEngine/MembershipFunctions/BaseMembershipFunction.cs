using FuzzyLogicEngine.FuzzyValues;
using FuzzyLogicEngine.Variables;
using System;
using UnityEngine;

namespace FuzzyLogicEngine.MembershipFunctions
{
    [Serializable]
    public abstract class BaseMembershipFunction
    {
        [SerializeField]
        private VariableName name;
        [SerializeField]
        private VariableValue value;

        // function values areas:
        [SerializeField]
        private float preValue;
        [SerializeField]
        private float midValue;
        [SerializeField]
        private float postValue;



        public VariableName Name { get { return name; } }
        public VariableValue Value { get { return value; } }
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
