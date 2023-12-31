﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Queries;
using MySpot.Application.Security;
using MySpot.Core.ValueObjects;
using Swashbuckle.AspNetCore.Annotations;

namespace MySpot.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(ICommandHandler<SignUp> signupHandler,
                                 ICommandHandler<SignIn> signInHandler,
                                 IQueryHandler<GetUser, UserDto?> getUserHandler,
                                 IQueryHandler<GetUsers, IEnumerable<UserDto>> getUsersHandler,
                                 ITokenStorage tokenStorage)
        : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Post(SignUp command)
        {
            command = command with { UserId = Guid.NewGuid() };
            await signupHandler.HandleAsync(command);
            return CreatedAtAction(nameof(Get), new {command.UserId}, null);
        }

        [HttpGet("{userId:guid}")]
        [Authorize(Policy = "is-admin")]
        [SwaggerOperation("Get signle by user Id if exists","some description")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> Get(Guid userId)
        {
            var user = await getUserHandler.HandleAsync(new GetUser(userId));
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet]
        [Authorize(Policy = "is-admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get([FromQuery] GetUsers query)
            => Ok(await getUsersHandler.HandleAsync(query));

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetMe()
        {
            if (string.IsNullOrWhiteSpace(HttpContext.User.Identity?.Name))
                return NotFound();

            var userId = Guid.Parse(HttpContext.User.Identity.Name);
            var user = await getUserHandler.HandleAsync(new GetUser(userId));
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPost("sign-in")]
        public async Task<ActionResult<JwtDto>> Post(SignIn command)
        {
            await signInHandler.HandleAsync(command);
            var jwt = tokenStorage.Get();
            return Ok(jwt);
        }
    }
}
