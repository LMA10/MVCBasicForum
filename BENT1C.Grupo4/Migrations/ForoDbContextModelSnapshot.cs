﻿// <auto-generated />
using System;
using BENT1C.Grupo4.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BENT1C.Grupo4.Migrations
{
    [DbContext(typeof(ForoDbContext))]
    partial class ForoDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9");

            modelBuilder.Entity("BENT1C.Grupo4.Models.Administrador", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(75);

                    b.Property<DateTime>("FechaAlta")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<byte[]>("Password")
                        .HasColumnType("BLOB");

                    b.HasKey("Id");

                    b.ToTable("Administrador");
                });

            modelBuilder.Entity("BENT1C.Grupo4.Models.Categoria", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(30);

                    b.HasKey("Id");

                    b.ToTable("Categoria");
                });

            modelBuilder.Entity("BENT1C.Grupo4.Models.Entrada", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CategoriaId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("MiembroId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Privada")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("MiembroId");

                    b.ToTable("Entrada");
                });

            modelBuilder.Entity("BENT1C.Grupo4.Models.EntradaMiembro", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("IdEntrada")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("IdMiembro")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Habilitado")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("IdEntrada");

                    b.HasIndex("IdMiembro");

                    b.ToTable("EntradaMiembro");
                });

            modelBuilder.Entity("BENT1C.Grupo4.Models.Like", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("TEXT");

                    b.Property<bool>("MeGusta")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("MiembroId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RespuestaId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MiembroId");

                    b.HasIndex("RespuestaId");

                    b.ToTable("Like");
                });

            modelBuilder.Entity("BENT1C.Grupo4.Models.Miembro", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(75);

                    b.Property<DateTime>("FechaAlta")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<byte[]>("Password")
                        .HasColumnType("BLOB");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(15);

                    b.HasKey("Id");

                    b.ToTable("Miembro");
                });

            modelBuilder.Entity("BENT1C.Grupo4.Models.Pregunta", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Activa")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(500);

                    b.Property<Guid>("EntradaId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("MiembroId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("EntradaId");

                    b.HasIndex("MiembroId");

                    b.ToTable("Pregunta");
                });

            modelBuilder.Entity("BENT1C.Grupo4.Models.Respuesta", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(250);

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("MiembroId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PreguntaId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MiembroId");

                    b.HasIndex("PreguntaId");

                    b.ToTable("Respuesta");
                });

            modelBuilder.Entity("BENT1C.Grupo4.Models.Entrada", b =>
                {
                    b.HasOne("BENT1C.Grupo4.Models.Categoria", "Categoria")
                        .WithMany("Entradas")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BENT1C.Grupo4.Models.Miembro", "Miembro")
                        .WithMany("Entradas")
                        .HasForeignKey("MiembroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BENT1C.Grupo4.Models.EntradaMiembro", b =>
                {
                    b.HasOne("BENT1C.Grupo4.Models.Entrada", "Entrada")
                        .WithMany("MiembrosHabilitados")
                        .HasForeignKey("IdEntrada")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BENT1C.Grupo4.Models.Miembro", "Miembro")
                        .WithMany("EntradasHabilitadas")
                        .HasForeignKey("IdMiembro")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BENT1C.Grupo4.Models.Like", b =>
                {
                    b.HasOne("BENT1C.Grupo4.Models.Miembro", "Miembro")
                        .WithMany("RespuestasQueMeGustan")
                        .HasForeignKey("MiembroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BENT1C.Grupo4.Models.Respuesta", "Respuesta")
                        .WithMany("Reacciones")
                        .HasForeignKey("RespuestaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BENT1C.Grupo4.Models.Pregunta", b =>
                {
                    b.HasOne("BENT1C.Grupo4.Models.Entrada", "Entrada")
                        .WithMany("Preguntas")
                        .HasForeignKey("EntradaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BENT1C.Grupo4.Models.Miembro", "Miembro")
                        .WithMany("Preguntas")
                        .HasForeignKey("MiembroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BENT1C.Grupo4.Models.Respuesta", b =>
                {
                    b.HasOne("BENT1C.Grupo4.Models.Miembro", "Miembro")
                        .WithMany("Respuestas")
                        .HasForeignKey("MiembroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BENT1C.Grupo4.Models.Pregunta", "Pregunta")
                        .WithMany("Respuestas")
                        .HasForeignKey("PreguntaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
