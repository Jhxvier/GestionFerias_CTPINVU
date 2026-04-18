using Microsoft.EntityFrameworkCore;

namespace GestionFerias_CTPINVU.Models
{
    /// <summary>
    /// Clase genérica de paginación. Encapsula una página de datos junto con
    /// la metadata necesaria para renderizar los controles de navegación.
    /// </summary>
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int TotalCount { get; private set; }
        public int PageSize { get; private set; }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        private PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        /// <summary>
        /// Crea una instancia paginada a partir de un IQueryable (ejecuta COUNT + pagina en BD).
        /// </summary>
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

        /// <summary>
        /// Crea una instancia paginada a partir de una lista ya cargada en memoria.
        /// </summary>
        public static PaginatedList<T> CreateFromList(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            var list = source.ToList();
            var count = list.Count;
            var items = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
