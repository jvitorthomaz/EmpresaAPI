using System;
using EmpresaAPI.Enums;
using EmpresaAPI.Models;

namespace EmpresaAPI.Entities
{
	public class DepartamentEntitie : DepartamentModel
    {
        public DepartamentEntitie()
        {
            EmployeesOnDepartament = new List<EmployeeModel>();
            Status = StatusEnum.Ativo;

        }

        public void Inativar()
        {
            Status = StatusEnum.Inativo;
        }

        public void Ativar()
        {
            Status = StatusEnum.Ativo;
        }
    }
}

