using Microsoft.EntityFrameworkCore; 

namespace LumenAPI.Database
{


  public class LearningResourceDb : DbContext  
  {
    public LearningResourceDb(DbContextOptions<LearningResourceDb> options) : base(options) { } 
    
      public DbSet<LearningResource> LearningResources => Set<LearningResource>(); 

      protected override void OnModelCreating(ModelBuilder builder)
          {
              base.OnModelCreating(builder);
              // Additional model configuration can be done here if needed
              builder.HasDefaultSchema("learningresources");
          }
  }
}
