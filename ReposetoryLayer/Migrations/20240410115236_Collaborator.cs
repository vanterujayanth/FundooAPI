using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReposetoryLayer.Migrations
{
    public partial class Collaborator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Label_user_UserId",
                table: "Label");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Label",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "LabelId",
                table: "Label",
                newName: "LabelID");

            migrationBuilder.RenameIndex(
                name: "IX_Label_UserId",
                table: "Label",
                newName: "IX_Label_UserID");

            migrationBuilder.CreateTable(
                name: "Collaborator",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    C__Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    NoteId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collaborator", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Collaborator_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Collaborator_UserNotes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "UserNotes",
                        principalColumn: "NoteId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Collaborator_NoteId",
                table: "Collaborator",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Collaborator_UserId",
                table: "Collaborator",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Label_user_UserID",
                table: "Label",
                column: "UserID",
                principalTable: "user",
                principalColumn: "userID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Label_user_UserID",
                table: "Label");

            migrationBuilder.DropTable(
                name: "Collaborator");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Label",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "LabelID",
                table: "Label",
                newName: "LabelId");

            migrationBuilder.RenameIndex(
                name: "IX_Label_UserID",
                table: "Label",
                newName: "IX_Label_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Label_user_UserId",
                table: "Label",
                column: "UserId",
                principalTable: "user",
                principalColumn: "userID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
