using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcCoreProcedures.Data;
using MvcCoreProcedures.Models;
using System.Data.Common;

namespace MvcCoreProcedures.Repositories
{
    #region procedures
    //    create procedure SP_GET_ESPECIALIDADES
    //as
    //	select distinct ESPECIALIDAD from DOCTOR
    //go


    //create procedure SP_UPDATE_DOCTOR_ESPECIALIDAD
    //(@esp nvarchar(50), @incremento int)
    //as
    //	update DOCTOR set SALARIO = SALARIO + @incremento

    //                  where DOCTOR.ESPECIALIDAD = @esp;
    //    go

    //    create procedure SP_GET_DOCTORES_ESPECIALIDAD
    //    (@esp nvarchar(50))
    //as
    //	select* from DOCTOR where ESPECIALIDAD = @esp
    //go

    //select* from DOCTOR
    #endregion
    public class RepositoryDoctores
    {
        private EnfermosContext context;
        public RepositoryDoctores(EnfermosContext context)
        {
            this.context = context;
        }

        public async Task<List<string>> GetEspecialidadesAsync()
        {
            using (DbCommand com = this.context.Database.GetDbConnection().CreateCommand())
            {
                string sql = "SP_GET_ESPECIALIDADES";
                com.CommandText = sql;
                com.CommandType = System.Data.CommandType.StoredProcedure;
                await com.Connection.OpenAsync();
                DbDataReader reader = await com.ExecuteReaderAsync();
                List<string> especialidades = new List<string>();
                while (await reader.ReadAsync())
                {
                    string esp = reader["ESPECIALIDAD"].ToString();
                    especialidades.Add(esp);
                }
                await reader.CloseAsync();
                await com.Connection.CloseAsync();
                return especialidades;
            }
        }

        public async Task UpdateDoctoresEspecialidad(string esp, int incremento)
        {
            string sql = "SP_UPDATE_DOCTOR_ESPECIALIDAD @esp, @incremento";
            SqlParameter pamEsp = new SqlParameter("@esp", esp);
            SqlParameter pamIncremento = new SqlParameter("@incremento", incremento);
            await this.context.Database.ExecuteSqlRawAsync(sql, [pamEsp, pamIncremento]);
        }

        public async Task UpdateDoctoresEspecialidadEF(string esp, int incremento)
        {
            var consulta = from datos in this.context.Doctores.AsEnumerable()
                           where datos.Especialidad == esp
                           select datos;


            List<Doctor> docs = consulta.Distinct().ToList();
            foreach (Doctor doc in docs)
            {
                doc.Salario += incremento;
            }
            await this.context.SaveChangesAsync();
        }

        public async Task<List<Doctor>> GetDoctoresEspecialidad(string esp)
        {

            using (DbCommand com = this.context.Database.GetDbConnection().CreateCommand())
            {
                string sql = "SP_GET_DOCTORES_ESPECIALIDAD";
                SqlParameter pamEsp = new SqlParameter("@esp", esp);
                com.CommandText = sql;
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.Parameters.Add(pamEsp);
                await com.Connection.OpenAsync();
                DbDataReader reader = await com.ExecuteReaderAsync();
                List<Doctor> doctores = new List<Doctor>();
                while (await reader.ReadAsync())
                {
                    Doctor doc = new Doctor();
                    doc.HospitalCod = int.Parse(reader["HOSPITAL_COD"].ToString());
                    doc.Especialidad = reader["ESPECIALIDAD"].ToString();
                    doc.Apellido = reader["APELLIDO"].ToString();
                    doc.DoctorCod = int.Parse(reader["DOCTOR_NO"].ToString());
                    doc.Salario = int.Parse(reader["SALARIO"].ToString());
                    doctores.Add(doc);
                }
                await reader.CloseAsync();
                await com.Connection.CloseAsync();
                return doctores;
            }
        }
    }
}
