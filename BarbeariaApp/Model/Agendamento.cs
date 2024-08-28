using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarbeariaApp.Model
{
    public class Agendamento
    {
        [PrimaryKey, AutoIncrement]
        public Int32 Codigo { get; set; }
        public string Cliente { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Horario { get; set; }
    }
}
