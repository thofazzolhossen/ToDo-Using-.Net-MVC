using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Models;

public class TodoController : Controller
{
    private readonly IRepository<Todo> _todoRepository;

    public TodoController(IRepository<Todo> todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<IActionResult> Index()
    {
        var todos = await _todoRepository.GetAll();
        return View(todos.OrderByDescending(t => t.Id));
    }

    [HttpPost]
    public async Task<IActionResult> Add(string title)
    {
        if (!string.IsNullOrEmpty(title))
        {
            var todo = new Todo { Title = title, IsCompleted = false };
            await _todoRepository.Add(todo);
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, string title)
    {
        var todo = await _todoRepository.GetById(id);
        if (todo != null)
        {
            todo.Title = title;
            await _todoRepository.Update(todo);
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _todoRepository.Delete(id);
        return RedirectToAction("Index");
    }
}
