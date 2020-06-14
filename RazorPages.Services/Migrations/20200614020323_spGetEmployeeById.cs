using Microsoft.EntityFrameworkCore.Migrations;

namespace RazorPages.Services.Migrations
{
    public partial class spGetEmployeeById : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"Create procedure spGetEmployeeById
                                @Id int
                                as
                                Begin
	                                Select * from Employees
	                                where Id = @Id
                                End";
            migrationBuilder.Sql(procedure);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            string procedure = @"Drop procedure spGetEmployeeById";
            migrationBuilder.Sql(procedure);
        }
    }
}
