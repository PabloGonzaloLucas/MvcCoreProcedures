using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MvcCoreProcedures.Data;
using MvcCoreProcedures.Models;
using System.Data.Common;

namespace MvcCoreProcedures.Repositories
{
    #region STORED PROCEDURES
//    create procedure SP_ALL_ENFERMOS
//as
//	select* from ENFERMO
//go
//create procedure SP_FIND_ENFERMO
//(@inscripcion nvarchar(50))
//as
//	select* from ENFERMO
//    where INSCRIPCION = @inscripcion
//go

//create procedure SP_DELETE_ENFERMO
//(@inscripcion nvarchar(50))
//as
//	delete from ENFERMO where INSCRIPCION = @inscripcion
//go
    #endregion
    public class RepositoryEnfermos
    {
        private EnfermosContext context;
        public RepositoryEnfermos(EnfermosContext context)
        {
            this.context = context;
        }

        public async Task<List<Enfermo>> GetEnfermos()
        {
            using(DbCommand com = 
                this.context.Database.GetDbConnection().CreateCommand())
            {
                string sql = "SP_ALL_ENFERMOS";
                com.CommandText = sql;
                com.CommandType = System.Data.CommandType.StoredProcedure;
                await com.Connection.OpenAsync();
                DbDataReader reader = await com.ExecuteReaderAsync();
                List<Enfermo> enfermos = new List<Enfermo>();
                while(await reader.ReadAsync())
                {
                    Enfermo enfermo = new Enfermo()
                    {
                        Inscripcion = reader["INSCRIPCION"].ToString(),
                        Apellido = reader["APELLIDO"].ToString(),
                        Direccion = reader["DIRECCION"].ToString(),
                        FechaNac = DateTime.Parse(reader["FECHA_NAC"].ToString()),
                        Genero = reader["S"].ToString(),
                        Nss = reader["NSS"].ToString()
                    };
                    enfermos.Add(enfermo);
                }
                await reader.CloseAsync();
                await com.Connection.CloseAsync();
                return enfermos;
            }
        }
    }
}
