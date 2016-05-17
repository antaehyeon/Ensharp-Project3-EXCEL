using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
/// <summary>
/// 수강에 관련된 기능이 모여있는 Class
/// 수강신청에 관한 데이터는 Singleton Class를 이용하였다
/// </summary>
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
            lectureIndex = new List<int>();                 // 과목의 Index를 저장하기 위한 List
        } // Constructor

        /// <summary>
        /// 과목을 찾는 메소드, 역시 mode 를 통해서 기능을 구분하였다.
        /// mode 1 : 과목명 찾기
        /// mode 2 : 교수명 찾기
        /// 찾는 과목이 없다면(에러) -1을 리턴해주며, 정상적으로 찾았을 경우에는 0을 리턴한다.
        /// </summary>
        public int findClass(int row, int mode)
        {
            string inputStrData;

            Console.Clear();

            if (mode == 1)  print.title("검색할 과목명을 입력해주세요 (전체검색하시려면 Enter)");
            else            print.title("검색할 교수님 성함을 입력하세요");

            Console.Write("→ ");

            inputStrData = Console.ReadLine();
            lectureIndex = searchLectureIndex(inputStrData, row);

            // 찾는 과목명이 존재하지 않을경우 ( mode : 1 )
            // 찾는 교수명이 존재하지 않을경우 ( mode : 2 )
            if(lectureIndex.Count == 0)
            {
                if (mode == 1)      print.notExistsLecture();
                else if (mode == 2) print.notExistsProfessor();
                return -1;
            }

            print.lectureTitle();
            for (int i = 0; i < lectureIndex.Count; i++)
            {
                print.lectureInfoInArray(lectureIndex[i]);
            }
            return 0;
        }

        /// <summary>
        /// 해당 과목의 Index를 찾는 메소드
        /// 찾고싶은 string data (strData)와 해당 열(column)을 매개변수로 받고 찾는다
        /// 조건이 맞다면 IndexList 에 추가하고, 해당 List 를 리턴
        /// 문제점이라면 역시 반복의 갯수가 정해져있다는점
        /// </summary>
        public List<int> searchLectureIndex(string strData, int column)
        {
            string str;
            int count = 0;

            List<int> LectureNameIndex = new List<int>();
            for (int i = 1; i < 104; i++)
            {
                str = (string)singleton.getArrayData().GetValue(i, column);
                if (str.Contains(strData))                  // 위에서 매개변수로 받은 strData가 포함되어 있으면
                {
                    count++;
                    LectureNameIndex.Add(i);                  // List 에 ADD
                }
            }
            return LectureNameIndex;
        }

        /// <summary>
        /// MODE 1 : 수강신청
        /// MODE 2 : 수강철회
        /// MODE 3 : 관심과목담기
        /// MODE 4 : 관심과목철회
        /// </summary>
        public void lectureApplication(int mode)
        {
            string inputLecNum, inputLecClass;

            // 출력
            if (mode == 1 || mode == 3)
            {
                print.whatApplicationLecture();
            }
            else if (mode == 2 || mode == 4)
            {
                if (mode == 2 && singleton.StudentLectureList.Count == 0)                   // 만약 과목이 하나도 존재하지 않는다면
                {
                    print.noExistsRetractionLecture();                   // 수강철회할 과목이 없다고 출력
                    return;
                }
                else if (mode == 4 && singleton.InterestLectureList.Count == 0)
                {
                    print.noExistsRetractionInterestLecture();
                    return;
                }
                print.whatRetractionLecture();
            }

            // 학수번호와 분반을 입력받음
            // b를 입력하면 뒤로가기 구현
            inputLecNum = Console.ReadLine();
            if (inputLecNum == "b") return;

            print.enterClassNum();

            inputLecClass = Console.ReadLine();
            if (inputLecClass == "b") return;

            // MODE 1 : 수강신청
            if (mode == 1)
            {
                // 위에서 찾은 과목갯수로 반복문을 돌림
                for (int i = 0; i < lectureIndex.Count; i++)
                {
                    // 위에서 입력한 학수번호와 분반이 맞다면
                    if (inputLecNum.Equals(arrayGetValueString(lectureIndex[i], 1)) && inputLecClass.Equals(arrayGetValueString(lectureIndex[i], 3)))
                    {
                        // 이미 수강한 목록에 과목이 존재하는지 검사
                        if (lectureDuplicationCheck(inputLecNum, 1) == false)
                        {
                            print.duplicationErrorMessage();
                            return;
                        }
                        // 이미 수강한 목록의 과목과 시간이 겹치는지 검사
                        if (!setCheckTimeTable(arrayGetValueString(lectureIndex[i], 2), arrayGetValueString(lectureIndex[i], 6), 1))
                        {
                            print.duplicationTimeMessage();
                            return;
                        }
                        // 조건이 모두 아니라면 singleton Class 의 학점데이터에 해당 과목의 학점을 더해준다
                        singleton.StudentGrade += Convert.ToInt32(singleton.getArrayData().GetValue(lectureIndex[i], 4));
                        // 그런데, 신청한 과목의 학점이 19학점을 초과한다면
                        if (singleton.StudentGrade > 19)
                        {
                            // 학점을 다시 빼주고
                            singleton.StudentGrade -= Convert.ToInt32(singleton.getArrayData().GetValue(lectureIndex[i], 4));
                            // Error Message 를 출력한다
                            print.limitGrade18Message();
                            return;
                        }
                        // 수강한 과목 List에 데이터를 ADD 해주면서 마무리
                        setStudentData(lectureIndex[i], 1);
                        return;
                    }
                }
                // 함수의 조건을 전부 만족하지 못하고 나왔다면 (= 입력한 정보와 맞는 과목이 없다면) 에러메세지 출력
                print.incorrectData();
            }
            // MODE 2 : 수강철회
            else if (mode == 2)
            {
                for (int i = 0; i < singleton.StudentLectureList.Count; i++)
                {
                    // 입력한 학수번호와 분반이 신청한 과목 List에 존재한다면
                    if (inputLecNum.Equals(singleton.StudentLectureList[i].Num) && inputLecClass.Equals(singleton.StudentLectureList[i].Classes))
                    {
                        // 과목갯수를 1개 줄여주고
                        singleton.ApplicationLectureNum -= 1;
                        // 학점도 빼준다
                        singleton.StudentGrade -= Convert.ToInt32(singleton.StudentLectureList[i].Point);
                        // 시간표 에서도 해당과목이 쓰여졌던 것을 공백으로 덮어씌운다
                        setCheckTimeTable(singleton.StudentLectureList[i].Name, singleton.StudentLectureList[i].Time, 2);
                        // 그리고 해당 Index(i)의 List를 삭제한다
                        singleton.StudentLectureList.RemoveAt(i);
                        return;
                    }
                }
                // 존재하지 않는다면 ERROR MESSAGE
                print.incorrectData();
            }

            // MODE 3 : 관심과목 담기
            // 수강신청 부분에서 전부 예외로 걸리기 때문에,
            // 관심과목에서는 예외처리가 같은 학수번호만 아니면 전부 담을 수 있게 설계
            else if (mode == 3)
            {
                for (int i = 0; i < lectureIndex.Count; i++)
                {
                    // 입력한 학수번호와 분반이 Array data 에 존재한다면
                    if (inputLecNum.Equals(arrayGetValueString(lectureIndex[i], 1)) && inputLecClass.Equals(arrayGetValueString(lectureIndex[i], 3)))
                    {
                        // 이미 관심과목 목록에 중복된 학수번호가 있는지 검사
                        // true : 중복없음 false : 중복존재
                        if (lectureDuplicationCheck(inputLecNum, 2) == false)
                        {
                            print.duplicationInterestLectureErrorMessage();
                            return;
                        }
                        // 관심과목 학점에 신청한 과목의 학점을 더해줌
                        singleton.InterestGrade += Convert.ToInt32(singleton.getArrayData().GetValue(lectureIndex[i], 4));
                        // 만약 관심과목 학점이 24를 넘는다면
                        if (singleton.InterestGrade > 25)
                        {
                            // 학점을 다시 빼주고 에러메세지 출력후 함수종료
                            singleton.InterestGrade -= Convert.ToInt32(singleton.getArrayData().GetValue(lectureIndex[i], 4));
                            print.limitGrade24Message();
                            return;
                        }
                        // 현재 과목 데이터 수강 List에 저장
                        setStudentData(lectureIndex[i], 2);
                        return;
                    }
                }
                print.incorrectData();
            }

            // MODE 4 : 관심과목 철회
            else if (mode == 4)
            {
                for (int i = 0; i < singleton.InterestLectureList.Count; i++)
                {
                    // 입력한 학수번호와 분반이 관심과목List에 존재한다면
                    if (inputLecNum.Equals(singleton.InterestLectureList[i].Num) && inputLecClass.Equals(singleton.InterestLectureList[i].Classes))
                    {
                        singleton.ApplicationInterestLectureNum -= 1;
                        singleton.InterestGrade -= Convert.ToInt32(singleton.InterestLectureList[i].Point);
                        // 해당 List 지워버림
                        singleton.InterestLectureList.RemoveAt(i);
                        return;
                    }
                }
                print.noExistsRetractionLecture();
            }
        }

        // 수강신청 데이터를 설정하는 메소드
        // row : 해당과목 INDEX
        // MODE 1 : 수강신청 List
        // MODe 2 : 관심과목 List
        public void setStudentData(int row, int mode)
        {
            // 원래 반복문으로 구성하려 했으나, 생성자 타입에 맞춰야해서 전부 작성함..
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

            // 수강신청 쪽
            if (mode == 1)
            {
                singleton.ApplicationLectureNum += 1;
                singleton.StudentLectureList.Add(a);
            }
            // 관심과목 쪽
            else
            {
                singleton.ApplicationInterestLectureNum += 1;
                singleton.InterestLectureList.Add(a);
            }
        }

        // singleton 의 Array data 에서 string 형태로 반환시키는 메소드
        public string arrayGetValueString(int row, int column)
        {
            return Convert.ToString(singleton.getArrayData().GetValue(row, column));
        }

        // 과목의 중복체크 메소드
        // MODE 1 : 수강신청 부분
        // MODE 2 : 관심과목 부분
        public bool lectureDuplicationCheck(string num, int mode)
        {
            int currentRegisterLectureNum;

            // 수강신청
            if (mode == 1)
            {
                // 현재의 수강신청 갯수를 받아와서
                currentRegisterLectureNum = singleton.StudentLectureList.Count;
                // 존재하지 않는다면, true 를 리턴
                if (singleton.StudentLectureList.Count == 0)
                {
                    return true;
                }
                // 만약 존재한다면 false 를 리턴
                for (int i = 0; i < currentRegisterLectureNum; i++)
                {
                    if (singleton.StudentLectureList[i].Num == num)
                    {
                        return false;
                    }
                }
            }
            // 관심과목
            else if (mode == 2)
            {
                currentRegisterLectureNum = singleton.InterestLectureList.Count;
                if (singleton.InterestLectureList.Count == 0)
                {
                    return true;
                }
                for (int i = 0; i < currentRegisterLectureNum; i++)
                {
                    if (singleton.InterestLectureList[i].Num == num)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // 관심과목으로부터 등록하는 메소드 (관심과목 신청내역에서 수강신청하는 부분)
        public void registerFromInterestLecture()
        {
            Console.Clear();

            // singleton 의 List 를 받아온다
            List<StudentLectureVO> interestLectureList = singleton.InterestLectureList;
            string inputLectureNum;
            string inputLecClasses;

            // 관심과목이 신청되있는 항목을 Print
            print.title("관심과목 내역");
            print.lectureTitle();
            for (int i = 0; i <= singleton.ApplicationInterestLectureNum; i++)
            {
                print.applicationLecture(i, 2);
            }

            // 만약 아무것도 관심과목으로 되어있지 않다면
            if (singleton.ApplicationInterestLectureNum == -1)
            {
                print.noExistsInterestLecture();
                return;
            }

            // 학수번호와 분반을 입력받는다
            print.whatApplicationLecture();
            inputLectureNum = Console.ReadLine();
            if (inputLectureNum == "b") return;
            print.enterClassNum();
            inputLecClasses = Console.ReadLine();
            if (inputLecClasses == "b") return;

            // 관심과목 갯수만큼 반복문을 돌린다
            for (int i = 0; i <= singleton.ApplicationInterestLectureNum; i++)
            {
                // 입력한 정보와 맞다면
                if (interestLectureList[i].Num == inputLectureNum && interestLectureList[i].Classes == inputLecClasses)
                {
                    // 시간이 중복되는지 검사
                    if (!setCheckTimeTable(interestLectureList[i].Name, interestLectureList[i].Time, 1))
                    {
                        print.duplicationTimeMessage();
                        return;
                    }
                    // 학수번호와 중복하는지 검사
                    if (!lectureDuplicationCheck(inputLectureNum, 1))
                    {
                        print.duplicationErrorMessage();
                        return;
                    }
                    // 우선 학점을 더하고
                    singleton.StudentGrade += Convert.ToInt32(interestLectureList[i].Point);
                    // 학점이 중복되는지 검사
                    if (singleton.StudentGrade > 19)
                    {
                        print.limitGrade18Message();
                        singleton.StudentGrade -= Convert.ToInt32(interestLectureList[i].Point);
                        return;
                    }
                    // 우선 관심과목 갯수와 학점을 빼준다
                    singleton.ApplicationInterestLectureNum -= 1;
                    singleton.InterestGrade -= Convert.ToInt32(interestLectureList[i].Point);
                    // 그리고 수강신청갯수를 늘려주고
                    // 관심과목 List 에서 수강 List 로 데이터를 추가해주고
                    // 관심과목 List 의 정보를 삭제한다
                    singleton.ApplicationLectureNum += 1;
                    singleton.StudentLectureList.Add(interestLectureList[i]);
                    interestLectureList.RemoveAt(i);
                    return;
                }
            }
            print.incorrectData();
        }

        // 시간의 중복을 체크하고
        // 신청한 과목을 시간표(TimeTable)배열에 넣어주는 메소드
        // MODE 1 : string 데이터를 설정 (시간표에 과목을 넣을때)
        // MODE 2 : 데이터를 삭제 (시간표에서 과목을 삭제할때)
        public bool setCheckTimeTable(string nameParam, string timeParam, int mode)
        {
            int time, startTime, endTime;
            int count = 0, row = 0, column = 0;

            string day;
            string firstDay = "", secondDay = "", checkData = "";

            // 입력받은 과목의 시간데이터에서 한글만 추출
            day = remainOnlyKorean(timeParam);
            // 만약 2글자라면, 각각 하나씩 분할
            if (day.Length == 2)
            {
                char[] divisonDay = day.ToCharArray();
                firstDay = Convert.ToString(divisonDay[0]);
                secondDay = Convert.ToString(divisonDay[1]);
            }
            // 한글자라면 첫번째 데이터에만 데이터를 설정
            else
            {
                firstDay = day;
            }

            // 시간 계산해서 앞시간과 뒷시간을 따로 나눔
            // count 에는 30분마다의 체크값을 계산함 (시간 = 각 Line 수)
            time = remainOnlyNumber(timeParam);
            startTime = (time / 10000);
            endTime = time - (startTime * 10000);
            count = (endTime - startTime) / 50;
            row = ((startTime - 900) / 100) * 2; // 과목의 시간위치를 찾는부분

            // 숫자는 100을 넘어야 넘어가므로, 30분단위의 계산을 맞춰주기 위해서 
            //(즉, 시간은 60분이지만 일반적인 정수는 100이기 때문에 오차가생김)
            // 조건에 해당하면 값을 1씩 증가시켜줌
            if ((startTime - 900) % 100 > 0)
            {
                row++;
            }
            if ((endTime - startTime) % 50 == 30)
            {
                count++;
            }

            day = firstDay;

            // 요일마다 column 을 설정해준다
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
                }

                // 해당 부분에 데이터가 있는지 없는지 체크하기 위해서 문자열을 전부 더함
                for (int i = row; i < row + count; i++)
                {
                    checkData += singleton.TimeTableCheck[i, column];
                }

                // 시작위치부터 count 까지 반복하는데
                for (int i = row; i < row + count; i++)
                {
                    // 앞에서 구한 checkData 에 아무 데이터도 존재하지 않다면
                    if (checkData == "" && mode == 1)
                    {
                        singleton.TimeTableCheck[i, column] = nameParam;                    // 데이터(해당과목)를 대입
                    }
                    // 삭제모드
                    else if (mode == 2)
                    {
                        singleton.TimeTableCheck[i, column] = "";
                    }
                    // 그 무엇도 아니라면 false 리턴
                    else
                    {
                        return false;
                    }
                }
                // 요일이 한번일 경우 탈출하기 위한 조건문
                // if를 두번사용하게 된 이유는, 두번째 요일도 계산해서 전부 검색하고나서
                // 이쪽으로 올때 secondDay 일 경우도 탈출하게 해줘야하기 때문이다..
                if (day == secondDay || secondDay == "")
                {
                    break;
                }
                // secondDay 에 데이터가 존재한다면
                if (secondDay != "")
                {
                    day = secondDay; // 대입하고
                    continue;        // 조건을 다시 반복
                }
            }
            return true;
        }

        // string 문자열에서 한글만 추출해내는 메소드
        public string remainOnlyKorean(string str)
        {
            StringBuilder sb = new StringBuilder();
            char[] c = str.ToCharArray();

            for (int i = 0; i < str.Length; i++)
            {
                if ((c[i] > '\uAC00' && c[i] <= '\uD7AF') || (c[i] >= '\u1100' && c[i] <= '\u11FF') || (c[i] >= '\u3130' && c[i] <= '\u318F'))
                {
                    sb.Append(c[i]);
                }
                else
                    sb.Append("");
            }
            return Convert.ToString(sb);
        }

        // string 문자열에서 숫자만 추출해내는 메소드
        public int remainOnlyNumber(string str)
        {
            StringBuilder sb = new StringBuilder();

            string strTmp = Regex.Replace(str, @"\D", "");

            return Convert.ToInt32(strTmp);
        }
    }
}
