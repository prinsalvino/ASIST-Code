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
    public class CoachControllerTest
    {
        HttpClient Client { get; }

        string token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJDb2FjaCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJib2JAbWFpbC5jb20iLCJVc2VySWQiOiI0IiwibmJmIjoxNjM1MTk5NzYxLCJleHAiOjE2MzUyODYxNjEsImlhdCI6MTYzNTE5OTc2MSwiaXNzIjoiRGVidWdJc3N1ZXIiLCJhdWQiOiJEZWJ1Z0F1ZGllbmNlIn0.NAtwpoFVJhDxRX_nCuZo1wX1yW4DzS1rbcWyOrIqgdQ";
        string studentToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJTdHVkZW50IiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6ImFsZXhAbWFpbC5jb20iLCJVc2VySWQiOiIyIiwibmJmIjoxNjM1MTEzNzcxLCJleHAiOjE2MzUyMDAxNzEsImlhdCI6MTYzNTExMzc3MSwiaXNzIjoiRGVidWdJc3N1ZXIiLCJhdWQiOiJEZWJ1Z0F1ZGllbmNlIn0.zwk42d2Gs_FfyLLxCrbrQf_D2cnE2hSpMgbAPBuY47w";
        public CoachControllerTest()
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
        public async Task GetAllCoachReturnCoaches()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage Response = await Client.GetAsync($"api/coaches");
            Assert.AreEqual(HttpStatusCode.OK, Response.StatusCode);
            string Message = await Response.Content.ReadAsStringAsync();
            var coaches = JsonConvert.DeserializeObject<IEnumerable<Coach>>(Message);
            Assert.That(coaches, Is.InstanceOf(typeof(IEnumerable<Coach>)));
        }

        [Test]
        public async Task GetCoachByIdReturnCorrectCoach()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage Response = await Client.GetAsync($"api/coaches/4");
            Assert.AreEqual(HttpStatusCode.OK, Response.StatusCode);
            string Result = await Response.Content.ReadAsStringAsync();
            var coach = JsonConvert.DeserializeObject<Coach>(Result);
            Assert.That(coach, Is.InstanceOf(typeof(Coach)));
            Assert.AreEqual(coach.UserId, 4);
        }

        [Test]
        public async Task GetOrganisationByCoachIdShouldReturnListOfOrganisations()
        {
             Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage Response = await Client.GetAsync($"api/coaches/4/organisations");
            Assert.AreEqual(HttpStatusCode.OK, Response.StatusCode);
            string Result = await Response.Content.ReadAsStringAsync();
            var orgs = JsonConvert.DeserializeObject<IEnumerable<Organisation>>(Result);
            Assert.That(orgs, Is.InstanceOf(typeof(IEnumerable<Organisation>)));
            Assert.AreEqual(orgs.Count(), 7);
        }

        [Test]
        public async Task GetAllCoachPerformedByStudentGiveForbidden()
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", studentToken);

            HttpResponseMessage Response = await Client.GetAsync($"api/coaches");
            Assert.AreEqual(HttpStatusCode.Forbidden, Response.StatusCode);
        }

    }
}
