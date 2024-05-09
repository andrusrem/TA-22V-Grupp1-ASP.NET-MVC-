using System.Diagnostics.CodeAnalysis;
namespace KooliProjekt.Data
{
    [ExcludeFromCodeCoverage]
    public abstract class Entity : IEntity
    {        
        public int Id { get; set; }

        public bool IsNew { get { return Id == 0; } }
    }

    public interface IEntity
    {
        bool IsNew { get; }
    }
}