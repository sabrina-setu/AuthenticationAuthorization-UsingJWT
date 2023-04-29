﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWT.Application.UseCase.Account;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string to, string subject, string body);
}
