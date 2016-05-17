using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Threading;
/// <summary>
/// 한쪽에서 객체를 모두 관리하기 위해서 singleton Pattern 을 이용
/// </summary>
namespace EnSharp_Project_3_EXCEL
{
    class Singleton
    {
        private static Singleton singleton;

        private Excel.Application ExcelApp;
        private Excel.Workbook workbook;
        private Excel.Sheets sheets;
        private Excel.Worksheet worksheet;
        private Excel.Range cellRange;

        private Array data; // 엑셀데이터
        private List<StudentLectureVO> studentLectureList;
        private List<StudentLectureVO> interestLectureList;

        private int applicationLectureNum = -1; // 수강신청된 갯수
        private int applicationInterestLectureNum = -1; // 관심과목 신청된 갯수
        private int studentGrade = 0;  // 수강신청된 학점
        private int interestGrade = 0; // 관심과목신청된 학점

        private string[,] timeTableCheck; // 과목시간의 중복을 체크하기 위해서 배열을 선언


        public Singleton()
        {
            ExcelApp = new Excel.Application();
            workbook = ExcelApp.Workbooks.Open(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\컴퓨터공학과.xlsx");
            sheets = workbook.Sheets;
            worksheet = sheets["Sheet1"] as Excel.Worksheet;
            cellRange = worksheet.get_Range("A2", "H104") as Excel.Range;                  // 엑셀의 데이터를 불러오는 부분
            data = cellRange.Cells.Value2;
            StudentLectureList = new List<StudentLectureVO>();                  // 학생의 수강과목 List
            InterestLectureList = new List<StudentLectureVO>();                 // 학생의 관심과목 List
            TimeTableCheck = new string[20, 5];                  // 시간표 중복처리 및 출력을 위한 Check Array

            // 위의 Check Array 를 빈문자열로 초기화
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    timeTableCheck[i, j] = "";
                }
            } // for - 초기화 끝
        } // Constructor

        public static Singleton GetInstance()
        {
            if (singleton == null) singleton = new Singleton();
            return singleton;
        }

        internal List<StudentLectureVO> StudentLectureList
        {
            get { return studentLectureList; }
            set { studentLectureList = value; }
        }

        public int ApplicationLectureNum
        {
            get { return applicationLectureNum; }
            set { applicationLectureNum = value; }
        }
        public int StudentGrade
        {
            get { return studentGrade; }
            set { studentGrade = value; }
        }

        public int InterestGrade
        {
            get { return interestGrade; }
            set { interestGrade = value; }
        }

        public int ApplicationInterestLectureNum
        {
            get { return applicationInterestLectureNum; }
            set { applicationInterestLectureNum = value; }
        }

        internal List<StudentLectureVO> InterestLectureList
        {
            get { return interestLectureList; }
            set { interestLectureList = value; }
        }

        public string[,] TimeTableCheck
        {
            get { return timeTableCheck; }
            set { timeTableCheck = value; }
        }

        public string getTimeTableByIndex(int row, int column)
        {
            return TimeTableCheck[row, column];
        }

        public Array getArrayData()
        {
            return data;
        }

        public void setArrayData(int row, int column, int param)
        {
            data.SetValue(param, row, column);
        }

        public void exitExcel()
        {
            ExcelApp.Workbooks.Close();
            ExcelApp.Quit();
        }

        /// <summary>
        /// Excel 로 저장하는 Method
        /// TimeTableCheck 에 저장된 string 을 그대로 저장시켜 준다
        /// 여기서 느낀것은 EXCEL 은 무조건 1,1 부터 시작한다는 것
        /// </summary>
        public void storeExcel()
        {
            string combineTime = "";

            Excel.Application excelApp = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;

            excelApp = new Excel.Application();
            wb = excelApp.Workbooks.Add();
            ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;

            string[] dayArray = new string[] { "시간표", "월", "화", "수", "목", "금" };
            string[] timeArray = new string[] {"09:00", "09:30", "10:00", "10:30", "11:00", "11:30", "12:00", "12:30", "13:00", "13:30", "14:00",
            "14:30", "15:00", "15:30", "16:00", "16:30", "17:00", "17:30", "18:00", "18:30", "19:00"};

            // 가로, 요일을 엑셀에 Write
            for (int i = 1; i <= 6; i++)
            {
                ws.Cells[1, i] = dayArray[i - 1];
            }

            // 세로, 시간부분을 Write
            for (int i = 2; i <= 21; i++)
            {
                combineTime = "";
                combineTime += timeArray[i - 2] + " ~ " + timeArray[i - 1];
                ws.get_Range("A1", "A20").ColumnWidth = 12;                 // 시간부분 너비12로 조정
                ws.get_Range("B1", "F20").ColumnWidth = 24;                 // 과목이름부분 너비 24로 조정

                ws.Cells[i, 1] = combineTime;
            }

            // 내용, 실제 수강한 과목을 Write
            for (int i = 2; i <= 21; i++)
            {
                for (int j = 2; j <= 6; j++)
                {
                    ws.Cells[i, j] = timeTableCheck[i - 2, j - 2];
                }
            }

            Console.WriteLine("바탕화면에 '나의시간표'로 저장됩니다");

            // 바탕화면에 나의시간표.xlsx 이라는 이름으로 저장
            wb.SaveAs(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\나의 시간표.xlsx");
            wb.Close(true);
            ExcelApp.Quit();

            Console.WriteLine("저장이 완료되었습니다");
            Console.WriteLine("2초 후에 메인메뉴로 이동합니다");

            Thread.Sleep(2000);
        }
    }
}
