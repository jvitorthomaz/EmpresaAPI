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
    public class EmployeeController: ControllerBase
	{

        private readonly ApplicationDbContext _DbContext;
        public EmployeeController(ApplicationDbContext DbContext)
		{
            _DbContext = DbContext;
        }


        /// <summary>
        /// Busca colaboradores
        /// </summary>
        /// <returns>Retorna os colaboradores da empresa</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Colaborador não encontrado</response>
        [HttpGet("buscar-colaboradores/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult BuscarColaboradorAll()
        {
            var colaborador = _DbContext.Employees.ToList();

            if (colaborador == null)
            {
                return NotFound();
            }

            return Ok(colaborador);

        }


        /// <summary>
        /// Busca colaboradores ativos na empresa
        /// </summary>
        /// <returns>Retorna os colaboradores ativos</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Colaborador buscado não encontrado</response>
        [HttpGet("buscar-colaboradores-ativos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult BuscarColaboradoresAtivos()
        {

            var todosColaboradoresAtivos = _DbContext.Employees.Where(c => c.Status == StatusEnum.Ativo).ToList();

            if (todosColaboradoresAtivos == null)
            {
                return NotFound();

            }

            return Ok(todosColaboradoresAtivos);

        }


        /// <summary>
        /// Busca colaboradores inativos na empresa
        /// </summary>
        /// <returns>Retorna os colaboradores inativos</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Colaborador buscado não encontrado</response>
        [HttpGet("buscar-colaboradores-inativos")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult BuscarColaboradoresInativos()
        {

            var todosColaboradoresInativos = _DbContext.Employees.Where(c => c.Status == StatusEnum.Inativo).ToList();

            if (todosColaboradoresInativos == null)
            {
                return NotFound();

            }

            return Ok(todosColaboradoresInativos);

        }


        /// <summary>
        /// Busca colaboradores ativos em um departamento
        /// </summary>
        /// <param name="idDepartamento">Id do departamento</param>
        /// <returns>Retorna os colaboradores ativos em um departamento</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Item não encontrado</response>
        [HttpGet("buscar-colaboradores-ativos-departamento/{idDepartamento}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult BuscarColaboradoresAtivosNoDepartamento(Guid idDepartamento)
        {
            var departamento = _DbContext.Departaments
                .Include(d => d.EmployeesOnDepartament.Where(c => c.Status == StatusEnum.Ativo))
                .SingleOrDefault(d => d.Id == idDepartamento && d.Status == StatusEnum.Ativo);

            if (departamento == null)
            {
                return NotFound("Departamento inativo ou não encontrado");
            }

            var colaboradoresAtivosDoDepartamento = departamento.EmployeesOnDepartament.Where(c => c.Status == StatusEnum.Ativo).ToList();

            if (colaboradoresAtivosDoDepartamento == null)
            {
                return NotFound();
            }

            return Ok(colaboradoresAtivosDoDepartamento);
        }


        /// <summary>
        /// Busca colaborador ativo por nome
        /// </summary>
        /// <param name="buscaColaborador">Campo de busca de nome ou documento</param>
        /// <returns>Retorna os colaboradores</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Colaborador buscado não encontrado</response>
        [HttpGet("buscar-colaborador-ativo-nome")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult BuscarColaboradoresAtivos(string? buscaColaborador)
        {

            var todosColaboradoresAtivos = _DbContext.Employees
                .Where(c => c.Status == StatusEnum.Ativo && (
                c.Employee_Name.ToLower().Contains(buscaColaborador.ToLower()) ||
                c.Employee_Document.Contains(buscaColaborador))
                ).ToList();

            if (buscaColaborador.Count() > 0)
            {
                return Ok(todosColaboradoresAtivos);

            }
            else
            {
                return NotFound();
            }


        }


        /// <summary>
        /// Busca colaborador ativo por departamento
        /// </summary>
        /// <param name="idDepartamento">Id do departamento</param>
        /// <param name="buscaColaborador">Campo de busca de nome ou documento</param>
        /// <returns>Retorna os colaborador ativo pelo departamento</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Item não encontrado</response>
        [HttpGet("buscar-colaborador-ativo-departamento/{idDepartamento}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult BuscarColaboradorAtivoPorDepartamento(Guid idDepartamento, string? buscaColaborador)
        {
            var departamento = _DbContext.Departaments
                .Include(d => d.EmployeesOnDepartament.Where(c => c.Status == StatusEnum.Ativo))
                .SingleOrDefault(d => d.Id == idDepartamento && d.Status == StatusEnum.Ativo);

            if (departamento == null)
            {
                return NotFound("Departamento inativo ou não encontrado");
            }

            var colaboradoresAtivosDoDepartamento = departamento.EmployeesOnDepartament
                .Where(c => c.Status == StatusEnum.Ativo && c.Employee_Name.ToLower().Contains(buscaColaborador.ToLower())).ToList();

            if (buscaColaborador.Count() > 0)
            {
                return Ok(colaboradoresAtivosDoDepartamento);

            }
            else
            {
                var colaboradorBuscadoComPesquisa = colaboradoresAtivosDoDepartamento.Where(d => d.Employee_Name.ToLower().Contains(buscaColaborador.ToLower()) ||
                d.Employee_Document.Contains(buscaColaborador)).ToList();

                if (colaboradorBuscadoComPesquisa.Count() == 0)
                {
                    return NotFound("Colaborador inativo ou não encontrado");
                }
                else
                {
                    return Ok(colaboradorBuscadoComPesquisa);

                }
            }
        }


        /// <summary>
        /// Busca colaboradores inativos
        /// </summary>
        /// <param name="buscaColaborador">Campo de busca de nome ou documento</param>
        /// <returns>Retorna todos colaboradores inativos e também por busca</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Colaborador não encontrado</response>
        [HttpGet("buscar-colaboradores-inativos-nome")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult BuscarColaboradorInativo(string? buscaColaborador)
        {

            var colaboradoresInativos = _DbContext.Employees
                .Where(c => c.Status == StatusEnum.Inativo && (
                c.Employee_Name.ToLower().Contains(buscaColaborador.ToLower()) ||
                c.Employee_Document.Contains(buscaColaborador))
                ).ToList();


            if (buscaColaborador.Count() > 0)
            {
                return Ok(colaboradoresInativos);
            }
            else
            {
                return NotFound();
            }
        }


        /// <summary>
        /// Busca colaboradores inativos em um departamento
        /// </summary>
        /// <param name="idDepartamento">Id do departamento</param>
        /// <returns>Retorna os colaboradores inativos em um departamento</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Item não encontrado</response>
        [HttpGet("buscar-colaboradores-inativos-departamento/{idDepartamento}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult BuscarColaboradoresInativosNoDepartamento(Guid idDepartamento)
        {
            var departamento = _DbContext.Departaments
                .Include(d => d.EmployeesOnDepartament.Where(c => c.Status == StatusEnum.Inativo))
                .SingleOrDefault(d => d.Id == idDepartamento && d.Status == StatusEnum.Ativo);

            if (departamento == null)
            {
                return NotFound("Departamento inativo ou não encontrado");
            }

            var colaboradoresInativosDoDepartamento = departamento.EmployeesOnDepartament.Where(c => c.Status == StatusEnum.Inativo).ToList();

            if (colaboradoresInativosDoDepartamento == null)
            {
                return NotFound();
            }

            return Ok(colaboradoresInativosDoDepartamento);
        }


        /// <summary>
        /// Busca colaborador por id
        /// </summary>
        /// <param name="id">Id do colaborador</param>
        /// <returns>Retorna o colaborador</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Colaborador não encontrado</response>
        [HttpGet("buscar-colaborador/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult BuscarColaboradorPorId(Guid id)
        {
            var colaborador = _DbContext.Employees.SingleOrDefault(d => d.Id == id);

            if (colaborador == null)
            {
                return NotFound();
            }

            return Ok(colaborador);

        }


        /// <summary>
        /// Cadastra um colaborador e relaciona ao departamento
        /// </summary>
        /// <param name="idDepartamento">Id do departamento</param>
        /// <param name="colaborador">Objeto de criação do departamento</param>
        /// <returns>Objeto criado</returns>
        /// <response code="201">Sucesso</response>
        /// <response code="400">Item requerido não inserido</response>
        /// <response code="404">Id do departamento inativo ou não encontrado</response>
        [HttpPost("cadastrar-colaborador/{idDepartamento}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CadastrarColaborador(Guid idDepartamento, EmployeeEntitie colaborador)
        {
            var departamento = _DbContext.Departaments.SingleOrDefault(c => c.Id == idDepartamento && c.Status == StatusEnum.Ativo);

            if (departamento == null)
            {
                return NotFound();
            }

            colaborador.Id_Departament = idDepartamento;

            _DbContext.Employees.Add(colaborador);

            _DbContext.SaveChanges();

            return CreatedAtAction(nameof(BuscarColaboradorPorId), new { id = colaborador.Id }, colaborador);
        }


        /// <summary>
        /// Atualiza um colaborador ativo
        /// </summary>
        /// <param name="id">Identificador do colaborador</param>
        /// <param name="input">Dados do colaborador</param>
        /// <returns>Nada.</returns>
        /// <response code="204">Sucesso</response>
        /// <response code="404">Id do colaborador inativo ou não encontrado</response>
        [HttpPut("atualizar-colaborador/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult AtualizarColaborador(Guid id, EmployeeEntitie input)
        {
            var colaborador = _DbContext.Employees.SingleOrDefault(d => d.Id == id && d.Status == StatusEnum.Ativo);

            if (colaborador == null)
            {
                return NotFound();
            }

            colaborador.Employee_Name = input.Employee_Name;
            colaborador.Employee_Document = input.Employee_Document;
            colaborador.Id_Departament = input.Id_Departament;

            _DbContext.Employees.Update(colaborador);
            _DbContext.SaveChanges();

            return NoContent();
        }


        /// <summary>
        /// Inativa um colaborador ativo
        /// </summary>
        /// <param name="id">Identificador do colaborador</param>
        /// <returns>Nada.</returns>
        /// <response code="204">Sucesso</response>
        /// <response code="404">Id do colaborador inativo ou não encontrado</response>
        [HttpDelete("inativar-colaborador/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult InativarColaborador(Guid id)
        {
            var colaboradorParaInativar = _DbContext.Employees.SingleOrDefault(d => d.Id == id && d.Status == StatusEnum.Ativo);

            if (colaboradorParaInativar == null)
            {
                return NotFound("Não encontrado");
            }


            colaboradorParaInativar.Status = StatusEnum.Inativo;
            _DbContext.Employees.Update(colaboradorParaInativar);
            _DbContext.SaveChanges();

            return NoContent();

        }

    }
}
