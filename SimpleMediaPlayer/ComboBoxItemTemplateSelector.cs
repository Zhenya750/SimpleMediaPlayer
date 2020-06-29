using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SimpleMediaPlayer
{
    public class ComboBoxItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SelectedItemTemplate { get; set; }
        public DataTemplate ItemTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            bool selected = false;

            // container is a contenrPresenter
            FrameworkElement fe = container as FrameworkElement;
            if (fe != null)
            {
                DependencyObject parent = fe.TemplatedParent;
                if (parent != null)
                {
                    var combobox = parent as ComboBox;
                    if (combobox != null)
                    {
                        selected = true;
                    }
                }
            }

            return selected ? SelectedItemTemplate : ItemTemplate;
        }
    }
}
