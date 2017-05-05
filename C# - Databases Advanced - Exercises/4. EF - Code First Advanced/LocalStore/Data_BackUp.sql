USE [LocalStoreContext]
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([Id], [Name], [DistributorName], [Description], [Price]) VALUES (1, N'Cucumber', N'Local Street Market', N'Product From Bulgaria', CAST(1.99 AS Decimal(18, 2)))
INSERT [dbo].[Products] ([Id], [Name], [DistributorName], [Description], [Price]) VALUES (2, N'Tomato', N'Local Street Market', N'Product From Bulgaria', CAST(1.49 AS Decimal(18, 2)))
INSERT [dbo].[Products] ([Id], [Name], [DistributorName], [Description], [Price]) VALUES (3, N'Potato', N'Local Street Market', N'Product From Bulgaria', CAST(0.99 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[Products] OFF
