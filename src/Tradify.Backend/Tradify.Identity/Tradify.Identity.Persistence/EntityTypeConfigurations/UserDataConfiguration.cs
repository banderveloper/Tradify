using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tradify.Identity.Domain.Entities;

namespace Tradify.Identity.Persistence.EntityTypeConfigurations;

public class UserDataConfiguration : IEntityTypeConfiguration<UserData>
{
    public void Configure(EntityTypeBuilder<UserData> builder)
    {
        builder.HasKey(ud => ud.Id);
        builder.HasOne(u => u.User)
            .WithOne(ud => ud.UserData);
    }
}