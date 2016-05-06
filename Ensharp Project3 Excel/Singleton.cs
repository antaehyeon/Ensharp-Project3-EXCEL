using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Threading;

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

        private Array data;
        private List<StudentLectureVO> studentLectureList;
        private List<StudentLectureVO> interestLectureList;

        private int applicationLectureNum = -1;
        private int applicationInterestLectureNum = -1;
        private int studentGrade = 0;
        private int interestGrade = 0;

        private string[,] timeTableCheck;

        public Singleton()
        {
            ExcelApp = new Excel.Application();
            workbook = ExcelApp.Workbooks.Open(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\컴퓨터공학과.xlsx");
            sheets = workbook.Sheets;
            worksheet = sheets["Sheet1"] as Excel.Worksheet;
            cellRange = worksheet.get_Range("A2", "H104") as Excel.Range;
            data = cellRange.Cells.Value2;
            StudentLectureList = new List<StudentLectureVO>();
            InterestLectureList = new List<StudentLectureVO>();
            TimeTableCheck = new string[20, 5];

            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    timeTableCheck[i, j] = "";
                }
            }

        }

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

        public void setTimeTableCheck(int row, int column, string str)
        {
            timeTableCheck[row, column] = str;
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

            for (int i = 1; i <= 6; i++)
            {
                ws.Cells[1, i] = dayArray[i - 1];
            }

            for (int i = 2; i <= 21; i++)
            {
                combineTime = "";
                combineTime += timeArray[i - 2] + " ~ " + timeArray[i - 1];
                ws.get_Range("A1", "A20").ColumnWidth = 12;
                ws.get_Range("B1", "F20").ColumnWidth = 24;

                ws.Cells[i, 1] = combineTime;
            }

            for (int i = 2; i <= 21; i++)
            {
                for (int j = 2; j <= 6; j++)
                {
                    ws.Cells[i, j] = timeTableCheck[i - 2, j - 2];
                }
            }

            Console.WriteLine("바탕화면에 '나의시간표'로 저장됩니다");

            wb.SaveAs(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\나의 시간표.xlsx");
            wb.Close(true);
            ExcelApp.Quit();

            Console.WriteLine("저장이 완료되었습니다");
            Console.WriteLine("2초 후에 메인메뉴로 이동합니다");

            Thread.Sleep(2000);

        }
    }
}
