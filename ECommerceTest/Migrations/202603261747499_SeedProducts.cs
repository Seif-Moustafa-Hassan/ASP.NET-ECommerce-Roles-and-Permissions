namespace ECommerceTest.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedProducts : DbMigration
    {
        public override void Up()
        {
            for (int i = 1; i <= 50; i++)
            {
                Sql($@"
                INSERT INTO Products (Name, Description, Price, Quantity, Status, CreatedAt, UpdatedAt)
                VALUES (
                    'Product {i}', 
                    'This is description for product {i}', 
                    {new Random().Next(50, 500)}, 
                    {new Random().Next(1, 100)}, 
                    1, 
                    GETDATE(), 
                    GETDATE()
                )
                ");
            }
        }
        
        public override void Down()
        {
           
        }
    }
}
