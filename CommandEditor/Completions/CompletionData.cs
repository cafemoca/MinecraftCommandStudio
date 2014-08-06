using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Windows.Media;

namespace Cafemoca.CommandEditor.Completions
{
    public class CompletionData : ICompletionData
    {
        public ImageSource Image { get; private set; }
        public string Text { get; private set; }
        public object Content { get; private set; }
        public object Description { get; private set; }
        public double Priority { get; private set; }

        private readonly string _completion;

        public CompletionData(string text, string desc)
            : this(null, text, text, desc, 0.0) { }

        public CompletionData(string text, string completion, string desc)
            : this(null, text, completion, desc, 0.0) { }

        public CompletionData(string text, string completion, string desc, double priority)
            : this(null, text, completion, desc, priority) { }

        public CompletionData(ImageSource image, string text, string completion, string desc, double priority)
        {
            _completion = completion;
            this.Image = image;
            this.Text = text;
            this.Content = text;
            this.Description = desc;
            this.Priority = priority;
        }

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            ((TextArea)textArea).Document.Replace(completionSegment, _completion);
        }
    }
}
