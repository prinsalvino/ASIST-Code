using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Newtonsoft.Json;
using NUnit.Framework;
namespace TestController
{
    public class TestAttemptControllerTest
    {

        HttpClient Client { get; }

        string token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJDb2FjaCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJib2JAbWFpbC5jb20iLCJVc2VySWQiOiI0IiwibmJmIjoxNjM1MTk5NzYxLCJleHAiOjE2MzUyODYxNjEsImlhdCI6MTYzNTE5OTc2MSwiaXNzIjoiRGVidWdJc3N1ZXIiLCJhdWQiOiJEZWJ1Z0F1ZGllbmNlIn0.NAtwpoFVJhDxRX_nCuZo1wX1yW4DzS1rbcWyOrIqgdQ";
        public TestAttemptControllerTest()
        {
            Client = new HttpClient();
            string HostName = Environment.GetEnvironmentVariable("functionHostName");
            if (HostName == null)
            {
                HostName = $"http://localhost:{7071}"; // Fallback for local debugging purposes
            }
            Client.BaseAddress = new Uri(HostName);
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }


        [Test]
        public async Task TestAttemptByStudentIdShouldReturnListOfTestAttemptsOftheStudentId()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage Response = await Client.GetAsync($"api/testsattempted/1");
            Assert.AreEqual(HttpStatusCode.OK, Response.StatusCode);
            string Message = await Response.Content.ReadAsStringAsync();
            var sa = JsonConvert.DeserializeObject<IEnumerable<TestAttempt>>(Message);
            Assert.That(sa, Is.InstanceOf(typeof(IEnumerable<TestAttempt>)));
            foreach (var t in sa)
            {
                Assert.AreEqual(t.StudentId, 1);
            }
        }

    }
}
