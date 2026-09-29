using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Proyecto_Tesis.Models; // Ajusta a tu namespace real

namespace Proyecto_Tesis.Data
{
    // Debe heredar de IdentityDbContext
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<UsuarioInvestigacion> UsuariosInvestigacion { get; set; }
        public DbSet<Consentimiento> Consentimientos { get; set; }
        public DbSet<ResultadoCognitivo> ResultadosCognitivos { get; set; }
        public DbSet<ResultadoEmocional> ResultadosEmocionales { get; set; }
        public DbSet<UsoIA> UsoIA { get; set; }
        public DbSet<InteractionLog> Logs { get; set; }
        public DbSet<ResultadoML> ResultadosML { get; set; }
        public DbSet<ResultadoSUS> ResultadosSUS { get; set; }
        public DbSet<ResultadoPrediccion> ResultadosPrediccion { get; set; }
    }
}