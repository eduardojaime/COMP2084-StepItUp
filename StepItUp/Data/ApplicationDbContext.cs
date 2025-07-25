﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StepItUp.Models;

namespace StepItUp.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets for CRUD operations for each controller
    public DbSet<Product> Product { get; set; } = default!;
    public DbSet<Category> Category { get; set; } = default!;
    public DbSet<Cart> Cart { get; set; } = default!;

    public DbSet<Order> Order { get; set; } = default!;
    public DbSet<OrderItem> OrderItem { get; set; } = default!;
}
