using GymManagment.DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagment.DAL.Data.Configurations
{
    internal class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(u => u.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50); 

            builder.Property(u => u.Email)
                .HasColumnType("varchar")
                .HasMaxLength(100);
            
            builder.HasIndex(u => u.Email).IsUnique();

            builder.HasIndex(u => u.Phone).IsUnique();
            builder.ToTable(tb=>
            {
                tb.HasCheckConstraint("PhoneCheck", "phone like '010%' or phone like '011%' or phone like '012%' or phone like '015%'");
                tb.HasCheckConstraint("EmailCheck", "Email LIKE '_%@_%.%'");
            });
            builder.OwnsOne(u => u.Address, a =>

            {
                a.Property(ad => ad.Street)
                    .HasColumnName("Street")
                    .HasColumnType("varchar")
                    .HasMaxLength(30);

                a.Property(ad => ad.City)
                    .HasColumnName("City")
                    .HasColumnType("varchar")
                    .HasMaxLength(50); 
            });

        }
    }
}
