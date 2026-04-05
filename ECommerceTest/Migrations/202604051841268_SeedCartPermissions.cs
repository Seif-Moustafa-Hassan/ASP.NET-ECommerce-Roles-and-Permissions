namespace ECommerceTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedCartPermissions : DbMigration
    {
        public override void Up()
        {
            // 1️⃣ Insert new permissions for Cart
            Sql(@"
                INSERT INTO Permissions (Name) VALUES 
                ('AddToCart'),
                ('ViewCart')
            ");

            // 2️⃣ Assign these permissions to User Role
            Sql(@"
                INSERT INTO RolePermissions (RoleId, PermissionId)
                SELECT 'ad7b8d12-21a9-4ac4-ad0a-d3fd761eb3da', Id
                FROM Permissions
                WHERE Name IN ('AddToCart', 'ViewCart')
            ");
        }
        
        public override void Down()
        {
            // Remove RolePermissions for these two
            Sql(@"
                DELETE FROM RolePermissions
                WHERE PermissionId IN (
                    SELECT Id FROM Permissions WHERE Name IN ('AddToCart', 'ViewCart')
                )
            ");

            // Remove the permissions themselves
            Sql(@"
                DELETE FROM Permissions WHERE Name IN ('AddToCart', 'ViewCart')
            ");
        }
    }
}
