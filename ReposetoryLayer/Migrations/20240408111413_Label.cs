using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReposetoryLayer.Migrations
{
    public partial class Label : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserNotes_user_UserId",
                table: "UserNotes");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserNotes",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotes_UserId",
                table: "UserNotes",
                newName: "IX_UserNotes_UserID");

            migrationBuilder.CreateTable(
                name: "Label",
                columns: table => new
                {
                    LabelId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LabelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    NoteId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Label", x => x.LabelId);
                    table.ForeignKey(
                        name: "FK_Label_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Label_UserNotes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "UserNotes",
                        principalColumn: "NoteId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Label_NoteId",
                table: "Label",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Label_UserId",
                table: "Label",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotes_user_UserID",
                table: "UserNotes",
                column: "UserID",
                principalTable: "user",
                principalColumn: "userID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserNotes_user_UserID",
                table: "UserNotes");

            migrationBuilder.DropTable(
                name: "Label");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "UserNotes",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotes_UserID",
                table: "UserNotes",
                newName: "IX_UserNotes_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotes_user_UserId",
                table: "UserNotes",
                column: "UserId",
                principalTable: "user",
                principalColumn: "userID",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
