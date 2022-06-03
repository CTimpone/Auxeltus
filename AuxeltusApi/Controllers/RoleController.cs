using Auxeltus.AccessLayer.Sql;
using Auxeltus.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auxeltus.Api
{
    [ApiController]
    [Route("roles")]
    public class RoleController : ControllerBase
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IRoleRepository _repository;

        public RoleController(IRoleCommand roleCommand, IRoleQuery roleQuery, ILogger<RoleController> logger)
        {
            _logger = logger;
            _repository = new RoleRepository(roleCommand, roleQuery, logger);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return new OkObjectResult("tempValue");
        }

        [HttpPost]
        [Route("new")]
        public IActionResult Create()
        {
            return new NoContentResult();
        }

        [HttpPatch]
        [Route("{id}/update")]
        public IActionResult Update()
        {
            return new NoContentResult();
        }

        [HttpDelete]
        [Route("{id}/delete")]
        public IActionResult Delete()
        {
            return new NoContentResult();
        }

    }
}
