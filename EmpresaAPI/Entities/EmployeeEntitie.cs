using System;
using EmpresaAPI.Enums;
using EmpresaAPI.Models;

namespace EmpresaAPI.Entities
{
	public class EmployeeEntitie : EmployeeModel
	{
        public EmployeeEntitie()
        {

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

