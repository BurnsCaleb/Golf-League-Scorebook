using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourseName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CourseLocation = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CourseRating = table.Column<double>(type: "REAL", precision: 4, scale: 1, nullable: false),
                    CourseSlope = table.Column<double>(type: "REAL", nullable: false),
                    NumHoles = table.Column<int>(type: "INTEGER", nullable: false),
                    CoursePar = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.CourseId);
                    table.CheckConstraint("CK_CourseSlope", "[CourseSlope] BETWEEN 55 AND 155");
                });

            migrationBuilder.CreateTable(
                name: "Golfer",
                columns: table => new
                {
                    GolferId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
                    Handicap = table.Column<double>(type: "REAL", precision: 4, scale: 2, nullable: false),
                    DateLastPlayed = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    FullName = table.Column<string>(type: "TEXT", nullable: false, computedColumnSql: "FirstName || ' ' || LastName")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Golfer", x => x.GolferId);
                    table.CheckConstraint("CK_Handicap", "[Handicap] <= '54'");
                });

            migrationBuilder.CreateTable(
                name: "ScoringRule",
                columns: table => new
                {
                    ScoringRuleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RuleName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoringRule", x => x.ScoringRuleId);
                });

            migrationBuilder.CreateTable(
                name: "Hole",
                columns: table => new
                {
                    HoleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HoleNum = table.Column<int>(type: "INTEGER", nullable: false),
                    Par = table.Column<int>(type: "INTEGER", nullable: false),
                    Distance = table.Column<int>(type: "INTEGER", nullable: false),
                    Handicap = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hole", x => x.HoleId);
                    table.CheckConstraint("CK_Distance", "[Distance] > 0");
                    table.CheckConstraint("CK_Handicap", "[Handicap] > 0");
                    table.CheckConstraint("CK_HoleNum", "[HoleNum] > 0");
                    table.CheckConstraint("CK_Par", "[Par] > 0");
                    table.ForeignKey(
                        name: "FK_Hole_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeagueSettings",
                columns: table => new
                {
                    LeagueSettingsId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LeagueSettingsName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    PlayDate = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    ScoringRuleId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamSize = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueSettings", x => x.LeagueSettingsId);
                    table.ForeignKey(
                        name: "FK_LeagueSettings_ScoringRule_ScoringRuleId",
                        column: x => x.ScoringRuleId,
                        principalTable: "ScoringRule",
                        principalColumn: "ScoringRuleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "League",
                columns: table => new
                {
                    LeagueId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LeagueName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LeagueSettingsId = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateAccessed = table.Column<DateOnly>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_League", x => x.LeagueId);
                    table.ForeignKey(
                        name: "FK_League_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_League_LeagueSettings_LeagueSettingsId",
                        column: x => x.LeagueSettingsId,
                        principalTable: "LeagueSettings",
                        principalColumn: "LeagueSettingsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GolferLeagueJunction",
                columns: table => new
                {
                    GolferId = table.Column<int>(type: "INTEGER", nullable: false),
                    LeagueId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GolferLeagueJunction", x => new { x.GolferId, x.LeagueId });
                    table.ForeignKey(
                        name: "FK_GolferLeagueJunction_Golfer_GolferId",
                        column: x => x.GolferId,
                        principalTable: "Golfer",
                        principalColumn: "GolferId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GolferLeagueJunction_League_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "League",
                        principalColumn: "LeagueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Season",
                columns: table => new
                {
                    SeasonId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SeasonName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LeagueId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Season", x => x.SeasonId);
                    table.ForeignKey(
                        name: "FK_Season_League_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "League",
                        principalColumn: "LeagueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TeamName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastPlayed = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    LeagueId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.TeamId);
                    table.ForeignKey(
                        name: "FK_Team_League_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "League",
                        principalColumn: "LeagueId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matchup",
                columns: table => new
                {
                    MatchupId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MatchupName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LeagueId = table.Column<int>(type: "INTEGER", nullable: false),
                    MatchupDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    SeasonId = table.Column<int>(type: "INTEGER", nullable: false),
                    HasPlayed = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    Week = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matchup", x => x.MatchupId);
                    table.ForeignKey(
                        name: "FK_Matchup_League_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "League",
                        principalColumn: "LeagueId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matchup_Season_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Season",
                        principalColumn: "SeasonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GolferTeamJunction",
                columns: table => new
                {
                    GolferId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GolferTeamJunction", x => new { x.GolferId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_GolferTeamJunction_Golfer_GolferId",
                        column: x => x.GolferId,
                        principalTable: "Golfer",
                        principalColumn: "GolferId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GolferTeamJunction_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GolferMatchupJunction",
                columns: table => new
                {
                    GolferId = table.Column<int>(type: "INTEGER", nullable: false),
                    MatchupId = table.Column<int>(type: "INTEGER", nullable: false),
                    PointsAwarded = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GolferMatchupJunction", x => new { x.GolferId, x.MatchupId });
                    table.ForeignKey(
                        name: "FK_GolferMatchupJunction_Golfer_GolferId",
                        column: x => x.GolferId,
                        principalTable: "Golfer",
                        principalColumn: "GolferId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GolferMatchupJunction_Matchup_MatchupId",
                        column: x => x.MatchupId,
                        principalTable: "Matchup",
                        principalColumn: "MatchupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Round",
                columns: table => new
                {
                    RoundId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
                    NetTotal = table.Column<int>(type: "INTEGER", nullable: false),
                    GrossTotal = table.Column<int>(type: "INTEGER", nullable: false),
                    DatePlayed = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    GolferId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false),
                    ActiveSubstitute = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    LeagueId = table.Column<int>(type: "INTEGER", nullable: false),
                    MatchupId = table.Column<int>(type: "INTEGER", nullable: false),
                    ScoreDifferential = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Round", x => x.RoundId);
                    table.ForeignKey(
                        name: "FK_Round_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Round_Golfer_GolferId",
                        column: x => x.GolferId,
                        principalTable: "Golfer",
                        principalColumn: "GolferId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Round_League_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "League",
                        principalColumn: "LeagueId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Round_Matchup_MatchupId",
                        column: x => x.MatchupId,
                        principalTable: "Matchup",
                        principalColumn: "MatchupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Round_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Substitute",
                columns: table => new
                {
                    GolferId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false),
                    MatchupId = table.Column<int>(type: "INTEGER", nullable: false),
                    OriginalGolferId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Substitute", x => new { x.GolferId, x.TeamId, x.MatchupId });
                    table.ForeignKey(
                        name: "FK_Substitute_Golfer_GolferId",
                        column: x => x.GolferId,
                        principalTable: "Golfer",
                        principalColumn: "GolferId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Substitute_Golfer_OriginalGolferId",
                        column: x => x.OriginalGolferId,
                        principalTable: "Golfer",
                        principalColumn: "GolferId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Substitute_Matchup_MatchupId",
                        column: x => x.MatchupId,
                        principalTable: "Matchup",
                        principalColumn: "MatchupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Substitute_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamMatchupJunction",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "INTEGER", nullable: false),
                    MatchupId = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamMatchupId = table.Column<int>(type: "INTEGER", nullable: false),
                    PointsAwarded = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMatchupJunction", x => new { x.TeamId, x.MatchupId });
                    table.CheckConstraint("CK_PointsAwarded", "[PointsAwarded] >= 0");
                    table.ForeignKey(
                        name: "FK_TeamMatchupJunction_Matchup_MatchupId",
                        column: x => x.MatchupId,
                        principalTable: "Matchup",
                        principalColumn: "MatchupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamMatchupJunction_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoleScore",
                columns: table => new
                {
                    GolferId = table.Column<int>(type: "INTEGER", nullable: false),
                    RoundId = table.Column<int>(type: "INTEGER", nullable: false),
                    HoleId = table.Column<int>(type: "INTEGER", nullable: false),
                    GrossScore = table.Column<int>(type: "INTEGER", nullable: false),
                    NetScore = table.Column<int>(type: "INTEGER", nullable: false),
                    DatePlayed = table.Column<DateOnly>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoleScore", x => new { x.HoleId, x.GolferId, x.RoundId });
                    table.CheckConstraint("CK_GrossScore", "[GrossScore] > 0");
                    table.CheckConstraint("CK_NetScore", "[NetScore] > 0");
                    table.ForeignKey(
                        name: "FK_HoleScore_Golfer_GolferId",
                        column: x => x.GolferId,
                        principalTable: "Golfer",
                        principalColumn: "GolferId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoleScore_Hole_HoleId",
                        column: x => x.HoleId,
                        principalTable: "Hole",
                        principalColumn: "HoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HoleScore_Round_RoundId",
                        column: x => x.RoundId,
                        principalTable: "Round",
                        principalColumn: "RoundId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseId",
                table: "Course",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_GolferId",
                table: "Golfer",
                column: "GolferId");

            migrationBuilder.CreateIndex(
                name: "IX_GolferName",
                table: "Golfer",
                columns: new[] { "LastName", "FirstName" });

            migrationBuilder.CreateIndex(
                name: "IX_GolferLeague",
                table: "GolferLeagueJunction",
                columns: new[] { "GolferId", "LeagueId" });

            migrationBuilder.CreateIndex(
                name: "IX_GolferLeagueJunction_LeagueId",
                table: "GolferLeagueJunction",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_GolferMatchup",
                table: "GolferMatchupJunction",
                columns: new[] { "GolferId", "MatchupId" });

            migrationBuilder.CreateIndex(
                name: "IX_GolferMatchupJunction_MatchupId",
                table: "GolferMatchupJunction",
                column: "MatchupId");

            migrationBuilder.CreateIndex(
                name: "IX_GolferTeam",
                table: "GolferTeamJunction",
                columns: new[] { "GolferId", "TeamId" });

            migrationBuilder.CreateIndex(
                name: "IX_GolferTeamJunction_TeamId",
                table: "GolferTeamJunction",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseHole",
                table: "Hole",
                columns: new[] { "CourseId", "HoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_HoleId",
                table: "Hole",
                column: "HoleId");

            migrationBuilder.CreateIndex(
                name: "IX_HoleGolferRound",
                table: "HoleScore",
                columns: new[] { "HoleId", "GolferId", "RoundId" });

            migrationBuilder.CreateIndex(
                name: "IX_HoleScore_GolferId",
                table: "HoleScore",
                column: "GolferId");

            migrationBuilder.CreateIndex(
                name: "IX_HoleScore_RoundId",
                table: "HoleScore",
                column: "RoundId");

            migrationBuilder.CreateIndex(
                name: "IX_League_CourseId",
                table: "League",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_League_LeagueSettingsId",
                table: "League",
                column: "LeagueSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueId",
                table: "League",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueSettings_ScoringRuleId",
                table: "LeagueSettings",
                column: "ScoringRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueSettingsId",
                table: "LeagueSettings",
                column: "LeagueSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Matchup_LeagueId",
                table: "Matchup",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Matchup_SeasonId",
                table: "Matchup",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchupId",
                table: "Matchup",
                column: "MatchupId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchupIdLeagueId",
                table: "Matchup",
                columns: new[] { "MatchupId", "LeagueId" });

            migrationBuilder.CreateIndex(
                name: "IX_MatchupIdSeasonId",
                table: "Matchup",
                columns: new[] { "MatchupId", "SeasonId" });

            migrationBuilder.CreateIndex(
                name: "IX_MatchupIdGolferId",
                table: "Round",
                columns: new[] { "MatchupId", "GolferId" });

            migrationBuilder.CreateIndex(
                name: "IX_Round_CourseId",
                table: "Round",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Round_GolferId",
                table: "Round",
                column: "GolferId");

            migrationBuilder.CreateIndex(
                name: "IX_Round_LeagueId",
                table: "Round",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Round_TeamId",
                table: "Round",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_RoundId",
                table: "Round",
                column: "RoundId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoringRuleId",
                table: "ScoringRule",
                column: "ScoringRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Season_LeagueId",
                table: "Season",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonId",
                table: "Season",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_GolferTeamMatchup",
                table: "Substitute",
                columns: new[] { "GolferId", "TeamId", "MatchupId" });

            migrationBuilder.CreateIndex(
                name: "IX_Matchup",
                table: "Substitute",
                column: "MatchupId");

            migrationBuilder.CreateIndex(
                name: "IX_OriginalGolfer",
                table: "Substitute",
                column: "OriginalGolferId");

            migrationBuilder.CreateIndex(
                name: "IX_Substitute_TeamId",
                table: "Substitute",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_LeagueId",
                table: "Team",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamId",
                table: "Team",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamIdMatchupId",
                table: "TeamMatchupJunction",
                columns: new[] { "TeamId", "MatchupId" });

            migrationBuilder.CreateIndex(
                name: "IX_TeamMatchupJunction_MatchupId",
                table: "TeamMatchupJunction",
                column: "MatchupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GolferLeagueJunction");

            migrationBuilder.DropTable(
                name: "GolferMatchupJunction");

            migrationBuilder.DropTable(
                name: "GolferTeamJunction");

            migrationBuilder.DropTable(
                name: "HoleScore");

            migrationBuilder.DropTable(
                name: "Substitute");

            migrationBuilder.DropTable(
                name: "TeamMatchupJunction");

            migrationBuilder.DropTable(
                name: "Hole");

            migrationBuilder.DropTable(
                name: "Round");

            migrationBuilder.DropTable(
                name: "Golfer");

            migrationBuilder.DropTable(
                name: "Matchup");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "Season");

            migrationBuilder.DropTable(
                name: "League");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "LeagueSettings");

            migrationBuilder.DropTable(
                name: "ScoringRule");
        }
    }
}
