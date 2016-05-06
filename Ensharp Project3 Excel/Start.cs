using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Program Class에서 start 를 통해 시작하는 부분
/// 처음의 메뉴선택 부분이 포함되어 있다
/// </summary>
namespace EnSharp_Project_3_EXCEL
{
    class Start
    {
        private Singleton singleton;
        private Print print;
        private LectureFunction lectureFunction;
        private Exception exception;
        private List<StudentLectureVO> studentLectureList;

        private int selectNum;                 // 숫자를 입력받기 위함(Switch 분기때 사용)
        private int width, height, menuNum;                 // width(콘솔창너비), height(콘솔창높이), menuNum(메뉴갯수)

        public Start()
        {
            singleton = Singleton.GetInstance();
            print = new Print();
            lectureFunction = new LectureFunction();
            exception = new Exception();
            studentLectureList = new List<StudentLectureVO>();
        } // Constructor

        /// <summary>
        /// courseFunction 은 수강에 관련된 기능을 처리해주는 메소드인데, 수강신청,철회, 관심과목담기, 철회 기능이 비슷해서 한 함수내에서 처리하도록 설계
        /// 안에 들어가는 숫자는 mode 로서 각각의 기능을 구분하도록 함
        /// </summary>
        public void run()
        {
            while (true)
            {
                Console.SetWindowSize(108, 46);                 // 콘솔창을 보기좋게 하기위해 크기를 조정 (108, 46)
                width = 1; height = 7; menuNum = 7;                 // Cursor 위치를 지정(width, height)하고 메뉴갯수(7)만큼 움직일 수 있게함

                selectNum = print.moveArrow(width, height, menuNum, "MAIN_MENU");                 // moveArrow가 커서 움직이는 기능을 만들어줌
                switch (selectNum) // height 값을 이용해서 무엇이 선택됬는지 구분함 (moveArrow 메소드에서 Enter 가 입력됬을 시 해당 위치 height 반환)
                {
                    case 7:                  // 수강기능(1번 : 수강신청)
                        coursesFunction(1);
                        break;
                    case 8:                  // 수강기능(2번 : 수강철회)
                        coursesFunction(2);
                        break;
                    case 9:                  // 수강기능(3번 : 관심과목담기)
                        coursesFunction(3);
                        break;
                    case 10:                 // 수강기능(4번 : 관심과목철회)
                        coursesFunction(4);
                        break;
                    case 11:                 // 엑셀에 저장된 과목을 전부 출력하는 메뉴[문제점이라면 104로 값이 고정되어 있다는점]
                        Console.Clear();
                        print.lectureTitle();
                        for (int i = 1; i < 104; i++)
                        {
                            print.lectureInfoInArray(i);
                        }
                        Console.ReadKey();
                        break;
                    case 12:                 // 수강신청한 시간표를 보여주고, 저장할 수 있는 메뉴
                        Console.Clear();
                        printTimeTableAndExcelSave();
                        break;
                    case 13:                 // 종료
                        print.exit();
                        break;
                }
            }
        }
        /// <summary>
        /// 수강기능을 담고있는 Method 이다
        /// 위에서 설명했듯이, 기능이 연관된게 많아서 한 메소드로 묶었으며, mode를 통해서 구분한다
        /// Mode (1: 수강신청, 2: 수강철회, 3: 관심과목담기, 4: 관심과목철회)
        /// </summary>
        /// <param name="mode"></param>
        public void coursesFunction(int mode)
        {
            int count, score;                   // 현재 수강신청 된 갯수(count), 수강신청된 학점(score)
            int interestCount, interestScore;                   // 관심과목 갯수(interestCount), 관심과목 학점(interestScore)

            while (true)
            {
                // 전부 Singleton Class 에서 불러옴
                count = singleton.ApplicationLectureNum;
                score = singleton.StudentGrade;
                interestCount = singleton.ApplicationInterestLectureNum;
                interestScore = singleton.InterestGrade;

                Console.Clear();

                if (mode == 1)
                {
                    print.title("수강 신청");
                }
                else if (mode == 2)
                {
                    print.title("수강 철회");
                }
                else if (mode == 3)
                {
                    print.title("관심과목 담기");
                }
                else if (mode == 4)
                {
                    print.title("관심과목 철회");
                }

                if (mode == 1 || mode == 2)                   // mode 1, 2 : 수강신청과 철회 부분에서 갯수와 학점을 Print
                {
                    Console.WriteLine(" 현재 학점 : {0}\n 신청 과목 수 : {1}", score, count + 1);
                }
                else                   // mode 3, 4 : 관심과목 부분에서 갯수와 학점을 Print
                {
                    Console.WriteLine(" 관심과목 학점 : {0}\n 관심과목 수 : {1}", interestScore, interestCount + 1);
                }

                print.lectureTitle();                   // 수강시간표 목록을 출력 (스키마부분)

                // 실제 수강신청쪽 과목과 관심과목쪽 신청되어 있는 과목을 Print
                if (mode == 1 || mode == 2)
                {
                    for (int i = 0; i <= count; i++)
                    {
                        print.applicationLecture(i, 1);
                    }
                }
                else if (mode == 3 || mode == 4)
                {
                    for (int i = 0; i <= interestCount; i++)
                    {
                        print.applicationLecture(i, 2);
                    }
                }

                // mode에 따라서 선택문 출력
                if (mode == 1)
                {
                    print.applicationLectureMenu();
                }
                else if (mode == 2)
                {
                    print.retractionLecture();
                }
                else if (mode == 3)
                {
                    print.applicationInterestLecture();
                }
                else if (mode == 4)
                {
                    print.retractionInterestLecture();
                }

                Console.Write(" → ");

                // 실제적으로 선택을 받아서 LectureFunction Class 의 기능을 불러온다
                // 여기서는 mode 1 과 mode 3 만 해당 (수강신청, 관심과목 신청)
                if (mode == 1 || mode == 3)
                {
                    switch (exception.inputData(4))                   // inputData(4) = 1~4의 번호만 받겠다는 뜻, 이외의 값이 들어오면 -1을 리턴
                    {
                        case 1:                   // 과목이름으로 검색
                            if (lectureFunction.findClass(2, 1) == -1)                   // 과목이 존재하지 않을경우 -1을 리턴하게 됨, 예외처리부분
                            {
                                Console.ReadKey();
                                if (mode == 1) this.coursesFunction(1);                   // (mode 1 : 수강신청)의 값을 가지고 다시 함수를 불러온다 = 뒤로가기
                                else this.coursesFunction(3);                             // 그게 아니라면 (mode 3 : 관심과목신청)의 값을 가짐
                            }
                            if (mode == 1) lectureFunction.lectureApplication(1);         // 수강신청 부분
                            else           lectureFunction.lectureApplication(3);         // 관심과목 신청 부분
                            break;
                        case 2:                   // 교수님성함으로 검색
                            if (lectureFunction.findClass(5, 2) == -1)
                            {
                                Console.ReadKey();
                                if (mode == 1) this.coursesFunction(1);
                                else this.coursesFunction(3);
                            }
                            if (mode == 1) lectureFunction.lectureApplication(1);
                            else           lectureFunction.lectureApplication(3);

                            break;
                        case 3:                   // 3번의 경우는 약간 다른데
                                                  // 수강신청일때는 관심과목에서 신청한 과목을 보여주는 메뉴이고
                                                  // 관심과목일때는 종료메뉴 이다
                                                  // 그래서 관심과목신청(mode : 3)일 경우, 종료기능을 수행
                            if (mode == 3)
                            {
                                this.run();
                            }
                            lectureFunction.registerFromILecture();
                            break;
                        case 4:                   // 4번의 경우도 관심과목 신청에서는 예외처리로 들어가야하는 부분이다
                                                  // 수강신청 (mode : 1)의 경우에는 종료메뉴
                            if (mode == 3)
                            {
                                Console.WriteLine("잘못 입력하셨습니다");
                                Console.ReadKey();
                                this.coursesFunction(mode);
                            }
                            this.run();
                            break;
                        case -1:                   // 위에서 1~4 이외의 값이 들어왔을때, mode값을 그대로 가지고 다시 메소드를 불러온다
                            this.coursesFunction(mode);
                            break;
                    }
                }
                // 수강철회(mode : 2)와 관심과목철회(mode : 4)
                else if (mode == 2 || mode == 4)
                {
                    switch (exception.inputData(2))
                    {
                        case 1:                   // 수강철회
                            if (mode == 2) lectureFunction.lectureApplication(2);
                            else           lectureFunction.lectureApplication(4);
                            break;
                        case 2:                   // 종료
                            this.run();
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 내가 신청한 과목을 보여주고, 엑셀로 저장하는 메소드이다.
        /// 기능은 Singleton 에 직접적으로 구현
        /// </summary>
        public void printTimeTableAndExcelSave()
        {
            print.printTimeTable();                   // 수강신청한 과목을 Print 해주고
            int selectNum = exception.inputData(2);   // 저장할것인지(1), 종료할것인지(2) 입력을 받는다
            if (selectNum == 1)                       // 저장할거라면
            {
                singleton.storeExcel();               // Excel 저장 메소드
            }
            else if (selectNum == 2)                  // 종료할거라면
            {
                this.run();                           // 메인메뉴로
            }
            else
            {
                Console.WriteLine("잘못 입력하셨습니다 :D");
                return;
            }
        }
    }
}
