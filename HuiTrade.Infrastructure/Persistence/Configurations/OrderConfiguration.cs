using HuiTrade.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HuiTrade.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        // OrderConfiguration.cs
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.OrderSn).HasColumnName("order_sn").IsRequired();
            builder.Property(x => x.Amount).HasColumnName("amount").HasPrecision(36, 18);

            // 将枚举映射为字符串，这样数据库里存 "Created", "Paid" 而不是 0, 1
            builder.Property(x => x.Status)
                   .HasColumnName("status")
                   .HasConversion<string>();

            builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        }
    }
}
