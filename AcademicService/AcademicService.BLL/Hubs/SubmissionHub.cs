using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicService.BLL.Hubs
{
    public interface ISubmissionHub
    {
        Task SubmissionCreated(object submission);
        Task SubmissionUpdated(object submission);
    }

    public class SubmissionHub : Hub<ISubmissionHub>
    {
    }
}