using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntryManagement.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parent",
                columns: table => new
                {
                    ParentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Parent__D339510FCBF0D5F5", x => x.ParentID);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserRole__8AFACE3AC702802C", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    DayOfBirth = table.Column<DateTime>(type: "datetime", nullable: false),
                    Class = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<int>(type: "int", nullable: true),
                    JoinDay = table.Column<DateTime>(type : "datetime" , nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Student__32C52A79FE9ED37B", x => x.StudentID);
                    table.ForeignKey(
                        name: "FK__Student__ParentI__398D8EEE",
                        column: x => x.ParentID,
                        principalTable: "Parent",
                        principalColumn: "ParentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RoleID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__1788CCAC7805CFA5", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Users_UserRole_RoleID",
                        column: x => x.RoleID,
                        principalTable: "UserRole",
                        principalColumn: "RoleID");
                });

            migrationBuilder.CreateTable(
                name: "AbsentReport",
                columns: table => new
                {
                    AbsentReportID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    ParentID = table.Column<int>(type: "int", nullable: false),
                    CreateDay = table.Column<DateTime>(type: "datetime", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AbsentRe__258DB924E376B7AC", x => x.AbsentReportID);
                    table.ForeignKey(
                        name: "FK_AbsentReport_Parent_ParentID",
                        column: x => x.ParentID,
                        principalTable: "Parent",
                        principalColumn: "ParentID");
                    table.ForeignKey(
                        name: "FK_AbsentReport_Student_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alert",
                columns: table => new
                {
                    AlertID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    AlertTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Alert__EBB16AED5FF88098", x => x.AlertID);
                    table.ForeignKey(
                        name: "FK_Alert_Student_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntryLog",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    LogTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, defaultValue: "Out")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__EntryLog__5E5499A85C9F3273", x => x.LogID);
                    table.ForeignKey(
                        name: "FK_EntryLog_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "StudentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbsentReport_ParentID",
                table: "AbsentReport",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_AbsentReport_StudentID",
                table: "AbsentReport",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Alert_StudentID",
                table: "Alert",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_EntryLog_StudentID",
                table: "EntryLog",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Student_ParentID",
                table: "Student",
                column: "ParentID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbsentReport");

            migrationBuilder.DropTable(
                name: "Alert");

            migrationBuilder.DropTable(
                name: "EntryLog");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Parent");
        }
    }
}
