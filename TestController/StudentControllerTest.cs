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
    public class StudentControllerTest
    {

        HttpClient Client { get; }

        string token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJDb2FjaCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJib2JAbWFpbC5jb20iLCJVc2VySWQiOiI0IiwibmJmIjoxNjM1MTk5NzYxLCJleHAiOjE2MzUyODYxNjEsImlhdCI6MTYzNTE5OTc2MSwiaXNzIjoiRGVidWdJc3N1ZXIiLCJhdWQiOiJEZWJ1Z0F1ZGllbmNlIn0.NAtwpoFVJhDxRX_nCuZo1wX1yW4DzS1rbcWyOrIqgdQ";
        string studentEmail = "testStudent@mail.com";
        string adminToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJtaWNoYWVsc2NvdHRAbWFpbC5jb20iLCJVc2VySWQiOiI2IiwibmJmIjoxNjM1MTY4NjIyLCJleHAiOjE2MzUyNTUwMjIsImlhdCI6MTYzNTE2ODYyMiwiaXNzIjoiRGVidWdJc3N1ZXIiLCJhdWQiOiJEZWJ1Z0F1ZGllbmNlIn0.fSxgk7aV4CvjZnK954tY-3WsYW0C1ACsndnbe_7Kaoo";

        public StudentControllerTest()
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
        public async Task GetyStudentsShouldReturnListOfStudents()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage Response = await Client.GetAsync($"api/students");
            Assert.AreEqual(HttpStatusCode.OK, Response.StatusCode);
            string Message = await Response.Content.ReadAsStringAsync();
            var sa = JsonConvert.DeserializeObject<IEnumerable<Student>>(Message);
            Assert.That(sa, Is.InstanceOf(typeof(IEnumerable<Student>)));

        }


        [Test]
        public async Task GetStudentByIdShouldReturnCorrectStudent()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage Response = await Client.GetAsync($"api/students/1");
            Assert.AreEqual(HttpStatusCode.OK, Response.StatusCode);
            string Message = await Response.Content.ReadAsStringAsync();
            var sa = JsonConvert.DeserializeObject<Student>(Message);
            Assert.That(sa, Is.InstanceOf(typeof(Student)));
            Assert.AreEqual(sa.UserId, 1);

        }

        [Test]
        public async Task AddStudentShouldGive201AndAddTheAdminToDatabase()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string dateInput = "Jan 1, 2011";
            var parsedDate = DateTime.Parse(dateInput);

            CreateStudentDto student = new CreateStudentDto()
            {
                EmailAddress = studentEmail,
                Password = "test123", 
                OrganisationId = 1,  
                FirstName = "Student",
                LastName = "Test", DateOfBirth = parsedDate,Gender=Gender.Male, UserRoles=UserRoles.Student
            };
            var jsonStudent = JsonConvert.SerializeObject(student);
            var data = new StringContent(jsonStudent, Encoding.UTF8, "application/json");

            HttpResponseMessage Response = await Client.PostAsync($"api/students", data);
            Assert.AreEqual(HttpStatusCode.Created, Response.StatusCode);

            Thread.Sleep(1000);

            HttpResponseMessage ResponseGet = await Client.GetAsync($"api/students");

            string Message = await ResponseGet.Content.ReadAsStringAsync();
            var sa = JsonConvert.DeserializeObject<IEnumerable<Student>>(Message);
            var cs = sa.Where(x => x.EmailAddress == student.EmailAddress).First();
            Assert.AreEqual(student.EmailAddress, cs.EmailAddress);

        }

        [Test]
        public async Task UpdateStudentByIdUpdateStudentFromDatabase()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
            string newEmail = "newEmail@mail.com";
            HttpResponseMessage ResponseGet = await Client.GetAsync($"api/students");

            string Message = await ResponseGet.Content.ReadAsStringAsync();
            var sa = JsonConvert.DeserializeObject<IEnumerable<Student>>(Message);
            var cs = sa.Where(x => x.EmailAddress == studentEmail).First();

            ModifyStudentDto modifyStudentDto = new ModifyStudentDto()
            {
                EmailAddress = newEmail,
                Password = "test123",
                OrganisationId = 1,
                FirstName = "Student",
                LastName = "Test"
            };
            var jsonStudent = JsonConvert.SerializeObject(modifyStudentDto);

            HttpResponseMessage Response = await Client.PutAsJsonAsync($"api/students/{cs.UserId}", jsonStudent);
            Assert.AreEqual(HttpStatusCode.NoContent, Response.StatusCode);

            Thread.Sleep(1000);

            HttpResponseMessage ResponseGet2 = await Client.GetAsync($"api/students");
            string Message2 = await ResponseGet2.Content.ReadAsStringAsync();
            var sa2 = JsonConvert.DeserializeObject<Student>(Message2);
            Assert.AreEqual(sa2.EmailAddress, newEmail);
        }

        [Test]
        public async Task DeleteStudentByIdRemoveStudentFromDatabase()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            HttpResponseMessage ResponseGet = await Client.GetAsync($"api/students");

            string Message = await ResponseGet.Content.ReadAsStringAsync();
            var sa = JsonConvert.DeserializeObject<IEnumerable<Student>>(Message);
            var cs = sa.Where(x => x.EmailAddress == studentEmail).First();

            HttpResponseMessage Response = await Client.DeleteAsync($"api/students/{cs.UserId}");
            Assert.AreEqual(HttpStatusCode.NoContent, Response.StatusCode);

            Thread.Sleep(1000);

            HttpResponseMessage ResponseGet2 = await Client.GetAsync($"api/students");
            string Message2 = await ResponseGet2.Content.ReadAsStringAsync();
            var sa2 = JsonConvert.DeserializeObject<IEnumerable<Student>>(Message2);
            foreach (var item in sa2)
            {
                Assert.AreNotEqual(item.EmailAddress, studentEmail);

            }
        }
    }
}
