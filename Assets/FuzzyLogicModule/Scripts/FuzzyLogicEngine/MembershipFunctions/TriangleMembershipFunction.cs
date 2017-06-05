using FuzzyLogicEngine.Variables;

namespace FuzzyLogicEngine.MembershipFunctions
{
    class TriangleMembershipFunction : TrapezoidMembershipFunction
    {
        // constructors:
        public TriangleMembershipFunction(VariableName name, VariableValue value,
                                          float a, float b, float c)
            : base(name, value, a, b, b, c)
        {
        }

        public TriangleMembershipFunction(VariableName name, VariableValue value,
                                          float a, float b, float c, float preValue, float midValue, float postValue)
            : base(name, value, a, b, b, c, preValue, midValue, postValue)
        {
        }
    }
}
