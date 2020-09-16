using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.CMMN.SqlServer.Host.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CaseFiles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    FileId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreateDateTime = table.Column<DateTime>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: false),
                    SerializedContent = table.Column<string>(nullable: true),
                    Owner = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CasePlanInstances",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    CasePlanId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    CaseState = table.Column<int>(nullable: true),
                    ExecutionContext = table.Column<string>(nullable: true),
                    CreateDateTime = table.Column<DateTime>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasePlanInstances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CasePlans",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    CasePlanId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CaseOwner = table.Column<string>(nullable: true),
                    CaseFileId = table.Column<string>(nullable: true),
                    CreateDateTime = table.Column<DateTime>(nullable: false),
                    SerializedContent = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasePlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CaseWorkers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Version = table.Column<int>(nullable: false),
                    CasePlanInstanceId = table.Column<string>(nullable: true),
                    CasePlanInstanceElementId = table.Column<string>(nullable: true),
                    CreateDateTime = table.Column<DateTime>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseWorkers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QueueMessageLst",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QueueName = table.Column<string>(nullable: true),
                    SerializedContent = table.Column<string>(nullable: true),
                    CreateDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueueMessageLst", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledMessageLst",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QueueName = table.Column<string>(nullable: true),
                    SerializedContent = table.Column<string>(nullable: true),
                    ElapsedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledMessageLst", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionLst",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(nullable: true),
                    CasePlanInstanceId = table.Column<string>(nullable: true),
                    CasePlanElementInstanceId = table.Column<string>(nullable: true),
                    IsCaptured = table.Column<bool>(nullable: false),
                    CaptureDateTime = table.Column<DateTime>(nullable: true),
                    CreationDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionLst", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CasePlanElementInstanceLst",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EltId = table.Column<string>(nullable: true),
                    ParentId = table.Column<long>(nullable: true),
                    CasePlanInstanceId = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    SerializedContent = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasePlanElementInstanceLst", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CasePlanElementInstanceLst_CasePlanInstances_CasePlanInstanceId",
                        column: x => x.CasePlanInstanceId,
                        principalTable: "CasePlanInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CasePlanElementInstanceLst_CasePlanElementInstanceLst_ParentId",
                        column: x => x.ParentId,
                        principalTable: "CasePlanElementInstanceLst",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CasePlanInstanceFileItemLst",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CasePlanInstanceId = table.Column<string>(nullable: true),
                    CasePlanElementInstanceId = table.Column<string>(nullable: true),
                    CaseFileItemType = table.Column<string>(nullable: true),
                    ExternalValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasePlanInstanceFileItemLst", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CasePlanInstanceFileItemLst_CasePlanInstances_CasePlanInstanceId",
                        column: x => x.CasePlanInstanceId,
                        principalTable: "CasePlanInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CasePlanInstanceWorkerTaskLst",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CasePlanInstanceId = table.Column<string>(nullable: true),
                    CreateDateTime = table.Column<DateTime>(nullable: false),
                    CasePlanElementInstanceId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasePlanInstanceWorkerTaskLst", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CasePlanInstanceWorkerTaskLst_CasePlanInstances_CasePlanInstanceId",
                        column: x => x.CasePlanInstanceId,
                        principalTable: "CasePlanInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CasePlanFileItemModel",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CasePlanId = table.Column<string>(nullable: true),
                    FileItemId = table.Column<string>(nullable: true),
                    FileItemName = table.Column<string>(nullable: true),
                    DefinitionType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CasePlanFileItemModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CasePlanFileItemModel_CasePlans_CasePlanId",
                        column: x => x.CasePlanId,
                        principalTable: "CasePlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: true),
                    RoleName = table.Column<string>(nullable: true),
                    IsCaseOwner = table.Column<bool>(nullable: false),
                    CasePlanInstanceId = table.Column<string>(nullable: true),
                    CasePlanId = table.Column<string>(nullable: true),
                    CaseWorkerTaskId = table.Column<string>(nullable: true),
                    CreateDateTime = table.Column<DateTime>(nullable: false),
                    UpdateDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_CasePlans_CasePlanId",
                        column: x => x.CasePlanId,
                        principalTable: "CasePlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Roles_CasePlanInstances_CasePlanInstanceId",
                        column: x => x.CasePlanInstanceId,
                        principalTable: "CasePlanInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Roles_CaseWorkers_CaseWorkerTaskId",
                        column: x => x.CaseWorkerTaskId,
                        principalTable: "CaseWorkers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Claims",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(nullable: true),
                    ClaimName = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Claims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CasePlanElementInstanceLst_CasePlanInstanceId",
                table: "CasePlanElementInstanceLst",
                column: "CasePlanInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_CasePlanElementInstanceLst_ParentId",
                table: "CasePlanElementInstanceLst",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_CasePlanFileItemModel_CasePlanId",
                table: "CasePlanFileItemModel",
                column: "CasePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_CasePlanInstanceFileItemLst_CasePlanInstanceId",
                table: "CasePlanInstanceFileItemLst",
                column: "CasePlanInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_CasePlanInstanceWorkerTaskLst_CasePlanInstanceId",
                table: "CasePlanInstanceWorkerTaskLst",
                column: "CasePlanInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_RoleId",
                table: "Claims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CasePlanId",
                table: "Roles",
                column: "CasePlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CasePlanInstanceId",
                table: "Roles",
                column: "CasePlanInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CaseWorkerTaskId",
                table: "Roles",
                column: "CaseWorkerTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseFiles");

            migrationBuilder.DropTable(
                name: "CasePlanElementInstanceLst");

            migrationBuilder.DropTable(
                name: "CasePlanFileItemModel");

            migrationBuilder.DropTable(
                name: "CasePlanInstanceFileItemLst");

            migrationBuilder.DropTable(
                name: "CasePlanInstanceWorkerTaskLst");

            migrationBuilder.DropTable(
                name: "Claims");

            migrationBuilder.DropTable(
                name: "QueueMessageLst");

            migrationBuilder.DropTable(
                name: "ScheduledMessageLst");

            migrationBuilder.DropTable(
                name: "SubscriptionLst");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "CasePlans");

            migrationBuilder.DropTable(
                name: "CasePlanInstances");

            migrationBuilder.DropTable(
                name: "CaseWorkers");
        }
    }
}
