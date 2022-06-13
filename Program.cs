using BaltaDataAccess.Models;
using Dapper;
using Microsoft.Data.SqlClient;

const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$;Encrypt=False";

using (var connection = new SqlConnection(connectionString))
{
  //CreateCategory(connection);
  //CreateManyCategory(connection);
  //UpdateCategory(connection);
  //DeleteCategory(connection);
  ListCategories(connection);
  //GetCategory(connection);
}

static void ListCategories(SqlConnection connection)
{
  var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category]");
  foreach (var item in categories)
  {
    Console.WriteLine($"{item.Id} - {item.Title}");
  }
}

static void GetCategory(SqlConnection connection) {
  var category = connection
    .QueryFirstOrDefault<Category>(
      "SELECT TOP 1 [Id], [Title] FROM [Category] WHERE [Id]=@id",
      new
      {
        id = "af3407aa-11ae-4621-a2ef-2028b85507c4"
      });
  Console.WriteLine($"{category.Id} - {category.Title}");
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

static void CreateManyCategory(SqlConnection connection)
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

  var category2 = new Category {
    Id = Guid.NewGuid(),
    Title = "Categoria Nova",
    Url = "categoria-nova",
    Summary = "Categoria Nova",
    Order = 9,
    Description = "Categoria",
    Featured = true
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

  var rows = connection.Execute(insertSql, new []{
    new {
      category.Id,
      category.Title,
      category.Url,
      category.Summary,
      category.Order,
      category.Description,
      category.Featured
    },
    new {
      category2.Id,
      category2.Title,
      category2.Url,
      category2.Summary,
      category2.Order,
      category2.Description,
      category2.Featured
    }});

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
    Id = new Guid("00000000-0000-0000-0000-000000000000")
  });

  Console.WriteLine($"{rows} registros excluídos");
}