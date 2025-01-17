﻿using ITHS_DB_Labb03.Model;
using Microsoft.EntityFrameworkCore;

namespace ITHS_DB_Labb03.Core;

internal class TodoDbContext : DbContext
{
    public DbSet<Todo> Todos { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<TodoCollection> TodoCollections { get; set; }
    public DbSet<AppState> AppState { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "mongodb://localhost:27017/";
        var dbName = "todoapp";

        optionsBuilder.UseMongoDB(connectionString, dbName);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}

