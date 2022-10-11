using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1.Models
{
    public partial class DostavaContext : DbContext
    {
        public DostavaContext()
        {
        }

        public DostavaContext(DbContextOptions<DostavaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Dostavljac> Dostavljac { get; set; }
        public virtual DbSet<Osoba> Osoba { get; set; }
        public virtual DbSet<Porudzbina> Porudzbina { get; set; }
        public virtual DbSet<Potrosac> Potrosac { get; set; }
        public virtual DbSet<Proizvod> Proizvod { get; set; }
        public virtual DbSet<Sadrzi> Sadrzi { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-IPLF13G;Database=Dostava;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dostavljac>(entity =>
            {
                entity.HasKey(e => e.IdDostavljac);

                entity.Property(e => e.IdDostavljac)
                    .HasColumnName("idDostavljac")
                    .ValueGeneratedNever();

                entity.Property(e => e.StatusProfila).HasColumnName("statusProfila");

                entity.HasOne(d => d.IdDostavljacNavigation)
                    .WithOne(p => p.Dostavljac)
                    .HasForeignKey<Dostavljac>(d => d.IdDostavljac)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dostavljac_Osoba");
            });

            modelBuilder.Entity<Osoba>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Adresa)
                    .IsRequired()
                    .HasColumnName("adresa")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DatumRodjenja)
                    .HasColumnName("datumRodjenja")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ImePrezime)
                    .IsRequired()
                    .HasColumnName("imePrezime")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.KorisnickoIme)
                    .IsRequired()
                    .HasColumnName("korisnickoIme")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Lozinka)
                    .IsRequired()
                    .HasColumnName("lozinka")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Slika)
                    .IsRequired()
                    .HasColumnName("slika")
                    .IsUnicode(false);

                entity.Property(e => e.TipKorisnika).HasColumnName("tipKorisnika");
            });

            modelBuilder.Entity<Porudzbina>(entity =>
            {
                entity.HasKey(e => e.IdPorudzbine);

                entity.Property(e => e.IdPorudzbine).HasColumnName("idPorudzbine");

                entity.Property(e => e.AdresaDostave)
                    .IsRequired()
                    .HasColumnName("adresaDostave")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Cijena).HasColumnName("cijena");

                entity.Property(e => e.Iddostavljac).HasColumnName("iddostavljac");

                entity.Property(e => e.Idpotrosac).HasColumnName("idpotrosac");


                entity.Property(e => e.Komentar)
                    .IsRequired()
                    .HasColumnName("komentar")
                    .IsUnicode(false);

                entity.Property(e => e.StatusPorudzbine).HasColumnName("statusPorudzbine");

                entity.HasOne(d => d.IddostavljacNavigation)
                    .WithMany(p => p.Porudzbina)
                    .HasForeignKey(d => d.Iddostavljac)
                    .HasConstraintName("FK_Porudzbina_Dostavljac");

                entity.HasOne(d => d.IdpotrosacNavigation)
                    .WithMany(p => p.Porudzbina)
                    .HasForeignKey(d => d.Idpotrosac)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Porudzbina_Potrosac");
            });

            modelBuilder.Entity<Potrosac>(entity =>
            {
                entity.HasKey(e => e.IdPotrosac);

                entity.Property(e => e.IdPotrosac)
                    .HasColumnName("idPotrosac")
                    .ValueGeneratedNever();

                entity.HasOne(d => d.IdPotrosacNavigation)
                    .WithOne(p => p.Potrosac)
                    .HasForeignKey<Potrosac>(d => d.IdPotrosac)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Potrosac_Osoba");
            });

            modelBuilder.Entity<Proizvod>(entity =>
            {
                entity.HasKey(e => e.IdProizvod);

                entity.Property(e => e.IdProizvod).HasColumnName("idProizvod");

                entity.Property(e => e.Cijena).HasColumnName("cijena");

                entity.Property(e => e.Ime)
                    .IsRequired()
                    .HasColumnName("ime")
                    .IsUnicode(false);

                entity.Property(e => e.Sastojci)
                    .IsRequired()
                    .HasColumnName("sastojci")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Sadrzi>(entity =>
            {
                entity.HasKey(e => e.IdSadrzi)
                    .HasName("PK_Sadrzi_1");

                entity.Property(e => e.IdSadrzi).HasColumnName("idSadrzi");

                entity.Property(e => e.Idproizvod).HasColumnName("idproizvod");

                entity.Property(e => e.Idprorudzbina).HasColumnName("idprorudzbina");

                entity.Property(e => e.KolicinaProizvoda).HasColumnName("kolicinaProizvoda");

                entity.HasOne(d => d.IdproizvodNavigation)
                    .WithMany(p => p.Sadrzi)
                    .HasForeignKey(d => d.Idproizvod)
                    .HasConstraintName("FK_Sadrzi_Proizvod");

                entity.HasOne(d => d.IdprorudzbinaNavigation)
                    .WithMany(p => p.Sadrzi)
                    .HasForeignKey(d => d.Idprorudzbina)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Sadrzi_Porudzbina");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
