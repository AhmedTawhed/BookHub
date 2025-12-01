-- Delete all existing roles
-- DELETE FROM [dbo].[AspNetRoles];

-- Insert new roles
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
VALUES
    (NEWID(), 'Admin', 'ADMIN', NEWID()),
    (NEWID(), 'User', 'USER', NEWID());