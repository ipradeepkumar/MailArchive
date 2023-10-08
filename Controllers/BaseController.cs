using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{

    [ApiController]
    public class BaseController<T> : ControllerBase
    {
        protected IConfiguration configuration;
        protected ILogger<T> logger;
        protected string tempFolder;
        protected string targetFolder;
        protected ResponseContext responseData;

        public BaseController(IConfiguration config, ILogger<T> log)
        {
            configuration = config;
            logger = log;
            tempFolder = configuration["TempFolder"];
            targetFolder = configuration["TargetFolder"];
            responseData = new ResponseContext();
        }

        protected ResponseContext SetResponse(bool isSuccess, dynamic? data, string? errorMessage)
        {
            return new ResponseContext
            {
                Data = data,
                IsSuccess = isSuccess,
                ErrorMessage = errorMessage
            };
        }

    }
    public class ResponseContext
    {
        public dynamic? Data { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string? ErrorMessage { get; set; }
    }
}
