	INSERT INTO [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [Salary], [PasswordHash], [SecurityStamp], [UserName], [Email], [EmailConfirmed], [PhoneNumber], [PhoneNumberConfirmed], 
                [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount]) 
                VALUES (N'15e68466-9b29-4182-9792-092f9ad5104a', N'admin', N'admin', 0, N'AFmez9rfQZ00C4FyTv8BgIPQy+rirkno0GIYEK8DLIsLOCSGlPaM5gJQsFVisZ56SQ==', 
                        N'867b6af9-a5d2-4ae6-9990-c0de98f1b5b4', N'admin', N'myemail@dom.com', 0, NULL, 0, 0, NULL, 0, 0)
    INSERT INTO [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [Salary], [PasswordHash], [SecurityStamp], [UserName], [Email], [EmailConfirmed], [PhoneNumber], [PhoneNumberConfirmed], 
                [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount]) 
                VALUES (N'67cab861-bb34-4c00-a394-38984a1b1173', N'Employee', N'rekrdd', 10000, N'ABb/95lEytVMmtX9qQGVG01FTF1BrelllH18W0/BNeQiZ0C8HS9B4FmgPcdhE1ZGxQ==', 
                        N'277573a0-ec17-4f51-9470-d5c0baafc459', N'NewEmp2', N'some@random.com', 0, NULL, 0, 0, NULL, 1, 0)
    INSERT INTO [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [Salary], [PasswordHash], [SecurityStamp], [UserName], [Email], [EmailConfirmed], [PhoneNumber], [PhoneNumberConfirmed], 
                [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount]) 
                VALUES (N'b6a57bbd-7a20-455e-a871-7fb5ada79b23', N'manager', N'rekrdd', 10000, N'ACFlrF3eKcyT8Y6VTMQIVgfgQtb9klIGrbx83GKEN1hf3cm1U6TaKtQNlAsfgawYDg==', 
                        N'32efb735-d143-4456-8538-d09b6767e389', N'manager1', N'some@manager.com', 0, NULL, 0, 0, NULL, 1, 0)
    INSERT INTO [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [Salary], [PasswordHash], [SecurityStamp], [UserName], [Email], [EmailConfirmed], [PhoneNumber], [PhoneNumberConfirmed], 
                [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount]) 
                VALUES (N'e1c0839b-4660-4f21-a65a-866542278547', N'Employee', N'Second', 10000, N'AICawoNz7uXCE6RiBQaZaGArvVIpNMiHJ/m+kPokRjAfNQ6OVp/2+TFLGkfWvlz5rQ==', 
                        N'f25e769e-a947-49d4-a818-6c90226957b1', N'NewEmp', N'newemail@dom2.com', 0, NULL, 0, 0, NULL, 1, 0)
    
    INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'dc8e73f1-3e8a-4d9a-9645-61408bd3b1db', N'Admin')
    INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'f17e5de1-44b1-46fd-8e50-90c1f8853020', N'Employee')
    INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'8e97cfe4-9920-46c1-a0b8-e53c8b908116', N'Manager')
    INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'c823c7d3-8222-4ec5-a524-535dfb6e3bd4', N'Project Manager')

    INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'15e68466-9b29-4182-9792-092f9ad5104a', N'dc8e73f1-3e8a-4d9a-9645-61408bd3b1db')
    INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'67cab861-bb34-4c00-a394-38984a1b1173', N'f17e5de1-44b1-46fd-8e50-90c1f8853020')
    INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'b6a57bbd-7a20-455e-a871-7fb5ada79b23', N'8e97cfe4-9920-46c1-a0b8-e53c8b908116')
    INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'e1c0839b-4660-4f21-a65a-866542278547', N'f17e5de1-44b1-46fd-8e50-90c1f8853020')
