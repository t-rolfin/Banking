using Banking.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Infrastructure.EntitiesConfigs
{
    public class ClientConfigs : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("clients")
                .HasKey(x => x.Id);

            builder.HasIndex(x => new { x.Id, x.CNP });

            builder.HasMany(x => x.Accounts)
                .WithOne()
                .HasForeignKey(x => x.Id);
        }
    }
}
