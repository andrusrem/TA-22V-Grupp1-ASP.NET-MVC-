using System.Diagnostics.CodeAnalysis;
namespace KooliProjekt.Data
{
    [ExcludeFromCodeCoverage]
    public abstract class Entity
    {
        
        public int Id { get; set; }
    }
}