﻿using FuzzyLogicEngine.FuzzyValues;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FuzzyLogicEngine.Rules
{
    [Serializable]
    public class Rule
    {
        [SerializeField]
        private FuzzyValue condition1;
        [SerializeField]
        private RuleOperator ruleOper;
        [SerializeField]
        private FuzzyValue condition2;
        [SerializeField]
        private FuzzyValue conclusion;



        public FuzzyValue Condition1 { get { return condition1; } }
        public RuleOperator RuleOper { get { return ruleOper; } }
        public FuzzyValue Condition2 { get { return condition2; } }
        public FuzzyValue Conclusion { get { return conclusion; } }


        // constructors:
        public Rule () { }

        public Rule(FuzzyValue condition1, FuzzyValue result) : this(condition1, RuleOperator.NONE, null, result)
        { }

        public Rule(FuzzyValue condition1, RuleOperator ruleOper, FuzzyValue condition2, FuzzyValue result)
        {
            this.condition1 = condition1;
            this.ruleOper = ruleOper;
            this.condition2 = condition2;
            this.conclusion = result;
        }

        // methods:
        public FuzzyValue Conclude(List<FuzzyValue> inputValues)
        {
            FuzzyValue cond1 = inputValues.Find(f => (f.LinguisticVariable == condition1.LinguisticVariable && f.LinguisticValue == condition1.LinguisticValue));
            if (cond1 == null) return null;

            float result = cond1.MembershipValue;

            if (condition2 != null && ruleOper != RuleOperator.NONE)
            {
                FuzzyValue cond2 = inputValues.Find(f => (f.LinguisticVariable == condition2.LinguisticVariable && f.LinguisticValue == condition2.LinguisticValue));
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

            return new FuzzyValue(conclusion.LinguisticVariable, conclusion.LinguisticValue, result);
        }
    }
}
