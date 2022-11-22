using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace gApp2
{
    /// <summary>
    /// Interaktionslogik für NewItemWindowxaml.xaml
    /// </summary>
    public partial class NewItemWindow : Window
    {
        // needed for multiple inputs
        public bool doNext = false;

        // focus name
        public NewItemWindow()
        {
            InitializeComponent();
            name.Focus();
        }

        private void CloseOnEscape(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.DialogResult = false;
        }

        private void click_done(object sender, RoutedEventArgs e)
        {
            doNext = false;
            DialogResult = true;
        }

        private void click_next(object sender, RoutedEventArgs e)
        {
            doNext = true;
            DialogResult = true;
        }

        private void contentControl(object sender, TextChangedEventArgs e)
        {
            float parseTemp;
            if (name.Text.Length > 0 && float.TryParse(value.Text, out parseTemp))
            {
                done.IsEnabled = true;
                next.IsEnabled = true;
            }
            else
            {
                done.IsEnabled = false;
                next.IsEnabled = false;
            }
        }
    }
}
