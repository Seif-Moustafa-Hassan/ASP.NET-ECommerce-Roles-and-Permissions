namespace ECommerceTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedPermissions : DbMigration
    {
        public override void Up()
        {
            // Insert Permissions
            Sql(@"
                INSERT INTO Permissions (Name) VALUES 
                ('Create'),
                ('Read'),
                ('Update'),
                ('Delete')
            ");

            // Admin → Full access
            Sql(@"
                INSERT INTO RolePermissions (RoleId, PermissionId)
                SELECT '2dce664e-84ea-4c7d-ae4b-f5d4b04f299d', Id FROM Permissions
            ");

            // User → Read only
            Sql(@"
                INSERT INTO RolePermissions (RoleId, PermissionId)
                SELECT 'ad7b8d12-21a9-4ac4-ad0a-d3fd761eb3da', Id 
                FROM Permissions WHERE Name = 'Read'
            ");
        }
        
        public override void Down()
        {
            Sql("DELETE FROM RolePermissions");
            Sql("DELETE FROM Permissions");
        }
    }
}
