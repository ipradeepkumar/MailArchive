using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{

    [ApiController]
    public class BaseController : ControllerBase
    {
        public class ResponseContext
        {
            public dynamic? Data { get; set; }
            public bool IsSuccess { get; set; } = true;
            public string? ErrorMessage { get; set; }
        }

    }
}
