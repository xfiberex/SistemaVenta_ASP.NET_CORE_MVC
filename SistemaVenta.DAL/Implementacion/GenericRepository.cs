using Microsoft.EntityFrameworkCore;
using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Interfaces;
using System.Linq.Expressions;

namespace SistemaVenta.DAL.Implementacion
{
    // Despues de tener las interfaces se implementan para su uso
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        // Declara una variable que hace referencia a DBVENTAContext y hereda la clase DbContext de -
        // Entity Framework Core.
        private readonly DBVENTAContext _dBContext;

        // Este constructor se utiliza para asignar un contexto de base de datos (DBVENTAContext) -
        // a una instancia de la clase GenericRepository. 
        public GenericRepository(DBVENTAContext dBContext)
        {
            _dBContext = dBContext;
        }

        // Este metodo permite obtener una entidad de una base de datos utilizando un filtro específico,
        // con programación asíncrona para no bloquear el hilo de ejecución principal.
        public async Task<TEntity> Obtener(Expression<Func<TEntity, bool>> filtro)
        {
            try
            {
                TEntity entidad = await _dBContext.Set<TEntity>().FirstOrDefaultAsync(filtro);
                return entidad;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Este método permite crear una nueva entidad y guardarla en una base de datos -
        // utilizando programación asíncrona para no bloquear el hilo de ejecución principal.
        public async Task<TEntity> Crear(TEntity entidad)
        {
            try
            {
                _dBContext.Set<TEntity>().Add(entidad);
                await _dBContext.SaveChangesAsync();
                return entidad;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Lo mismo que el anterio metodo, pero este actualiza y guarda.
        public async Task<bool> Editar(TEntity entidad)
        {
            try
            {
                _dBContext.Set<TEntity>().Update(entidad);
                await _dBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Lo mismo que los dos metodos anteriores, pero este elimina y guarda.
        public async Task<bool> Eliminar(TEntity entidad)
        {
            try
            {
                _dBContext.Set<TEntity>().Remove(entidad);
                await _dBContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Este método permite consultar entidades en una base de datos utilizando un filtro opcional,
        // devolviendo una consulta que puede ser más refinada con la aplicación de filtros -
        // adicionales o ser ejecutada para obtener los resultados finales.
        public async Task<IQueryable<TEntity>> Consultar(Expression<Func<TEntity, bool>> filtro = null)
        {
            IQueryable<TEntity> queryEntidad = filtro == null ? _dBContext.Set<TEntity>() : _dBContext.Set<TEntity>().Where(filtro);
            return queryEntidad;
        }
    }
}