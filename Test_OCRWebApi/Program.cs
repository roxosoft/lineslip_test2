using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_OCRWebApi
{
    class Program
    {
        static RestConnector _restConnector = new RestConnector("https://localhost:44322/", 10000);

        static void Main(string[] args)
        {
            Test("image_with_text.jpg");
            Test("image_with_text.jpg");//test caching
            Test("Test.jpg");
            Console.ReadKey();
        }

        static void Test(string sampleFile)
        {
            var samplesDir = @"..\..\input";
            Console.WriteLine($"Processing file: {sampleFile}");
            var res = _restConnector.GetData("api/values", $@"{samplesDir}\{sampleFile}").Result;
            Console.WriteLine("Result:");
            Console.WriteLine(res);
            Console.WriteLine("==================================================================================================================");
        }
    }
}
