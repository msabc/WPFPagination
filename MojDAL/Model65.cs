namespace MojDAL
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model65 : DbContext
    {
        public Model65()
            : base("name=Model65CS")
        {
        }

        public virtual DbSet<Drzava> Drzava { get; set; }
        public virtual DbSet<Grad> Grad { get; set; }
        public virtual DbSet<Kategorija> Kategorija { get; set; }
        public virtual DbSet<Komercijalist> Komercijalist { get; set; }
        public virtual DbSet<KreditnaKartica> KreditnaKartica { get; set; }
        public virtual DbSet<Kupac> Kupac { get; set; }
        public virtual DbSet<Potkategorija> Potkategorija { get; set; }
        public virtual DbSet<Proizvod> Proizvod { get; set; }
        public virtual DbSet<Racun> Racun { get; set; }
        public virtual DbSet<Stavka> Stavka { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Drzava>()
                .HasMany(e => e.Grad)
                .WithOptional(e => e.Drzava)
                .HasForeignKey(e => e.DrzavaID);

            modelBuilder.Entity<Grad>()
                .HasMany(e => e.Kupac)
                .WithOptional(e => e.Grad)
                .HasForeignKey(e => e.GradID);

            modelBuilder.Entity<Kategorija>()
                .HasMany(e => e.Potkategorija)
                .WithRequired(e => e.Kategorija)
                .HasForeignKey(e => e.KategorijaID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Komercijalist>()
                .HasMany(e => e.Racun)
                .WithOptional(e => e.Komercijalist)
                .HasForeignKey(e => e.KomercijalistID);

            modelBuilder.Entity<KreditnaKartica>()
                .HasMany(e => e.Racun)
                .WithOptional(e => e.KreditnaKartica)
                .HasForeignKey(e => e.KreditnaKarticaID);

            modelBuilder.Entity<Kupac>()
                .HasMany(e => e.Racun)
                .WithRequired(e => e.Kupac)
                .HasForeignKey(e => e.KupacID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Potkategorija>()
                .HasMany(e => e.Proizvod)
                .WithOptional(e => e.Potkategorija)
                .HasForeignKey(e => e.PotkategorijaID);

            modelBuilder.Entity<Proizvod>()
                .Property(e => e.CijenaBezPDV)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Proizvod>()
                .HasMany(e => e.Stavka)
                .WithRequired(e => e.Proizvod)
                .HasForeignKey(e => e.ProizvodID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Racun>()
                .HasMany(e => e.Stavka)
                .WithRequired(e => e.Racun)
                .HasForeignKey(e => e.RacunID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Stavka>()
                .Property(e => e.CijenaPoKomadu)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Stavka>()
                .Property(e => e.PopustUPostocima)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Stavka>()
                .Property(e => e.UkupnaCijena)
                .HasPrecision(38, 6);
        }
    }
}
