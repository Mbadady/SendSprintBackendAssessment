namespace BackendAssessment.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<T> Set<T>() where T : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        DbSet<Order> Orders { get; }
        public DbSet<Transaction> Transactions { get; set; }
        DbSet<ApplicationUser> ApplicationUsers { get; set; }
        DbSet<Product> Products { get; }
    }
}
