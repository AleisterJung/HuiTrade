using System;
using System.Collections.Generic;
using System.Text;
using HuiTrade.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HuiTrade.Infrastructure.Persistence.Configurations
{
    public class WalletTransactionConfiguration : IEntityTypeConfiguration<WalletTransaction>
    {
        public void Configure(EntityTypeBuilder<WalletTransaction> builder)
        {
            builder.ToTable("wallet_transactions");
            builder.HasKey(x => x.Id);


            builder.Property(x => x.Type).HasConversion<string>().HasColumnName("tx_type");
            builder.Property(x => x.Direction).HasConversion<string>().HasColumnName("direction");


            builder.Property(x => x.Amount).HasPrecision(36, 18);
            builder.Property(x => x.BeforeBalance).HasPrecision(36, 18).HasColumnName("balance_before");
            builder.Property(x => x.AfterBalance).HasPrecision(36, 18).HasColumnName("balance_after");

            builder.Property(x => x.RelatedOrderId).HasColumnName("order_id");
            builder.Property(x => x.Description).HasMaxLength(500);

        }
    }
}
