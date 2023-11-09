using Microsoft.AspNetCore.Mvc;
using ListaTarefasAPI.Models;
using System;
using static ListaTarefasAPI.Models.ListaDeTarefasDTO;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net;

[Route("api/listadetarefas")]
[ApiController]
public class ListaDeTarefasController : ControllerBase
{
    private readonly IListaTarefasRepository _listaTarefasRepository;

    public ListaDeTarefasController(IListaTarefasRepository listaTarefasRepository)
    {
        _listaTarefasRepository = listaTarefasRepository;
    }

    /// <summary>
    /// Adiciona uma nova lista de tarefas.
    /// </summary>
    /// <param name="listaDeTarefasDTO">Os detalhes da nova lista de tarefas a ser adicionada.</param>
    /// <returns>Retorna um objeto ActionResult indicando o resultado da operação.</returns>
    [HttpPost("adicionarlista", Name = "AdicionarListaDeTarefas")]
    public IActionResult AdicionarListaDeTarefas([FromBody] ListaDeTarefasDTO listaDeTarefasDTO)
    {
        try
        {
            _listaTarefasRepository.AddListaDeTarefas(listaDeTarefasDTO);
            return CreatedAtRoute("ListarTarefas", new { id = listaDeTarefasDTO.ListaDeTarefasID }, listaDeTarefasDTO);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro: {ex.Message}");
        }
    }

    /// <summary>
    /// Obtém a lista de todas as tarefas disponíveis.
    /// </summary>
    /// <returns>Retorna um objeto ActionResult contendo a lista de tarefas,
    /// ou NotFound se nenhuma lista de tarefas for encontrada.</returns>
    [HttpGet("listartarefas", Name = "ListarTarefas")]
    public ActionResult<IEnumerable<ListaDeTarefasDTO>> GetListasDeTarefas()
    {
        var listasDeTarefas = _listaTarefasRepository.GetListasDeTarefas();

        if (listasDeTarefas == null || !listasDeTarefas.Any())
        {
            return NotFound();
        }

        return Ok(listasDeTarefas);
    }

    /// <summary>
    /// Adiciona uma nova tarefa a uma lista existente.
    /// </summary>
    /// <param name="tarefaDTO">Objeto contendo os detalhes da tarefa a ser adicionada.</param>
    /// <returns>
    /// Retorna um objeto ActionResult indicando sucesso, juntamente com a URI da tarefa criada,
    /// ou BadRequest se ocorrer um erro ao adicionar a tarefa.
    /// </returns>
    [HttpPost("listastarefas/tarefas")]
    public IActionResult AddTarefaALista([FromBody] TarefaDTO tarefaDTO)
    {
        try
        {
            _listaTarefasRepository.AddTarefaALista(tarefaDTO);
            return CreatedAtRoute("GetTarefaById", new { tarefaId = tarefaDTO.TarefaID }, tarefaDTO);
        }
        catch (Exception ex)
        {
            return BadRequest("Erro ao adicionar a tarefa: " + ex.Message);
        }
    }

    /// <summary>
    /// Obtém detalhes de uma tarefa com base no seu identificador único.
    /// </summary>
    /// <param name="tarefaId">O identificador único da tarefa.</param>
    /// <returns>
    /// Retorna um ActionResult contendo os detalhes da tarefa se encontrada, 
    /// ou NotFound se a tarefa não for encontrada.
    /// </returns>
    [HttpGet("tarefas/{tarefaId}", Name = "GetTarefaById")]
    public ActionResult<TarefaDTO> GetTarefaById(int tarefaId)
    {
        var tarefa = _listaTarefasRepository.GetTarefaById(tarefaId);

        if (tarefa == null)
        {
            return NotFound("Tarefa não encontrada.");
        }

        return Ok(tarefa);
    }

    /// <summary>
    /// Obtém as tarefas de uma lista de tarefas, associada ao identificador único da lista na tarefa.
    /// </summary>
    /// <param name="listaDeTarefasId">O identificador único da lista de tarefas.</param>
    /// <returns>
    /// Retorna um ActionResult contendo a lista de tarefas se encontrada, 
    /// ou NotFound se a lista de tarefas não for encontrada ou não possuir tarefas.
    /// </returns>
    [HttpGet("listastarefas/{listaDeTarefasId}/tarefas", Name = "GetTarefasDaLista")]
    public ActionResult<IEnumerable<TarefaDTO>> GetTarefasDaLista(int listaDeTarefasId)
    {
        var tarefas = _listaTarefasRepository.GetTarefasDaLista(listaDeTarefasId);

        if (tarefas == null || !tarefas.Any())
        {
            return NotFound("Nenhuma tarefa encontrada para esta lista.");
        }

        return Ok(tarefas);
    }

    /// <summary>
    /// Atualiza o status de conclusão da tarefa com base no identificador único da tarefa.
    /// </summary>
    /// <param name="tarefaId">O identificador único da tarefa a ser atualizada.</param>
    /// <param name="tarefaDTO">Os dados da tarefa contendo o novo status de conclusão.</param>
    /// <returns>
    /// Retorna um ActionResult representando o status da atualização.
    /// Retorna NotFound se a tarefa não for encontrada.
    /// Retorna NoContent se a atualização for bem-sucedida sem dados adicionais.
    /// </returns>
    [HttpPut("tarefas/{tarefaId}")]
    public IActionResult UpdateTarefaConcluida(int tarefaId, [FromBody] TarefaDTO tarefaDTO)
    {
        var existingTarefa = _listaTarefasRepository.GetTarefaById(tarefaId);

        if (existingTarefa == null)
        {
            return NotFound("Tarefa não encontrada.");
        }

        // Atualiza apenas o campo Concluida com status "false" ou "true"
        existingTarefa.Concluida = tarefaDTO.Concluida; 

        _listaTarefasRepository.UpdateTarefa(existingTarefa);

        // Retorna um status 204 No Content para indicar sucesso sem dados adicionais.
        return NoContent();
    }

    /// <summary>
    /// Exclui uma tarefa com base no identificador único da tarefa.
    /// </summary>
    /// <param name="tarefaId">O identificador único da tarefa a ser excluída.</param>
    /// <returns>
    /// Retorna um ActionResult representando o status da exclusão.
    /// Retorna NotFound se a tarefa não for encontrada.
    /// Retorna NoContent se a exclusão for bem-sucedida sem dados adicionais.
    /// </returns>
    [HttpDelete("tarefas/{tarefaId}")]
    public IActionResult DeleteTarefa(int tarefaId)
    {
        var existingTarefa = _listaTarefasRepository.GetTarefaById(tarefaId);

        if (existingTarefa == null)
        {
            return NotFound("Tarefa não encontrada.");
        }

        _listaTarefasRepository.DeleteTarefa(existingTarefa);

        // Retorna um status 204 No Content para indicar sucesso sem dados adicionais.
        return NoContent(); 
    }

    /// <summary>
    /// Exclui uma lista de tarefas com base no identificador único da lista.
    /// </summary>
    /// <param name="listaDeTarefasId">O identificador único da lista de tarefas a ser excluída.</param>
    /// <returns>
    /// Retorna um ActionResult representando o status da exclusão.
    /// Retorna NotFound se a lista de tarefas não for encontrada.
    /// Retorna NoContent se a exclusão for bem-sucedida sem dados adicionais.
    /// </returns>
    [HttpDelete("listadetarefas/{listaDeTarefasId}")]
    public IActionResult DeleteListaDeTarefas(int listaDeTarefasId)
    {
        var existingListaDeTarefas = _listaTarefasRepository.GetListaDeTarefasById(listaDeTarefasId);

        if (existingListaDeTarefas == null)
        {
            return NotFound("Lista de tarefas não encontrada.");
        }

        _listaTarefasRepository.DeleteListaDeTarefas(listaDeTarefasId);

        // Retorna um status 204 No Content para indicar sucesso sem dados adicionais.
        return NoContent();
    }

    /// <summary>
    /// Obtém uma lista de tarefas por ID, incluindo suas tarefas associadas, 
    /// com opção de personalizar a serialização JSON.
    /// </summary>
    /// <param name="listaDeTarefasId">O ID da lista de tarefas desejada.</param>
    /// <returns>Uma resposta HTTP contendo os detalhes da lista de tarefas e suas tarefas associadas.</returns>
    [HttpGet("listastarefas/{listaDeTarefasId}", Name = "GetListaDeTarefasById")]
    public IActionResult GetListaDeTarefasById(int listaDeTarefasId)
    {
        var result = _listaTarefasRepository.GetListaDeTarefasById(listaDeTarefasId);

        if (result == null)
        {
            return NotFound("Lista de tarefas não encontrada.");
        }

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            // Opção de adicionar mais configurações para o JSON caso seja necessário.
        };

        var json = JsonSerializer.Serialize(result, options);

        return new ContentResult
        {
            StatusCode = (int)HttpStatusCode.OK,
            Content = json,
            ContentType = "application/json"
        };
    }

}
