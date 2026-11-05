using SQLite;
using System;

namespace Examen1CRUDempleado.Models
{
    public class Empleado
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Puesto { get; set; }
        public decimal Salario { get; set; }
        public string Correo { get; set; }
        public DateTime FechaIngreso { get; set; }
    }
}