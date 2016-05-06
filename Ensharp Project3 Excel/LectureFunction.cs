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
            lectureIndex = new List<int>();
        }

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
            }

            Console.Write("→ ");

            inputStrData = Console.ReadLine();
            lectureIndex = searchLectureIndex(inputStrData, row);

            if (lectureIndex[0] == -1)
            {
                if (mode == 1)
                {
                    print.notExistsLecture();
                }
                else if (mode == 2)
                {
                    print.notExistsProfessor();
                }
                return -1;
            }

            print.lectureTitle();
            for (int i = 0; i < lectureIndex.Count; i++)
            {
                print.lectureInfoInArray(lectureIndex[i]);
            }
            return 0;
        }

        public void lectureApplication(int mode)
        {
            string inputLecNum, inputLecClass;

            if (mode == 1 || mode == 3)
            {
                Console.WriteLine("\n무엇을 신청하시겠습니까? (학수번호로 입력해주세요)");
                Console.WriteLine("이전 메뉴로 가시려면 Enter 두번을 눌러주세요");
                Console.Write("→ ");
            }
            else if (mode == 2 || mode == 4)
            {
                if (mode == 2 && singleton.StudentLectureList.Count == 0)
                {
                    Console.WriteLine("수강철회 할 과목이 존재하지 않습니다");
                    Console.ReadKey();
                    return;
                }
                else if (mode == 4 && singleton.InterestLectureList.Count == 0)
                {
                    Console.WriteLine("철회 할 관심과목이 존재하지 않습니다");
                    Console.ReadKey();
                    return;
                }
                Console.WriteLine("\n무엇을 철회하시겠습니까? (학수번호로 입력해주세요)");
                Console.WriteLine("이전 메뉴로 가시려면 Enter 두번을 눌러주세요");
                Console.Write("→ ");
            }
            inputLecNum = Console.ReadLine();

            Console.WriteLine("해당 과목의 분반을 입력해주세요 (Ex. 001, 002, 003)");
            Console.Write("→ ");
            inputLecClass = Console.ReadLine();


            if (mode == 1)
            {
                for (int i = 0; i < lectureIndex.Count; i++)
                {
                    if (inputLecNum.Equals(arrayGetValueString(lectureIndex[i], 1)) && inputLecClass.Equals(arrayGetValueString(lectureIndex[i], 3)))
                    {
                        if (lectureDuplicationCheck(inputLecNum, 1) == false)
                        {
                            Console.WriteLine("중복된 학수번호를 수강신청할 수 없습니다");
                            Console.ReadKey();
                            return;
                        }
                        if (!setCheckTimeTable(arrayGetValueString(lectureIndex[i], 2), arrayGetValueString(lectureIndex[i], 6), 1))
                        {
                            Console.WriteLine("시간이 중복됩니다");
                            Console.ReadKey();
                            return;
                        }
                        singleton.StudentGrade += Convert.ToInt32(singleton.getArrayData().GetValue(lectureIndex[i], 4));
                        if (singleton.StudentGrade > 19)
                        {
                            Console.WriteLine("수강신청은 18학점까지 가능합니다");
                            singleton.StudentGrade -= Convert.ToInt32(singleton.getArrayData().GetValue(lectureIndex[i], 4));
                            Console.ReadKey();
                            return;
                        }
                        setStudentData(lectureIndex[i], 1);
                    }
                }
            }
            else if (mode == 2)
            {
                for (int i = 0; i < singleton.StudentLectureList.Count; i++)
                {
                    if (inputLecNum.Equals(singleton.StudentLectureList[i].Num) && inputLecClass.Equals(singleton.StudentLectureList[i].Classes))
                    {
                        singleton.ApplicationLectureNum -= 1;
                        singleton.StudentGrade -= Convert.ToInt32(singleton.StudentLectureList[i].Point);
                        singleton.StudentLectureList.RemoveAt(i);
                        setCheckTimeTable(arrayGetValueString(lectureIndex[i], 2), arrayGetValueString(lectureIndex[i], 6), 2);
                        return;
                    }
                }
                Console.WriteLine("입력하신 정보와 맞는 수강신청 내역이 존재하지 않아, 수강철회를 할 수 없습니다");
                Console.ReadKey();
            }

            else if (mode == 3)
            {
                for (int i = 0; i < lectureIndex.Count; i++)
                {
                    if (inputLecNum.Equals(arrayGetValueString(lectureIndex[i], 1)) && inputLecClass.Equals(arrayGetValueString(lectureIndex[i], 3)))
                    {
                        if (lectureDuplicationCheck(inputLecNum, 2) == false)
                        {
                            Console.WriteLine("중복된 학수번호를 관심과목으로 담을 수 없습니다");
                            Console.ReadKey();
                            return;
                        }
                        singleton.InterestGrade += Convert.ToInt32(singleton.getArrayData().GetValue(lectureIndex[i], 4));
                        if (singleton.InterestGrade > 25)
                        {
                            Console.WriteLine("관심과목은 24학점까지 담을 수 있습니다");
                            singleton.InterestGrade -= Convert.ToInt32(singleton.getArrayData().GetValue(lectureIndex[i], 4));
                            Console.ReadKey();
                            return;
                        }
                        setStudentData(lectureIndex[i], 2);
                    }
                }
            }

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
                    }
                }
                Console.WriteLine("입력하신 정보와 맞는 관심과목 내역이 존재하지 않아, 관심과목 철회를 할 수 없습니다");
                Console.ReadKey();
            }
        }

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
            }
            else
            {
                singleton.ApplicationInterestLectureNum += 1;
                singleton.InterestLectureList.Add(a);
            }
        }

        public string arrayGetValueString(int row, int column)
        {
            return Convert.ToString(singleton.getArrayData().GetValue(row, column));
        }

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
                }
            }
            if (count == 0)
            {
                LectureNameIndex.Add(-1);
            }
            return LectureNameIndex;
        }

        public bool lectureDuplicationCheck(string num, int mode)
        {
            int currentRegisterLectureNum;

            if (mode == 1)
            {
                currentRegisterLectureNum = singleton.StudentLectureList.Count;
                if (singleton.StudentLectureList.Count == 0)
                {
                    return true;
                }
                for (int i = 0; i < currentRegisterLectureNum; i++)
                {
                    if (singleton.StudentLectureList[i].Num == num)
                    {
                        return false;
                    }
                }
            }
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
            }

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
                    {
                        Console.WriteLine("시간이 중복됩니다");
                        Console.ReadKey();
                        return;
                    }
                    if (!lectureDuplicationCheck(inputLectureNum, 1))
                    {
                        Console.WriteLine("이미 수강신청된 학수번호입니다.");
                        Console.ReadKey();
                        return;
                    }
                    singleton.StudentGrade += Convert.ToInt32(interectLectureList[i].Point);
                    if (singleton.StudentGrade > 19)
                    {
                        Console.WriteLine("수강신청은 18학점까지 가능합니다");
                        singleton.StudentGrade -= Convert.ToInt32(interectLectureList[i].Point);
                        Console.ReadKey();
                        return;
                    }

                    singleton.ApplicationInterestLectureNum -= 1;
                    singleton.InterestGrade -= Convert.ToInt32(interectLectureList[i].Point);
                    singleton.ApplicationLectureNum += 1;
                    singleton.StudentLectureList.Add(interectLectureList[i]);
                    interectLectureList.RemoveAt(i);
                }
            }
        }

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
            }
            else if (day.Length == 1)
            {
                firstDay = day;
            }

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
                }


                for (int i = row; i < row + count; i++)
                {
                    checkData += singleton.TimeTableCheck[i, column];
                }

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
                }
                if (day == secondDay || secondDay == "")
                {
                    break;
                }
                if (secondDay != "")
                {
                    day = secondDay;
                    continue;
                }
            }
            return true;
        }

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
            }

            returnStr = Convert.ToString(sb);

            return returnStr;
        }

        public int remainOnlyNumber(string str)
        {
            StringBuilder sb = new StringBuilder();

            string strTmp = Regex.Replace(str, @"\D", "");

            return Convert.ToInt32(strTmp);
        }
    }
}
