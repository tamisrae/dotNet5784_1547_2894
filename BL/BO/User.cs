using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO;

public class User
{
    public int Id { get; init; }
    public required string UserName { get; set; }
    public required string Password { get; set; }

    public override string ToString() => this.ToStringProperty();
}
