using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Queries;

namespace MySpot.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(ICommandHandler<SignUp> signupHandler,
                                 IQueryHandler<GetUser, UserDto> getUserHandler,
                                 IQueryHandler<GetUsers, IEnumerable<UserDto>> getUsersHandler)
        : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> Post(SignUp command)
        {
            await signupHandler.HandleAsync(command with { UserId = Guid.NewGuid() });
            return NoContent();
        }

        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<UserDto>> Get(Guid userId)
        {
            var user = await getUserHandler.HandleAsync(new GetUser(userId));
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get()
            => Ok(await getUsersHandler.HandleAsync(new GetUsers()));
    }
}
