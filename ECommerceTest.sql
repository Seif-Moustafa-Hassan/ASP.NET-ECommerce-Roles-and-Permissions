use ECommerceTest


SELECT * FROM AspNetUsers

select * from AspNetRoles

select * from AspNetUserClaims

select * from AspNetUserRoles

select * from Products

select * from Carts


-- Update user@gmail.com to username 'user'
UPDATE AspNetUsers
SET UserName = 'user@gmail.com'
WHERE Email = 'user@gmail.com';

-- Update admin@gmail.com to username 'admin'
UPDATE AspNetUsers
SET UserName = 'admin@gmail.com'
WHERE Email = 'admin@gmail.com';


ALTER TABLE AspNetUsers
ADD DisplayName NVARCHAR(256) NULL;


UPDATE AspNetUsers
SET UserName = 'user'
WHERE Email = 'user@gmail.com';

UPDATE AspNetUsers
SET UserName = 'admin'
WHERE Email = 'admin@gmail.com';



CREATE TABLE Permissions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(250) NULL
);

INSERT INTO Permissions (Name, Description)
VALUES
('ViewProduct', 'Permission to view products'),
('CreateProduct', 'Permission to create products'),
('EditProduct', 'Permission to edit products'),
('DeleteProduct', 'Permission to delete products');

select * from Permissions


CREATE TABLE RolePermissions (
    RoleId NVARCHAR(128) NOT NULL,
    PermissionId INT NOT NULL,
    PRIMARY KEY(RoleId, PermissionId),
    FOREIGN KEY(RoleId) REFERENCES AspNetRoles(Id),
    FOREIGN KEY(PermissionId) REFERENCES Permissions(Id)
);



-- Admin Role ID
DECLARE @AdminRoleId NVARCHAR(128) = '2dce664e-84ea-4c7d-ae4b-f5d4b04f299d';
-- User Role ID
DECLARE @UserRoleId NVARCHAR(128) = 'ad7b8d12-21a9-4ac4-ad0a-d3fd761eb3da';

-- Assign all permissions to Admin
INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT @AdminRoleId, Id FROM Permissions
WHERE NOT EXISTS (
    SELECT 1 FROM RolePermissions
    WHERE RoleId = @AdminRoleId AND PermissionId = Permissions.Id
);

-- Assign only ViewProduct permission to User
INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT @UserRoleId, Id FROM Permissions
WHERE Name = 'ViewProduct'
  AND NOT EXISTS (
    SELECT 1 FROM RolePermissions
    WHERE RoleId = @UserRoleId AND PermissionId = Permissions.Id
);



INSERT INTO Permissions (Name, Description) VALUES
('Create', 'Create products'),
('Read', 'View products'),
('Update', 'Update products'),
('Delete', 'Delete products');


INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT '2dce664e-84ea-4c7d-ae4b-f5d4b04f299d', Id FROM Permissions;

INSERT INTO RolePermissions (RoleId, PermissionId)
SELECT 'ad7b8d12-21a9-4ac4-ad0a-d3fd761eb3da', Id 
FROM Permissions
WHERE Name = 'Read';


select * from AspNetRoles

select * from AspNetUserRoles

select * from Permissions

select * from RolePermissions


select * from RolePermissions rp , Permissions p , AspNetRoles r
where r.Id = rp.RoleId and rp.PermissionId = p.Id




