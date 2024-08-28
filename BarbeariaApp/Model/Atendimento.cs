using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BarbeariaApp.Model
{
    public class Atendimento
    {
        [PrimaryKey, AutoIncrement]
        public Int32 Codigo { get; set; }
        public DateTime Data { get; set; }
        public string Cliente { get; set; }
        public string CodigoProdutos { get; set; }
        public string CodigoServicos { get; set; }
        public string Observacao { get; set; }
        public double ValorTotal { get; set; }
    }
}
