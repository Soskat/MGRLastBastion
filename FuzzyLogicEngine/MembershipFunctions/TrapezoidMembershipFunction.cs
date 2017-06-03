using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuzzyLogicEngine.FuzzyValues;
using FuzzyLogicEngine.Variables;

namespace FuzzyLogicEngine.MembershipFunctions
{
    class TrapezoidMembershipFunction : BaseMembershipFunction
    {
        private float a;
        private float b;
        private float c;
        private float d;

        public float A { get { return a; } }
        public float B { get { return b; } }
        public float C { get { return c; } }
        public float D { get { return d; } }



        // constructors:
        public TrapezoidMembershipFunction(VariableName name, VariableValue value,
                                           float a, float b, float c, float d)
            : base(name, value)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }
        
        public TrapezoidMembershipFunction(VariableName name, VariableValue value,
                                           float a, float b, float c, float d,
                                           float preValue, float midValue, float postValue)
            : base(name, value, preValue, midValue, postValue)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }


        // calculate the function's membership value
        public override FuzzyValue GetMembershipValue(float inputValue)
        {
            throw new NotImplementedException();
        }
    }
}
