using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeerReviewAssigner
{
    class Program
    {
        static void Main(string[] args)
        {
            // load and parse cvs file
            string filename = @"..\..\" + @"20180405094210_BB-Cou-UUVA-73288_groupmembers.csv";
            System.Console.WriteLine("Loading cvs file: " + filename);
            StreamReader inputReader = new StreamReader(filename);

            List<Student> allStudents = new List<Student>();
            Dictionary<int, Group> allGroups = new Dictionary<int, Group>();
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

            foreach (var group in allGroups)
            {
                System.Console.WriteLine(group.Value.ToString());
            }

            // assign random groups to review
            Random random = new Random();
            List<int> groupIdList = allGroups.Keys.ToList();
            foreach (Student student in allStudents)
            {
                int randomElementIdx = random.Next(0, groupIdList.Count);
                int groupToAssign = groupIdList[randomElementIdx];
                groupIdList.RemoveAt(randomElementIdx);
                student.ReviewGroup1 = allGroups[groupToAssign];
                if (groupIdList.Count == 0)
                {
                    groupIdList = allGroups.Keys.ToList();
                }
            }

            groupIdList = allGroups.Keys.ToList();
            foreach (Student student in allStudents)
            {
                int randomElementIdx = random.Next(0, groupIdList.Count);
                int groupToAssign = groupIdList[randomElementIdx];
                groupIdList.RemoveAt(randomElementIdx);
                student.ReviewGroup2 = allGroups[groupToAssign];
                if (groupIdList.Count == 0)
                {
                    groupIdList = allGroups.Keys.ToList();
                }
            }

            int numberOfCollisions = 0;
            int numberOfSameGroupTwice = 0;
            bool someOneReviewOwnGroup = false;
            bool someOneReviewsSameGroupTwice = false;
            // do 
            // for each student
            // if student review own group, swap with random group
            // until no-one reviews their own group
            do
            {
                someOneReviewOwnGroup = false;
                someOneReviewsSameGroupTwice = false;
                foreach (Student student in allStudents)
                {
                    if (student.ReviewGroup1.ContainsStudent(student))
                    {
                        someOneReviewOwnGroup = true;
                        var indexOfOneToSwap = random.Next(0, allStudents.Count);
                        var studentToSwapWith = allStudents[indexOfOneToSwap];
                        Group temp = studentToSwapWith.ReviewGroup1;
                        studentToSwapWith.ReviewGroup1 = student.ReviewGroup1;
                        student.ReviewGroup1 = temp;
                        numberOfCollisions++;
                    }

                    if (student.ReviewGroup2.ContainsStudent(student))
                    {
                        someOneReviewOwnGroup = true;
                        var indexOfOneToSwap = random.Next(0, allStudents.Count);
                        var studentToSwapWith = allStudents[indexOfOneToSwap];
                        Group temp = studentToSwapWith.ReviewGroup2;
                        studentToSwapWith.ReviewGroup2 = student.ReviewGroup2;
                        student.ReviewGroup2 = temp;
                        numberOfCollisions++;
                    }

                    if (student.ReviewGroup1.Id == student.ReviewGroup2.Id)
                    {
                        someOneReviewsSameGroupTwice = true;
                        var indexOfOneToSwap = random.Next(0, allStudents.Count);
                        var studentToSwapWith = allStudents[indexOfOneToSwap];
                        Group temp = studentToSwapWith.ReviewGroup2;
                        studentToSwapWith.ReviewGroup2 = student.ReviewGroup2;
                        student.ReviewGroup2 = temp;
                        numberOfSameGroupTwice++;
                    }
                }
            } while (someOneReviewOwnGroup || someOneReviewsSameGroupTwice);

            // write review assignment list
            foreach (Student student in allStudents)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(student.AuId + " (" + student.Name + ") : ");
                builder.Append("Review groups: " +  student.ReviewGroup1.Id + " and " + student.ReviewGroup2.Id);
                builder.Append(" - Own group: " + student.OwnGroup);
                Console.WriteLine(builder.ToString());
            }

            Console.WriteLine("Number of collisions solved: " + numberOfCollisions);
            Console.WriteLine("Number of review of same group twice solved: " + numberOfSameGroupTwice);

            Dictionary<int, int> numberOfReviewForEachGroup = new Dictionary<int, int>();
            foreach (Student student in allStudents)
            {
                int review1 = student.ReviewGroup1.Id;
                int review2 = student.ReviewGroup2.Id;
                if (!numberOfReviewForEachGroup.ContainsKey(review1))
                {
                    numberOfReviewForEachGroup.Add(review1, 1);
                }
                else
                {
                    numberOfReviewForEachGroup[review1] = numberOfReviewForEachGroup[review1] + 1;
                }

                if (!numberOfReviewForEachGroup.ContainsKey(review2))
                {
                    numberOfReviewForEachGroup.Add(review2, 1);
                }
                else
                {
                    numberOfReviewForEachGroup[review2] = numberOfReviewForEachGroup[review2] + 1;
                }
            }

            Console.WriteLine("Number of groups: " + allGroups.Count);
            foreach (KeyValuePair<int, int> keyValuePair in numberOfReviewForEachGroup)
            {
                Console.WriteLine("Group " + keyValuePair.Key + " has " + keyValuePair.Value + " reviews");
            }
#if DEBUG
            // Wait for keypress before exiting (otherwise the program exits a debug session)
            Console.WriteLine("Press a key to exit.");
            Console.ReadKey();
#endif
        }
    }
}
