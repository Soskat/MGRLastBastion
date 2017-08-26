using System;


namespace LastBastion.Game.SurveySystem
{
    /// <summary>
    /// Represents question type.
    /// </summary>
    [Serializable]
    public enum QuestionType : short
    {
        Sex,            // Male | Female
        Age,            // 18 < | 18 - 25 | > 25
        PlayRoutine,    // I dont' play | few times per month | few times per week | evertyday
        TrueFalse,      // True | False
        Scale,          // 1 | 2 | 3 | 4 | 5
        AorB,           // LevelA | LevelB
        Open            // open question
    }
}
