using System;
using System.Text.Json.Serialization;
using EmpresaAPI.Enums;

namespace EmpresaAPI.Models;

public class EmployeeModel
{
    public Guid Id { get; set; }
    public string Employee_Name { get; set; }
    public string Employee_Document { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public StatusEnum Status { get; set; }
    public Guid Id_Departament { get; set; }
        
}

