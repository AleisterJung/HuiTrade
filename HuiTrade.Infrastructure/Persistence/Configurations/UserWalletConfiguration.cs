using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using HuiTrade.Domain.Aggregates;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace HuiTrade.Infrastructure.Persistence.Configurations
{

    public class UserWalletConfiguration : IEntityTypeConfiguration<UserWallet>
    {
        public void Configure(EntityTypeBuilder<UserWallet> builder)
        {
            builder.ToTable("user_wallets");

            // 1. 明确 user_id 是主键
            builder.HasKey(x => x.UserId);
            builder.Property(x => x.Version)
       .HasColumnName("version")  
       .IsConcurrencyToken();     // 👈 用于并发控制
            // 映射到数据库中的 user_id 字段
           
            builder.Property(x => x.UserId)
                   .HasColumnName("user_id")
                   .ValueGeneratedNever();

            builder.Property(x => x.Balance).HasColumnName("balance").HasPrecision(36, 18);
            builder.Property(x => x.FrozenBalance).HasColumnName("frozen_balance").HasPrecision(36, 18);
            builder.Property(x => x.Version).HasColumnName("version").IsConcurrencyToken();
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at");
        }
    }
}