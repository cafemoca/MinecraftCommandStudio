namespace Cafemoca.CommandEditor.Indentations
{
    public class IndentationSettings
    {
        public string IndentString { get; set; }
        public bool LeaveEmptyLines { get; set; }

        public IndentationSettings()
            : this("\t", true) { }

        public IndentationSettings(string indentString, bool leaveEmptyLines)
        {
            this.IndentString = indentString;
            this.LeaveEmptyLines = leaveEmptyLines;
        }
    }
}
