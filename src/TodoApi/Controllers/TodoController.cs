using System.Collections.Generic;
using TodoApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Controllers
{
	[Route("api/[controller]")]
	public class TodoController : Controller
	{
		public ITodoRepository TodoItems { get; set; }

		public TodoController(ITodoRepository todoItems)
		{
			TodoItems = todoItems;
		}

		// GET: api/todo
		[HttpGet]
		public IEnumerable<TodoItem> GetAll()
		{
			return TodoItems.GetAll();
		}

		// GET: api/todo/{id}
		[HttpGet("{id}", Name = "GetTodo")]
		public IActionResult GetById(string id)
		{
			var item = TodoItems.Find(id);
			if (item == null)
			{
				return NotFound();
			}
	
			return new ObjectResult(item);
		}

		// POST: api/todo/
		[HttpPost]
		public IActionResult Create([FromBody] TodoItem item)
		{
			if (item == null)
			{
				return BadRequest();
			}
			TodoItems.Add(item);
			return CreatedAtRoute("GetTodo", new {id = item.Key}, item);
		}

		// PUT: api/todo/
		[HttpPut("{id}")]
		public IActionResult Update(string id, [FromBody] TodoItem item)
		{
			if (item == null || id != item.Key)
			{
				return BadRequest();
			}

			var todo = TodoItems.Find(id);
			if (todo == null)
			{
				return NotFound();
			}

			TodoItems.Update(item);
			return NoContent();
		}

		[HttpPatch("{id}")]
		public IActionResult Update([FromBody] TodoItem item, string id)
		{
			if (item == null)
			{
				return BadRequest();
			}

			var todo = TodoItems.Find(id);

			if (todo == null)
			{
				return NotFound();
			}

			item.Key = id;
			TodoItems.Update(item);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(string id)
		{
			var todo = TodoItems.Find(id);

			if (todo == null)
			{
				return NotFound();
			}

			TodoItems.Remove(id);
			return NoContent();
		}

	}
}
