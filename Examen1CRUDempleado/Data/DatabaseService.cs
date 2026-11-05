using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Examen1CRUDempleado.Models;

namespace Examen1CRUDempleado.Data
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        private async Task Init()
        {
            if (_database != null)
                return;

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "empleado.db3");
            _database = new SQLiteAsyncConnection(databasePath);
            await _database.CreateTableAsync<Empleado>();
        }

        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            await Init();
            return await _database.Table<Empleado>().ToListAsync();
        }

        public async Task<int> SaveEmpleadoAsync(Empleado empleado)
        {
            await Init();
            if (empleado.Id != 0)
            {
                return await _database.UpdateAsync(empleado);
            }
            else
            {
                return await _database.InsertAsync(empleado);
            }
        }

        public async Task<int> DeleteEmpleadoAsync(Empleado empleado)
        {
            await Init();
            return await _database.DeleteAsync(empleado);
        }
    }
}