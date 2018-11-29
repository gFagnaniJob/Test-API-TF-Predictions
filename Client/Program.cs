using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Client
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static int count = 0;
        private static readonly Dictionary<int, string> classesDic = new Dictionary<int, string>
        {
            { 1, "component00" },
            { 2, "component01" },
            { 3, "component02a" },
            { 4, "component02b" },
            { 5, "component02c" },
            { 6, "component02d" },
            { 7, "component03a" },
            { 8, "component03b" },
            { 9, "component04a" },
            { 10, "component04b" },
            { 11, "component05a" },
            { 12, "component05b" }
        };

        static void Main(string[] args)
        {
            Method();
            Console.ReadKey(true);
        }

        static async void Method ()
        {
            //To get the location the assembly normally resides on disk or the install directory
            string path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;

            //once you have the path you get the directory with:
            var directory = Path.GetDirectoryName(path).Replace("file:\\", "").Replace("bin\\Debug\\netcoreapp2.0", "images\\");

            Console.WriteLine(directory);

            using (var stream = File.Open(directory + "P_20180911_102739_vHDR_On.jpg", FileMode.Open))
            {
                Byte[] bytes = new byte[(int)stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);

                var base64Image = Convert.ToBase64String(bytes);
                string name = "image" + count;

                var dic = new Dictionary<string, string>
                {
                    {"img", base64Image },
                    {"name", name }
                };

                string json = JsonConvert.SerializeObject(dic);

                var content = new StringContent(json);
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                var response = await client.PostAsync("http://192.168.0.95:5000/test", content);

                var responseString = await response.Content.ReadAsStringAsync();

                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response);

                Prediction p = JsonConvert.DeserializeObject<Prediction>(responseString);

                List<List<double>> classes = p.classes;
                List<List<double>> scores = p.scores;

                string Class = "";
                classesDic.TryGetValue((int) classes[0][0], out Class);

                double probability = scores[0][0];

                Console.WriteLine($"CLASS = {Class}\nPROBABILITY = {probability*100} %");
            }
        }
    }
}
