using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaVenta.BLL.Implementacion;
using SistemaVenta.BLL.Interfaces;
using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Implementacion;
using SistemaVenta.DAL.Interfaces;

namespace SistemaVenta.IOC
{
    public static class Dependencia
    {
        // Metodo para la conexion de la BD con la aplicación.
        // El cual contiene varios servicios
        public static void InyectarDependencia(this IServiceCollection services, IConfiguration Configuration)
        {
            /* Este código configura y registra el contexto de la 
             * base de datos SQL Server DBVENTAContext en la aplicación ASP.NET Core, 
             * permitiendo que se pueda acceder a él a través de la inyección de dependencias y 
             * especificando la cadena de conexión que utilizará para conectarse a la base de datos. */
            services.AddDbContext<DBVENTAContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("CadenaSQL"));
            });

            /* Este código establece una relación de inyección de dependencias entre una interfaz genérica y 
             * su implementación genérica correspondiente, lo que permite que la aplicación resuelva la implementación 
             * adecuada cuando se solicite la interfaz genérica.*/
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            /* Este código establece una relación de inyección de dependencias entre una interfaz (IVentaRepository) y 
             * su implementación (VentaRepository), asegurando que la implementación adecuada sea utilizada cuando se solicite 
             * la interfaz en el contexto de una solicitud HTTP.*/
            services.AddScoped<IVentaRepository, VentaRepository>();

            /* Lo mismo que el servicio anterior, pero con esta interfaz y clase.
             * Igual para todos los que van despues de este */
            services.AddScoped<ICorreoService, CorreoService>();
            services.AddScoped<IFireBaseService, FireBaseService>();
            services.AddScoped<IUtilidadesService, UtilidadesService>();
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<INegocioService, NegocioService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<ITipoDocumentoVentaService, TipoDocumentoVentaService>();
            services.AddScoped<IVentaService, VentaService>();
            services.AddScoped<IDashBoardService, DashBoardService>();
            services.AddScoped<IMenuService, MenuService>();
        }
    }
}
