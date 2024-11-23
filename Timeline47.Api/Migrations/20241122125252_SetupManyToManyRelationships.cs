using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Timeline47.Api.Migrations
{
    /// <inheritdoc />
    public partial class SetupManyToManyRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArticleSubjects");

            migrationBuilder.AddColumn<Guid>(
                name: "TimelineEventId",
                table: "Articles",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ArticleSubject",
                columns: table => new
                {
                    ArticlesId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleSubject", x => new { x.ArticlesId, x.SubjectsId });
                    table.ForeignKey(
                        name: "FK_ArticleSubject_Articles_ArticlesId",
                        column: x => x.ArticlesId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleSubject_Subjects_SubjectsId",
                        column: x => x.SubjectsId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimelineEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(1023)", maxLength: 1023, nullable: true),
                    EventDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimelineEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubjectTimelineEvent",
                columns: table => new
                {
                    EventsId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectTimelineEvent", x => new { x.EventsId, x.SubjectsId });
                    table.ForeignKey(
                        name: "FK_SubjectTimelineEvent_Subjects_SubjectsId",
                        column: x => x.SubjectsId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectTimelineEvent_TimelineEvents_EventsId",
                        column: x => x.EventsId,
                        principalTable: "TimelineEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_TimelineEventId",
                table: "Articles",
                column: "TimelineEventId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleSubject_SubjectsId",
                table: "ArticleSubject",
                column: "SubjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTimelineEvent_SubjectsId",
                table: "SubjectTimelineEvent",
                column: "SubjectsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_TimelineEvents_TimelineEventId",
                table: "Articles",
                column: "TimelineEventId",
                principalTable: "TimelineEvents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_TimelineEvents_TimelineEventId",
                table: "Articles");

            migrationBuilder.DropTable(
                name: "ArticleSubject");

            migrationBuilder.DropTable(
                name: "SubjectTimelineEvent");

            migrationBuilder.DropTable(
                name: "TimelineEvents");

            migrationBuilder.DropIndex(
                name: "IX_Articles_TimelineEventId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "TimelineEventId",
                table: "Articles");

            migrationBuilder.CreateTable(
                name: "ArticleSubjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ArticleId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubjectId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleSubjects_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleSubjects_ArticleId",
                table: "ArticleSubjects",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleSubjects_SubjectId",
                table: "ArticleSubjects",
                column: "SubjectId");
        }
    }
}
