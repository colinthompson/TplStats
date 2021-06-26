namespace TplStats.Core.Entities
{
    /// <summary>
    /// An entity within TPL Stat's domain model.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// Gets the entity's unique identifier.
        /// </summary>
        int Id { get; }
    }
}
