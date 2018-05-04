using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeerReviewAssigner
{
    public class Group
    {
        private readonly int _id;
        private readonly List<Student> students = new List<Student>();

        public Group(int id)
        {
            _id = id;
        }

        public int Id
        {
            get { return _id; }
        }

        public void AddStudent(Student s)
        {
            students.Add(s);
        }

        public bool ContainsStudent(Student s)
        {
            return students.Contains(s);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Group: " + Id + ":");
            builder.Append(" Students:");
            foreach (var student in students)
            {
                builder.Append(" " + student.Name + ",");
            }
            return builder.ToString();
        }
    }
}
