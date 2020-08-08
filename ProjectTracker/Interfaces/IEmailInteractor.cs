using ProjectTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectTracker.Interfaces
{
    public interface IEmailInteractor
    {

        Task SendEmail(EmailModel email);
    }
}
