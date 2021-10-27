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
    public class OrganisationControllerTest
    {

        HttpClient Client { get; }

        string token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJDb2FjaCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJib2JAbWFpbC5jb20iLCJVc2VySWQiOiI0IiwibmJmIjoxNjM1MTk5NzYxLCJleHAiOjE2MzUyODYxNjEsImlhdCI6MTYzNTE5OTc2MSwiaXNzIjoiRGVidWdJc3N1ZXIiLCJhdWQiOiJEZWJ1Z0F1ZGllbmNlIn0.NAtwpoFVJhDxRX_nCuZo1wX1yW4DzS1rbcWyOrIqgdQ";
        string studentToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJTdHVkZW50IiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6ImFsZXhAbWFpbC5jb20iLCJVc2VySWQiOiIyIiwibmJmIjoxNjM1MTEzNzcxLCJleHAiOjE2MzUyMDAxNzEsImlhdCI6MTYzNTExMzc3MSwiaXNzIjoiRGVidWdJc3N1ZXIiLCJhdWQiOiJEZWJ1Z0F1ZGllbmNlIn0.zwk42d2Gs_FfyLLxCrbrQf_D2cnE2hSpMgbAPBuY47w";
        string adminToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJtaWNoYWVsc2NvdHRAbWFpbC5jb20iLCJVc2VySWQiOiI2IiwibmJmIjoxNjM1MTY4NjIyLCJleHAiOjE2MzUyNTUwMjIsImlhdCI6MTYzNTE2ODYyMiwiaXNzIjoiRGVidWdJc3N1ZXIiLCJhdWQiOiJEZWJ1Z0F1ZGllbmNlIn0.fSxgk7aV4CvjZnK954tY-3WsYW0C1ACsndnbe_7Kaoo";

        public OrganisationControllerTest()
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
        public async Task GetAllOrganisationReturnOrganisations()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage Response = await Client.GetAsync($"api/organisations");
            Assert.AreEqual(HttpStatusCode.OK, Response.StatusCode);
            string Message = await Response.Content.ReadAsStringAsync();
            var orgs = JsonConvert.DeserializeObject<IEnumerable<Organisation>>(Message);
            Assert.That(orgs, Is.InstanceOf(typeof(IEnumerable<Organisation>)));
            Assert.AreEqual(orgs.Where(x => x.OrganisationId == 2).FirstOrDefault().OrganisationName, "Asist academy");
        }

        [Test]
        public async Task GetOrganisationByIdReturnCorrectOrganisation()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage Response = await Client.GetAsync($"api/organisations/4");
            Assert.AreEqual(HttpStatusCode.OK, Response.StatusCode);
            string Message = await Response.Content.ReadAsStringAsync();
            var org = JsonConvert.DeserializeObject<Organisation>(Message);
            Assert.That(org, Is.InstanceOf(typeof(Organisation)));
            Assert.AreEqual(org.OrganisationId, 4);
            Assert.AreEqual(org.OrganisationName, "University of Utrecht");
        }


        [Test]
        public async Task GetStudentsByOrgIdReturnListOfStudent()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage Response = await Client.GetAsync($"api/organisations/1/students");
            Assert.AreEqual(HttpStatusCode.OK, Response.StatusCode);
            string Message = await Response.Content.ReadAsStringAsync();
            var stdnts = JsonConvert.DeserializeObject<IEnumerable<Student>>(Message);
            Assert.That(stdnts, Is.InstanceOf(typeof(IEnumerable<Student>)));
            Assert.AreEqual(stdnts.Where(x => x.UserId == 7).FirstOrDefault().OrganisationId, 1);
        }


        [Test]
        public async Task GetCoachesByOrgIdReturnListOfCoacht()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage Response = await Client.GetAsync($"api/organisations/1/coaches");
            Assert.AreEqual(HttpStatusCode.OK, Response.StatusCode);
            string Message = await Response.Content.ReadAsStringAsync();
            var stdnts = JsonConvert.DeserializeObject<IEnumerable<Coach>>(Message);
            Assert.That(stdnts, Is.InstanceOf(typeof(IEnumerable<Coach>)));
            Assert.AreEqual(stdnts.Where(x => x.UserId == 4).FirstOrDefault().UserRole, UserRoles.Coach);
        }
        [Test]
        public async Task AssignStudentToOrg()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
              
            HttpResponseMessage Response = await Client.PostAsync($"api/organisations/4/students/1", null);
            Assert.AreEqual(HttpStatusCode.NoContent, Response.StatusCode);

            HttpResponseMessage result = await Client.GetAsync($"api/organisations/4/students");
            string Message = await result.Content.ReadAsStringAsync();
            var stdnts = JsonConvert.DeserializeObject<IEnumerable<Student>>(Message);
            var stdnt = stdnts.Where(x => x.UserId == 1).First();

            Assert.AreEqual(stdnt.UserId, 1);
        }

        [Test]
        public async Task UnAssignCoachToOrgByAdminShouldDeleteCoach()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            HttpResponseMessage Response = await Client.DeleteAsync($"api/organisations/4/coaches/4");
            Assert.AreEqual(HttpStatusCode.NoContent, Response.StatusCode);

            HttpResponseMessage result = await Client.GetAsync($"api/organisations/4/coaches");
            string Message = await result.Content.ReadAsStringAsync();
            var coaches = JsonConvert.DeserializeObject<IEnumerable<Coach>>(Message);
            bool found = coaches.Where(x => x.UserId == 4).Any();
            Assert.False(found);
        }

        [Test]
        public async Task AssignCoachToOrgByAdminShouldAddCoach()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            HttpResponseMessage Response = await Client.PostAsync($"api/organisations/4/coaches/4", null);
            Assert.AreEqual(HttpStatusCode.NoContent, Response.StatusCode);

            HttpResponseMessage result = await Client.GetAsync($"api/organisations/4/coaches");
            string Message = await result.Content.ReadAsStringAsync();
            var coaches = JsonConvert.DeserializeObject<IEnumerable<Coach>>(Message);
            var coach = coaches.Where(x => x.UserId == 4).First();

            Assert.AreEqual(coach.UserId, 4);
        }

        [Test]
        public async Task AssignCoachToOrgByCoachShouldReturnForbidden()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage Response = await Client.PostAsync($"api/organisations/4/coaches/4", null);
            Assert.AreEqual(HttpStatusCode.Forbidden, Response.StatusCode);
        }
    }
}
