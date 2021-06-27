namespace TplStats.Core.Exceptions
{
    using System;
    using System.Collections.Generic;
    using TplStats.Core.Entities;

    /// <summary>
    /// An exception thrown when an attempt is made to schedule a game that conflicts with another.
    /// </summary>
    public class ScheduleConflictException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScheduleConflictException"/> class.
        /// </summary>
        /// <param name="game">The game that is being scheduled.</param>
        /// <param name="conflicts">The games that <paramref name="game"/> conflicts with.</param>
        public ScheduleConflictException(Game game, IEnumerable<Game> conflicts)
        {
            Game = game;
            Conflicts = conflicts;
        }

        /// <summary>
        /// Gets the game that was being scheduled.
        /// </summary>
        public Game Game { get; }

        /// <summary>
        /// Gets the games that conflict with <see cref="Game"/>.
        /// </summary>
        public IEnumerable<Game> Conflicts { get; }
    }
}
