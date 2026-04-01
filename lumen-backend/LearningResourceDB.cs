using Microsoft.EntityFrameworkCore; 

class LearningResourceDb : DbContext  
{
  public LearningResourceDb(DbContextOptions<LearningResourceDb> options) : base(options) { } 
  
    public DbSet<LearningResource> LearningResources => Set<LearningResource>(); 
  
}
