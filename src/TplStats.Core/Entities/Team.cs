namespace TplStats.Core.Entities
{
    /// <summary>
    /// A team that compets in a <see cref="Season"/>.
    /// </summary>
    public class Team : IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Team"/> class.
        /// </summary>
        /// <param name="id">The team's id.</param>
        /// <param name="name">The team's name.</param>
        internal Team(int id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Gets the team's id.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets or sets the team's name.
        /// </summary>
        public string Name { get; set; }
    }
}
