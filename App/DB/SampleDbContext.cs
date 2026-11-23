using Microsoft.EntityFrameworkCore;
using App.Domain.Entities;
using App.Domain.Entities.Applications;

namespace App.DB {
    public class SampleDbContext : DbContext {
        public SampleDbContext(DbContextOptions options) : base(options) {
        }

        public DbSet<Member> Members { get; set; } = null!;
        public DbSet<DueDateUnit> DueDateUnits { get; set; } = null!;
        public DbSet<TargetUnit> TargetUnits { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            var dueDate = modelBuilder.Entity<DueDateUnit>();
            dueDate.HasOne(unit => unit.ReplacedBy)
                .WithOne()
                .HasForeignKey<DueDateUnit>(unit => unit.ReplacedById)
                .OnDelete(DeleteBehavior.ClientNoAction);

            dueDate.HasOne(unit => unit.TargetUnit)
                .WithMany()
                .OnDelete(DeleteBehavior.ClientNoAction);

            dueDate.Property(unit => unit.ReplacedById).IsConcurrencyToken();

            dueDate.HasOne(unit => unit.Origin)
                .WithMany()
                .HasForeignKey(unit => unit.OriginId)
                .OnDelete(DeleteBehavior.ClientNoAction);
            dueDate.HasIndex(unit => new { unit.OriginId, unit.ReplacedById, unit.DiscardedAt });

            ConfigureApplications(modelBuilder);
        }

        /// <summary>
        /// DueDateApplication の派生クラス（カテゴリー別の応用データ）を検出し、TPC でマップする。
        /// 本体アセンブリと、派生コンテキスト（テスト用など）のアセンブリを対象にする。
        /// </summary>
        private void ConfigureApplications(ModelBuilder modelBuilder) {
            var applicationTypes = new[] { typeof(SampleDbContext).Assembly, GetType().Assembly }
                .Distinct()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && !type.IsAbstract && typeof(DueDateApplication).IsAssignableFrom(type))
                .ToList();
            if (applicationTypes.Count == 0) {
                return;
            }

            modelBuilder.Entity<DueDateApplication>(application => {
                application.UseTpcMappingStrategy();
                application.HasKey(a => a.OriginUnitId);
                application.Property(a => a.OriginUnitId).ValueGeneratedNever();
                application.HasOne(a => a.OriginUnit)
                    .WithOne()
                    .HasForeignKey<DueDateApplication>(a => a.OriginUnitId)
                    .OnDelete(DeleteBehavior.ClientNoAction);
            });

            foreach (var type in applicationTypes) {
                modelBuilder.Entity(type).ToTable(type.Name + "s");
            }
        }
    }
}
