using FuzzyLogicEngine.FuzzyValues;
using FuzzyLogicEngine.Rules;
using FuzzyLogicEngine.Variables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyEngine : MonoBehaviour {

    [SerializeField]
    private List<LinguisticVariable> inputVariables;
    [SerializeField]
    private List<LinguisticVariable> outputVariables;
    [SerializeField]
    private RuleSet ruleSet;




    // Use this for initialization
    void Start () {
        inputVariables = new List<LinguisticVariable>();
        outputVariables = new List<LinguisticVariable>();
        ruleSet = CreateRuleSet();
    }
	
	// Update is called once per frame
	void Update () {
		
	}



    public static RuleSet CreateRuleSet()
    {
        RuleSet rules = new RuleSet();

        // gsr:
        var gsr_low = new FuzzyValue(VariableName.GSR, VariableValue.Low);
        var gsr_mid_low = new FuzzyValue(VariableName.GSR, VariableValue.Mid_Low);
        var gsr_mid_high = new FuzzyValue(VariableName.GSR, VariableValue.Mid_High);
        var gsr_high = new FuzzyValue(VariableName.GSR, VariableValue.High);
        // hr:
        var hr_low = new FuzzyValue(VariableName.HR, VariableValue.Low);
        var hr_medium = new FuzzyValue(VariableName.HR, VariableValue.Medium);
        var hr_high = new FuzzyValue(VariableName.HR, VariableValue.High);
        // arousal:
        var arousal_low = new FuzzyValue(VariableName.Arousal, VariableValue.Low);
        var arousal_mid_low = new FuzzyValue(VariableName.Arousal, VariableValue.Mid_Low);
        var arousal_mid_high = new FuzzyValue(VariableName.Arousal, VariableValue.Mid_High);
        var arousal_high = new FuzzyValue(VariableName.Arousal, VariableValue.High);

        // add rules:
        rules.AddRule(new Rule(gsr_high, arousal_high));
        rules.AddRule(new Rule(gsr_mid_high, arousal_mid_high));
        rules.AddRule(new Rule(gsr_mid_low, arousal_mid_low));
        rules.AddRule(new Rule(gsr_low, arousal_low));
        rules.AddRule(new Rule(hr_low, arousal_low));
        rules.AddRule(new Rule(hr_high, arousal_high));
        rules.AddRule(new Rule(gsr_low, RuleOperator.AND, hr_high, arousal_mid_low));
        rules.AddRule(new Rule(gsr_high, RuleOperator.AND, hr_low, arousal_mid_high));
        rules.AddRule(new Rule(gsr_high, RuleOperator.AND, hr_medium, arousal_high));
        rules.AddRule(new Rule(gsr_mid_high, RuleOperator.AND, hr_medium, arousal_mid_high));
        rules.AddRule(new Rule(gsr_mid_low, RuleOperator.AND, hr_medium, arousal_mid_low));

        return rules;
    }
}
