using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Softuni_Fest.Models;

namespace Softuni_Fest
{
    public partial class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public override DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderProduct> OrderProducts { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; } = null!;
        public virtual DbSet<OrderOrderStatus> OrderOrderStatuses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<OrderOrderStatus>(x =>
            {
                x.HasKey(r => new { r.OrderId, r.OrderStatusId });
                x.ToTable("OrderOrderStatuses");
            }
            );
        }
    }
}
