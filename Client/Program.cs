using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static int count = 0;
        private static bool done = false, start = true;
        private static string filename = "";
        private static readonly string serverIp = "192.168.0.95";
        private static readonly string serverProtocol = "http";
        private static readonly int port = 5000;

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
            while (true)
            {
                Console.WriteLine("Please enter the name of image");
                filename = Console.ReadLine();
                Method();
                Console.WriteLine("Wait please...");
                string key = Console.ReadLine();
                if (key.ToLower() == "exit")
                    break;
                else
                    Console.Clear();
                
            }
        }

        static async void Method ()
        {
            //To get the location the assembly normally resides on disk or the install directory
            string path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;

            //once you have the path you get the directory with:
            var directory = Path.GetDirectoryName(path).Replace("file:\\", "").Replace("bin\\Debug\\netcoreapp2.0", "images\\");

            using (var stream = File.Open(directory + filename, FileMode.Open))
            {
                Byte[] bytes = new byte[(int)stream.Length];
                stream.Read(bytes, 0, (int)stream.Length);

                var base64Image = Convert.ToBase64String(bytes);
                string name = filename.Split(".jpg")[0];

                var dic = new Dictionary<string, string>
                {
                    {"img", base64Image },
                    {"name", name }
                };

                string json = JsonConvert.SerializeObject(dic);

                var content = new StringContent(json);
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                string api_path = "api/prediction";

                var response = await client.PostAsync($"{serverProtocol}://{serverIp}:{port}/{api_path}", content);

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

                Console.Write("\nType exit to end application, or press enter to resend image\n");
                
                
            }
        }
    }
}
