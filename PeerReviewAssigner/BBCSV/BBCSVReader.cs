using System;
using System.Collections.Generic;
using System.IO;

namespace PeerReviewAssignerCode.PeerReviewAssigner.BBCSV
{
    public class BBCSVReader
    {
        private string filename;
        private List<Student> allStudents = new List<Student>();
        private Dictionary<int, Group> allGroups = new Dictionary<int, Group>();

        public  BBCSVReader(string filename) {
            this.filename = filename;
        }

        public void ReadFile() {
            StreamReader inputReader = new StreamReader(filename);

            // load students and groups
            while (!inputReader.EndOfStream)
            {
                var line = inputReader.ReadLine();

                if (line != null)
                {
                    line = line.Replace("\"", "");
                    string[] strings = line.Split(new[] { ';', ',' });
                    // the array contains "Group Code","User Name","Student Id","First Name","Last Name"
                    string group = strings[0];
                    string auId = strings[1];
                    string studentId = strings[2];
                    string firstName = strings[3];
                    string lastName = strings[4];

                    // group id is in the format: Lab-Handins_gc_1
                    var groupIdSplit = @group.Split(new char[] {'_'});
                    string id = groupIdSplit[groupIdSplit.Length - 1];
                    int idAsInt = Int32.Parse(id);
                    if (!allGroups.ContainsKey(idAsInt))
                    {
                        allGroups.Add(idAsInt, new Group(idAsInt));
                    }

                    Student s = new Student(auId, firstName + " " + lastName, idAsInt);

                    allGroups[idAsInt].AddStudent(s);
                    allStudents.Add(s);
                }
            }
        }

        public List<Student> GetStudents() {
            return new List<Student>(allStudents);
        }

        public Dictionary<int, Group> GetGroups() {
            return new Dictionary<int, Group>(allGroups);
        }

    }
}