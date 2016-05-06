using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace EnSharp_Project_3_EXCEL
{
    class LectureFunction
    {
        private Print print;
        private Singleton singleton;
        private Exception exception;
        private List<int> lectureIndex;

        public LectureFunction()
        {
            print = new Print();
            exception = new Exception();
            singleton = Singleton.GetInstance();
            lectureIndex = new List<int>(); // Excel 에서 받아온 Data 의 row (=index) 를 알기위한 List
        } // Constructor

        // 과목명이나 교수님이름으로 찾기
        public int findClass(int row, int mode)
        {
            string inputStrData;

            Console.Clear();

            if (mode == 1)
            {
                print.title("검색할 과목명을 입력해주세요 (전체검색하시려면 Enter)");
            }
            else
            {
                print.title("검색할 교수님 성함을 입력하세요");
            } // if - else

            Console.Write("→ ");

            // 과목명을 입력받아서 Singleton 의 array Data에서
            // 해당 과목명이 위치한 index를 찾는다
            inputStrData = Console.ReadLine();
            lectureIndex = searchLectureIndex(inputStrData, row);

            // 만약 첫 데이터에 -1 이 존재한다면
            // [예외처리] 과목이 존재하지 않는것
            // Method Exit
            if (lectureIndex[0] == -1)
            {
                if (mode == 1)
                {
                    print.notExistsLecture(); // 과목이 존재하지 않다는 Error Message 출력
                }
                else if (mode == 2)
                {
                    print.notExistsProfessor(); // 교수님이 존재하지 않다는 Error Message 출력
                }
                return -1;
            } // if

            // 해당하는 과목 출력문
            // print Class의 lectureInfo(과목정보) 의 인자로
            // 위에서 찾은 index 를 반복문을 통해 보내주고 출력
            print.lectureTitle();
            for (int i = 0; i < lectureIndex.Count; i++)
            {
                print.lectureInfoInArray(lectureIndex[i]);
            } // for
            return 0;
        } // Method - findClassName

        // 수강 관련 기능
        public void lectureApplication(int mode)
        {
            string inputLecNum, inputLecClass;

            // 무엇을 신청할지 입력받는다
            // 학수번호와 분반은 int형이기 때문에
            // Exception Class의 숫자만 추출하는 함수를 이용해 숫자만 추출한다
            if (mode == 1 || mode == 3)
            {
                Console.WriteLine("\n무엇을 신청하시겠습니까? (학수번호로 입력해주세요)");
                Console.WriteLine("이전 메뉴로 가시려면 Enter 두번을 눌러주세요");
                Console.Write("→ ");
            } // if
            else if (mode == 2 || mode == 4)
            {
                if (mode == 2 && singleton.StudentLectureList.Count == 0)
                {
                    Console.WriteLine("수강철회 할 과목이 존재하지 않습니다");
                    Console.ReadKey();
                    return;
                } // if
                else if (mode == 4 && singleton.InterestLectureList.Count == 0)
                {
                    Console.WriteLine("철회 할 관심과목이 존재하지 않습니다");
                    Console.ReadKey();
                    return;
                } // else if
                Console.WriteLine("\n무엇을 철회하시겠습니까? (학수번호로 입력해주세요)");
                Console.WriteLine("이전 메뉴로 가시려면 Enter 두번을 눌러주세요");
                Console.Write("→ ");
            } // else if
            inputLecNum = Console.ReadLine();

            Console.WriteLine("해당 과목의 분반을 입력해주세요 (Ex. 001, 002, 003)");
            Console.Write("→ ");
            inputLecClass = Console.ReadLine();

            // LectureNameIndex : 해당 과목이 존재하는 index (row)
            // inputLecNum : 학수번호 입력받은 변수
            // inputLecClass : 분반 입력받은 변수

            // 수강신청
            if (mode == 1)
            {
                for (int i = 0; i < lectureIndex.Count; i++)
                { // 학수번호와 분반이 맞다면
                    if (inputLecNum.Equals(arrayGetValueString(lectureIndex[i], 1)) && inputLecClass.Equals(arrayGetValueString(lectureIndex[i], 3)))
                    {
                        if (lectureDuplicationCheck(inputLecNum, 1) == false)
                        {// 이미 같은 학수번호가 존재한다면
                            Console.WriteLine("중복된 학수번호를 수강신청할 수 없습니다");
                            Console.ReadKey();
                            return; // 함수종료
                        } // if
                        if (!setCheckTimeTable(arrayGetValueString(lectureIndex[i], 2), arrayGetValueString(lectureIndex[i], 6), 1))
                        { // 만약, TimeTable 배열에 이미 다른값(1)이 들어가 있다면, 시간이 중복되는 것이므로 예외처리
                            Console.WriteLine("시간이 중복됩니다");
                            Console.ReadKey();
                            return;
                        } // if
                        singleton.StudentGrade += Convert.ToInt32(singleton.getArrayData().GetValue(lectureIndex[i], 4)); // 학점부분을 더함
                        if (singleton.StudentGrade > 19)
                        {// 신청 학점이 18학점을 넘을경우
                            Console.WriteLine("수강신청은 18학점까지 가능합니다");
                            singleton.StudentGrade -= Convert.ToInt32(singleton.getArrayData().GetValue(lectureIndex[i], 4)); // 학점을 다시 빼줌
                            Console.ReadKey();
                            return;
                        } // if
                        setStudentData(lectureIndex[i], 1); // 학생데이터를 설정함(수강신청 목록에 넣음), 위에서 찾은 Index 부분에
                    } // if
                } // for
            } // if
            // 수강철회
            else if (mode == 2)
            {
                for (int i = 0; i < singleton.StudentLectureList.Count; i++)
                { // 이미 수강신청한 리스트에서 학수번호와 분반이 맞다면
                    if (inputLecNum.Equals(singleton.StudentLectureList[i].Num) && inputLecClass.Equals(singleton.StudentLectureList[i].Classes))
                    {
                        singleton.ApplicationLectureNum -= 1; // 수강횟수 -1
                        singleton.StudentGrade -= Convert.ToInt32(singleton.StudentLectureList[i].Point); // 학점부분을 뺌
                        singleton.StudentLectureList.RemoveAt(i); // 해당 Index의 학생데이터를 지워버림
                        setCheckTimeTable(arrayGetValueString(lectureIndex[i], 2), arrayGetValueString(lectureIndex[i], 6), 2);
                        return;
                    } // if
                } // for
                Console.WriteLine("입력하신 정보와 맞는 수강신청 내역이 존재하지 않아, 수강철회를 할 수 없습니다");
                Console.ReadKey();
            } // else if - 강의 철회

            // 관심과목 담기
            else if (mode == 3)
            {
                for (int i = 0; i < lectureIndex.Count; i++)
                {
                    if (inputLecNum.Equals(arrayGetValueString(lectureIndex[i], 1)) && inputLecClass.Equals(arrayGetValueString(lectureIndex[i], 3)))
                    {
                        if (lectureDuplicationCheck(inputLecNum, 2) == false)
                        {// 이미 같은 학수번호가 존재한다면
                            Console.WriteLine("중복된 학수번호를 관심과목으로 담을 수 없습니다");
                            Console.ReadKey();
                            return; // 함수종료
                        } // if
                        singleton.InterestGrade += Convert.ToInt32(singleton.getArrayData().GetValue(lectureIndex[i], 4));
                        if (singleton.InterestGrade > 25)
                        {
                            Console.WriteLine("관심과목은 24학점까지 담을 수 있습니다");
                            singleton.InterestGrade -= Convert.ToInt32(singleton.getArrayData().GetValue(lectureIndex[i], 4));
                            Console.ReadKey();
                            return;
                        } // if
                        setStudentData(lectureIndex[i], 2);
                    } // if
                } // for 
                  // 신청 갯수 증가
            } // else if - 관심과목 신청

            // 관심과목 철회
            else if (mode == 4)
            {
                for (int i = 0; i < lectureIndex.Count; i++)
                {
                    if (inputLecNum.Equals(singleton.InterestLectureList[i].Num) && inputLecClass.Equals(singleton.InterestLectureList[i].Classes))
                    {
                        singleton.ApplicationInterestLectureNum -= 1;
                        singleton.InterestGrade -= Convert.ToInt32(singleton.InterestLectureList[i].Point);
                        singleton.InterestLectureList.RemoveAt(i);
                        return;
                    } // if
                } // for
                Console.WriteLine("입력하신 정보와 맞는 관심과목 내역이 존재하지 않아, 관심과목 철회를 할 수 없습니다");
                Console.ReadKey();
            } // else if - 관심과목 철회
        } // Method - applicationLecture

        // 학생데이터 설정하기
        public void setStudentData(int row, int mode)
        {
            StudentLectureVO a = new StudentLectureVO(
                arrayGetValueString(row, 1),
                arrayGetValueString(row, 2),
                arrayGetValueString(row, 3),
                arrayGetValueString(row, 4),
                arrayGetValueString(row, 5),
                arrayGetValueString(row, 6),
                arrayGetValueString(row, 7),
                arrayGetValueString(row, 8)
                );

            if (mode == 1)
            {
                singleton.ApplicationLectureNum += 1;
                singleton.StudentLectureList.Add(a);
            } // if
            else
            {
                singleton.ApplicationInterestLectureNum += 1;
                singleton.InterestLectureList.Add(a);
            } // else
        } // Method - setStudentData

        public string arrayGetValueString(int row, int column)
        {
            return Convert.ToString(singleton.getArrayData().GetValue(row, column));
        } // Method - arrayGetValueSring

        // String 을 입력받는다
        // singleton 의 array data 에서 전부찾는다 [i][index = 2]
        // .contains 를 이용해서 true 로 나오는것들을 전부 반환받는다 (i값을 반환 받으면 될듯)
        public List<int> searchLectureIndex(string strData, int index)
        {
            string str;
            int count = 0;

            List<int> LectureNameIndex = new List<int>();
            for (int i = 1; i < 104; i++)
            {
                str = (string)singleton.getArrayData().GetValue(i, index);
                if (str.Contains(strData))
                {
                    count++;
                    LectureNameIndex.Add(i);
                } // if
            } // for
            if (count == 0)
            {
                LectureNameIndex.Add(-1);
            }
            return LectureNameIndex;
        } // Method - searchLectureName

        // 과목의 중복을 체크할때에는 학수번호만 같아도 신청이 안되게끔 설계
        public bool lectureDuplicationCheck(string num, int mode)
        {
            int currentRegisterLectureNum;

            if (mode == 1)
            {
                currentRegisterLectureNum = singleton.StudentLectureList.Count;
                if (singleton.StudentLectureList.Count == 0)
                {
                    return true;
                } // if
                for (int i = 0; i < currentRegisterLectureNum; i++)
                {
                    if (singleton.StudentLectureList[i].Num == num)
                    {
                        return false;
                    } // if
                } // for
            } // if
            else if (mode == 2)
            {
                currentRegisterLectureNum = singleton.InterestLectureList.Count;
                if (singleton.InterestLectureList.Count == 0)
                {
                    return true;
                } // if
                for (int i = 0; i < currentRegisterLectureNum; i++)
                {
                    if (singleton.InterestLectureList[i].Num == num)
                    {
                        return false;
                    } // if
                } // for
            } // else if

            return true;
        } // Method - lectureDuplicationCheck


        // 관심과목에서 수강신청하기
        public void registerFromILecture()
        {
            Console.Clear();

            List<StudentLectureVO> interectLectureList = singleton.InterestLectureList;
            string inputLectureNum;
            string inputLecClasses;

            print.title("관심과목 내역");
            print.lectureTitle();
            for (int i = 0; i <= singleton.ApplicationInterestLectureNum; i++)
            {
                print.applicationLecture(i, 2);
            } // for

            if (singleton.ApplicationInterestLectureNum == -1)
            {
                Console.WriteLine("관심과목에 담겨진 과목이 없습니다");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\n무엇을 신청하시겠습니까? (학수번호로 입력해주세요)");
            Console.WriteLine("이전 메뉴로 가시려면 Enter 두번을 눌러주세요");
            Console.Write("→ ");
            inputLectureNum = Console.ReadLine();
            Console.WriteLine("해당 과목의 분반을 입력해주세요 (Ex. 001, 002, 003)");
            Console.Write("→ ");
            inputLecClasses = Console.ReadLine();

            for (int i = 0; i <= singleton.ApplicationInterestLectureNum; i++)
            {
                if (interectLectureList[i].Num == inputLectureNum && interectLectureList[i].Classes == inputLecClasses)
                {
                    if (!setCheckTimeTable(interectLectureList[i].Name, interectLectureList[i].Time, 1))
                    { // 만약, TimeTable 배열에 이미 다른값(1)이 들어가 있다면, 시간이 중복되는 것이므로 예외처리
                        Console.WriteLine("시간이 중복됩니다");
                        Console.ReadKey();
                        return;
                    } // if
                    if (!lectureDuplicationCheck(inputLectureNum, 1))
                    { // 중복이라면
                        Console.WriteLine("이미 수강신청된 학수번호입니다.");
                        Console.ReadKey();
                        return;
                    } // if
                    singleton.StudentGrade += Convert.ToInt32(interectLectureList[i].Point);
                    if (singleton.StudentGrade > 19)
                    {// 신청 학점이 18학점을 넘을경우
                        Console.WriteLine("수강신청은 18학점까지 가능합니다");
                        singleton.StudentGrade -= Convert.ToInt32(interectLectureList[i].Point); // 학점을 다시 빼줌
                        Console.ReadKey();
                        return;
                    } // if

                    singleton.ApplicationInterestLectureNum -= 1;
                    singleton.InterestGrade -= Convert.ToInt32(interectLectureList[i].Point);
                    singleton.ApplicationLectureNum += 1;
                    singleton.StudentLectureList.Add(interectLectureList[i]);
                    interectLectureList.RemoveAt(i);
                } // if
            } // for
        } // Method - 관심과목에서 수강신청하기

        public bool setCheckTimeTable(string name, string str, int mode)
        {
            string day;
            string firstDay = "", secondDay = "";
            int time;
            int startTime, endTime;
            int count = 0;
            int row = 0, column = 0;
            string checkData = "";

            day = remainOnlyKorean(str);
            if (day.Length == 2)
            {
                char[] divisonDay = day.ToCharArray();
                firstDay = Convert.ToString(divisonDay[0]);
                secondDay = Convert.ToString(divisonDay[1]);
            } // if
            else if (day.Length == 1)
            {
                firstDay = day;
            } // else if

            time = remainOnlyNumber(str);
            startTime = (time / 10000);
            endTime = time - (startTime * 10000);
            count = (endTime - startTime) / 50;
            row = ((startTime - 900) / 100) * 2;

            if ((startTime - 900) % 100 > 0)
            {
                row++;
            }
            if ((endTime - startTime) % 50 == 30)
            {
                count++;
            }

            day = firstDay;

            while (true)
            {
                switch (day)
                {
                    case "월":
                        column = 0;
                        break;
                    case "화":
                        column = 1;
                        break;
                    case "수":
                        column = 2;
                        break;
                    case "목":
                        column = 3;
                        break;
                    case "금":
                        column = 4;
                        break;
                } // switch


                for (int i = row; i < row + count; i++)
                {
                    checkData += singleton.TimeTableCheck[i, column];
                } // for

                for (int i = row; i < row + count; i++)
                {
                    if (checkData == "" && mode == 1)
                    {
                        singleton.TimeTableCheck[i, column] = name;
                    }
                    else if (mode == 2)
                    {
                        singleton.TimeTableCheck[i, column] = "";
                    }
                    else
                    {
                        return false;
                    }
                } // for
                if (day == secondDay || secondDay == "")
                {
                    break;
                } // if
                if (secondDay != "")
                {
                    day = secondDay;
                    continue;
                } // if
            } // while
            return true;
        } // Method - setCheckTimeTable

    public string remainOnlyKorean(string str)
        {
            StringBuilder sb = new StringBuilder();
            char[] c = str.ToCharArray();
            string returnStr;

            for (int i = 0; i < str.Length; i++)
            {
                if ((c[i] > '\uAC00' && c[i] <= '\uD7AF') || (c[i] >= '\u1100' && c[i] <= '\u11FF') || (c[i] >= '\u3130' && c[i] <= '\u318F'))
                {
                    sb.Append(c[i]);
                }
                else
                    sb.Append("");
            } // for

            returnStr = Convert.ToString(sb);

            return returnStr;
        } // Method - remainOnlyKorean

        public int remainOnlyNumber(string str)
        {
            StringBuilder sb = new StringBuilder();

            string strTmp = Regex.Replace(str, @"\D", "");
            //char[] c = strTmp.ToCharArray();

            return Convert.ToInt32(strTmp);
        } // Class LectureFunction
    }
}
