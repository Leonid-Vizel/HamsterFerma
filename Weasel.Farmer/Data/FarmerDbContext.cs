using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Weasel.Farmer.Data;

public sealed class FarmerDbContext : IdentityDbContext
{
    public FarmerDbContext(DbContextOptions<FarmerDbContext> options) : base(options) { }
}
