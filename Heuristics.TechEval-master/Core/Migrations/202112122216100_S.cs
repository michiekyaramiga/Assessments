namespace Heuristics.TechEval.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class S : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Member", "Category", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Member", "Category");
        }
    }
}
