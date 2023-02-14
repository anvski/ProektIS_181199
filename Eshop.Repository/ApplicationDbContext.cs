using Eshop.Domain.DomainModels;
using Eshop.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Eshop.Repository
{
    public class ApplicationDbContext : IdentityDbContext<EshopAppUser, EshopAppRoles, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<TicketInShoppingCart> TicketInShoppingCarts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<TicketsInOrder> TicketsInOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Ticket>().Property(z => z.ID).ValueGeneratedOnAdd();

            builder.Entity<ShoppingCart>().Property(z => z.ID).ValueGeneratedOnAdd();

            builder.Entity<TicketInShoppingCart>().HasKey(z => new { z.TicketId, z.ShoppingCartId });

            builder.Entity<TicketInShoppingCart>().HasOne(z => z.Ticket)
                .WithMany(z => z.TicketInShoppingCarts)
                .HasForeignKey(z => z.TicketId);

            builder.Entity<TicketInShoppingCart>().HasOne(z => z.ShoppingCart)
                .WithMany(z => z.TicketInShoppingCarts)
                .HasForeignKey(z => z.ShoppingCartId);

            builder.Entity<ShoppingCart>().HasOne<EshopAppUser>(z => z.OwnerOfCart)
                .WithOne(z => z.UserShoppingCart).HasForeignKey<ShoppingCart>(z => z.OwnerId);

            builder.Entity<TicketsInOrder>().HasKey(z => new { z.OrderId, z.TicketId });

            builder.Entity<TicketsInOrder>().HasOne(z => z.Order)
                .WithMany(z => z.TicketsInOrder)
                .HasForeignKey(z => z.OrderId);

            builder.Entity<TicketsInOrder>().HasOne(z => z.Ticket)
                .WithMany(z => z.Orders)
                .HasForeignKey(z => z.TicketId);
        }
    }
}

