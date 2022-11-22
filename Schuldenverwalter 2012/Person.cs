using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Collections.Specialized;
using System.ComponentModel;

namespace gApp2
{
    public class Person : INotifyPropertyChanged
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

        private string name;
        public string Name { get { return this.name; } set { this.name = value; } }

        private DateTime lastChanged;
        public DateTime LastChanged { get { return this.lastChanged; } set { this.lastChanged = value; OnPropertyChanged("LastChanged"); } }

        public string LastChangedString { get { return this.LastChanged.ToString("dd.MM.yyyy HH:mm:ss"); } }

        // trigger event when changed
        private float total;
        public float Total { get { return this.total; } set { this.total = value; OnPropertyChanged("Total"); } }

        private ObservableCollection<Item> items = new ObservableCollection<Item>();
        public ObservableCollection<Item> Items { get { return this.items; } }

        public int ItemCount { get { return Items.Count; } }


        public Person()
        {
            Items.CollectionChanged += new NotifyCollectionChangedEventHandler(Items_CollectionChanged);
        }

        //constructor
        public Person(string name)
        {
            LastChanged = DateTime.Now;
            Name = name;
            Total = 0;
            Items.CollectionChanged += new NotifyCollectionChangedEventHandler(Items_CollectionChanged);
        }

        /// <summary>
        /// Calculates the new Total when Item-Collection is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            float sum = 0;
            DateTime date = DateTime.MinValue;
            foreach (Item i in Items)
            {
                if (i.CreationDate >= date)
                    date = i.CreationDate;
                sum += i.Value;
            }
            Total = sum;
            LastChanged = date;
            // ItemCount changed
            OnPropertyChanged("ItemCount");
            // LastChangedString changed
            OnPropertyChanged("LastChangedString");
        }

        /// <summary>
        /// Adds a new Item to the Person
        /// </summary>
        /// <param name="item"> the new item to be added </param>
        public void AddItem(Item item)
        {
            Items.Add(item);
        }

        public void RemoveItem(Item item)
        {
            Items.Remove(item);
        }
    }
}
