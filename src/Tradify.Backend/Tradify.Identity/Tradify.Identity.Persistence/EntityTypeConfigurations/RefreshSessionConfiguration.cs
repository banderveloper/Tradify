using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tradify.Identity.Domain.Entities;

namespace Tradify.Identity.Persistence.EntityTypeConfigurations
{
    public class RefreshSessionConfiguration : IEntityTypeConfiguration<RefreshSession>
    {
        public void Configure(EntityTypeBuilder<RefreshSession> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.User);
        }
    }
}
