using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;

namespace Serialization_XML_JSON_Urok26
{
    class Program
    {
        static void Main(string[] args)
        {
            var groups = new List<Group>(); // Список групп.
            var students = new List<Student>();  // Список студентов.

            for (int i = 0; i < 10; i++)
            {
                var group = new Group(i, "Группа " + i);
                group.GetPrivet(i);
                groups.Add(group);
            }

            for (int i = 0; i < 10; i++)
            {
                var studen = new Student(Guid.NewGuid().ToString().Substring(0, 5), i % 100) // Имена студентов и возраст.
                {
                    Group = groups[i % 9]
                };

                students.Add(studen);
            }
            #region ВИДЫ СЕРИАЛИЗАЦИИ
            #region 1) БИНАРНАЯ СЕРИАЛИЗАЦИЯ // Атрибут класса [Serializable] ВЕС 939 байт.
            var binFormatter = new BinaryFormatter();  // Форматер для сериализации.

            using (var file = new FileStream("groups.bin", FileMode.OpenOrCreate))
            {
                binFormatter.Serialize(file, groups);
            }

            using (var file = new FileStream("groups.bin", FileMode.OpenOrCreate)) // Десериализация
            {
                var newGroups = binFormatter.Deserialize(file) as List<Group>;

                if (newGroups != null)
                {
                    foreach (var group in newGroups)
                    {
                        Console.WriteLine(group);
                    }
                }
            }
            Console.ReadLine();
            #endregion
            #region 2) СОАП СЕРИАЛИЗАЦИЯ // Атрибут класса [Serializable]. ВЕС 5 177 байт.
            // Не может работать с Листом, нужно приводить к Массиву 
            // Весят в сохраненном файле больше чем бинарная, но универсальные и более защищенные.
            var soapFormatter = new SoapFormatter();  // Форматер для сериализации.

            using (var file = new FileStream("groups.soap", FileMode.OpenOrCreate))
            {
                soapFormatter.Serialize(file, groups.ToArray());
            }

            using (var file = new FileStream("groups.soap", FileMode.OpenOrCreate)) // Десериализация
            {
                var newGroups = soapFormatter.Deserialize(file) as Group[];

                if (newGroups != null)
                {
                    foreach (var group in newGroups)
                    {
                        Console.WriteLine(group);
                    }
                }
            }
            Console.ReadLine();
            #endregion
            #region 3) XML СЕРИАЛИЗАЦИЯ // Атрибут класса [Serializable]. ВЕС 953 байт.
            // Не может сериализовать приватные поля / по размеру похож бинарной.
            var xmlFormatter = new XmlSerializer(typeof(List<Group>));  // Форматер для сериализации.

            using (var file = new FileStream("groups.xml", FileMode.OpenOrCreate))
            {
                xmlFormatter.Serialize(file, groups);
            }

            using (var file = new FileStream("groups.xml", FileMode.OpenOrCreate)) // Десериализация
            {
                var newGroups = xmlFormatter.Deserialize(file) as List<Group>;

                if (newGroups != null)
                {
                    foreach (var group in newGroups)
                    {
                        Console.WriteLine(group);
                    }
                }
            }
            Console.ReadLine();
            #endregion
            #region 4) JSON СЕРИАЛИЗАЦИЯ // Атрибут класса [DataContract]. ВЕС 251 байт.
            // нужно в необходимом классе указывать атрибут [DataContract] и подключать в ручную через "NuGet" библиотеку "System.Runtime.Serialization.Json".
            // И необходимо в ручную каждое поле отмечать [DataMember], которое хотим чтобы сериализовалось.
            // Есть ограничения на "set;" - при отсутствии не сериализует
            // Сохраненный файл весит больше из все что здесь приведены.
            var jsonFormatter = new DataContractJsonSerializer(typeof(List<Student>));  // Форматер для сериализации. Для работы нужно закомментировать предыдущие сериализации так как нет класса с [Serializable]

            using (var file = new FileStream("students.json", FileMode.Create))
            {
                jsonFormatter.WriteObject(file, students);
            }

            using (var file = new FileStream("students.json", FileMode.OpenOrCreate)) // Десериализация
            {
                var newStudents = jsonFormatter.ReadObject(file) as List<Student>;

                if (newStudents != null)
                {
                    foreach (var student in newStudents)
                    {
                        Console.WriteLine(student);
                    }
                }
            }
            Console.ReadLine();
            #endregion
            #endregion
            Console.ReadLine();
        }
    }
}
