using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RPEnglish.MVC.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "annotations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_annotations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "words",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    translation = table.Column<string>(type: "text", nullable: true),
                    observation = table.Column<string>(type: "text", nullable: true),
                    categoryid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_words", x => x.id);
                    table.ForeignKey(
                        name: "FK_words_categories_categoryid",
                        column: x => x.categoryid,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users_passwords",
                columns: table => new
                {
                    id = table.Column<Guid>(nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    userid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users_passwords", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_passwords_users_userid",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_passwords_userid",
                table: "users_passwords",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_words_categoryid",
                table: "words",
                column: "categoryid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "annotations");

            migrationBuilder.DropTable(
                name: "users_passwords");

            migrationBuilder.DropTable(
                name: "words");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
