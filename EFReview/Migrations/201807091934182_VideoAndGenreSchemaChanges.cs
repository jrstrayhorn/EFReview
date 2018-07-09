namespace EFReview.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VideoAndGenreSchemaChanges : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Videos", new[] { "Genres_Id" });
            RenameColumn(table: "dbo.Videos", name: "Genres_Id", newName: "GenreId");
            AlterColumn("dbo.Genres", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Videos", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Videos", "Classification", c => c.Byte(nullable: false));
            AlterColumn("dbo.Videos", "GenreId", c => c.Byte(nullable: false));
            CreateIndex("dbo.Videos", "GenreId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Videos", new[] { "GenreId" });
            AlterColumn("dbo.Videos", "GenreId", c => c.Byte());
            AlterColumn("dbo.Videos", "Classification", c => c.Int(nullable: false));
            AlterColumn("dbo.Videos", "Name", c => c.String());
            AlterColumn("dbo.Genres", "Name", c => c.String());
            RenameColumn(table: "dbo.Videos", name: "GenreId", newName: "Genres_Id");
            CreateIndex("dbo.Videos", "Genres_Id");
        }
    }
}
