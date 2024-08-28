using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarbeariaApp.Model
{
    public class Servico
    {
        [PrimaryKey, AutoIncrement]
        public Int32 Codigo { get; set; }
        public string Descricao { get; set; }
        public double Valor { get; set; }
    }
}
