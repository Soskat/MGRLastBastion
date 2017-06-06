using FuzzyLogicEngine.FuzzyValues;
using FuzzyLogicEngine.Rules;
using FuzzyLogicEngine.Variables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuzzyPropertiesTest : MonoBehaviour {


    public Rule rule = new Rule(new FuzzyValueType(VariableName.HR, VariableValue.Medium),
                                RuleOperator.AND,
                                new FuzzyValueType(VariableName.GSR, VariableValue.Mid_High),
                                new FuzzyValueType(VariableName.Arousal, VariableValue.Mid_High));

    public Rule[] rules;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
