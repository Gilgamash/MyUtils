using MyUtils.T4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            TableInfoEntity table = DBEntityHelper.GetTableInfo(@"H:\githubformine\MyUtils\ConsoleApplication\", "TCInterVacationCommon", "ImageBasicInfo");
            DBEntityHelper.SetModel(table);
        }
    }
}
