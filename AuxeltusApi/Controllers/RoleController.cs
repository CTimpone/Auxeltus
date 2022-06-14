using Auxeltus.AccessLayer.Sql;
using Auxeltus.Api.Attributes;
using Auxeltus.Api.Interfaces;
using Auxeltus.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
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

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            try
            {
                var response = await _repository.RetrieveRoleAsync(id).ConfigureAwait(false);

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
                _logger.LogError(ex, $"Exception thrown in {nameof(RoleController)}.{nameof(Get)}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("new")]
        public async Task<IActionResult> Create([FromBody] Role role)
        {
            var req = Request;
            try
            {
                var response = await _repository.CreateRoleAsync(role).ConfigureAwait(false);

                if (response.Success)
                {
                    return Created(new Uri($"/roles/{response.Content.Id}", UriKind.Relative), response);
                }
                else
                {
                    return new BadRequestObjectResult(response);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in {nameof(RoleController)}.{nameof(Create)}");
                return StatusCode(500);
            }
        }

        [HttpPatch]
        [Route("{id}/update")]
        public async Task<IActionResult> Update([FromRoute] int id, [PatchFromBody] RolePatch role)
        {
            try
            {
                var response = await _repository.UpdateRoleAsync(id, role).ConfigureAwait(false);

                if (response.Success)
                {
                    return Ok(response);
                }
                else
                {
                    return new BadRequestObjectResult(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception thrown in {nameof(RoleController)}.{nameof(Create)}");
                return StatusCode(500);
            }
        }

        [HttpDelete]
        [Route("{id}/delete")]
        public IActionResult Delete()
        {
            return new NoContentResult();
        }

    }
}
