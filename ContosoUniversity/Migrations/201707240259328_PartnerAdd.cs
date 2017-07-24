namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PartnerAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Partner",
                c => new
                    {
                        PartnerID = c.Int(nullable: false),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.PartnerID);
            
            AddColumn("dbo.Course", "PartnerID", c => c.Int(nullable: false));
            CreateIndex("dbo.Course", "PartnerID");
            AddForeignKey("dbo.Course", "PartnerID", "dbo.Partner", "PartnerID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Course", "PartnerID", "dbo.Partner");
            DropIndex("dbo.Course", new[] { "PartnerID" });
            DropColumn("dbo.Course", "PartnerID");
            DropTable("dbo.Partner");
        }
    }
}
