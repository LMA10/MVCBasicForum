using BENT1C.Grupo4.Models;
using Microsoft.EntityFrameworkCore;

namespace BENT1C.Grupo4.Data
{
    public class ForoDbContext : DbContext
    {
        public ForoDbContext(DbContextOptions<ForoDbContext> options) : base(options)
        {
        }

        public DbSet<Administrador> Administrador { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Entrada> Entrada { get; set; }
        public DbSet<EntradaMiembro> EntradaMiembro { get; set; }
        public DbSet<Like> Like { get; set; }
        public DbSet<Miembro> Miembro { get; set; }
        public DbSet<Pregunta> Pregunta { get; set; }
        public DbSet<Respuesta> Respuesta { get; set; }
    }
}
