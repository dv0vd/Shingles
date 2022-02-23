namespace toi444
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model12")
        {
        }

        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<Shingles> Shingles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Document>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Document>()
                .HasMany(e => e.Shingles)
                .WithRequired(e => e.Document)
                .HasForeignKey(e => e.id_text)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Shingles>()
                .Property(e => e.hash)
                .IsUnicode(false);
        }
    }
}
