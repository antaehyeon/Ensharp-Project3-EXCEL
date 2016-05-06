using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnSharp_Project_3_EXCEL
{
    class Start
    {
        private Singleton singleton;
        private Print print;
        private LectureFunction lectureFunction;
        private Exception exception;
        private List<StudentLectureVO> studentLectureList;

        private int selectNum;
        private int width, height, menuNum;

        public Start()
        {
            singleton = Singleton.GetInstance();
            print = new Print();
            lectureFunction = new LectureFunction();
            exception = new Exception();
            studentLectureList = new List<StudentLectureVO>();
        }


        public void run()
        {
            while (true)
            {
                Console.SetWindowSize(108, 46);
                width = 1; height = 7; menuNum = 7;

                selectNum = print.moveArrow(width, height, menuNum, "MAIN_MENU");
                switch (selectNum)
                {
                    case 7:
                        coursesFunction(1);
                        break;
                    case 8:
                        coursesFunction(2);
                        break;
                    case 9:
                        coursesFunction(3);
                        break;
                    case 10:
                        coursesFunction(4);
                        break;
                    case 11:
                        Console.Clear();
                        print.lectureTitle();
                        for (int i = 1; i < 104; i++)
                        {
                            print.lectureInfoInArray(i);
                        }
                        Console.ReadKey();
                        break;
                    case 12:
                        Console.Clear();
                        printTimeTableAndExcelSave();
                        break;
                    case 13:
                        print.exit();
                        break;
                }
            }
        }

        public void coursesFunction(int mode)
        {
            int count, interestCount;
            int score, interestScore;

            while (true)
            {
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

                if (mode == 1 || mode == 2)
                {
                    Console.WriteLine(" 현재 학점 : {0}\n 신청 과목 수 : {1}", score, count + 1);
                }
                else
                {
                    Console.WriteLine(" 관심과목 학점 : {0}\n 관심과목 수 : {1}", interestScore, interestCount + 1);
                }

                print.lectureTitle();

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

                if (mode == 1 || mode == 3)
                {
                    switch (exception.inputData(4))
                    {
                        case 1:
                            if (lectureFunction.findClass(2, 1) == -1)
                            {
                                Console.ReadKey();
                                if (mode == 1) this.coursesFunction(1);
                                else this.coursesFunction(3);
                            }
                            if (mode == 1) lectureFunction.lectureApplication(1);
                            else lectureFunction.lectureApplication(3);
                            break;
                        case 2:
                            if (lectureFunction.findClass(5, 2) == -1)
                            {
                                Console.ReadKey();
                                if (mode == 1) this.coursesFunction(1);
                                else this.coursesFunction(3);
                            }
                            if (mode == 1) lectureFunction.lectureApplication(1);
                            else lectureFunction.lectureApplication(3);

                            break;
                        case 3:
                            if (mode == 3)
                            {
                                this.run();
                            }
                            lectureFunction.registerFromILecture();
                            break;
                        case 4:
                            if (mode == 3)
                            {
                                Console.WriteLine("잘못 입력하셨습니다");
                                Console.ReadKey();
                                this.coursesFunction(mode);
                            }
                            this.run();
                            break;
                        case -1:
                            this.coursesFunction(mode);
                            break;
                    }
                }
                else if (mode == 2 || mode == 4)
                {
                    switch (exception.inputData(2))
                    {
                        case 1:
                            if (mode == 2) lectureFunction.lectureApplication(2);
                            else lectureFunction.lectureApplication(4);
                            break;
                        case 2:
                            this.run();
                            break;
                    }
                }
            }
        }

        public void printTimeTableAndExcelSave()
        {
            print.printTimeTable();
            int selectNum = exception.inputData(2);
            if (selectNum == 1)
            {
                singleton.storeExcel();
            }
            else if (selectNum == 2)
            {
                this.run();
            }
            else
            {
                Console.WriteLine("잘못 입력하셨습니다 :D");
                return;
            }
        }
    }
}
