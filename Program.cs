using BaltaDataAccess.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$;Encrypt=False";

using (var connection = new SqlConnection(connectionString))
{
  //CreateCategory(connection);
  //CreateManyCategory(connection);
  //UpdateCategory(connection);
  //DeleteCategory(connection);
  //ListCategories(connection);
  //GetCategory(connection);
  //ExecuteProcedure(connection);
  //ExecuteReadProcedure(connection);
  //ExecuteScalar(connection);
  //ReadView(connection);
  //OneToOne(connection);
  //OneToMany(connection);
  //QueryMultiple(connection);
  //SelectIn(connection);
  //Like(connection, "api");
  Transaction(connection);
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

static void ExecuteProcedure(SqlConnection connection)
{
  var procedure = "[spDeleteStudent]";
  var pars = new { StudentId = "c78d389b-bbb4-4b60-bc5f-285487c1e952" };

  var affectedRows = connection.Execute(procedure, pars, commandType: CommandType.StoredProcedure);

  Console.WriteLine($"{affectedRows} linhas afetadas");
}

static void ExecuteReadProcedure(SqlConnection connection)
{
  var procedure = "[spGetCoursesByCategory]";
  var pars = new { CategoryId = "09ce0b7b-cfca-497b-92c0-3290ad9d5142" };

  var courses = connection.Query(procedure, pars, commandType: CommandType.StoredProcedure);

  foreach (var item in courses) {
    Console.WriteLine($"{item.Id} - {item.Title}");
  }
}

static void ExecuteScalar(SqlConnection connection)
{
  var category = new Category {
  Title = "Amazon AWS",
  Url = "amazon",
  Summary = "AWS Cloud",
  Order = 8,
  Description = "Categoria destinada a serviços do AWS",
  Featured = false
};

var insertSql = @"INSERT INTO
      [Category]
    OUTPUT inserted.[Id]
    VALUES (
      NEWID(),
      @Title,
      @Url,
      @Summary,
      @Order,
      @Description,
      @Featured
    )";

  var id = connection.ExecuteScalar<Guid>(insertSql, new {
      category.Title,
      category.Url,
      category.Summary,
      category.Order,
      category.Description,
      category.Featured
    });

    Console.WriteLine($"A categoria inserida foi: {id}");
}

static void ReadView(SqlConnection connection)
{

  var sql = "SELECT * FROM [vwCourses]";

  var courses = connection.Query(sql);
  foreach (var item in courses)
  {
    Console.WriteLine($"{item.Id} - {item.Title}");
  }
}

static void OneToOne(SqlConnection connection)
{
  var sql = @"
    SELECT
      *
    FROM
      [CareerItem]
    INNER JOIN
      [Course] ON [CareerItem].[CourseId] = [Course].[Id]";

  var items = connection.Query<CareerItem, Course, CareerItem>(
    sql,
    (careerItem, course) => {
      careerItem.Course = course;
      return careerItem;
    }, splitOn: "Id");

  foreach (var item in items) {
    Console.WriteLine($"{item.Title} - Curso: {item.Course.Title}");
  }
}

static void OneToMany(SqlConnection connection)
{
  var sql = @"
      SELECT
        [Career].[Id],
        [Career].[Title],
        [CareerItem].[CareerId],
        [CareerItem].[Title]
      FROM
        [Career]
      INNER JOIN
        [CareerItem] ON [CareerItem].[CareerId] = [Career].[Id]
      ORDER BY
        [Career].[Title]";

  var careers = new List<Career>();
  var items = connection.Query<Career, CareerItem, Career>(
    sql,
    (career, careerItem) =>
    {
      var getCareer = careers.Where(x => x.Id == career.Id).FirstOrDefault();
      if (getCareer == null)
      {
        getCareer = career;
        getCareer.CareerItems.Add(careerItem);
        careers.Add(getCareer);
      }
      else
      {
        getCareer.CareerItems.Add(careerItem);
      }
      career.CareerItems.Add(careerItem);
      return career;
    }, splitOn: "CareerId");

  foreach (var career in careers) {
    Console.WriteLine($"{career.Title}");
    foreach (var carrerItem in career.CareerItems) {
      Console.WriteLine($" - {carrerItem.Title}");
    }
  }
}

static void QueryMultiple(SqlConnection connection)
{
  var query = "SELECT * FROM [Category]; SELECT * FROM [Course]";

  using(var multi = connection.QueryMultiple(query))
  {
    var categories = multi.Read<Category>();
    var courses = multi.Read<Course>();

    Console.WriteLine("Categorias:");
    foreach(var item in categories)
    {
      Console.WriteLine(item.Title);
    }

    Console.WriteLine("Cursos:");
    foreach(var item in courses)
    {
      Console.WriteLine(item.Title);
    }
  }
}

static void SelectIn(SqlConnection connection)
{
  var query = @"SELECT * FROM [Career] WHERE [Id] IN @Id";
  
  var items = connection.Query<Career>(query, new
  {
    Id = new[]{
      "e6730d1c-6870-4df3-ae68-438624e04c72",
      "4327ac7e-963b-4893-9f31-9a3b28a4e72b"
    }
  });

  foreach (var item in items)
  {
    Console.WriteLine(item.Title);
  }
}

static void Like(SqlConnection connection, string term)
{
  var query = @"SELECT * FROM [Course] WHERE [Title] LIKE @exp";
  
  var items = connection.Query<Course>(query, new
  {
    exp = $"%{term}%"
  });

  foreach (var item in items)
  {
    Console.WriteLine(item.Title);
  }
}

static void Transaction(SqlConnection connection)
{
  var category = new Category {
  Id = Guid.NewGuid(),
  Title = "Minha categoria que não",
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

  connection.Open();
  using (var transaction = connection.BeginTransaction())
  {
    var rows = connection.Execute(insertSql, new {
          category.Id,
          category.Title,
          category.Url,
          category.Summary,
          category.Order,
          category.Description,
          category.Featured
        }, transaction);

    transaction.Commit();
    //transaction.Rollback();

    Console.WriteLine($"{rows} linhas inseridas");
  }

    
}