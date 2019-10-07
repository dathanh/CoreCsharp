using Framework.DomainModel.ValueObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ServiceLayer.Interfaces;
using System;
using System.Globalization;
using System.Linq;

namespace ProjectNameApi.Controllers
{
    [Route("api/home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;

        }

    }
}
