using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnSharp_Project_3_EXCEL
{
    class Print
    {
        private Singleton singleton;
        private Array excelData;
        private List<StudentLectureVO> studentLecture;
        private List<StudentLectureVO> interestLecture;

        public Print()
        {
            singleton = Singleton.GetInstance();
            excelData = singleton.getArrayData();
            studentLecture = singleton.StudentLectureList;
            interestLecture = singleton.InterestLectureList;
        }


        enum lecture { empty, number, name, classes, grade, professor, time, place, department }

        public int moveArrow(int pWidth, int pHeight, int menuNumber, string mode)
        {
            ConsoleKeyInfo cki;
            int width = pWidth, height = pHeight;

            while (true)
            {
                Console.Clear();

                switch (mode)
                {
                    case "MAIN_MENU":
                        title("수강신청에 오신것을 환영합니다");
                        mainMenu();
                        break;
                    case "APPLICATION_LECTURE":
                        Console.Write("수행할 작업을 입력하세요");
                        applicationLectureMenu();
                        break;
                }


                Console.SetCursorPosition(width, height);
                Console.Write('→');

                cki = Console.ReadKey(true);
                switch (cki.Key)
                {
                    case ConsoleKey.UpArrow:
                        height--;
                        break;
                    case ConsoleKey.DownArrow:
                        height++;
                        break;
                    case ConsoleKey.Enter:
                        return height;
                }

                if (height == pHeight - 1)
                {
                    height = pHeight + menuNumber - 1;
                }
                else if (height == pHeight + menuNumber)
                {
                    height = pHeight;
                }
            }
        }

        public void title(string StrData)
        {
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine("\n{0}\n", hangleCenterArrange(108, StrData));
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        }

        public void mainMenu()
        {
            Console.WriteLine("수행할 작업을 선택하시기 바랍니다\n");
            Console.WriteLine("[  ] 수강 신청");
            Console.WriteLine("[  ] 수강 철회");
            Console.WriteLine("[  ] 관심과목 담기");
            Console.WriteLine("[  ] 관심과목 철회");
            Console.WriteLine("[  ] 전체 시간표 출력");
            Console.WriteLine("[  ] 수강시간표 출력 및 저장");
            Console.WriteLine("[  ] 종료");
        }

        public void applicationLectureMenu()
        {
            Console.WriteLine("1. 과목이름 검색하기");
            Console.WriteLine("2. 교수명으로 검색하기");
            Console.WriteLine("3. 관심과목으로 신청하기");
            Console.WriteLine("4. 뒤로 가기");
        }

        public void applicationInterestLecture()
        {
            Console.WriteLine("1. 과목이름 검색하기");
            Console.WriteLine("2. 교수명으로 검색하기");
            Console.WriteLine("3. 뒤로 가기");
        }

        public void retractionLecture()
        {
            Console.WriteLine("1. 수강 철회");
            Console.WriteLine("2. 뒤로 가기");
        }

        public void retractionInterestLecture()
        {
            Console.WriteLine("1. 관심과목 철회");
            Console.WriteLine("2. 뒤로 가기");
        }

        public void notExistsLecture()
        {
            Console.Clear();
            Console.WriteLine("\n\n\n\n");
            title("교과목명이 존재하지 않습니다");
        }

        public void notExistsProfessor()
        {
            Console.Clear();
            Console.WriteLine("\n\n\n\n");
            title("교수님이 존재하지 않습니다");
        }

        public void notRetractLecture()
        {
            Console.Clear();
            Console.WriteLine("\n\n\n\n");
            title("철회할 과목이 존재하지 않습니다");
        }

        public void exit()
        {
            Console.Clear();
            Console.WriteLine("\n\n\n\n");
            title("수강신청을 이용해주셔서 감사합니다");

            singleton.exitExcel();
            Environment.Exit(0);
        }

        public void startLine()
        {
            Console.WriteLine("┏━━━━┳━━━━━━━━━━━━┳━━┳━━┳━━━━┳━━━━━━━━┳━━━┳━━━━━━━━━━┓");
        }

        public void endLine()
        {
            Console.WriteLine("┗━━━━┻━━━━━━━━━━━━┻━━┻━━┻━━━━┻━━━━━━━━┻━━━┻━━━━━━━━━━┛");
        }

        public void emptyTable()
        {
            Console.Write("┃{0, -8}", " ");
            Console.Write("┃{0}", " ");
            Console.Write("┃{0, -4}", " ");
            Console.Write("┃{0, 4}", " ");
            Console.Write("┃{0}", " ");
            Console.Write("┃{0}", " ");
            Console.Write("┃{0}", " ");
            Console.WriteLine("┃{0}┃", " ");
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        }

        public void lectureTitle()
        {
            startLine();
            Console.Write("┃{0}", hangleCenterArrange(8, "학수번호"));
            Console.Write("┃{0}", hangleCenterArrange(24, "교과목명"));
            Console.Write("┃{0}", hangleCenterArrange(4, "분반"));
            Console.Write("┃{0}", hangleCenterArrange(4, "학점"));
            Console.Write("┃{0}", hangleCenterArrange(8, "교수명"));
            Console.Write("┃{0}", hangleCenterArrange(16, "요일 및 강의시간"));
            Console.Write("┃{0}", hangleCenterArrange(6, "강의실"));
            Console.WriteLine("┃{0}┃", hangleCenterArrange(20, "개설학과전공"));
            endLine();
        }

        public void lectureInfoInArray(int i)
        {
            string name = (string)excelData.GetValue(i, 2);
            string professor = (string)excelData.GetValue(i, 5);
            string time = (string)excelData.GetValue(i, 6);
            string place = (string)excelData.GetValue(i, 7);
            string department = (string)excelData.GetValue(i, 8);

            Console.Write("┃{0, -8}", excelData.GetValue(i, 1));
            Console.Write("┃{0}", hangleLineUp(24, name));
            Console.Write("┃{0, -4}", excelData.GetValue(i, 3));
            Console.Write("┃{0, 4}", excelData.GetValue(i, 4));
            Console.Write("┃{0}", hangleLineUp(8, professor));
            Console.Write("┃{0}", hangleLineUp(16, time));
            Console.Write("┃{0}", hangleLineUp(6, place));
            Console.WriteLine("┃{0}┃", hangleLineUp(20, department));
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        }

        public void applicationLecture(int i, int mode)
        {
            string name;
            string professor;
            string time;
            string place;
            string department;

            if (mode == 1)
            {
                name = studentLecture[i].Name;
                professor = studentLecture[i].Professor;
                time = studentLecture[i].Time;
                place = studentLecture[i].Place;
                department = studentLecture[i].Department;

                Console.Write("┃{0, -8}", studentLecture[i].Num);
                Console.Write("┃{0}", hangleLineUp(24, name));
                Console.Write("┃{0, -4}", studentLecture[i].Classes);
                Console.Write("┃{0, 4}", studentLecture[i].Point);
                Console.Write("┃{0}", hangleLineUp(8, professor));
                Console.Write("┃{0}", hangleLineUp(16, time));
                Console.Write("┃{0}", hangleLineUp(6, place));
                Console.WriteLine("┃{0}┃", hangleLineUp(20, department));
            }
            else if (mode == 2)
            {
                name = interestLecture[i].Name;
                professor = interestLecture[i].Professor;
                time = interestLecture[i].Time;
                place = interestLecture[i].Place;
                department = interestLecture[i].Department;


                Console.Write("┃{0, -8}", interestLecture[i].Num);
                Console.Write("┃{0}", hangleLineUp(24, name));
                Console.Write("┃{0, -4}", interestLecture[i].Classes);
                Console.Write("┃{0, 4}", interestLecture[i].Point);
                Console.Write("┃{0}", hangleLineUp(8, professor));
                Console.Write("┃{0}", hangleLineUp(16, time));
                Console.Write("┃{0}", hangleLineUp(6, place));
                Console.WriteLine("┃{0}┃", hangleLineUp(20, department));
            }

            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        }

        public void printTimeTable()
        {
            int hour = 10;
            int count = -2;
            int row = 0;

            string lectureName;

            Console.SetWindowSize(156, 50);
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine("\n{0}\n", hangleCenterArrange(154, "시간표"));
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

            Console.WriteLine("┏━━━━━━━━━━┳━━━━━━━━━━━━┳━━━━━━━━━━━━┳━━━━━━━━━━━━┳━━━━━━━━━━━━┳━━━━━━━━━━━━┓");
            Console.WriteLine("┃　　　　시간　　　　┃　　　　　월　　　　　　┃　　　　　화　　　　　　┃　　　　　수　　　　　　┃　　　　　목　　　　　　┃　　　　　금　　　　　　┃");
            Console.WriteLine("┣━━━━━━━━━━╋━━━━━━━━━━━━╋━━━━━━━━━━━━╋━━━━━━━━━━━━╋━━━━━━━━━━━━╋━━━━━━━━━━━━┫");


            while (true)
            {
                if (count >= 0 && count % 2 == 0)
                {
                    Console.Write("┃{0} : 00 ~ {1} : 30   ┃", Convert.ToString(hour), Convert.ToString(hour));
                }
                else if (count >= 0 && count % 2 == 1)
                {
                    Console.Write("┃{0} : 30 ~ ", Convert.ToString(hour));
                    hour++;
                    Console.Write("{0} : 00   ┃", Convert.ToString(hour));
                }
                if (count == -2)
                {
                    Console.Write("┃09 : 00 ~ 09 : 30   ┃");
                }
                if (count == -1)
                {
                    Console.Write("┃09 : 30 ~ 10 : 00   ┃");
                }
                count++;
                for (int i = 0; i < 5; i++)
                {
                    lectureName = singleton.getTimeTableByIndex(row, i);
                    Console.Write("{0}┃", hangleLineUp(24, lectureName));
                }
                Console.WriteLine();
                if (hour == 19)
                {
                    Console.WriteLine("┗━━━━━━━━━━┻━━━━━━━━━━━━┻━━━━━━━━━━━━┻━━━━━━━━━━━━┻━━━━━━━━━━━━┻━━━━━━━━━━━━┛");
                    break;
                }
                Console.WriteLine("┣━━━━━━━━━━╋━━━━━━━━━━━━╋━━━━━━━━━━━━╋━━━━━━━━━━━━╋━━━━━━━━━━━━╋━━━━━━━━━━━━┫");
                row++;
            }
            Console.WriteLine("현재 시간표를 엑셀로 저장하시겠습니까? (1: 저장, 2: 저장안함)");
            Console.Write(" → ");

        }

        public string hangleLineUp(int length, string strData)
        {
            string strToPrint = strData;
            int gap = length - Encoding.Default.GetBytes(strToPrint).Length;

            return "".PadLeft(gap) + strToPrint;
        }

        public string hangleCenterArrange(int length, string strData)
        {
            string strToPrint = strData;
            int gap = length - Encoding.Default.GetBytes(strToPrint).Length;

            int frontGap = gap / 2;
            int rearGap = gap - frontGap;

            return "".PadRight(frontGap) + strToPrint + "".PadRight(rearGap);
        }
    }
}
