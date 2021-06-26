namespace TplStats.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using TplStats.Core.Entities;
    using TplStats.Infrastructure.Database;

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
        public SeasonsController(TplStatsContext tplStatsContext)
        {
            Db = tplStatsContext;
        }

        private TplStatsContext Db { get; }

        /// <summary>
        /// Gets a list of all <see cref="Season"/> entities.
        /// </summary>
        /// <returns>All the <see cref="Season"/> entities.</returns>
        [HttpGet]
        public IAsyncEnumerable<Season> List() => Db.Seasons;

        /// <summary>
        /// Retrieves a single <see cref="Season"/> entity.
        /// </summary>
        /// <param name="id">The id of the <see cref="Season"/> to retrieve.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>The requested <see cref="Season"/>, or <c>404 NOT FOUND</c> if no such <see cref="Season"/> exists.</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Season>> DetailsAsync(int id, CancellationToken cancellationToken) => await Db.Seasons.SingleOrDefaultAsync(s => s.Id == id, cancellationToken: cancellationToken) switch
        {
            Season s => s,
            _ => NotFound(),
        };
    }
}
