using System;
using System.Collections.Generic;
using System.Text;
using HuiTrade.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace HuiTrade.Infrastructure.Persistence
{
    public class HuiTradeDbContext : DbContext
    { 
        public HuiTradeDbContext(DbContextOptions<HuiTradeDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<UserWallet> Wallets => Set<UserWallet>();

        public DbSet<Order> Orders => Set<Order>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<WalletTransaction> Transactions => Set<WalletTransaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HuiTradeDbContext).Assembly);

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // 类名 -> 表名 (UserAddress -> user_addresses)
                var tableName = entity.GetTableName()?.ToSnakeCase();
                entity.SetTableName(tableName);

                foreach (var property in entity.GetProperties())
                {
                    // 属性名 -> 列名 (ReceiverName -> receiver_name)
                    property.SetColumnName(property.Name.ToSnakeCase());
                }
            }

        }


        
    }
}
