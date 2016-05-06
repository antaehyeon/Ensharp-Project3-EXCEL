using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// 객체지향적으로 구성하기 위해 학생의 수강신청 데이터를 한 Class 로 구성하였음
/// 포함된 데이터는 학수번호, 과목이름, 분반, 학점, 교수, 시간, 강의실장소, 학과 이다.
/// </summary>
namespace EnSharp_Project_3_EXCEL
{
    class StudentLectureVO
    {
        private string num;
        private string name;
        private string classes;
        private string point;
        private string professor;
        private string time;
        private string place;
        private string department;

        public StudentLectureVO() { }
        public StudentLectureVO(string num, string name, string classes, string point, string professor, string time, string place, string department)
        {
            this.Num = num;
            this.Name = name;
            this.classes = classes;
            this.point = point;
            this.professor = professor;
            this.time = time;
            this.place = place;
            this.department = department;
        }

        public string Num
        {
            get { return num; }
            set { num = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Classes
        {
            get { return classes; }
            set { classes = value; }
        }

        public string Point
        {
            get { return point; }
            set { point = value; }
        }

        public string Professor
        {
            get { return professor; }
            set { professor = value; }
        }

        public string Time
        {
            get { return time; }
            set { time = value; }
        }

        public string Place
        {
            get { return place; }
            set { place = value; }
        }

        public string Department
        {
            get { return department; }
            set { department = value; }
        }
    }
}
