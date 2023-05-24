using LoansAppWebApi.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoansAppWebApi.Core.Interfaces
{
    public interface IRoleService
    {
        Task<bool> CheckRoleExists(string roleName);
        Task<bool> CreateRole(Role role);
    }
}
