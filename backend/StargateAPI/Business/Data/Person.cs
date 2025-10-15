using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace StargateAPI.Business.Data;

[Table("Person")]
public class Person
{
    public int Id { get; init; }

    public required string Name { get; init; }

    public virtual AstronautDetail? AstronautDetail { get; init; }

    public virtual ICollection<AstronautDuty> AstronautDuties { get; init; } = new HashSet<AstronautDuty>();
}

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.HasOne(z => z.AstronautDetail).WithOne(z => z.Person).HasForeignKey<AstronautDetail>(z => z.PersonId);
        builder.HasMany(z => z.AstronautDuties).WithOne(z => z.Person).HasForeignKey(z => z.PersonId);
    }
}