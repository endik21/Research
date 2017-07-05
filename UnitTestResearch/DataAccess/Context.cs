namespace DataAccess
{
    using System.Data.Entity;

    using Domain;

    public class Context: DbContext
    {
        public Context()
            : base("name=SampleMock")
        {
        }

        public DbSet<User> Users { get; set; }

        public void FixEfProviderServicesProblem()
        {
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
    }
    
}
