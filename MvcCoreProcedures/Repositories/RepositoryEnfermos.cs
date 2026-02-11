using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
            using (DbCommand com =
                this.context.Database.GetDbConnection().CreateCommand())
            {
                string sql = "SP_ALL_ENFERMOS";
                com.CommandText = sql;
                com.CommandType = System.Data.CommandType.StoredProcedure;
                await com.Connection.OpenAsync();
                DbDataReader reader = await com.ExecuteReaderAsync();
                List<Enfermo> enfermos = new List<Enfermo>();
                while (await reader.ReadAsync())
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

        public async Task<Enfermo> FindEnfermoAsync(string inscripcion)
        {
            string sql = "SP_FIND_ENFERMO @inscripcion";
            SqlParameter pamIns = new SqlParameter("@inscripcion"
                , inscripcion);
            var consulta = await
                this.context.Enfermos.FromSqlRaw(sql, pamIns).ToListAsync();
            ;
            Enfermo enfermo =
                consulta.FirstOrDefault();
            return enfermo;
        }

        public async Task DeleteEnfermoAsync(string inscripcion)
        {
            string sql = "SP_DELETE_ENFERMO";
            SqlParameter pamIns = new SqlParameter("@inscripcion", inscripcion);
            using (DbCommand com =
                this.context.Database.GetDbConnection().CreateCommand())
            {
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = sql;
                com.Parameters.Add(pamIns);
                await com.Connection.OpenAsync();
                await com.ExecuteNonQueryAsync();
                await com.Connection.CloseAsync();
                com.Parameters.Clear();
            }
        }

        public async Task DeleteEnfermoRawAsync(string inscripcion)
        {
            string sql = "SP_DELETE_ENFERMO @inscripcion";

            SqlParameter pamIns =
                new SqlParameter("@inscripcion", inscripcion);
            await this.context.Database.ExecuteSqlRawAsync(sql, pamIns);

        }

        public async Task CreateEnfermoAsync(string apellido, string direccion, string fecha, string genero, string nss)
        {
            string sql = "SP_CREATE_ENFERMO @apellido, @direccion, @fechaNac, @genero, @nss";
            SqlParameter pamApellido = new SqlParameter("@apellido", apellido);
            SqlParameter pamDireccion = new SqlParameter("@direccion", direccion);
            SqlParameter pamFecha = new SqlParameter("@fechaNac", DateTime.Parse(fecha));
            SqlParameter pamGenero = new SqlParameter("@genero", genero);
            SqlParameter pamNss = new SqlParameter("@nss", nss);
            await this.context.Database.ExecuteSqlRawAsync(sql, [pamApellido, pamDireccion, pamFecha, pamGenero, pamNss]);
        }
    }
}
