using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
/// <summary>
/// 예외처리를 담당하는 Class
/// </summary>
namespace EnSharp_Project_3_EXCEL
{
    class Exception
    {
        public Exception() { } // Constructor

        public int inputData(int num)
        {
            string str;
            str = Console.ReadLine();

            if (ExceptionString(str, num) != -1)
                return Convert.ToInt32(str);
            else
                return -1;
        }

        public int ExceptionString(string str, int num)
        {
            int[] array = new int[num];

            for (int i = 0; i < num; i++)
            {
                array[i] = i + 1;
            }

            for (int i = 0; i < num; i++)
            {
                if (str == Convert.ToString(array[i]))
                    return 0;
            }
            Console.WriteLine("              잘못 입력하셨습니다 :D");
            Console.ReadKey();
            return -1;
        }

        public int extractNumber(string strData)
        {
            string strTmp = Regex.Replace(strData, @"\D", "");
            int nTmp = int.Parse(strTmp);

            return nTmp;
        }
    }
}
