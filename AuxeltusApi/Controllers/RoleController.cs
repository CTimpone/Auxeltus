using Auxeltus.AccessLayer.Sql;
using Auxeltus.Api.Attributes;
using Auxeltus.Api.Interfaces;
using Auxeltus.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auxeltus.Api
{
    [Controller]
    [Route("roles")]
    [ModelValidation]
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
        public async Task<IActionResult> Index([FromQuery] int? startIndex, [FromQuery] int? maxReturns)
        {
            try
            {
                var response = await _repository.RetrieveRolesAsync(startIndex, maxReturns).ConfigureAwait(false);
                
                if (response == null)
                {
                    return NoContent();
                }
                else if (!response.Success)
                {
                    return new BadRequestObjectResult(response);
                }
                else
                {
                    return new OkObjectResult(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in {nameof(RoleController)}.{nameof(Index)}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("new")]
        public IActionResult Create([FromBody] Role role)
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
