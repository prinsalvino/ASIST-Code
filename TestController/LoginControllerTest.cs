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
    public class LoginControllerTest
    {
        HttpClient Client { get; }


        public LoginControllerTest()
        {
            Client = new HttpClient();
            string HostName = Environment.GetEnvironmentVariable("functionHostName");
            if (HostName == null)
            {
                HostName = $"http://localhost:{7071}"; // Fallback for local debugging purposes
            }
            Client.BaseAddress = new Uri(HostName);
        }


        [Test]
        public async Task LoginGiveToken()
        {

            UserLogin userLogin = new UserLogin()
            {
                EmailAddress = "kate@mail.com",
                Password = "kate123"
            };
            var jsonLogin = JsonConvert.SerializeObject(userLogin);
            var data = new StringContent(jsonLogin, Encoding.UTF8, "application/json");
            HttpResponseMessage Response = await Client.PostAsync($"api/login", data);

            string Message = await Response.Content.ReadAsStringAsync();

            Assert.AreEqual(HttpStatusCode.OK, Response.StatusCode);
            Assert.IsTrue(Message.Contains("accessToken"));
        }

        [Test]
        public async Task IncorrectLoginGiveNotFound()
        {

            UserLogin userLogin = new UserLogin()
            {
                EmailAddress = "213mail.com",
                Password = "34"
            };
            var jsonLogin = JsonConvert.SerializeObject(userLogin);
            var data = new StringContent(jsonLogin, Encoding.UTF8, "application/json");
            HttpResponseMessage Response = await Client.PostAsync($"api/login", data);


            Assert.AreEqual(HttpStatusCode.NotFound, Response.StatusCode);
        }
    }
}
