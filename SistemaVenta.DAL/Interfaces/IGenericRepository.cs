using System.Linq.Expressions;

namespace SistemaVenta.DAL.Interfaces
{
    // Esta interfas se va a usar para todas las entidades
    // Este se va a llamar TEntity y sera una clase
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        // Obtener va a hacer una funcion de TEntity e igual para las otras tareas
        Task<TEntity> Obtener(Expression<Func<TEntity, bool>> filtro);

        Task<TEntity> Crear(TEntity entidad);

        // Se pone booleano porque tiene que decidir falso o verdadero, si se va -
        // a editar o guardar. Similar con eliminar
        Task<bool> Editar(TEntity entidad);

        Task<bool> Eliminar(TEntity entidad);

        // Esto Proporciona funcionalidad para evaluar consultas en un origen de datos específico -
        // en el que no se especifica el tipo de datos.
        Task<IQueryable<TEntity>> Consultar(Expression<Func<TEntity, bool>> filtro = null);
    }
}
