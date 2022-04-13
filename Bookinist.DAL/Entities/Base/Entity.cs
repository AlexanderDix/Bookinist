using Bookinist.Interfaces;

namespace Bookinist.DAL.Entities.Base
{
    public abstract class Entity : IEntity
    {
        public int Id { get; set; }
    }
}