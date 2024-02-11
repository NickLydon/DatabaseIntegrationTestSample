using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Cascade_Delete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPostAttachments_Attachments_AttachmentId",
                table: "BlogPostAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogPostAttachments_BlogPosts_BlogPostId",
                table: "BlogPostAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageAttachments_Attachments_AttachmentId",
                table: "MessageAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageAttachments_Messages_MessageId",
                table: "MessageAttachments");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostAttachments_Attachments_AttachmentId",
                table: "BlogPostAttachments",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostAttachments_BlogPosts_BlogPostId",
                table: "BlogPostAttachments",
                column: "BlogPostId",
                principalTable: "BlogPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageAttachments_Attachments_AttachmentId",
                table: "MessageAttachments",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageAttachments_Messages_MessageId",
                table: "MessageAttachments",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPostAttachments_Attachments_AttachmentId",
                table: "BlogPostAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogPostAttachments_BlogPosts_BlogPostId",
                table: "BlogPostAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageAttachments_Attachments_AttachmentId",
                table: "MessageAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageAttachments_Messages_MessageId",
                table: "MessageAttachments");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostAttachments_Attachments_AttachmentId",
                table: "BlogPostAttachments",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostAttachments_BlogPosts_BlogPostId",
                table: "BlogPostAttachments",
                column: "BlogPostId",
                principalTable: "BlogPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageAttachments_Attachments_AttachmentId",
                table: "MessageAttachments",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageAttachments_Messages_MessageId",
                table: "MessageAttachments",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
