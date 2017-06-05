using FuzzyLogicEngine.FuzzyValues;
using FuzzyLogicEngine.Variables;
using System;
using UnityEngine;

namespace FuzzyLogicEngine.MembershipFunctions
{
    [Serializable]
    public class TrapezoidMembershipFunction : BaseMembershipFunction
    {
        [SerializeField]
        private float a;
        [SerializeField]
        private float b;
        [SerializeField]
        private float c;
        [SerializeField]
        private float d;
        [SerializeField]
        private float centerOfHeight;

        public float A { get { return a; } }
        public float B { get { return b; } }
        public float C { get { return c; } }
        public float D { get { return d; } }
        public float CenterOfHeight { get { return centerOfHeight; } }



        // constructors:
        public TrapezoidMembershipFunction(VariableName name, VariableValue value,
                                           float a, float b, float c, float d)
            : this(name, value, a, b, c, d, 0f, 1f, 0f)
        { }
        
        public TrapezoidMembershipFunction(VariableName name, VariableValue value,
                                           float a, float b, float c, float d,
                                           float preValue, float midValue, float postValue)
            : base(name, value, preValue, midValue, postValue)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;

            // calculate function's center of height:
            if (this.b == this.c)
            {
                if (this.c != this.d) centerOfHeight = this.b;                      // triangular shape
                else if (base.PreValue < base.MidValue) centerOfHeight = this.b;    // linear .../''' shape
                else if (base.PreValue > base.MidValue) centerOfHeight = this.a;    // linear '''\... shape
            }
            else centerOfHeight = (this.c - this.b) / 2f + this.b;                           // trapezoid shape
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
                // function shape: .../'''\...
                if (base.PreValue < base.MidValue)
                {
                    outputValue = (inputValue - a) / (b - a);
                }
                // function shape: '''\.../'''
                else
                {
                    outputValue = (b - inputValue) / (b - a);
                }
            }
            else if (inputValue > c)
            {
                // function shape: .../'''\...
                if (base.PreValue < base.MidValue)
                {
                    outputValue = (d - inputValue) / (d - c);
                }
                // function shape: '''\.../'''
                else
                {
                    outputValue = (inputValue - c) / (d - c);
                }
            }

            return new FuzzyValue(base.Name, base.Value, outputValue);
        }
    }
}
