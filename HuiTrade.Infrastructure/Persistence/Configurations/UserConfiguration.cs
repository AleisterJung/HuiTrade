using HuiTrade.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace HuiTrade.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(x => x.Id);

            // 基础属性对齐 (假设数据库里是蛇形命名)
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.Username).HasColumnName("username").IsRequired();
            builder.Property(x => x.PasswordHash).HasColumnName("password_hash").IsRequired();
            builder.Property(x => x.Email).HasColumnName("email");
            builder.Property(x => x.CreatedAt).HasColumnName("created_at");

            builder.HasOne(u => u.UserWallet)
            .WithOne(w => w.User) 
            .HasForeignKey<UserWallet>(w => w.UserId)
            .IsRequired();


            // 关键：告诉 EF 读写私有字段 _addresses
            var navigation = builder.Metadata.FindNavigation(nameof(User.Addresses));
            navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne(u => u.UserWallet)
        .WithOne(w => w.User)
        .HasForeignKey<UserWallet>(w => w.UserId);
        }
    }
}
