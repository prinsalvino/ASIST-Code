using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ASIST_Web_API.DTO;
using Domain;
using Newtonsoft.Json;
using NUnit.Framework;

namespace TestController
{
    public class AdminControllerTest
    {

        HttpClient Client { get; }

        string token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJDb2FjaCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJib2JAbWFpbC5jb20iLCJVc2VySWQiOiI0IiwibmJmIjoxNjM1MTk5NzYxLCJleHAiOjE2MzUyODYxNjEsImlhdCI6MTYzNTE5OTc2MSwiaXNzIjoiRGVidWdJc3N1ZXIiLCJhdWQiOiJEZWJ1Z0F1ZGllbmNlIn0.NAtwpoFVJhDxRX_nCuZo1wX1yW4DzS1rbcWyOrIqgdQ";
        string adminEmail = "testAdmin@mail.com";
        string adminToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJtaWNoYWVsc2NvdHRAbWFpbC5jb20iLCJVc2VySWQiOiI2IiwibmJmIjoxNjM1MTY4NjIyLCJleHAiOjE2MzUyNTUwMjIsImlhdCI6MTYzNTE2ODYyMiwiaXNzIjoiRGVidWdJc3N1ZXIiLCJhdWQiOiJEZWJ1Z0F1ZGllbmNlIn0.fSxgk7aV4CvjZnK954tY-3WsYW0C1ACsndnbe_7Kaoo";

        public AdminControllerTest()
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
        public async Task GetAdminShouldReturnListOfAdmin()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            HttpResponseMessage Response = await Client.GetAsync($"api/v2/admins");
            Assert.AreEqual(HttpStatusCode.OK, Response.StatusCode);
            string Message = await Response.Content.ReadAsStringAsync();
            var sa = JsonConvert.DeserializeObject<IEnumerable<Admin>>(Message);
            Assert.That(sa, Is.InstanceOf(typeof(IEnumerable<Admin>)));
        }


        [Test]
        public async Task GetAdminByIdShouldReturnCorrectAdmin()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            HttpResponseMessage Response = await Client.GetAsync($"api/v2/admins/6");
            Assert.AreEqual(HttpStatusCode.OK, Response.StatusCode);
            string Message = await Response.Content.ReadAsStringAsync();
            var sa = JsonConvert.DeserializeObject<Admin>(Message);
            Assert.That(sa, Is.InstanceOf(typeof(Admin)));
            Assert.AreEqual(sa.UserId, 6);

        }

        [Test]
        public async Task AddAdminShouldGive201AndAddTheAdminToDatabase()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
            string dateInput = "Jan 1, 2011";
            var parsedDate = DateTime.Parse(dateInput);

            CreateAdminDto admin = new CreateAdminDto()
            {
                EmailAddress = adminEmail,
                Password = "test123",
                FirstName = "Admin",
                LastName = "Test"
            };
            var jsonStudent = JsonConvert.SerializeObject(admin);
            var data = new StringContent(jsonStudent, Encoding.UTF8, "application/json");

            HttpResponseMessage Response = await Client.PostAsync($"api/v2/admins", data);
            Assert.AreEqual(HttpStatusCode.Created, Response.StatusCode);

            Thread.Sleep(1000);


            HttpResponseMessage ResponseGet = await Client.GetAsync($"api/v2/admins");

            string Message = await ResponseGet.Content.ReadAsStringAsync();
            var sa = JsonConvert.DeserializeObject<IEnumerable<Admin>>(Message);
            var cs = sa.Where(x => x.EmailAddress == admin.EmailAddress).First();
            Assert.AreEqual(admin.EmailAddress, cs.EmailAddress);

        }

        [Test]
        public async Task DeleteAdminByIdShouldGive204AndRemoveAdminFromDB()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            HttpResponseMessage ResponseGet = await Client.GetAsync($"api/v2/admins");

            string Message = await ResponseGet.Content.ReadAsStringAsync();
            var sa = JsonConvert.DeserializeObject<IEnumerable<Admin>>(Message);
            var cs = sa.Where(x => x.EmailAddress == adminEmail).First();

            HttpResponseMessage Response = await Client.DeleteAsync($"api/v2/admins/{cs.UserId}");
            Assert.AreEqual(HttpStatusCode.NoContent, Response.StatusCode);

            Thread.Sleep(1000);


            HttpResponseMessage ResponseGet2 = await Client.GetAsync($"api/v2/admins");
            string Message2 = await ResponseGet2.Content.ReadAsStringAsync();
            var sa2 = JsonConvert.DeserializeObject<IEnumerable<Admin>>(Message2);
            foreach (var item in sa2)
            {
                Assert.AreNotEqual(item.EmailAddress, adminEmail);

            }
        }
    }
}
