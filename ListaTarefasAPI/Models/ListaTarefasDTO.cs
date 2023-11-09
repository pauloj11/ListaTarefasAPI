using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ListaTarefasAPI.Models
{
    public class ListaTarefasDbContext : DbContext
    {
        public DbSet<ListaDeTarefasDTO> ListasDeTarefas { get; set; }
        public DbSet<TarefaDTO> Tarefas { get; set; }

        public ListaTarefasDbContext(DbContextOptions<ListaTarefasDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TarefaDTO>()
                .ToTable(t => t.HasTrigger("AtualizarDataModificacao"))
                .HasOne(t => t.ListaDeTarefas)
                .WithMany(l => l.Tarefas)
                .HasForeignKey(t => t.ListaDeTarefasID);

            base.OnModelCreating(modelBuilder);
        }
    }

    public class ListaDeTarefasDTO
    {
        [Key]
        public int ListaDeTarefasID { get; set; }
        public string? Nome { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataModificacao { get; set; }

        [JsonIgnore]
        public List<TarefaDTO>? Tarefas { get; set; }
    }
}
