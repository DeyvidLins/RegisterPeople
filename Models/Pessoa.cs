using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CadastroPessoas.Models
{
    public class Pessoa
    {
        public int id { get; set; }

        [Required] //Para o valor não ser Nulo no banco
        [Display(Name = "Nome")]
        [Column(TypeName ="varchar(120)")]
        public string nome { get; set; }


        [Display(Name = "Data de Nascimeto:")]
        public DateTime dataNascimento { get; set; }

        [Display(Name = "Salário")]
        [Column(TypeName = "money")]
        public decimal salario { get; set; }

        public List<Telefone> Telefones { get; set; }// Lista de telefone


    }
}
