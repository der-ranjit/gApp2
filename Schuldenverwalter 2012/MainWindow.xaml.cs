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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.IO;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using System.Globalization;

namespace gApp2
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        // add event handler
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, args);
            }
        }

        private ObservableCollection<Person> _personList;

        private float totalFieldValue;
        public float TotalFieldValue { get { return this.totalFieldValue; } set { this.totalFieldValue = value; OnPropertyChanged("TotalFieldValue"); } }

        //sets the datacontexts; 
        public MainWindow()
        {
            InitializeComponent();
            // make sure xaml uses correct culture
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            initList();
            personListBox.DataContext = _personList;
            totalField.DataContext = this;
            updateTotalField();
        }

        // checks for list/creates a new one and sets the datacontext of the personListBox to that list
        private void initList()
        {
            if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "list.xml"))
            {
                _personList = loadXML(System.AppDomain.CurrentDomain.BaseDirectory + "list.xml");
            }
            else
            {
                MessageBox.Show("Could not find a list. Creating new one...", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _personList = new ObservableCollection<Person>();
            }
        }
        // opens a new window to enter data for a new person and adds it to the _personList
        private void newPerson()
        {
            NewPersonWindow dialog = new NewPersonWindow();
            // set owner so window can be centered
            dialog.Owner = this;
            dialog.ShowDialog();

            // check wheter entry already exists 
            if (dialog.DialogResult == true)
            {
                bool add = true;
                Person person = new Person(dialog.name.Text);
                foreach (Person p in _personList)
                {
                    if (person.Name == p.Name)
                    {
                        MessageBox.Show("Entry \"" + person.Name + "\" already exists. Aborting.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        add = false;
                        break;
                    }
                }
                // pre-select and scroll into
                if (add)
                {
                    _personList.Add(person);
                    personListBox.SelectedItem = person;
                    personListBox.ScrollIntoView(personListBox.SelectedItem);
                }
            }
        }

        // opens a new window to enter data for a new item and adds it to the currently selected person
        private void newItem()
        {
            bool cont = true;
            while (cont)
            {
                NewItemWindow dialog = new NewItemWindow();
                // set owner for center-window
                dialog.Owner = this;
                dialog.ShowDialog();
                if (dialog.DialogResult == true)
                {
                    Item item = new Item(dialog.name.Text, float.Parse(dialog.value.Text));
                    ((Person)personListBox.SelectedItem).AddItem(item);
                    updateTotalField();
                }
                cont = dialog.doNext;
            }
        }

        // updates the total field
        private void updateTotalField()
        {
            float sum = 0;
            foreach (Person p in _personList)
            {
                sum += p.Total;
            }
            TotalFieldValue = sum;
        }

        private void balancePerson()
        {
            MessageBoxResult result = MessageBox.Show("This will balance this Person. Are you sure?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.OK)
            {
                Person p = (Person)personListBox.SelectedItem;
                p.AddItem(new Item("Balance Total", -p.Total));
                updateTotalField();
            }

        }

        private void balanceItem()
        {
            MessageBoxResult result = MessageBox.Show("This will balance this Item. Are you sure?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.OK)
            {
                Person p = (Person)personListBox.SelectedItem;
                Item i = (Item)itemListBox.SelectedItem;
                p.AddItem(new Item("Balance " + i.Name, -i.Value));
                updateTotalField();
            }

        }

        private void removePerson()
        {
            Person person = (Person)personListBox.SelectedItem;
            MessageBoxResult result = MessageBox.Show("This will delete Person \"" + person.Name + "\". Are you sure?", "Delete \"" + person.Name + "\"", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.OK)
            {
                result = MessageBox.Show("Really?", "Confirm", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                    _personList.Remove(person);
            }
            updateTotalField();
        }

        private void removeItem()
        {
            Person person = (Person)personListBox.SelectedItem;
            Item item = (Item)itemListBox.SelectedItem;
            MessageBoxResult result = MessageBox.Show("This will delete Item \"" + item.Name + "\". Are you sure?", "Delete \"" + item.Name + "\"", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
                person.RemoveItem(item);
            updateTotalField();
        }

        private void saveXML(ObservableCollection<Person> list, string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ObservableCollection<Person>));
            TextWriter textWriter = new StreamWriter(path);
            serializer.Serialize(textWriter, list);
            textWriter.Close();
        }

        private ObservableCollection<Person> loadXML(string path)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(ObservableCollection<Person>));
            TextReader textReader = new StreamReader(path);
            ObservableCollection<Person> persons;
            persons = (ObservableCollection<Person>)deserializer.Deserialize(textReader);
            textReader.Close();
            return persons;
        }

        private void newPerson_Click(object sender, RoutedEventArgs e)
        {
            newPerson();
        }

        private void newItem_Click(object sender, RoutedEventArgs e)
        {
            newItem();
        }

        private void removeItem_Click(object sender, RoutedEventArgs e)
        {
            removeItem();
        }

        private void balancePerson_Click(object sender, RoutedEventArgs e)
        {
            balancePerson();
        }

        private void balanceItem_Click(object sender, RoutedEventArgs e)
        {
            balanceItem();
        }

        private void removePerson_Click(object sender, RoutedEventArgs e)
        {
            removePerson();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            saveXML(_personList, System.AppDomain.CurrentDomain.BaseDirectory + "list.xml");
        }

        /// <summary>
        /// Sorts the person List 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void personSorting_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (personListBox != null)
            {
                CollectionViewSource sortedItems = personListBox.FindResource("SortedPersons") as CollectionViewSource;
                if (sortedItems != null)
                {
                    switch (rb.Content.ToString())
                    {
                        case "Total":
                            sortedItems.SortDescriptions.Clear();
                            sortedItems.SortDescriptions.Add(new SortDescription("Total", ListSortDirection.Descending));
                            break;

                        case "Name":
                            sortedItems.SortDescriptions.Clear();
                            sortedItems.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                            break;

                        case "Date":
                            sortedItems.SortDescriptions.Clear();
                            sortedItems.SortDescriptions.Add(new SortDescription("LastChanged", ListSortDirection.Descending));
                            break;

                        case "Items":
                            sortedItems.SortDescriptions.Clear();
                            sortedItems.SortDescriptions.Add(new SortDescription("ItemCount", ListSortDirection.Descending));
                            break;

                        default:
                            sortedItems.SortDescriptions.Clear();
                            sortedItems.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                            break;
                    }
                    personListBox.ScrollIntoView(personListBox.Items[0]);
                }
            }
        }

        /// <summary>
        /// Sorts the items List 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemSorting_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (itemListBox != null)
            {
                CollectionViewSource sortedItems = itemListBox.FindResource("SortedItems") as CollectionViewSource;
                if (sortedItems != null)
                {
                    switch (rb.Content.ToString())
                    {
                        case "Value":
                            sortedItems.SortDescriptions.Clear();
                            sortedItems.SortDescriptions.Add(new SortDescription("Value", ListSortDirection.Descending));
                            break;

                        case "Name":
                            sortedItems.SortDescriptions.Clear();
                            sortedItems.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                            break;

                        case "Date":
                            sortedItems.SortDescriptions.Clear();
                            sortedItems.SortDescriptions.Add(new SortDescription("CreationDate", ListSortDirection.Descending));
                            break;

                        default:
                            sortedItems.SortDescriptions.Clear();
                            sortedItems.SortDescriptions.Add(new SortDescription("Value", ListSortDirection.Ascending));
                            break;
                    }
                    itemListBox.ScrollIntoView(personListBox.Items[0]);
                }
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Delete)
            {
                // ctrl+delete = remove person
                if (Keyboard.Modifiers == ModifierKeys.Control && personListBox.SelectedItem != null)
                    removePerson();
                // del = remove item
                else if (Keyboard.Modifiers == ModifierKeys.None && itemListBox.SelectedItem != null)
                    removeItem();
            }

            if (e.Key == Key.Add)
            {
                // ctrl+add = new person
                if (Keyboard.Modifiers == ModifierKeys.Control)
                    newPerson();
                // add = new item
                else if (Keyboard.Modifiers == ModifierKeys.None && personListBox.SelectedItem != null)
                    newItem();
            }


            if (e.Key == Key.Space)
            {
                //balance person
                if (Keyboard.Modifiers == ModifierKeys.Control && personListBox.SelectedItem != null)
                    balancePerson();
                //balance item
                else if (Keyboard.Modifiers == ModifierKeys.None && itemListBox.SelectedItem != null)
                    balanceItem();
            }
        }

    }
}
