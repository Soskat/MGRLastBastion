using FuzzyLogicEngine.FuzzyValues;
using FuzzyLogicEngine.Variables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;


namespace FuzzyLogicEngine.Rules
{
    [Serializable]
    public class RuleSet
    {
        [SerializeField]
        private List<Rule> rules;


        // constructors:
        public RuleSet()
        {
            rules = new List<Rule>();
        }


        // methods
        public void AddRule(Rule rule)
        {
            rules.Add(rule);
        }


        // infer the output fuzzy values:
        public List<FuzzyValue> Infer(List<FuzzyValue> inputValues)
        {
            Dictionary<VariableValue, FuzzyValue> outputValues = new Dictionary<VariableValue, FuzzyValue>();

            // check each rule:
            foreach(Rule rule in rules)
            {
                FuzzyValue result = rule.Conclude(inputValues);
                if (result == null) continue;

                // there's no such conclusion in output set yet:
                if (!outputValues.ContainsKey(result.LinguisticValue))
                {
                    outputValues.Add(result.LinguisticValue, result);
                }
                else
                {
                    // get the max membershipValue:
                    if(outputValues[result.LinguisticValue].MembershipValue < result.MembershipValue)
                    {
                        outputValues[result.LinguisticValue].MembershipValue = result.MembershipValue;
                    }
                }
            }

            return outputValues.Values.ToList();
        }
    }
}
