using System;
using System.Runtime.Serialization;

namespace Serialization_XML_JSON_Urok26
{
    [Serializable] // Сериализация
    public class Group
    {
        [NonSerialized]  // То что не нужно сериализовать 
        private readonly Random rnd = new Random(DateTime.Now.Millisecond);

        private int privateint;

        public int Number { get; set; }

        public string Name { get; set; }

        public Group () 
        {
            Number = rnd.Next(1, 10);
            Name = "Группа " + rnd;
        }

        public void GetPrivet(int i)
        {
            privateint = i;
        }
        public int GetPrivet()
        {
            return privateint;
        }

        public Group (int number, string name)
        {
            Number = number;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
