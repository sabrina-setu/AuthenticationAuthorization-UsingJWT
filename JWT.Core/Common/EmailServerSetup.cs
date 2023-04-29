using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWT.Core.Common;

public class EmailServerSetup
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string UserEmail { get; set; }
    public string Password { get; set; }
}
