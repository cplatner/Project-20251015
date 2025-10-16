using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace StargateAPI.Business.Data;

[Table("AstronautDetail")]
public class AstronautDetail
{
    public int Id { get; init; }

    public int PersonId { get; init; }

    public string CurrentRank { get; set; }

    public string CurrentDutyTitle { get; set; } 

    public DateTime CareerStartDate { get; init; }

    public DateTime? CareerEndDate { get; set; }

    public virtual Person Person { get; set; }
}

public class AstronautDetailConfiguration : IEntityTypeConfiguration<AstronautDetail>
{
    public void Configure(EntityTypeBuilder<AstronautDetail> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
    }
}