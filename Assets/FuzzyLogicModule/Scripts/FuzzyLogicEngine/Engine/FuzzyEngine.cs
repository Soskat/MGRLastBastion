using FuzzyLogicEngine.FuzzyValues;
using FuzzyLogicEngine.Rules;
using FuzzyLogicEngine.Variables;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FuzzyEngine : MonoBehaviour {

    //[SerializeField]
    //private List<LinguisticVariable> inputVariables;
    //[SerializeField]
    //private List<LinguisticVariable> outputVariables;


    [SerializeField]
    private List<Rule> rules;


    // Use this for initialization
    void Start () {
        //    inputVariables = new List<LinguisticVariable>();
        //    outputVariables = new List<LinguisticVariable>();
        //    ruleSet = CreateRuleSet();
    }

    // Update is called once per frame
    void Update () {
		
	}



    public static List<Rule> CreateRuleSet()
    {
        List<Rule> rules = new List<Rule>();

        // gsr:
        var gsr_low = new FuzzyValueType(VariableName.GSR, VariableValue.Low);
        var gsr_mid_low = new FuzzyValueType(VariableName.GSR, VariableValue.Mid_Low);
        var gsr_mid_high = new FuzzyValueType(VariableName.GSR, VariableValue.Mid_High);
        var gsr_high = new FuzzyValueType(VariableName.GSR, VariableValue.High);
        // hr:
        var hr_low = new FuzzyValueType(VariableName.HR, VariableValue.Low);
        var hr_medium = new FuzzyValueType(VariableName.HR, VariableValue.Medium);
        var hr_high = new FuzzyValueType(VariableName.HR, VariableValue.High);
        // arousal:
        var arousal_low = new FuzzyValueType(VariableName.Arousal, VariableValue.Low);
        var arousal_mid_low = new FuzzyValueType(VariableName.Arousal, VariableValue.Mid_Low);
        var arousal_mid_high = new FuzzyValueType(VariableName.Arousal, VariableValue.Mid_High);
        var arousal_high = new FuzzyValueType(VariableName.Arousal, VariableValue.High);

        // add rules:
        rules.Add(new Rule(gsr_high, arousal_high));
        rules.Add(new Rule(gsr_mid_high, arousal_mid_high));
        rules.Add(new Rule(gsr_mid_low, arousal_mid_low));
        rules.Add(new Rule(gsr_low, arousal_low));
        rules.Add(new Rule(hr_low, arousal_low));
        rules.Add(new Rule(hr_high, arousal_high));
        rules.Add(new Rule(gsr_low, RuleOperator.AND, hr_high, arousal_mid_low));
        rules.Add(new Rule(gsr_high, RuleOperator.AND, hr_low, arousal_mid_high));
        rules.Add(new Rule(gsr_high, RuleOperator.AND, hr_medium, arousal_high));
        rules.Add(new Rule(gsr_mid_high, RuleOperator.AND, hr_medium, arousal_mid_high));
        rules.Add(new Rule(gsr_mid_low, RuleOperator.AND, hr_medium, arousal_mid_low));

        return rules;
    }



    // infer the output fuzzy values:
    public List<FuzzyValue> Infer(IEnumerable<Rule> rules, List<FuzzyValue> inputValues)
    {
        Dictionary<VariableValue, FuzzyValue> outputValues = new Dictionary<VariableValue, FuzzyValue>();

        // check each rule:
        foreach (Rule rule in rules)
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
                if (outputValues[result.LinguisticValue].MembershipValue < result.MembershipValue)
                {
                    outputValues[result.LinguisticValue].MembershipValue = result.MembershipValue;
                }
            }
        }

        return outputValues.Values.ToList();
    }


}
