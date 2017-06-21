using FuzzyLogicEngine.FuzzyValues;
using FuzzyLogicEngine.MembershipFunctions;
using FuzzyLogicEngine.Rules;
using FuzzyLogicEngine.Variables;
using System;
using System.Collections.Generic;

namespace FuzzyLogicEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            RuleSet rules = Program.CreateRuleSet();

            // input variable - HR
            LinguisticVariable HRvariable = new LinguisticVariable(VariableName.HR);
            HRvariable.AddFunction(new TrapezoidMembershipFunction(HRvariable.Name, VariableValue.Low, 10f, 20f, 30f, 40f));
            HRvariable.AddFunction(new TriangleMembershipFunction(HRvariable.Name, VariableValue.Medium, 30f, 40f, 50f));
            HRvariable.AddFunction(new LinearMembershipFunction(HRvariable.Name, VariableValue.High, 40f, 50f));
            // input variable - GSR
            LinguisticVariable GSRvariable = new LinguisticVariable(VariableName.GSR);
            GSRvariable.AddFunction(new LinearMembershipFunction(GSRvariable.Name, VariableValue.Low, 0f, 10f, 1f, 0f));
            GSRvariable.AddFunction(new TrapezoidMembershipFunction(GSRvariable.Name, VariableValue.Mid_Low, 0f, 10f, 20f, 30f));
            GSRvariable.AddFunction(new TrapezoidMembershipFunction(GSRvariable.Name, VariableValue.Mid_High, 20f, 30f, 40f, 50f));
            GSRvariable.AddFunction(new TriangleMembershipFunction(GSRvariable.Name, VariableValue.High, 40f, 50f, 60f));

            // output variable - Arousal
            LinguisticVariable ArousalVariable = new LinguisticVariable(VariableName.Arousal);
            ArousalVariable.AddFunction(new TriangleMembershipFunction(ArousalVariable.Name, VariableValue.Low, 0f, 15f, 30f));
            ArousalVariable.AddFunction(new TriangleMembershipFunction(ArousalVariable.Name, VariableValue.Mid_Low, 20f, 30f, 40f));
            ArousalVariable.AddFunction(new TriangleMembershipFunction(ArousalVariable.Name, VariableValue.Mid_High, 30f, 40f, 50f));
            ArousalVariable.AddFunction(new TriangleMembershipFunction(ArousalVariable.Name, VariableValue.High, 40f, 50f, 60f));


            float inputValue = 35f;

            Console.WriteLine(">> Fuzzify input crisp value = {0}:", inputValue);
            List<FuzzyValue> fuzzyValues = new List<FuzzyValue>();
            fuzzyValues.AddRange(HRvariable.Fuzzify(inputValue));
            fuzzyValues.AddRange(GSRvariable.Fuzzify(inputValue));
            foreach (var fuzzyNo in fuzzyValues)
            {
                Console.WriteLine(fuzzyNo);
            }


            Console.WriteLine("\n>> Infer conclusions:");
            List<FuzzyValue> conclusions = rules.Infer(fuzzyValues);
            foreach (var conclusion in conclusions)
            {
                Console.WriteLine(conclusion);
            }


            Console.WriteLine("\n>> Deffuzify fuzzy values:");
            float arousal = ArousalVariable.Deffuzify(conclusions);
            Console.WriteLine("Crisp arousal value = {0}", arousal);

            Console.ReadKey();
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
    }
}
