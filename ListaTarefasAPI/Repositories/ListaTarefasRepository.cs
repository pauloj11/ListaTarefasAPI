using Microsoft.EntityFrameworkCore;
using ListaTarefasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static ListaTarefasAPI.Models.ListaDeTarefasDTO;
using Microsoft.AspNetCore.Mvc;

public interface IListaTarefasRepository
{
    void AddListaDeTarefas(ListaDeTarefasDTO listaDeTarefasDTO);
    IEnumerable<ListaDeTarefasDTO> GetListasDeTarefas();
    object GetListaDeTarefasById(int listaDeTarefasID);
    void DeleteListaDeTarefas(int listaDeTarefasID);
    void AddTarefaALista(TarefaDTO tarefaDTO);
    TarefaDTO GetTarefaById(int tarefaId);
    IEnumerable<TarefaDTO> GetTarefasDaLista(int listaDeTarefasID);
    void UpdateTarefa(TarefaDTO tarefaDTO);
    void DeleteTarefa(TarefaDTO tarefaDTO);
}

public class ListaTarefasRepository : IListaTarefasRepository
{
    private readonly ListaTarefasDbContext _context;

    public ListaTarefasRepository(ListaTarefasDbContext context)
    {
        _context = context;
    }

    public void AddListaDeTarefas(ListaDeTarefasDTO listaDeTarefasDTO)
    {
        _context.ListasDeTarefas.Add(listaDeTarefasDTO);
        _context.SaveChanges();
    }

    public IEnumerable<ListaDeTarefasDTO> GetListasDeTarefas()
    {
        return _context.ListasDeTarefas.ToList();
    }

    public object GetListaDeTarefasById(int listaDeTarefasID)
    {
        var listaDeTarefas = _context.ListasDeTarefas.FirstOrDefault(l => l.ListaDeTarefasID == listaDeTarefasID);

        if (listaDeTarefas == null)
        {
            return null;
        }

        var tarefas = _context.Tarefas
            .Where(t => t.ListaDeTarefasID == listaDeTarefasID)
            .ToList();

        var result = new
        {
            ListaDeTarefas = listaDeTarefas,
            Tarefas = tarefas
        };

        return result;
    }

    public IEnumerable<TarefaDTO> GetTarefasDaLista(int listaDeTarefasID)
    {
        return _context.Tarefas
            .Where(t => t.ListaDeTarefasID == listaDeTarefasID)
            .ToList();
    }

    public void DeleteListaDeTarefas(int listaDeTarefasID)
    {
        var listaDeTarefas = _context.ListasDeTarefas.Find(listaDeTarefasID);
        if (listaDeTarefas != null)
        {
            _context.ListasDeTarefas.Remove(listaDeTarefas);
            _context.SaveChanges();
        }
    }

    public TarefaDTO GetTarefaById(int tarefaId)
    {
        var tarefa = _context.Tarefas.FirstOrDefault(t => t.TarefaID == tarefaId);

        if (tarefa == null)
        {
            return null;
        }

        return tarefa;
    }

    public void AddTarefaALista(TarefaDTO tarefaDTO)
    {
        if (tarefaDTO != null)
        {
            _context.Tarefas.Add(tarefaDTO);
            _context.SaveChanges();
        }
        else
        {
            throw new Exception("Tarefa inválida.");
        }
    }

    public void UpdateTarefa(TarefaDTO tarefaDTO)
    {
        _context.Tarefas.Update(tarefaDTO);
        _context.SaveChanges();
    }

    public void DeleteTarefa(TarefaDTO tarefaDTO)
    {
        _context.Tarefas.Remove(tarefaDTO);
        _context.SaveChanges();
    }
}
