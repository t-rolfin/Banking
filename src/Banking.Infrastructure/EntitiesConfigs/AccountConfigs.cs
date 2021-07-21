using Banking.Core.Entities;
using Banking.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Infrastructure.EntitiesConfigs
{
    public class AccountConfigs : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("accounts")
                .HasKey(x => x.Id);

            builder.Ignore(x => x.AccountType);

            builder.Property<AccountTypeEnum>("AccType");

            builder.HasMany(x => x.Transactions)
                .WithOne()
                .HasForeignKey("SourceAccountId");
        }
    }
}
