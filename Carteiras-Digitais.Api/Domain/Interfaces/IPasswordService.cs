using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carteiras_Digitais.Core.Domain.Interfaces
{
    public interface IPasswordService
    {
        string Hasher(string password);
        bool Compare(string password, string comparePassword);
    }
}
