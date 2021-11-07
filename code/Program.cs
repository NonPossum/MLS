using System;
using System.Threading.Tasks;
using Sender;

namespace PubKeyLogger
{
    class Program
    {
        static async Task Main()
        {
            //key should be length of data or bigger in this type of crypt
            PubSender test = new PubSender("my secret", "5ya5emm34");
            var res = await test.sendtoCDA("https://www.cda.pl/video/822223108");

            Console.WriteLine(res.StatusCode);
            Console.ReadKey();

        }
    }

    
}
