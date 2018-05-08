using System.Collections.Generic;
using System.IO;

namespace PeerReviewAssignerCode.CheckReview.GoogleCSV
{
    public class GoogleCSVReader
    {
        private string filename;
        private Dictionary<string, Student> students = new Dictionary<string, Student>();

        public GoogleCSVReader(string filename) {
            this.filename = filename;
        }

        public void ReadFile() {
            var stream = new StreamReader(filename);

            while (!stream.EndOfStream) {
                var line = stream.ReadLine();

                if (!line.StartsWith("2018")) {
                    continue;
                }

                line = line.Replace("\"", "");
                var parts = line.Split(new[] {',', ';'} );
                // 2018/04/17 4:48:41 PM GMT+2,Martin Andersen,201605036,4,1,....
                var name = parts[1];
                var auid = parts[2];
                var groupNo = parts[3];
                var groupReview = parts[4];

                Student student;
                students.TryGetValue(auid.ToLower(), out student);
                if (student == null) {
                    student = new Student(name, auid, groupNo);
                    students.Add(auid.ToLower(), student);
                }
                student.AddReview(groupReview);
            }
        }

        public Dictionary<string, Student> GetStudents() {
            return students;
        }
    }
}