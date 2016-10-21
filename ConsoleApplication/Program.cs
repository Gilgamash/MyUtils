using MyUtils.Base.T4;
using MyUtils.T4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            TableInfoEntity table = new DBEntityHelper().GetTableInfo("ImageBasicInfo");
        }
    }
}
