using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class Updatelabel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabelsTable_NotesTable_NoteId",
                table: "LabelsTable");

            migrationBuilder.AlterColumn<long>(
                name: "NoteId",
                table: "LabelsTable",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_LabelsTable_NotesTable_NoteId",
                table: "LabelsTable",
                column: "NoteId",
                principalTable: "NotesTable",
                principalColumn: "NoteId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LabelsTable_NotesTable_NoteId",
                table: "LabelsTable");

            migrationBuilder.AlterColumn<long>(
                name: "NoteId",
                table: "LabelsTable",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LabelsTable_NotesTable_NoteId",
                table: "LabelsTable",
                column: "NoteId",
                principalTable: "NotesTable",
                principalColumn: "NoteId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
