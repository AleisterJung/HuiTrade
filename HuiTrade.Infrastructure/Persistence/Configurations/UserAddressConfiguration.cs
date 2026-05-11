using HuiTrade.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HuiTrade.Infrastructure.Persistence.Configurations
{
    public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.ToTable("user_addresses");

            builder.HasKey(x => x.Id);
        
            // 基础字段映射
            builder.Property(x => x.ReceiverName).HasColumnName("receiver_name").HasMaxLength(50).IsRequired();
            builder.Property(x => x.Phone).HasColumnName("phone").HasMaxLength(20).IsRequired();
            builder.Property(x => x.DetailAddress).HasColumnName("detail_address").HasMaxLength(500).IsRequired();
            builder.Property(x => x.IsDefault).HasColumnName("is_default");
            builder.Property(x => x.CreatedAt).HasColumnName("created_at");


            // 索引：通常我们会按 UserId 查询地址，加个索引提速
            builder.HasIndex(x => x.UserId);
        }
    }
}
