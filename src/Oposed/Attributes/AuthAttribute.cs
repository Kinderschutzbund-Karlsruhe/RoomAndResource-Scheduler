﻿using Flurl.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Oposed.Models;

namespace Oposed.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Method)]
    public class AuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string LOGIN_URL = "/User/Login";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Cookies.TryGetValue("AuthKey", out var authkey))
            {
                context.HttpContext.Response.Redirect(LOGIN_URL);
                return;
            }  

            var apiUrl = Settings.UrlApi;

            try
            {
                User usr = await $"{apiUrl}/User/Current"
                                    .WithHeader("AuthKey", authkey)
                                    .GetJsonAsync<User>();

                context.HttpContext.Items["User"] = usr;
            }
            catch (Exception)
            {
                context.HttpContext.Response.Redirect(LOGIN_URL);
                return;
            }
            
            await next();
        }
    }
}
