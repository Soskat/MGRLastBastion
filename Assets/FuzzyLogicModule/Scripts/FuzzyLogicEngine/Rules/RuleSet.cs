using FuzzyLogicEngine.FuzzyValues;
using FuzzyLogicEngine.Variables;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FuzzyLogicEngine.Rules
{
    class RuleSet : Collection<Rule>
    {
        //private Dictionary<VariableName, Dictionary<VariableValue, Rule>> rules;


        //// constructors:
        //public RuleSet()
        //{
        //    rules = new Dictionary<VariableName, Dictionary<VariableValue, Rule>>();
        //}


        // methods
        public void AddRule(Rule rule)
        {
            this.Add(rule);
        }


        // infer the output fuzzy values:
        public List<FuzzyValue> Infer(List<FuzzyValue> inputValues)
        {
            Dictionary<VariableValue, FuzzyValue> outputValues = new Dictionary<VariableValue, FuzzyValue>();

            // check each rule:
            foreach(Rule rule in this)
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
