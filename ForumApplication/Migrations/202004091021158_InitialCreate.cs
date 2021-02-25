namespace ForumApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostID = c.Int(nullable: false, identity: true),
                        Content = c.Int(nullable: false),
                        ImagePath = c.String(),
                        PublicationDate = c.DateTime(nullable: false),
                        Post_PostID = c.Int(),
                        User_UserID = c.Int(),
                    })
                .PrimaryKey(t => t.PostID)
                .ForeignKey("dbo.Posts", t => t.Post_PostID)
                .ForeignKey("dbo.Users", t => t.User_UserID)
                .Index(t => t.Post_PostID)
                .Index(t => t.User_UserID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        Avatar = c.Binary(),
                        BackgroundImage = c.Binary(),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "User_UserID", "dbo.Users");
            DropForeignKey("dbo.Posts", "Post_PostID", "dbo.Posts");
            DropIndex("dbo.Posts", new[] { "User_UserID" });
            DropIndex("dbo.Posts", new[] { "Post_PostID" });
            DropTable("dbo.Users");
            DropTable("dbo.Posts");
        }
    }
}
