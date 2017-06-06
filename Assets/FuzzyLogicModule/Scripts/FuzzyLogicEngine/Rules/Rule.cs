using FuzzyLogicEngine.FuzzyValues;
using FuzzyLogicEngine.Variables;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FuzzyLogicEngine.Rules
{
    [Serializable]
    public class Rule
    {
        [SerializeField]
        private FuzzyValueType condition1;
        [SerializeField]
        private RuleOperator ruleOper;
        [SerializeField]
        private FuzzyValueType condition2;
        [SerializeField]
        private FuzzyValueType conclusion;



        public FuzzyValueType Condition1 { get { return condition1; } }
        public RuleOperator RuleOper { get { return ruleOper; } }
        public FuzzyValueType Condition2 { get { return condition2; } }
        public FuzzyValueType Conclusion { get { return conclusion; } }


        // constructors:
        public Rule () { }

        public Rule(FuzzyValueType condition1, FuzzyValueType result) : this(condition1, RuleOperator.NONE, FuzzyValueType.Default, result)
        { }

        public Rule(FuzzyValueType condition1, RuleOperator ruleOper, FuzzyValueType condition2, FuzzyValueType result)
        {
            this.condition1 = condition1;
            this.ruleOper = ruleOper;
            this.condition2 = condition2;
            this.conclusion = result;
        }

        // methods:
        public FuzzyValue Conclude(List<FuzzyValue> inputValues)
        {
            FuzzyValue cond1 = inputValues.Find(f => (f.LinguisticVariable == condition1.Type && f.LinguisticValue == condition1.Value));
            if (cond1 == null) return null;

            float result = cond1.MembershipValue;

            if (condition2.Type != VariableName.None && ruleOper != RuleOperator.NONE)
            {
                FuzzyValue cond2 = inputValues.Find(f => (f.LinguisticVariable == condition2.Type && f.LinguisticValue == condition2.Value));
                if (cond2 == null) return null;

                float condValue2 = cond2.MembershipValue;

                if(ruleOper == RuleOperator.AND)
                {
                    result = (condValue2 < result) ? condValue2 : result;
                }
                else if(ruleOper == RuleOperator.OR)
                {
                    result = (condValue2 > result) ? condValue2 : result;
                }
            }

            return new FuzzyValue(conclusion.Type, conclusion.Value, result);
        }
    }
}
