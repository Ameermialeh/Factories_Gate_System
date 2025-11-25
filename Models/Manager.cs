using FactoriesGateSystem.Enums;
using System.Data;

namespace FactoriesGateSystem.Models
{
    public class Manager :User
    {
        public override UserType access() => UserType.Manager;  
 
    }
}
