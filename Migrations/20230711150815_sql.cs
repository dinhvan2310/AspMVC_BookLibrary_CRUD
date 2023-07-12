using System;
using Bogus;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC_BookLibrary.Migrations
{
    /// <inheritdoc />
    public partial class sql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Publisher = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishYear = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataFile = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookModels", x => x.Id);
                });

                Randomizer.Seed = new Random(231023);
                Faker<BookModel> faker = new Faker<BookModel>();
                faker.RuleFor(b => b.Title, f => f.Name.JobTitle())
                    .RuleFor(b => b.Author, f => f.Name.FullName())
                    .RuleFor(b => b.Publisher, f => f.Company.CompanyName())
                    .RuleFor(b => b.PublishYear, f => f.Date.Past(20));
                

                for(int i = 1; i <= 100; i++)
                {
                    var fakeBook = faker.Generate();
                    migrationBuilder.InsertData("BookModels", 
                                    new string[] {"Title", "Author", "Publisher", "PublishYear"}, 
                                    new Object[]{
                                        fakeBook.Title, fakeBook.Author, fakeBook.Publisher, fakeBook.PublishYear
                                    });
                }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookModels");
        }
    }
}
