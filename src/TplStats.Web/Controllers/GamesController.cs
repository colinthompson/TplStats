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
    /// HTTP API controller for <see cref="Game"/> entities.
    /// </summary>
    [ApiController]
    [Route("/api/games")]
    public class GamesController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GamesController"/> class.
        /// </summary>
        /// <param name="tplStatsContext">Database context.</param>
        /// <param name="mapper">Mapper.</param>
        public GamesController(TplStatsContext tplStatsContext, IMapper mapper)
        {
            Db = tplStatsContext;
            Mapper = mapper;
        }

        private TplStatsContext Db { get; }

        private IMapper Mapper { get; }

        /// <summary>
        /// Retrieves a single <see cref="Game"/> entity.
        /// </summary>
        /// <param name="id">The id of the <see cref="Game"/> to retrieve.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>The requested <see cref="Game"/>, or <c>404 NOT FOUND</c> if no such <see cref="Game"/> exists.</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GameModel>> DetailsAsync(int id, CancellationToken cancellationToken) => await Db.Games
            .Include(g => g.HomeTeam)
            .Include(g => g.AwayTeam)
            .SingleOrDefaultAsync(g => g.Id == id, cancellationToken: cancellationToken) switch
        {
            Game g => Mapper.Map<GameModel>(g),
            _ => NotFound(),
        };
    }
}
