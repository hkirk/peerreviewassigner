using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PeerReviewAssignerCode.PeerReviewAssigner.BBCSV
{
    public class Student
    {
        private readonly string _auId;
        private readonly int _ownGroup;

        public Group ReviewGroup1 { get; set; } = null;
        public Group ReviewGroup2 { get; set; } = null;

        public string Name { get; }

        public int OwnGroup
        {
            get { return _ownGroup; }
        }

        public string AuId
        {
            get { return _auId; }
        }

        public Student(string auId, string name, int ownGroup)
        {
            _auId = auId;
            _ownGroup = ownGroup;
            this.Name = name;
        }

        public override string ToString() {
            return "Name: " + Name + ", auid: " + _auId + ", from group: " + _ownGroup;
        }

        public string ToCSVString() {
            return string.Format("{0},{1},{2}", Name, _auId, _ownGroup);
        }
    }
}
