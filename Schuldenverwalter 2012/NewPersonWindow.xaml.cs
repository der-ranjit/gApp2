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
    /// Interaktionslogik für NewPersonWindow.xaml
    /// </summary>
    public partial class NewPersonWindow : Window
    {
        // focus name field
        public NewPersonWindow()
        {
            InitializeComponent();
            name.Focus();
        }

        // closing window with escape buttom
        private void CloseOnEscape(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.DialogResult = false;
        }

        private void click_done(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        
        // text must contain content
        private void contentCheck(object sender, TextChangedEventArgs e)
        {
            if (name.Text.Length > 0)
                done.IsEnabled = true;
            else
                done.IsEnabled = false;
        }
    }
}
