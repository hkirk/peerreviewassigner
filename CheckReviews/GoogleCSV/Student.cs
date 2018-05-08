using System.Collections.Generic;

namespace PeerReviewAssignerCode.CheckReview.GoogleCSV
{
    public class Student
    {
        private string name;
        private string auid;
        private string group;

        private List<string> reviews = new List<string>();

        public Student(string name, string auid, string group) {
            this.name = name;
            this.auid = auid;
            this.group = group;
        }

        public void AddReview(string group) {
            reviews.Add(group);
        }

        public override string ToString() {
            var reivewsStr = string.Join(", ", reviews);
            return "Name: " + name + ", auid: " + auid + " from group: " + group + ". Reviewed: " + reivewsStr;
        }

        public string ToCSVString() {
            var reivewsStr = string.Join(", ", reviews);
            return string.Format("{0},{1},{2},{3},{4}", name, auid, group, reviews.Count,reivewsStr);
        }
    }
}