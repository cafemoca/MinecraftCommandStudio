﻿using System.Windows;
using System.Windows.Controls;
using Cafemoca.McCommandStudio.ViewModels.Layouts.Documents;
using Cafemoca.McCommandStudio.ViewModels.Layouts.Tools;

namespace Cafemoca.McCommandStudio.Views.Panes
{
    public class PanesTemplateSelector : DataTemplateSelector
    {
        public PanesTemplateSelector()
        {
        }

        public DataTemplate StartPageTemplate { get; set; }
        public DataTemplate DocumentTemplate { get; set; }
        public DataTemplate RecentFilesTemplate { get; set; }
        public DataTemplate CompletionEditorTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is StartPageViewModel)
            {
                return this.StartPageTemplate;
            }
            if (item is DocumentViewModel)
            {
                return this.DocumentTemplate;
            }
            if (item is RecentFilesViewModel)
            {
                return this.RecentFilesTemplate;
            }
            if (item is CompletionEditorViewModel)
            {
                return this.CompletionEditorTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
