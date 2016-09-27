using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Controllers;
using TodoApi.Models;
using Xunit;

namespace TodoApi.Tests
{
	public class TodoControllerTests
	{
		public static IEnumerable<object[]> MismatchedKeyTodos => new[]
		{
			new object[] { "key1", new TodoItem {Key = "keyOne"} },
			new object[] { "foo", new TodoItem {Key = "bar"} },
			new object[] { "a", new TodoItem {Key = "A"} }
		};

		public static IEnumerable<object[]> MissingKeyTodos => new[]
		{
			new object[] { "nope", new TodoItem {Key = "nope"} },
			new object[] { "foo", new TodoItem {Key = "foo"} },
			new object[] { "bar", new TodoItem {Key = "bar"} }
		};

		[Fact]
		public void GetAll_ShouldReturnAllTodos()
		{
			var todoRepository = new MockTodoRepository();
			var todoController = new TodoController(todoRepository);

			Assert.Equal(todoRepository.Todos.Values, todoController.GetAll());
		}

		[Theory]
		[InlineData("-1")]
		[InlineData("asdf")]
		[InlineData("foobar")]
		public void GetById_WithWrongId_ShouldReturnNotFound(string key)
		{
			var todoRepository = new MockTodoRepository();
			var todoController = new TodoController(todoRepository);

			Assert.IsType(typeof(NotFoundResult), todoController.GetById(key));
		}

		[Theory]
		[InlineData("1")]
		[InlineData("2")]
		[InlineData("3")]
		public void GetById_WithGoodId_ShouldReturnTodo(string key)
		{
			var todoRepository = new MockTodoRepository();
			var todoController = new TodoController(todoRepository);

			var result = todoController.GetById(key) as ObjectResult;
			Assert.NotNull(result); 
			Assert.IsType(typeof(TodoItem), result.Value);
		}

		[Fact]
		public void Create_WithNullTodo_ShouldReturnBadRequest()
		{
			var todoRepository = new MockTodoRepository();
			var todoController = new TodoController(todoRepository);

			var result = todoController.Create(null);
			Assert.IsType(typeof (BadRequestResult), result);
		}

		[Theory, MemberData(nameof(MismatchedKeyTodos))]
		public void Update_WithMismatchKey_ShouldReturnBadRequest(string key, TodoItem item)
		{
			var todoRepository = new MockTodoRepository();
			var todoController = new TodoController(todoRepository);

			var result = todoController.Update(key, item);
			Assert.IsType(typeof (BadRequestResult), result);
		}

		[Theory, MemberData(nameof(MissingKeyTodos))]
		public void Update_WithMissingItem_ShouldReturnNotFound(string key, TodoItem item)
		{
			var todoRepository = new MockTodoRepository();
			var todoController = new TodoController(todoRepository);

			var result = todoController.Update(key, item);
			Assert.IsType(typeof(NotFoundResult), result);
		}

		[Theory]
		[InlineData("-1")]
		[InlineData("asdf")]
		[InlineData("foobar")]
		public void Delete_WithWrongId_ShouldReturnNotFound(string key)
		{
			var todoRepository = new MockTodoRepository();
			var todoController = new TodoController(todoRepository);

			var result = todoController.Delete(key);
			Assert.IsType(typeof(NotFoundResult), result);
		}

		[Theory]
		[InlineData("1")]
		[InlineData("2")]
		[InlineData("3")]
		public void Delete_WithId_ShouldReturnNoContent(string key)
		{
			var todoRepository = new MockTodoRepository();
			var todoController = new TodoController(todoRepository);

			var result = todoController.Delete(key);
			Assert.IsType(typeof(NoContentResult), result);
		}
	}
}
