using System;
using System.Collections.Generic;


namespace LastBastion.Game.Plot
{
    /// <summary>
    /// Class that represents the text of the introduction story.
    /// </summary>
    [Serializable]
    public class IntroText
    {
        #region Public fields
        /// <summary>The content of the introduction text.</summary>
        public List<IntroLine> Content;
        #endregion


        #region Constructors
        /// <summary>
        /// Creates an instance of <see cref="IntroText"/> class.
        /// </summary>
        public IntroText() : this(new List<IntroLine>()) { }

        /// <summary>
        /// Creates an instance of <see cref="IntroText"/> class.
        /// </summary>
        /// <param name="content">The following lines of the introduction text</param>
        public IntroText(List<IntroLine> content)
        {
            Content = content;
        }
        #endregion
    }
}
