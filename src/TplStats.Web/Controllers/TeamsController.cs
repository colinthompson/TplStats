namespace TplStats.Web.Controllers
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using TplStats.Core.Entities;
    using TplStats.Infrastructure.Database;
    using static TplStats.Web.ViewModels;

    /// <summary>
    /// API controller for <see cref="Team"/> entities.
    /// </summary>
    [ApiController]
    [Route("/api/teams")]
    public class TeamsController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamsController"/> class.
        /// </summary>
        /// <param name="tplStatsContext">Database context.</param>
        /// <param name="mapper">Mapper.</param>
        public TeamsController(TplStatsContext tplStatsContext, IMapper mapper)
        {
            Db = tplStatsContext;
            Mapper = mapper;
        }

        private TplStatsContext Db { get; }

        private IMapper Mapper { get; }

        /// <summary>
        /// Retrieves a single <see cref="Team"/> entity.
        /// </summary>
        /// <param name="id">The id of the <see cref="Team"/> to retrieve.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>The requested <see cref="Team"/>, or <c>404 NOT FOUND</c> if no such <see cref="Season"/> exists.</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TeamModel>> DetailsAsync(int id, CancellationToken cancellationToken) => await Db.Teams.SingleOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken) switch
        {
            Team t => Mapper.Map<TeamModel>(t),
            _ => NotFound(),
        };
    }
}
