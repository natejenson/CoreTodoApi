using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Models;

namespace TodoApi.Tests
{
	internal class MockTodoRepository : ITodoRepository
	{
		public ConcurrentDictionary<string, TodoItem> Todos;

		public MockTodoRepository()
		{
			Todos =
				new ConcurrentDictionary<string, TodoItem>(
					GetTestTodoItems().Select(t => new KeyValuePair<string, TodoItem>(t.Key, t)));
		}

		public void Add(TodoItem item)
		{
			Todos.TryAdd(Guid.NewGuid().ToString(), item);
		}

		public void Update(TodoItem item)
		{
			Todos[item.Key] = item;
		}

		public IEnumerable<TodoItem> GetAll()
		{
			return Todos.Values;
		}

		public TodoItem Find(string key)
		{
			TodoItem todo;
			Todos.TryGetValue(key, out todo);
			return todo;
		}

		public TodoItem Remove(string key)
		{
			TodoItem todo;
			Todos.TryRemove(key, out todo);
			return todo;
		}

		public static IEnumerable<TodoItem> GetTestTodoItems()
		{
			return new []
			{
				new TodoItem { Key = "1", Name = "Go for a bike ride", IsComplete = true },
				new TodoItem { Key = "2", Name = "Read the Iliad", IsComplete = false },
				new TodoItem { Key = "3", Name = "Brew some beer", IsComplete = true },
				new TodoItem { Key = "4", Name = "Go hiking", IsComplete = false },
				new TodoItem { Key = "5", Name = "Adopt a puppy", IsComplete = false },
				new TodoItem { Key = "6", Name = "Build a .Net Core API", IsComplete = true }
			};
		} 
	}
}
