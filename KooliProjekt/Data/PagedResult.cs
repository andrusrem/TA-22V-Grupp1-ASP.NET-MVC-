using System.Diagnostics.CodeAnalysis;
namespace KooliProjekt.Data
{
    [ExcludeFromCodeCoverage]
    public class PagedResult<T> : PagedResultBase
    {
        public IList<T> Results {get; set;}
        public PagedResult()
        {
            Results = new List<T>();
        }

    }
}