﻿using Flurl.Http;
using Microsoft.AspNetCore.Mvc;
using RoomAndResourcesScheduler.Attributes;
using RoomAndResourcesScheduler.Models;
using System.Diagnostics;

namespace RoomAndResourcesScheduler.Controllers
{
    public class HomeController : Controller
    {
        private const string LOGIN_URL = "/User/Login";

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Auth]
        public async Task<IActionResult?> IndexAsync()
        {
            var apiUrl = ApplicationSettings.GetConfiguration().GetValue<string>("ApiUrl");

            List<Resource> resourcen = new List<Resource>();

            try
            {
                User usr = HttpContext.Items["User"] as User;
                resourcen = await $"{apiUrl}/Resource"
                                    .WithHeader("AuthKey", usr.AuthKey)
                                    .GetJsonAsync<List<Resource>>();
            }
            catch (Exception)
            {
                HttpContext.Response.Redirect(LOGIN_URL);
                return null;
            }

            return View(resourcen);
        }

        [Auth]
        [Route("Resource/{resourceId}")]
        public async Task<IActionResult?> Resource(int resourceId)
        { // /Home/Resource/1
             var apiUrl = ApplicationSettings.GetConfiguration().GetValue<string>("ApiUrl");

           ResourceViewModel vm = new ResourceViewModel();

            try
            {
                User usr = HttpContext.Items["User"] as User;
                vm.Resource = await $"{apiUrl}/Resource/{resourceId}"
                                    .WithHeader("AuthKey", usr.AuthKey)
                                    .GetJsonAsync<Resource>();
                vm.Events = await $"{apiUrl}/Event/Resource/{resourceId}"
                                    .WithHeader("AuthKey", usr.AuthKey)
                                    .GetJsonAsync<List<Event>>();

            }
            catch (Exception)
            {
                HttpContext.Response.Redirect(LOGIN_URL);
                return null;
            }

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}