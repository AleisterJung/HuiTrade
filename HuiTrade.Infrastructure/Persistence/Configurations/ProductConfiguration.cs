using HuiTrade.Domain.Aggregates;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace HuiTrade.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration:IEntityTypeConfiguration<Product>
    {
         
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("products");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Price).HasColumnName("price").HasPrecision(36, 18);

            // psql 里的 images 字段是 jsonb 或 text 数组
            // EF Core 8+ 可以直接处理，或者手动转换成字符串
            builder.Property(x => x.Images)
                   .HasColumnName("images")
                   .HasColumnType("jsonb");
        }
    }
}
