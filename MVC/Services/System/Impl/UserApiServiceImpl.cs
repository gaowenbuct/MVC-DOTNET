using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC.Models.System;
using RestSharp;
using MVC.Services.Impl;

namespace MVC.Services.System.Impl
{
    public class UserApiServiceImpl : BaseServiceImpl, UserApiService
    {
        private string url = "http://localhost:8080/Web/";
        private string resource = "api/user/";
        private class User1
        {
            public string username { get; set; }
            public string password { get; set; }
        }
        public bool doCreate(User user)
        {
            getLogger(this).Info("doCreate");
            var client = new RestClient(url);
            var request = new RestRequest(resource, Method.POST);
            request.AddJsonBody(user);
            var response = client.Execute(request);
            return true;
        }

        public bool doDelete(string username)
        {
            var client = new RestClient(url);
            var request = new RestRequest(resource+username, Method.DELETE);
            var response = client.Execute(request);
            return true;
        }

        public List<User> doFindAll()
        {
            var client = new RestClient(url);
            var request = new RestRequest(resource, Method.GET);
            var response = client.Execute<List<User>>(request);
            return response.Data;
        }

        public User doGet(string username)
        {
            var client = new RestClient(url);
            var request = new RestRequest(resource+username, Method.GET);
            var response = client.Execute(request);
            return null;
        }

        public bool doUpdate(User user)
        {
            var client = new RestClient(url);
            var request = new RestRequest(resource, Method.PUT);
            request.AddJsonBody(user);
            var response = client.Execute(request);
            return true;
        }
    }
}