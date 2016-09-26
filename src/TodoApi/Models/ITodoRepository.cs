using System.Collections.Generic;

namespace TodoApi.Models
{
	public interface ITodoRepository
	{
		void Add(TodoItem item);
		void Update(TodoItem item);
		IEnumerable<TodoItem> GetAll();
		TodoItem Find(string key);
		TodoItem Remove(string key);
	}
}
