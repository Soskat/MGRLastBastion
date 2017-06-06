using FuzzyLogicEngine.Variables;
using System;

namespace FuzzyLogicEngine.FuzzyValues
{
    [Serializable]
    public struct FuzzyValueType
    {
        public static FuzzyValueType Default = new FuzzyValueType(VariableName.None, VariableValue.None);


        public VariableName Type;
        public VariableValue Value;

        public FuzzyValueType(VariableName type, VariableValue value)
        {
            Type = type;
            Value = value;
        }
    }    
}
