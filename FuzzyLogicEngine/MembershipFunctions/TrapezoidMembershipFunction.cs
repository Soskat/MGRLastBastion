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
            float outputValue = 0f;
            if (inputValue <= a) outputValue = base.PreValue;
            else if (inputValue >= d) outputValue = base.PostValue;
            else if (inputValue >= b && inputValue <= c) outputValue = base.MidValue;
            else if (inputValue < b)
            {
                // function values: 0 - 1 - 0
                if (base.PreValue < base.MidValue)
                {
                    outputValue = (inputValue - a) / (b - a);
                }
                // function values: 1 - 0 - 1
                else
                {
                    outputValue = (b - inputValue) / (b - a);
                }
            }
            else if (inputValue > c)
            {
                // function values: 0 - 1 - 0
                if (base.PreValue < base.MidValue)
                {
                    outputValue = (d - inputValue) / (d - c);
                }
                // function values: 1 - 0 - 1
                else
                {
                    outputValue = (inputValue - c) / (d - c);
                }
            }

            return new FuzzyValue(base.Name, base.Value, outputValue);
        }
    }
}
