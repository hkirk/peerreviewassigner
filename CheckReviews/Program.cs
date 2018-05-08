using System;
using System.Collections.Generic;
using PeerReviewAssignerCode.PeerReviewAssigner.BBCSV;
using PeerReviewAssignerCode.CheckReview.GoogleCSV;

namespace PeerReviewAssignerCode.CheckReview
{
    public class Program
    {
        public static void Main(string[] args) {
            if (args.Length != 2) {
                Console.WriteLine("We need two arguments, CSV from google forms and CSV from BB with groups and members");
                return;
            }

            string googleCSV = args[0];
            string bbCSV = args[1];

            var bBCSVReader = new BBCSVReader(bbCSV);
            bBCSVReader.ReadFile();
            var students = bBCSVReader.GetStudents();

            var googleCSVReader = new GoogleCSVReader(googleCSV);
            googleCSVReader.ReadFile();
            var studentReviews = googleCSVReader.GetStudents();

            Dictionary<string, PeerReviewAssigner.BBCSV.Student> allStudents = new Dictionary<string, PeerReviewAssigner.BBCSV.Student>();
            foreach(var student in students) {
                allStudents.Add(student.AuId.ToLower(), student);
            }

            // Find students with review
            var studentsWithReview = new List<GoogleCSV.Student>();
            var auidToRemove = new List<string>();
            foreach(var auid in studentReviews.Keys) {
                PeerReviewAssigner.BBCSV.Student student;
                if (allStudents.TryGetValue(auid, out student)) {
                    auidToRemove.Add(auid);
                    allStudents.Remove(auid);
                    studentsWithReview.Add(studentReviews.GetValueOrDefault(auid));
                }
            }
            foreach(var auid in auidToRemove) {
                studentReviews.Remove(auid);
            }

            Console.WriteLine("Students with reviews");
            foreach(var student in studentsWithReview) {
                Console.WriteLine(student.ToCSVString());
            }

            Console.WriteLine("Students without reviews");
            foreach(var student in allStudents.Values) {
                Console.WriteLine(student.ToCSVString());
            }

            Console.WriteLine("Unknown students with review");
            foreach(var student in studentReviews.Values) {
                Console.WriteLine(student.ToCSVString());
            }

        }
    }
}