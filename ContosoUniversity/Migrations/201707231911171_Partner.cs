namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Partner : DbMigration
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
            
            AddColumn("dbo.Course", "Partner_PartnerID", c => c.Int());
            CreateIndex("dbo.Course", "Partner_PartnerID");
            AddForeignKey("dbo.Course", "Partner_PartnerID", "dbo.Partner", "PartnerID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Course", "Partner_PartnerID", "dbo.Partner");
            DropIndex("dbo.Course", new[] { "Partner_PartnerID" });
            DropColumn("dbo.Course", "Partner_PartnerID");
            DropTable("dbo.Partner");
        }
    }
}
