using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace EnSharp_Project_3_EXCEL
{
    class Program
    {
        static void Main(string[] args)
        {
            Start start = new Start();

            start.run();                    // Start Class 를 통해서 프로그램을 시작
        }
    }
}
