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
        // 구현하지 못한부분
        // 시간표 중복처리
        // 학점 제한
        // 신청한 과목에 대해 중복처리
        // 관심과목
        // 시간표 출력 및 저장
        // 로그인 (부가기능)

        // 중점
        // 객체지향적 구조
        // singleton
        // 함수의 재활용성

        static void Main(string[] args)
        {
            Start start = new Start();

            start.run();
        }
    }
}
