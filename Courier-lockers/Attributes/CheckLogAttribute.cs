using Microsoft.AspNetCore.Mvc.Filters;

namespace Courier_lockers.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CheckLogAttribute : ActionFilterAttribute, IExceptionFilter 
    {
        public CheckLogAttribute()
        {
        }

        public void OnException(ExceptionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
