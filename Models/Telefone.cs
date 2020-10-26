using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CadastroPessoas.Models
{
   
    public class Telefone
    {
        
        public int id { get; set; }

        [Required] //Para o valor não ser Nulo no banco
        [Display(Name = "Número")]
        [Column(TypeName = "varchar(11)")]
        public string numero { get; set; }

        [Required]
        [Display(Name = "Tipo")]
        [Column(TypeName = "varchar(07)")]
        public string tipo { get; set; }

        public int id_pessoa { get; set; }

        [ForeignKey(nameof(id_pessoa))]
        public Pessoa Pessoa { get; set; }//Chave estrangeira
    }
}
