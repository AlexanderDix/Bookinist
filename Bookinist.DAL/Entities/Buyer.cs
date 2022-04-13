using Bookinist.DAL.Entities.Base;

namespace Bookinist.DAL.Entities
{
    public class Buyer : Person
    {
        public override string ToString() => $"Покупатель {Surname} {Name} {Patronymic}";
    }
}