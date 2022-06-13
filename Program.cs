using BaltaDataAccess.Models;
using Dapper;
using Microsoft.Data.SqlClient;

const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$;Encrypt=False";

using (var connection = new SqlConnection(connectionString))
{
  //CreateCategory(connection);
  //UpdateCategory(connection);
  // DeleteCategory(connection);
  ListCategories(connection);
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

static void UpdateCategory(SqlConnection connection)
{
  var updateQuery = @"UPDATE [Category]
    SET 
      [Title]=@Title
    WHERE
      [Id]=@Id";
  
  var rows = connection.Execute(updateQuery, new {
    Title = "Frontend 2022",
    Id = new Guid("af3407aa-11ae-4621-a2ef-2028b85507c4")
  });

  Console.WriteLine($"{rows} registros atualizados");
}

static void DeleteCategory(SqlConnection connection)
{
  var deleteQuery = "DELETE FROM [Category] WHERE [Id]=@Id";
  
  var rows = connection.Execute(deleteQuery, new {
    Id = new Guid("af3407aa-11ae-4621-a2ef-2028b85507c4")
  });

  Console.WriteLine($"{rows} registros excluídos");
}