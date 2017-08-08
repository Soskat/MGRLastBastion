using System;


namespace LastBastion.Game.Plot
{
    /// <summary>
    /// Class that represents one line in text of introduction story.
    /// </summary>
    [Serializable]
    public class IntroLine
    {
        #region Public fields
        /// <summary>The cooldown time before displaying the line on the screen.</summary>
        public float Cooldown;
        /// <summary>The duration of the line on the screen.</summary>
        public float Duration;
        /// <summary>The text of the line.</summary>
        public string Text;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates an instance of <see cref="IntroLine"/> class.
        /// </summary>
        public IntroLine() : this(1.0f, 1.0f, "<empty line>") { }

        /// <summary>
        /// Creates an instance of <see cref="IntroLine"/> class.
        /// </summary>
        /// <param name="cooldown">cooldown time before displaying the line on the screen</param>
        /// <param name="duration">duration of the line on the screen</param>
        /// <param name="text">text of the line</param>
        public IntroLine(float cooldown, float duration, string text)
        {
            Cooldown = cooldown;
            Duration = duration;
            Text = text;
        }
        #endregion
    }
}
