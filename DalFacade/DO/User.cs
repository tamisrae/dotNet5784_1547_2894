using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO;

public record User
(
    int Id,
    string UserName,
    string Password
)
{
    public User() : this(0, "", "") { }//empty ctor
}

