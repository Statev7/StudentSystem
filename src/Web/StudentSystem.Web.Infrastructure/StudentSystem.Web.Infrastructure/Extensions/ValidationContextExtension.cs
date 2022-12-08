namespace StudentSystem.Web.Infrastructure.Extensions
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public static class ValidationContextExtension
    {
        public static bool IsRequestCallUpdateAction(this ValidationContext context)
        {
            var httpContextAccessor = (IHttpContextAccessor)context.GetService(typeof(IHttpContextAccessor));

            var isUpdateAction = httpContextAccessor.HttpContext.Request.Path.Value.Contains("Update");

            return isUpdateAction;
        }
    }
}
