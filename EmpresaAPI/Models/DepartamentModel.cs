using System;
using System.Text.Json.Serialization;
using EmpresaAPI.Enums;

namespace EmpresaAPI.Models
{
	public class DepartamentModel
	{
		public Guid Id { get; set; }
		public string Departament_Name { get; set; }
		public string Departament_Acronym { get; set; }

		public List<EmployeeModel> EmployeesOnDepartament { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public StatusEnum Status { get; set; }
	}
}

