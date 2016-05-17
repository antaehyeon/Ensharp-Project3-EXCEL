using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// 프로그램에서 출력을 담당하는 Class
/// </summary>
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
            excelData = singleton.getArrayData();                   // singleton 의 Array data(Excel에서 불러온 데이터)를 불러옴
            studentLecture = singleton.StudentLectureList;
            interestLecture = singleton.InterestLectureList;
        }

        // 화살표로 움직일 수 있게 해주는 메소드
        // mode 를 통해서 여러개를 구분하려 했으나, 그렇게 크게 구분할것이 생기지 않았음..
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

                // KEY 의 입력을 받는 부분
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

                // 맨위에서 UpArrow 이벤트가 발생했을 때(즉 위방향키를 눌렀을 때) 맨 아래로 가게해주도록 설계
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

        // 콘솔창 맨위의 TITLE 을 출력해주는 메소드
        // 안의 문구는 자동으로 가운데정렬을 시켜준다
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

        public void incorrectData()
        {
            Console.Clear();
            Console.WriteLine("\n\n\n\n");
            title("입력하신 정보와 맞는 과목이 존재하지 않습니다");
            Console.ReadKey();
        }

        public void whatApplicationLecture()
        {
            Console.WriteLine("\n무엇을 신청하시겠습니까? (학수번호로 입력해주세요)");
            Console.WriteLine("이전 메뉴로 가시려면 b를 입력해주세요");
            Console.Write("→ ");
        }

        public void noExistsRetractionLecture()
        {
            setConsoleColor("수강철회 할 과목이 존재하지 않습니다");
            Console.ReadKey();
        }

        public void noExistsRetractionInterestLecture()
        {
            setConsoleColor("철회 할 관심과목이 존재하지 않습니다");
            Console.ReadKey();
        }

        public void noExistsInterestLecture()
        {
            Console.WriteLine("관심과목에 담겨진 과목이 없습니다");
            Console.ReadKey();
        }

        public void whatRetractionLecture()
        {
            Console.WriteLine("\n무엇을 철회하시겠습니까? (학수번호로 입력해주세요)");
            Console.WriteLine("이전 메뉴로 가시려면 b를 입력해주세요");
            Console.Write("→ ");
        }

        public void enterClassNum()
        {
            Console.WriteLine("해당 과목의 분반을 입력해주세요 (Ex. 001, 002, 003)");
            Console.WriteLine("이전 메뉴로 가시려면 b를 입력해주세요");
            Console.Write("→ ");
        }

        public void duplicationErrorMessage()
        {
            setConsoleColor("중복된 학수번호를 수강신청할 수 없습니다");
            Console.ReadKey();
        }

        public void duplicationInterestLectureErrorMessage()
        {
            setConsoleColor("중복된 학수번호를 관심과목으로 담을 수 없습니다");
            Console.ReadKey();
        }

        public void duplicationTimeMessage()
        {
            setConsoleColor(" 현재 수강한 과목과 시간이 중복됩니다");
            Console.ReadKey();
        }

        public void limitGrade18Message()
        {
            setConsoleColor("수강신청은 18학점까지 가능합니다");
            Console.ReadKey();
        }

        public void limitGrade24Message()
        {
            setConsoleColor("관심과목담기는 24학점까지 가능합니다");
            Console.ReadKey();
        }

        public void exit()
        {
            Console.Clear();
            Console.WriteLine("\n\n\n\n");
            title("수강신청 시스템을 이용해주셔서 감사합니다");

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

        // 과목의 TITLE (스키마)를 출력해주는 메소드
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

        // singleton 의 Array data 에서 과목을 Print
        // i 는 row 값 즉, array data 에서의 Index 값이다 (= 해당과목의 위치)
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

        // 신청과목을 Print 해주는 메소드
        // 위와 같이 Index 값을 통해서 출력하게 된다.
        // mode 1 : 수강신청 데이터
        // mode 2 : 관심과목 데이터
        // 위와 합치고 싶었으나, Array 데이터의 성격이 달라서 우선은 따로구현..
        public void applicationLecture(int i, int mode)
        {
            List<StudentLectureVO> tempList = new List<StudentLectureVO>();

            if (mode == 1)       tempList = studentLecture;
            else if (mode == 2)  tempList = interestLecture;

            string name = tempList[i].Name;
            string professor = tempList[i].Professor;
            string time = tempList[i].Time;
            string place = tempList[i].Place;
            string department = tempList[i].Department;

            Console.Write("┃{0, -8}", tempList[i].Num);
            Console.Write("┃{0}", hangleLineUp(24, name));
            Console.Write("┃{0, -4}", tempList[i].Classes);
            Console.Write("┃{0, 4}", tempList[i].Point);
            Console.Write("┃{0}", hangleLineUp(8, professor));
            Console.Write("┃{0}", hangleLineUp(16, time));
            Console.Write("┃{0}", hangleLineUp(6, place));
            Console.WriteLine("┃{0}┃", hangleLineUp(20, department));
            Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
        }

        // 시간표를 Print 해주는 메소드
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
                // 시간 부분을 출력해주는 부분 (09:00 ~ 19:00)
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
                // 09시 부분의 간격을 맞추기 위해서 따로 Print
                if (count == -2) Console.Write("┃09 : 00 ~ 09 : 30   ┃");
                if (count == -1) Console.Write("┃09 : 30 ~ 10 : 00   ┃");
                count++;
                // 월화수목금, 시간에 해당하는 과목을 Print
                for (int i = 0; i < 5; i++)
                {
                    lectureName = singleton.getTimeTableByIndex(row, i);
                    Console.Write("{0}┃", hangleLineUp(24, lectureName));
                }
                Console.WriteLine();
                // 모든 시간을 출력했다면, 마무리
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

        // length (총길이), strData (문자열) 을 이용해서
        // 문자열 이외의 부분은 공백으로 정렬하는 메소드
        public string hangleLineUp(int length, string strData)
        {
            string strToPrint = strData;
            int gap = length - Encoding.Default.GetBytes(strToPrint).Length;

            return "".PadLeft(gap) + strToPrint;
        }

        // 한글을 가운데 정렬해주는 메소드
        // length 를 이용해서 길이를 계산한다
        public string hangleCenterArrange(int length, string strData)
        {
            string strToPrint = strData;
            int gap = length - Encoding.Default.GetBytes(strToPrint).Length;

            int frontGap = gap / 2;
            int rearGap = gap - frontGap;

            return "".PadRight(frontGap) + strToPrint + "".PadRight(rearGap);
        }

        public void setConsoleColor(string str)
        {
            var Color = ConsoleColor.Red;
            Console.ForegroundColor = Color;

            Console.Write(str);
            Console.ResetColor();
        }
    }
}
