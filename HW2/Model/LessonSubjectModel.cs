using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW2.Model {
    public class LessonSubjectModel {
        public long StudentId { get; set; }
        public long TutorId { get; set; }
        public string Tytle { get; set; }
        public string Place { get; set; }
        public string Result { get; set; }
        public string Notes { get; set; }
        public int Points { get; set; }
    }
}
