using BaltaDataAccess.Models;
using Dapper;
using Microsoft.Data.SqlClient;

const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$;Encrypt=False";

using (var connection = new SqlConnection(connectionString))
{
  ListCategories(connection);
  //CreateCategory(connection);
}

static void ListCategories(SqlConnection connection)
{
  var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category]");
  foreach (var item in categories)
  {
    Console.WriteLine($"{item.Id} - {item.Title}");
  }
}

static void CreateCategory(SqlConnection connection)
{
  var category = new Category {
  Id = Guid.NewGuid(),
  Title = "Amazon AWS",
  Url = "amazon",
  Summary = "AWS Cloud",
  Order = 8,
  Description = "Categoria destinada a serviços do AWS",
  Featured = false
};

var insertSql = @"INSERT INTO
      [Category]
    VALUES (
      @Id,
      @Title,
      @Url,
      @Summary,
      @Order,
      @Description,
      @Featured
    )";

  var rows = connection.Execute(insertSql, new {
      category.Id,
      category.Title,
      category.Url,
      category.Summary,
      category.Order,
      category.Description,
      category.Featured
    });

    Console.WriteLine($"{rows} linhas inseridas");
}