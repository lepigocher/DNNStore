using System;

namespace DotNetNuke.Modules.Store.Components
{
    public class EvalStatementEventArgs : EventArgs
    {
        public readonly string Keyword;
        public readonly string Condition;
        public bool IsValid;
        public bool ParseContent;
        public readonly string Content;
        public string Value;

        public EvalStatementEventArgs(string keyword, string condition, string content)
        {
            Keyword = keyword;
            Condition = condition;
            Content = content;
            IsValid = false;
            ParseContent = true;
        }
    }
}