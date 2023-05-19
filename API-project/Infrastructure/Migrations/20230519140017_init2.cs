using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentEntity_AspNetUsers_UserId",
                table: "CommentEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentEntity_CommentEntity_commentId",
                table: "CommentEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentEntity_PublishEntity_publishId",
                table: "CommentEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Publish_PublishTag_PublishEntity_PublishesId",
                table: "Publish_PublishTag");

            migrationBuilder.DropForeignKey(
                name: "FK_Publish_PublishTag_PublishTagEntity_PublishTagsId",
                table: "Publish_PublishTag");

            migrationBuilder.DropForeignKey(
                name: "FK_PublishEntity_AspNetUsers_UserId",
                table: "PublishEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLikes_PublishEntity_PublishLikesId",
                table: "UserLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PublishTagEntity",
                table: "PublishTagEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PublishEntity",
                table: "PublishEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentEntity",
                table: "CommentEntity");

            migrationBuilder.RenameTable(
                name: "PublishTagEntity",
                newName: "Tags");

            migrationBuilder.RenameTable(
                name: "PublishEntity",
                newName: "Publishes");

            migrationBuilder.RenameTable(
                name: "CommentEntity",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_PublishEntity_UserId",
                table: "Publishes",
                newName: "IX_Publishes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentEntity_UserId",
                table: "Comments",
                newName: "IX_Comments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentEntity_publishId",
                table: "Comments",
                newName: "IX_Comments_publishId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentEntity_commentId",
                table: "Comments",
                newName: "IX_Comments_commentId");

            migrationBuilder.AddColumn<Guid>(
                name: "PublishAlbumEntityId",
                table: "Publishes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Comments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                table: "Tags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Publishes",
                table: "Publishes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Albums_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Publishes_PublishAlbumEntityId",
                table: "Publishes",
                column: "PublishAlbumEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_UserId",
                table: "Albums",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_commentId",
                table: "Comments",
                column: "commentId",
                principalTable: "Comments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Publishes_publishId",
                table: "Comments",
                column: "publishId",
                principalTable: "Publishes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Publish_PublishTag_Publishes_PublishesId",
                table: "Publish_PublishTag",
                column: "PublishesId",
                principalTable: "Publishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Publish_PublishTag_Tags_PublishTagsId",
                table: "Publish_PublishTag",
                column: "PublishTagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Publishes_Albums_PublishAlbumEntityId",
                table: "Publishes",
                column: "PublishAlbumEntityId",
                principalTable: "Albums",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Publishes_AspNetUsers_UserId",
                table: "Publishes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikes_Publishes_PublishLikesId",
                table: "UserLikes",
                column: "PublishLikesId",
                principalTable: "Publishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_commentId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Publishes_publishId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Publish_PublishTag_Publishes_PublishesId",
                table: "Publish_PublishTag");

            migrationBuilder.DropForeignKey(
                name: "FK_Publish_PublishTag_Tags_PublishTagsId",
                table: "Publish_PublishTag");

            migrationBuilder.DropForeignKey(
                name: "FK_Publishes_Albums_PublishAlbumEntityId",
                table: "Publishes");

            migrationBuilder.DropForeignKey(
                name: "FK_Publishes_AspNetUsers_UserId",
                table: "Publishes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLikes_Publishes_PublishLikesId",
                table: "UserLikes");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                table: "Tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Publishes",
                table: "Publishes");

            migrationBuilder.DropIndex(
                name: "IX_Publishes_PublishAlbumEntityId",
                table: "Publishes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "PublishAlbumEntityId",
                table: "Publishes");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "PublishTagEntity");

            migrationBuilder.RenameTable(
                name: "Publishes",
                newName: "PublishEntity");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "CommentEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Publishes_UserId",
                table: "PublishEntity",
                newName: "IX_PublishEntity_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_UserId",
                table: "CommentEntity",
                newName: "IX_CommentEntity_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_publishId",
                table: "CommentEntity",
                newName: "IX_CommentEntity_publishId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_commentId",
                table: "CommentEntity",
                newName: "IX_CommentEntity_commentId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CommentEntity",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PublishTagEntity",
                table: "PublishTagEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PublishEntity",
                table: "PublishEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentEntity",
                table: "CommentEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentEntity_AspNetUsers_UserId",
                table: "CommentEntity",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentEntity_CommentEntity_commentId",
                table: "CommentEntity",
                column: "commentId",
                principalTable: "CommentEntity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentEntity_PublishEntity_publishId",
                table: "CommentEntity",
                column: "publishId",
                principalTable: "PublishEntity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Publish_PublishTag_PublishEntity_PublishesId",
                table: "Publish_PublishTag",
                column: "PublishesId",
                principalTable: "PublishEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Publish_PublishTag_PublishTagEntity_PublishTagsId",
                table: "Publish_PublishTag",
                column: "PublishTagsId",
                principalTable: "PublishTagEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PublishEntity_AspNetUsers_UserId",
                table: "PublishEntity",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLikes_PublishEntity_PublishLikesId",
                table: "UserLikes",
                column: "PublishLikesId",
                principalTable: "PublishEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
