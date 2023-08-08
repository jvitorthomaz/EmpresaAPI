using System;
using EmpresaAPI.Entities;
using EmpresaAPI.Enums;
using EmpresaAPI.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmpresaAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentController : ControllerBase
	{

		private readonly ApplicationDbContext _DbContext;
        public DepartamentController(ApplicationDbContext DbContext)
		{
            _DbContext = DbContext;

        }


        /// <summary>
        /// Busca os departamentos
        /// </summary>
        /// <returns>Retorna os departamento</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Id do departamento inativo ou não encontrado</response>
        [HttpGet("listar-departamentos/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ListarAllDepartamentos()
        {
            var departamento = _DbContext.Departaments.Include(x => x.EmployeesOnDepartament).ToList();

            if (departamento == null)
            {
                return NotFound();
            }

            departamento.ForEach(x =>  x.EmployeesOnDepartament = x.EmployeesOnDepartament.Where(y => y.Status == StatusEnum.Ativo).ToList());

            return Ok(departamento);

        }

        /// <summary>
        /// Listar departamentos ativos 
        /// </summary>
        /// <returns>Retorna os departamentos ativos </returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        [HttpGet("listar-departamentos-ativos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ListarDepartamentosAtivos()
        {
            var departamentosAtivos = _DbContext.Departaments.Where(d => d.Status == StatusEnum.Ativo)
                .Include(x => x.EmployeesOnDepartament).Where(d => d.Status == StatusEnum.Ativo).ToList();

            if (departamentosAtivos == null)
            {
                return NotFound();

            }

            departamentosAtivos.ForEach(x => x.EmployeesOnDepartament = x.EmployeesOnDepartament.Where(y => y.Status == StatusEnum.Ativo).ToList());

            return Ok(departamentosAtivos);
        }


        /// <summary>
        /// Listar departamentos inativos 
        /// </summary>
        /// <returns>Retorna os departamentos inativos </returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        [HttpGet("listar-departamentos-inativos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult ListarDepartamentosInativos()
        {
            var departamentosInativos = _DbContext.Departaments.Where(d => d.Status == StatusEnum.Inativo);

            if (departamentosInativos == null)
            {
                return NotFound();

            }

            return Ok(departamentosInativos);
        }


        /// <summary>
        /// Busca departamentos ativos por nome ou sigla
        /// </summary>
        /// <param name="buscaDepartamento">Campo de busca de nome ou sigla</param>
        /// <returns>Retorna os departamentos ativos filtrados por nome ou sigla</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        [HttpGet("buscar-departamento-ativo-nome")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult BuscarDepartamentosAtivosPorNome(string? buscaDepartamento)
        {
            var departamentosAtivos = _DbContext.Departaments
                .Include(x => x.EmployeesOnDepartament)
                .Where(d => d.Status == StatusEnum.Ativo && (
                d.Departament_Name.ToLower().Contains(buscaDepartamento.ToLower())
                || d.Departament_Acronym.ToLower().Contains(buscaDepartamento.ToLower()))
                ).ToList();

            if (buscaDepartamento.Count() > 0)
            {
                departamentosAtivos.ForEach(x => x.EmployeesOnDepartament = x.EmployeesOnDepartament.Where(y => y.Status == StatusEnum.Ativo).ToList());

                return Ok(departamentosAtivos);

            }
            else
            {
                return NotFound();

            }
        }

        /// <summary>
        /// Busca departamentos inativos por nome ou sigla
        /// </summary>
        /// <param name="buscaDepartamento">Campo de busca de nome ou sigla</param>
        /// <returns>Retorna todos departamentos inativos no banco</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Departamento buscado não encontrado</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("buscar-departamento-inativo-nome")]
        public IActionResult BuscarDepartamentoInativoPorNome(string? buscaDepartamento)
        {
            var departamentosDesativados = _DbContext.Departaments.Where(d => d.Status == StatusEnum.Inativo && (
                d.Departament_Name.ToLower().Contains(buscaDepartamento.ToLower()) || d.Departament_Acronym.ToLower().Contains(buscaDepartamento.ToLower()))
                ).ToList();

            if (buscaDepartamento.Count() > 0)
            {
                return Ok(departamentosDesativados);

            }
            else
            {
        
                return NotFound();

            }
        }


        /// <summary>
        /// Busca departamento por id
        /// </summary>
        /// <param name="id">Identificador do departamento</param>
        /// <returns>Retorna o departamento específico</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Id do departamento não encontrado</response>
        [HttpGet("buscar-departamento/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult BuscarDepartamentoPorId(Guid id)
        {
            var departamento = _DbContext.Departaments
                .Include(de => de.EmployeesOnDepartament)
                .SingleOrDefault(d => d.Id == id);

            if (departamento == null)
            {
                return NotFound();
            }
            if(departamento.Status == StatusEnum.Ativo)
            {
                departamento.EmployeesOnDepartament = departamento.EmployeesOnDepartament.Where(x => x.Status == StatusEnum.Ativo).ToList();
                return Ok(departamento);
            }

            return Ok(departamento);

        }

        /// <summary>
        /// Cria um novo departamento
        /// </summary>
        /// <param name="departamento">Objeto de criação do departamento</param>
        /// <returns>Objeto criado</returns>
        /// <response code="201">Sucesso</response>
        /// <response code="400">Item requerido não inserido</response>
        [HttpPost("criar-departamento")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CriarDepartamento(DepartamentEntitie departamento)
        {
            var departamentoDb = _DbContext.Departaments
               .Where(d => d.Departament_Name == departamento.Departament_Name && d.Departament_Acronym == departamento.Departament_Acronym); 

            if (departamentoDb.Count() > 0)
            {
                return BadRequest();
            }

            _DbContext.Departaments.Add(departamento);

            _DbContext.SaveChanges();

            return CreatedAtAction(nameof(BuscarDepartamentoPorId), new { id = departamento.Id }, departamento);
        }


        /// <summary>
        /// Atualiza um departamento ativo
        /// </summary>
        /// <param name="id">Identificador do departamento</param>
        /// <param name="input">Dados do departamento</param>
        /// <response code="200">Sucesso</response>
        /// <response code="400">Item requerido não inserido</response>
        /// <response code="404">Id do departamento inativo ou não encontrado</response>
        [HttpPut("atualizar-departamento/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult AtualizarDepartamento(Guid id, DepartamentEntitie input)
        {
            var departamento = _DbContext.Departaments.SingleOrDefault(d => d.Id == id && d.Status == StatusEnum.Ativo);

            if (departamento == null)
            {
                return NotFound();
            }

            departamento.Departament_Name = input.Departament_Name;
            departamento.Departament_Acronym = input.Departament_Acronym;

            _DbContext.Departaments.Update(departamento);
            _DbContext.SaveChanges();

            return Ok();
        }


        /// <summary>
        /// Inativa um departamento ativo
        /// </summary>
        /// <param name="id">Identificador do departamento</param>
        /// <returns>Nada.</returns>
        /// <response code="204">Sucesso</response>
        /// <response code="404">Item não encontrado</response>
        [HttpDelete("inativar-departamento/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult InativarDepartamento(Guid id)
        {
            var departamentoParaInativar = _DbContext.Departaments
                .Include(x => x.EmployeesOnDepartament)
                .SingleOrDefault(d => d.Id == id && d.Status == StatusEnum.Ativo);

            if (departamentoParaInativar == null)
            {
                return NotFound("Não encontrado");
            }

            int numeroDeColaboradoresAtivos = departamentoParaInativar.EmployeesOnDepartament == null ? 0 : departamentoParaInativar.EmployeesOnDepartament.Count();

            if (numeroDeColaboradoresAtivos > 0)
            {
                return NotFound("Não é possível inativar um departamento com colaboradores ativos");
            }

            departamentoParaInativar.Status = StatusEnum.Inativo;
            _DbContext.Departaments.Update(departamentoParaInativar);
            _DbContext.SaveChanges();

            return NoContent();

        }

    }
}
