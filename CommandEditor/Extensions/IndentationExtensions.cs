using Cafemoca.CommandEditor.Indentations;
using ICSharpCode.AvalonEdit.Indentation;
using System;

namespace Cafemoca.CommandEditor.Extensions
{
    public static class IndentationExtensions
    {
        public static CommandIndentationStrategy AsCommandIndentationStrategy(this IIndentationStrategy strategy)
        {
            if (strategy is CommandIndentationStrategy)
            {
                return (CommandIndentationStrategy)strategy;
            }
            else
            {
                throw new ArgumentException("strategy");
            }
        }
    }
}
