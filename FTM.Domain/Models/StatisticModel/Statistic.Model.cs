using System.ComponentModel.DataAnnotations.Schema;
using FTM.Domain.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace FTM.Domain.Models.StatisticModel;

[Index(nameof(Key), IsUnique = true)]
public partial class Statistic : BaseEntity
{
    public string Key { get; set; }
    public int Counter { get; set; }
    [Column(TypeName = "jsonb")] 
    public object? Data { get; set; }

    public override BaseData ToData()
    {
        throw new NotImplementedException();
    }
}