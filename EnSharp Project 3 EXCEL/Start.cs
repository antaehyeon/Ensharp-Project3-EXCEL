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
        } // Constructor


        public void run() {
            while (true) {
                Console.SetWindowSize(108, 46);
                width = 1; height = 7; menuNum = 7;

                selectNum = print.moveArrow(width, height, menuNum, "MAIN_MENU");
                switch (selectNum) {
                    case 7: // 수강신청
                        coursesFunction(1);
                        break;
                    case 8: // 수강철회
                        coursesFunction(2);
                        break;
                    case 9: // 관심과목 담기
                        coursesFunction(3);
                        break;
                    case 10: // 관심과목 철회
                        coursesFunction(4);
                        break;
                    case 11: // 시간표 출력
                        Console.Clear();
                        print.lectureTitle();
                        for (int i = 1; i < 104; i++) {
                            print.lectureInfoInArray(i);
                        }
                        Console.ReadKey();
                        break;
                    case 12: // 시간표 저장
                        Console.Clear();
                        printTimeTableAndExcelSave();
                        break;
                    case 13: // 종료
                        print.exit();
                        break;
                } // switch - selectNum
            } // while
        } // Method - run

        // 수강 기능
        public void coursesFunction(int mode) {
            int count, interestCount;
            int score, interestScore;

            while (true) {
                count = singleton.ApplicationLectureNum; // count = 현재 수강신청 갯수
                score = singleton.StudentGrade; // score = 신청 학점
                interestCount = singleton.ApplicationInterestLectureNum; // 관심과목 신청갯수
                interestScore = singleton.InterestGrade; // 관심과목 신청학점

                Console.Clear();    

                if (mode == 1) {
                    print.title("수강 신청");
                } // if
                else if (mode == 2) {
                    print.title("수강 철회");
                } // else if
                else if (mode == 3) {
                    print.title("관심과목 담기");
                } // else if
                else if (mode == 4) {
                    print.title("관심과목 철회");
                } // else if

                if(mode == 1 || mode == 2) {
                    // 수강신청된 내용을 출력해주는 부분
                    Console.WriteLine(" 현재 학점 : {0}\n 신청 과목 수 : {1}", score, count + 1);
                } // if
                else {
                    Console.WriteLine(" 관심과목 학점 : {0}\n 관심과목 수 : {1}", interestScore, interestCount + 1);
                } // else

                print.lectureTitle(); // 수강신청 스키마 출력부분

                if (mode == 1 || mode == 2) { // 수강신청 모드
                    for (int i = 0; i <= count; i++) {
                        print.applicationLecture(i, 1);
                    } // for
                } // if
                else if (mode == 3 || mode == 4) { // 관심과목 모드
                    for (int i = 0; i <= interestCount; i++) {
                        print.applicationLecture(i, 2);
                    } // for
                } // else
                

                // MODE에 따라서 출력해주는 부분
                // MODE(1) 수강신청
                // MODE(2) 수강철회
                // MODE(3) 관심과목신청
                // MODE(4) 관심과목철회
                if (mode == 1) { 
                    print.applicationLectureMenu();
                } // if
                else if (mode == 2) { 
                    print.retractionLecture();
                } // else if
                else if (mode == 3) { 
                    print.applicationInterestLecture();
                } // else if
                else if (mode == 4) { 
                    print.retractionInterestLecture();
                } // else if
                // 나중에 추가될 경우가 있을 경우를 고려해서 else if 로 마무리!

                Console.Write(" → ");

                if (mode == 1 || mode == 3) { // MODE(1) 수강신청 MODE(3) 수강신청
                    switch (exception.inputData(4)) {
                        case 1: // 과목이름 검색
                            // findClass(row, mode)
                            // 나중에 row는 항목들을 찾는 기능으로 쓰임
                            // 0 : 학수번호 1 : 과목명 2 : 분반 등...
                            // mode는 그냥 함수 하나에서 모드를 구별해서 사용하기 위해 만들어줌
                            if(lectureFunction.findClass(2, 1) == -1) { 
                                Console.ReadKey();
                                if (mode == 1)  this.coursesFunction(1); // 수강신청
                                else            this.coursesFunction(3); // 관심과목
                            } // if
                            // lectureApplication (수강관련기능)
                            // 1 : 수강신청 2 : 수강철회 3 : 관심과목신청 4: 관심과목철회
                            if (mode == 1) lectureFunction.lectureApplication(1);
                            else           lectureFunction.lectureApplication(3);
                            break;
                        case 2: // 교수님이름으로 검색
                            // findClass(row, mode)
                            // row(검색할 행)
                            if(lectureFunction.findClass(5, 2) == -1) {
                                Console.ReadKey();
                                if (mode == 1) this.coursesFunction(1);
                                else           this.coursesFunction(3);
                            } // if
                            if (mode == 1) lectureFunction.lectureApplication(1);
                            else           lectureFunction.lectureApplication(3);
                            
                            break;
                        case 3: // 관심과목 내역 검색 (MODE(1))
                            if (mode == 3) { // 함수를 같이쓰기 때문에, MODE 3번 (관심과목담기) 이라면 3번이 뒤로가기 이므로 예외로 처리
                                this.run();
                            } // if
                            lectureFunction.registerFromILecture();
                            break;
                        case 4: // 뒤로가기
                            if(mode == 3)
                            {
                                Console.WriteLine("잘못 입력하셨습니다");
                                Console.ReadKey();
                                this.coursesFunction(mode);
                            }
                            this.run();
                            break;
                        case -1: // 예외
                            this.coursesFunction(mode);
                            break;
                    } // Switch 
                } // if 
                else if (mode == 2 || mode == 4) { // MODE(2) 수강철회 MODE(4) 관심과목 철회
                    switch (exception.inputData(2)) {
                        case 1: // 수강철회
                            if (mode == 2) lectureFunction.lectureApplication(2);
                            else           lectureFunction.lectureApplication(4);
                            break;
                        case 2: // 뒤로가기
                            this.run();
                            break;
                    } // switch
                } // else if
            } // while  
        } // Method - lectureApplication

        public void printTimeTableAndExcelSave()
        {
            print.printTimeTable();
            int selectNum = exception.inputData(2);
            if (selectNum == 1)
            {
                singleton.storeExcel();
            } // if
            else if (selectNum == 2)
            {
                this.run();
            } // else
            else
            {
                Console.WriteLine("잘못 입력하셨습니다 :D");
                return;
            }
        } // Method - printTimeTableAndExcelSave
    } // Class - start
}
