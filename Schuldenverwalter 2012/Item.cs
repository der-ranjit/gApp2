using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace gApp2
{
    public class Item 
    {
        private DateTime creationDate;
        public DateTime CreationDate { get { return this.creationDate; } set { this.creationDate = value; } }

        public string CreationDateString { get { return this.CreationDate.ToString("dd.MM.yyyy HH:mm:ss"); } }

        private float value;
        public float Value { get { return this.value; } set { this.value = value; } }

        private string name;
        public String Name { get { return this.name; } set { this.name = value; } }


        public Item()
        {
        }

        // constructor
        public Item(string name, float value)
        {
            CreationDate = DateTime.Now;
            Value = value;
            Name = name;
        }

    }
}
