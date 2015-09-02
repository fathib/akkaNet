using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class NainsEpaulesGeants
    {
        [TestMethod]
        public void TestMethod1()
        {
            int nbRelations= 8;
            Dictionary<int, Person> people = new Dictionary<int, Person>();
            
            List<string> relations = new List<string> {"1 2",
                                                    "1 3",
                                                    "3 4",
                                                    "2 4",
                                                    "2 5",
                                                    "10 11",
                                                    "10 1",
                                                    "10 3"};

            for (int i = 0; i < nbRelations; i++)
            {
                int a = int.Parse(relations[i].Split(' ')[0]);
                int b = int.Parse(relations[i].Split(' ')[1]);

                if (!people.ContainsKey(a))
                {
                    people.Add(a,new Person(a));
                }

                if (!people.ContainsKey(b))
                {
                    people.Add(b , new Person(b));
                }
                people[a].InfluencedPersons.Add(people[b]);
            }


            //get not influenced people
            var firstguys = people.Where(p => !people.Values.SelectMany(x=> x.InfluencedPersons).Contains(p.Value)).ToList();

            int maxDepth = 0;
            foreach (var guys in firstguys)
            {
                int d =guys.Value.MaxDepth(0);
                if (d > maxDepth)
                    maxDepth = d;
            }

            Assert.AreEqual(4,maxDepth);
        }
    }





    public class Person
    {

        public int MaxDepth(int depth)
        {
            int relDepth = depth+1;

            if (this.InfluencedPersons.Count != 0)
            { 
                foreach (var person in InfluencedPersons)
                {
                    int d = person.MaxDepth(depth + 1);
                    if (d > relDepth)
                        relDepth = d;
                }
            }

            return relDepth;
        }


        public Person(int id)
        {
            Id = id;
            InfluencedPersons= new List<Person>();
        }

        public int Id { get; set; }

        public List<Person> InfluencedPersons { get; set; }
    }
}
