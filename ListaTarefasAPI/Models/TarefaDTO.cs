using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ListaTarefasAPI.Models.ListaDeTarefasDTO;

namespace ListaTarefasAPI.Models
{
    [NotMapped]
    public class TarefaDTO
    {
        [Key]
        public int TarefaID { get; set; }
        public int ListaDeTarefasID { get; set; }
        public string? Descricao { get; set; }
        public bool Concluida { get; set; }
        public DateTime DataCriacao { get; set; }

        [ForeignKey("ListaDeTarefasID")]
        public ListaDeTarefasDTO? ListaDeTarefas { get; set; }
    }
}
