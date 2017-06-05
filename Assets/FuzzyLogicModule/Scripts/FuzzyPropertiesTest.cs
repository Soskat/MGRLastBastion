using FuzzyLogicEngine.FuzzyValues;
using FuzzyLogicEngine.Rules;
using FuzzyLogicEngine.Variables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyPropertiesTest : MonoBehaviour {


    public Rule rule = new Rule(new FuzzyValue(VariableName.HR, VariableValue.Medium, 0f),
                                RuleOperator.AND,
                                new FuzzyValue(VariableName.GSR, VariableValue.Mid_High, 0f),
                                new FuzzyValue(VariableName.Arousal, VariableValue.Mid_High, 0f));

    public Rule[] rules;
    //public FuzzyValue fuzzyValue = new FuzzyValue(VariableName.Arousal, VariableValue.Mid_High, 123455f);
    //public FuzzyValue fuzzyValue2;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
