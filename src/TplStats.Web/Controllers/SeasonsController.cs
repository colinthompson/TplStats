namespace TplStats.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using TplStats.Core.Entities;
    using TplStats.Infrastructure.Database;
    using static TplStats.Web.ViewModels;

    /// <summary>
    /// API controller for <see cref="Season"/> entities.
    /// </summary>
    [ApiController]
    [Route("/api/seasons")]
    public class SeasonsController : ControllerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonsController"/> class.
        /// </summary>
        /// <param name="tplStatsContext">Database context.</param>
        /// <param name="mapper">Mapper.</param>
        public SeasonsController(TplStatsContext tplStatsContext, IMapper mapper)
        {
            Db = tplStatsContext;
            Mapper = mapper;
        }

        private TplStatsContext Db { get; }

        private IMapper Mapper { get; }

        /// <summary>
        /// Gets a list of all <see cref="Season"/> entities.
        /// </summary>
        /// <returns>All the <see cref="Season"/> entities.</returns>
        [HttpGet]
        public IAsyncEnumerable<SeasonModel> List() => Db.Seasons.ProjectTo<SeasonModel>(Mapper.ConfigurationProvider).AsAsyncEnumerable();

        /// <summary>
        /// Retrieves a single <see cref="Season"/> entity.
        /// </summary>
        /// <param name="id">The id of the <see cref="Season"/> to retrieve.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>The requested <see cref="Season"/>, or <c>404 NOT FOUND</c> if no such <see cref="Season"/> exists.</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<SeasonModel>> DetailsAsync(int id, CancellationToken cancellationToken) => await Db.Seasons.SingleOrDefaultAsync(s => s.Id == id, cancellationToken: cancellationToken) switch
        {
            Season s => Mapper.Map<SeasonModel>(s),
            _ => NotFound(),
        };

        /// <summary>
        /// Retrieves the ids of the teams competing in the given season.
        /// </summary>
        /// <param name="id">The id of the <see cref="Season"/>.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>The ids of all teams competing in the season, or <c>404 NOT FOUND</c> if no such <see cref="Season"/> exists.</returns>
        [HttpGet("{id:int}/teams")]
        public async Task<ActionResult<IEnumerable<int>>> ListTeamsAsync(int id, CancellationToken cancellationToken)
        {
            if (await Db.Seasons.AnyAsync(s => s.Id == id, cancellationToken: cancellationToken))
            {
                return await Db.Seasons
                    .Where(s => s.Id == id)
                    .SelectMany(s => s.Teams)
                    .Select(t => t.Id)
                    .ToListAsync(cancellationToken: cancellationToken);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Retrieves a list of the games scheduled as part of the given season.
        /// </summary>
        /// <param name="id">The id of the <see cref="Season"/>.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A list of games scheduled to be played as part of the season, or <c>404 NOT FOUND</c> if no such <see cref="Season"/> exists.</returns>
        [HttpGet("{id:int}/games")]
        public async Task<ActionResult<IEnumerable<GameModel>>> ListGamesAsync(int id, CancellationToken cancellationToken)
        {
            if (await Db.Seasons.AnyAsync(s => s.Id == id, cancellationToken: cancellationToken))
            {
                return await Db.Seasons
                    .Where(s => s.Id == id)
                    .SelectMany(s => s.Games)
                    .ProjectTo<GameModel>(Mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken: cancellationToken);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
