﻿using EBay.Domain.Entities;
using EBay.Domain.Entities.Identity;
using EBay.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EBay.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor)
             : base(options)
        {
            if (httpContextAccessor != null)
            {
                _httpContextAccessor = httpContextAccessor;
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<VehicleDetail>().HasKey(s => s.VehicleDetailId);
        }

        public DbSet<VehicleDetail> VehicleDetails { get; set; }
    }
}
